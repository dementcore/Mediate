// File: EventDispatcherServiceTests.cs
// The MIT License
//
// Copyright (c) 2026 DementCore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using Mediate.BackgroundEventDispatch.HostedService;
using Mediate.BackgroundEventDispatch.Queue;
using Mediate.BackgroundEventDispatch.Test.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Test;

[TestFixture]
public sealed class EventDispatcherServiceTests
{
    private static readonly TimeSpan HandlerTimeout = TimeSpan.FromSeconds(5);

    private static (EventQueue Queue, EventQueueDispatchStrategy Strategy, EventDispatcherService Service) CreateSut(
        FakeExceptionHandler? exceptionHandler = null)
    {
        EventQueue queue = new EventQueue();
        EventQueueDispatchStrategy strategy = new EventQueueDispatchStrategy(queue);
        EventDispatcherService service = new EventDispatcherService(
            NullLogger<EventDispatcherService>.Instance,
            queue,
            exceptionHandler ?? new FakeExceptionHandler());
        return (queue, strategy, service);
    }

    [Test]
    public async Task ExecuteAsync_WhenEventEnqueued_CallsHandler()
    {
        TaskCompletionSource tcs = new TaskCompletionSource();
        RecordingEventHandler handler = new RecordingEventHandler(() => tcs.TrySetResult());
        (EventQueue _, EventQueueDispatchStrategy strategy, EventDispatcherService service) = CreateSut();

        await strategy.Dispatch(new TestEvent(), new[] { handler });

        using CancellationTokenSource cts = new CancellationTokenSource();
        await service.StartAsync(cts.Token);

        await tcs.Task.WaitAsync(HandlerTimeout);

        await cts.CancelAsync();
        await service.StopAsync(CancellationToken.None);

        Assert.That(handler.CallCount, Is.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsync_WhenMultipleEventsEnqueued_CallsAllHandlers()
    {
        int remaining = 3;
        TaskCompletionSource tcs = new TaskCompletionSource();
        RecordingEventHandler handler = new RecordingEventHandler(() =>
        {
            if (Interlocked.Decrement(ref remaining) == 0)
            {
                tcs.TrySetResult();
            }
        });

        (EventQueue _, EventQueueDispatchStrategy strategy, EventDispatcherService service) = CreateSut();

        await strategy.Dispatch(new TestEvent(), new[] { handler });
        await strategy.Dispatch(new TestEvent(), new[] { handler });
        await strategy.Dispatch(new TestEvent(), new[] { handler });

        using CancellationTokenSource cts = new CancellationTokenSource();
        await service.StartAsync(cts.Token);

        await tcs.Task.WaitAsync(HandlerTimeout);

        await cts.CancelAsync();
        await service.StopAsync(CancellationToken.None);

        Assert.That(handler.CallCount, Is.EqualTo(3));
    }

    [Test]
    public async Task ExecuteAsync_WhenHandlerThrows_CallsExceptionHandler()
    {
        TaskCompletionSource tcs = new TaskCompletionSource();
        FakeExceptionHandler exceptionHandler = new FakeExceptionHandler(tcs);
        InvalidOperationException inner = new InvalidOperationException("handler error");
        FailingEventHandler handler = new FailingEventHandler(inner);

        (EventQueue _, EventQueueDispatchStrategy strategy, EventDispatcherService service) = CreateSut(exceptionHandler);

        await strategy.Dispatch(new TestEvent(), new[] { handler });

        using CancellationTokenSource cts = new CancellationTokenSource();
        await service.StartAsync(cts.Token);

        await tcs.Task.WaitAsync(HandlerTimeout);

        await cts.CancelAsync();
        await service.StopAsync(CancellationToken.None);

        Assert.That(exceptionHandler.CallCount, Is.EqualTo(1));
        Assert.That(exceptionHandler.CapturedException, Is.Not.Null);
        Assert.That(exceptionHandler.CapturedException.InnerExceptions, Has.Count.EqualTo(1));
        Assert.That(exceptionHandler.CapturedException.InnerExceptions[0], Is.SameAs(inner));
    }

    [Test]
    public async Task ExecuteAsync_WhenMultipleHandlersThrow_ExceptionHandlerReceivesAllInners()
    {
        TaskCompletionSource tcs = new TaskCompletionSource();
        FakeExceptionHandler exceptionHandler = new FakeExceptionHandler(tcs);
        InvalidOperationException ex1 = new InvalidOperationException("first");
        InvalidOperationException ex2 = new InvalidOperationException("second");

        (EventQueue _, EventQueueDispatchStrategy strategy, EventDispatcherService service) = CreateSut(exceptionHandler);

        await strategy.Dispatch(new TestEvent(), new[] {
            new FailingEventHandler(ex1),
            new FailingEventHandler(ex2),
        });

        using CancellationTokenSource cts = new CancellationTokenSource();
        await service.StartAsync(cts.Token);

        await tcs.Task.WaitAsync(HandlerTimeout);

        await cts.CancelAsync();
        await service.StopAsync(CancellationToken.None);

        Assert.That(exceptionHandler.CapturedException, Is.Not.Null);
        Assert.That(exceptionHandler.CapturedException!.InnerExceptions, Has.Count.EqualTo(2));
        Assert.That(exceptionHandler.CapturedException.InnerExceptions, Contains.Item(ex1));
        Assert.That(exceptionHandler.CapturedException.InnerExceptions, Contains.Item(ex2));
    }

    [Test]
    public async Task ExecuteAsync_WhenHandlerThrows_EventNamePassedToExceptionHandler()
    {
        TaskCompletionSource tcs = new TaskCompletionSource();
        FakeExceptionHandler exceptionHandler = new FakeExceptionHandler(tcs);

        (EventQueue _, EventQueueDispatchStrategy strategy, EventDispatcherService service) = CreateSut(exceptionHandler);

        await strategy.Dispatch(new TestEvent(), new[] { new FailingEventHandler(new Exception("boom")) });

        using CancellationTokenSource cts = new CancellationTokenSource();
        await service.StartAsync(cts.Token);

        await tcs.Task.WaitAsync(HandlerTimeout);

        await cts.CancelAsync();
        await service.StopAsync(CancellationToken.None);

        Assert.That(exceptionHandler.CapturedEventName, Is.EqualTo(nameof(TestEvent)));
    }

    [Test]
    public async Task ExecuteAsync_WhenHandlerThrows_ContinuesProcessingSubsequentEvents()
    {
        int remaining = 2;
        TaskCompletionSource tcs = new TaskCompletionSource();
        FakeExceptionHandler exceptionHandler = new FakeExceptionHandler(new TaskCompletionSource());
        RecordingEventHandler successHandler = new RecordingEventHandler(() =>
        {
            if (Interlocked.Decrement(ref remaining) == 0)
            {
                tcs.TrySetResult();
            }
        });

        (EventQueue _, EventQueueDispatchStrategy strategy, EventDispatcherService service) = CreateSut(exceptionHandler);

        await strategy.Dispatch(new TestEvent(), new[] { new FailingEventHandler(new Exception("first")) });
        await strategy.Dispatch(new TestEvent(), new[] { successHandler });
        await strategy.Dispatch(new TestEvent(), new[] { successHandler });

        using CancellationTokenSource cts = new CancellationTokenSource();
        await service.StartAsync(cts.Token);

        await tcs.Task.WaitAsync(HandlerTimeout);

        await cts.CancelAsync();
        await service.StopAsync(CancellationToken.None);

        Assert.That(successHandler.CallCount, Is.EqualTo(2));
    }

    [Test]
    public async Task StopAsync_StopsProcessing()
    {
        (EventQueue _, EventQueueDispatchStrategy _, EventDispatcherService service) = CreateSut();

        using CancellationTokenSource cts = new CancellationTokenSource();
        await service.StartAsync(cts.Token);
        await cts.CancelAsync();

        Assert.DoesNotThrowAsync(async () => await service.StopAsync(CancellationToken.None));
    }
}

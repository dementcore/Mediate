// File: SequentialEventDispatchStrategyTests.cs
// The MIT License
//
// Copyright (c) 2025 DementCore
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

using Mediate.Abstractions;
using Mediate.DispatchStrategies;
using Mediate.Test.Helpers;

namespace Mediate.Test;

[TestFixture]
public sealed class SequentialEventDispatchStrategyTests
{
    private static SequentialEventDispatchStrategy CreateStrategy()
    {
        return new SequentialEventDispatchStrategy();
    }

    [Test]
    public async Task Dispatch_CallsAllHandlers()
    {
        List<string> log = new List<string>();
        IEventHandler<TestEvent>[] handlers = new IEventHandler<TestEvent>[]
        {
            new RecordingEventHandler(log, "h1"),
            new RecordingEventHandler(log, "h2"),
            new RecordingEventHandler(log, "h3"),
        };

        await CreateStrategy().Dispatch(new TestEvent(), handlers);

        Assert.That(log, Has.Count.EqualTo(3));
        Assert.That(log, Contains.Item("h1"));
        Assert.That(log, Contains.Item("h2"));
        Assert.That(log, Contains.Item("h3"));
    }

    [Test]
    public async Task Dispatch_InvokesHandlersInRegistrationOrder()
    {
        List<string> log = new List<string>();
        IEventHandler<TestEvent>[] handlers = new IEventHandler<TestEvent>[]
        {
            new RecordingEventHandler(log, "h1"),
            new RecordingEventHandler(log, "h2"),
            new RecordingEventHandler(log, "h3"),
        };

        await CreateStrategy().Dispatch(new TestEvent(), handlers);

        Assert.That(log, Is.EqualTo(new[] { "h1", "h2", "h3" }));
    }

    [Test]
    public void Dispatch_WhenHandlerThrows_WrapsInAggregateException()
    {
        InvalidOperationException inner = new InvalidOperationException("handler failed");
        IEventHandler<TestEvent>[] handlers = new IEventHandler<TestEvent>[]
        {
            new FailingEventHandler(inner),
        };

        AggregateException ex = Assert.ThrowsAsync<AggregateException>(
            async () => await CreateStrategy().Dispatch(new TestEvent(), handlers));

        Assert.That(ex.InnerExceptions, Has.Count.EqualTo(1));
        Assert.That(ex.InnerExceptions[0], Is.SameAs(inner));
    }

    [Test]
    public void Dispatch_WhenMultipleHandlersThrow_CollectsAllExceptions()
    {
        InvalidOperationException ex1 = new InvalidOperationException("first");
        InvalidOperationException ex2 = new InvalidOperationException("second");
        IEventHandler<TestEvent>[] handlers = new IEventHandler<TestEvent>[]
        {
            new FailingEventHandler(ex1),
            new FailingEventHandler(ex2),
        };

        AggregateException ex = Assert.ThrowsAsync<AggregateException>(
            async () => await CreateStrategy().Dispatch(new TestEvent(), handlers));

        Assert.That(ex.InnerExceptions, Has.Count.EqualTo(2));
        Assert.That(ex.InnerExceptions, Contains.Item(ex1));
        Assert.That(ex.InnerExceptions, Contains.Item(ex2));
    }

    [Test]
    public async Task Dispatch_WhenFirstHandlerThrows_StillCallsRemainingHandlers()
    {
        List<string> log = new List<string>();
        IEventHandler<TestEvent>[] handlers = new IEventHandler<TestEvent>[]
        {
            new FailingEventHandler(new InvalidOperationException("boom")),
            new RecordingEventHandler(log, "h2"),
            new RecordingEventHandler(log, "h3"),
        };

        Assert.ThrowsAsync<AggregateException>(
            async () => await CreateStrategy().Dispatch(new TestEvent(), handlers));

        Assert.That(log, Is.EqualTo(new[] { "h2", "h3" }));
    }

    [Test]
    public async Task Dispatch_WithEmptyHandlers_CompletesWithoutError()
    {
        await CreateStrategy().Dispatch(new TestEvent(), Enumerable.Empty<IEventHandler<TestEvent>>());
    }

    [Test]
    public async Task Dispatch_PassesCancellationTokenToHandlers()
    {
        CancellationToken capturedToken = CancellationToken.None;
        CapturingEventHandler capturingHandler = new CapturingEventHandler(t => capturedToken = t);
        using CancellationTokenSource cts = new CancellationTokenSource();

        await CreateStrategy().Dispatch(new TestEvent(), new[] { capturingHandler }, cts.Token);

        Assert.That(capturedToken, Is.EqualTo(cts.Token));
    }

    private sealed class CapturingEventHandler : IEventHandler<TestEvent>
    {
        private readonly Action<CancellationToken> _capture;

        internal CapturingEventHandler(Action<CancellationToken> capture)
        {
            _capture = capture;
        }

        public Task Handle(TestEvent @event, CancellationToken cancellationToken)
        {
            _capture(cancellationToken);
            return Task.CompletedTask;
        }
    }
}

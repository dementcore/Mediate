// File: EventQueueDispatchStrategyTests.cs
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

using Mediate.Abstractions;
using Mediate.BackgroundEventDispatch.Queue;
using Mediate.BackgroundEventDispatch.Test.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Test;

[TestFixture]
public sealed class EventQueueDispatchStrategyTests
{
    private static EventQueueDispatchStrategy CreateStrategy()
        => new EventQueueDispatchStrategy(new EventQueue());

    [Test]
    public async Task Dispatch_WithHandlers_DoesNotThrow()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();
        RecordingEventHandler handler = new RecordingEventHandler();

        Assert.DoesNotThrowAsync(async () =>
            await strategy.Dispatch(new TestEvent(), new[] { handler }));
    }

    [Test]
    public async Task Dispatch_WithEmptyHandlers_DoesNotThrow()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();

        Assert.DoesNotThrowAsync(async () =>
            await strategy.Dispatch(new TestEvent(), Array.Empty<IEventHandler<TestEvent>>()));
    }

    [Test]
    public async Task Dispatch_WithCancellationToken_DoesNotThrow()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();
        using CancellationTokenSource cts = new CancellationTokenSource();

        Assert.DoesNotThrowAsync(async () =>
            await strategy.Dispatch(new TestEvent(), Array.Empty<IEventHandler<TestEvent>>(), cts.Token));
    }

    [Test]
    public void Dispatch_WithoutCancellationToken_UsesDefaultToken()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();

        Assert.DoesNotThrowAsync(async () =>
            await strategy.Dispatch(new TestEvent(), Array.Empty<IEventHandler<TestEvent>>()));
    }

    [Test]
    public void Dispose_DoesNotThrow()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();

        Assert.DoesNotThrow(() => strategy.Dispose());
    }

    [Test]
    public void Dispose_CalledMultipleTimes_DoesNotThrow()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();

        Assert.DoesNotThrow(() =>
        {
            strategy.Dispose();
            strategy.Dispose();
        });
    }

    [Test]
    public void Strategy_ImplementsIEventDispatchStrategy()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();

        Assert.That(strategy, Is.InstanceOf<IEventDispatchStrategy>());
    }

    [Test]
    public void Strategy_ImplementsIDisposable()
    {
        EventQueueDispatchStrategy strategy = CreateStrategy();

        Assert.That(strategy, Is.InstanceOf<IDisposable>());
    }
}

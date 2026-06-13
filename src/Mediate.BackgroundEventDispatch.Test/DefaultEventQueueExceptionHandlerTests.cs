// File: DefaultEventQueueExceptionHandlerTests.cs
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

using Mediate.BackgroundEventDispatch.Abstractions;
using Mediate.BackgroundEventDispatch.Test.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Test;

[TestFixture]
public sealed class DefaultEventQueueExceptionHandlerTests
{
    private static (DefaultEventQueueExceptionHandler Handler, CapturingLogger<DefaultEventQueueExceptionHandler> Logger) CreateSut()
    {
        CapturingLogger<DefaultEventQueueExceptionHandler> logger = new CapturingLogger<DefaultEventQueueExceptionHandler>();
        DefaultEventQueueExceptionHandler handler = new DefaultEventQueueExceptionHandler(logger);
        return (handler, logger);
    }

    [Test]
    public async Task Handle_LogsErrorWithException()
    {
        (DefaultEventQueueExceptionHandler handler, CapturingLogger<DefaultEventQueueExceptionHandler> logger) = CreateSut();
        AggregateException ex = new AggregateException(new InvalidOperationException("inner"));

        await handler.Handle(ex, "TestEvent");

        Assert.That(logger.Logs, Has.Count.EqualTo(1));
        Assert.That(logger.Logs[0].Level, Is.EqualTo(LogLevel.Error));
        Assert.That(logger.Logs[0].Exception, Is.SameAs(ex));
    }

    [Test]
    public async Task Handle_LogsMessageContainingEventName()
    {
        (DefaultEventQueueExceptionHandler handler, CapturingLogger<DefaultEventQueueExceptionHandler> logger) = CreateSut();
        AggregateException ex = new AggregateException(new Exception("err"));

        await handler.Handle(ex, "MyCustomEvent");

        Assert.That(logger.Logs[0].Message, Does.Contain("MyCustomEvent"));
    }

    [Test]
    public async Task Handle_ReturnsCompletedTask()
    {
        (DefaultEventQueueExceptionHandler handler, CapturingLogger<DefaultEventQueueExceptionHandler> _) = CreateSut();
        AggregateException ex = new AggregateException(new Exception("err"));

        Task result = handler.Handle(ex, "TestEvent");

        Assert.That(result.IsCompleted, Is.True);
        await result;
    }

    [Test]
    public void Handler_ImplementsIEventQueueExceptionHandler()
    {
        (DefaultEventQueueExceptionHandler handler, CapturingLogger<DefaultEventQueueExceptionHandler> _) = CreateSut();

        Assert.That(handler, Is.InstanceOf<IEventQueueExceptionHandler>());
    }

    [Test]
    public async Task Handle_WithMultipleInnerExceptions_LogsOnce()
    {
        (DefaultEventQueueExceptionHandler handler, CapturingLogger<DefaultEventQueueExceptionHandler> logger) = CreateSut();
        AggregateException ex = new AggregateException(
            new Exception("first"),
            new Exception("second"),
            new Exception("third"));

        await handler.Handle(ex, "TestEvent");

        Assert.That(logger.Logs, Has.Count.EqualTo(1));
    }
}

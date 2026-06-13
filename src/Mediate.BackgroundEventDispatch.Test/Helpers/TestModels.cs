// File: TestModels.cs
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
using Mediate.BackgroundEventDispatch.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Test.Helpers;

internal sealed class TestEvent : IEvent { }

internal sealed class RecordingEventHandler : IEventHandler<TestEvent>
{
    private int _callCount;
    private readonly Action _onHandle;

    internal RecordingEventHandler(Action? onHandle = null)
    {
        _onHandle = onHandle ?? (() => { });
    }

    internal int CallCount => _callCount;

    public Task Handle(TestEvent @event, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _callCount);
        _onHandle();
        return Task.CompletedTask;
    }
}

internal sealed class FailingEventHandler : IEventHandler<TestEvent>
{
    private readonly Exception _exception;

    internal FailingEventHandler(Exception exception)
    {
        _exception = exception;
    }

    public Task Handle(TestEvent @event, CancellationToken cancellationToken)
    {
        throw _exception;
    }
}

internal sealed class FakeExceptionHandler : IEventQueueExceptionHandler
{
    private readonly TaskCompletionSource? _tcs;
    private int _callCount;

    internal AggregateException? CapturedException { get; private set; }
    internal string? CapturedEventName { get; private set; }
    internal int CallCount => _callCount;

    internal FakeExceptionHandler(TaskCompletionSource? tcs = null)
    {
        _tcs = tcs;
    }

    public Task Handle(AggregateException aggregateException, string eventName)
    {
        CapturedException = aggregateException;
        CapturedEventName = eventName;
        Interlocked.Increment(ref _callCount);
        _tcs?.TrySetResult();
        return Task.CompletedTask;
    }
}

internal sealed class CapturingLogger<T> : ILogger<T>
{
    internal List<(LogLevel Level, Exception? Exception, string Message)> Logs { get; }
        = new List<(LogLevel, Exception?, string)>();

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
        => NullDisposable.Instance;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        Logs.Add((logLevel, exception, formatter(state, exception)));
    }

    private sealed class NullDisposable : IDisposable
    {
        internal static readonly NullDisposable Instance = new NullDisposable();
        public void Dispose() { }
    }
}
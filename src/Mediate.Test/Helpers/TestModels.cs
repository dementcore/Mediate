// File: TestModels.cs
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

namespace Mediate.Test.Helpers;

internal sealed class TestEvent : IEvent
{
    internal string Data { get; set; } = string.Empty;
}

internal sealed class TestQuery : IQuery<string>
{
    internal string Input { get; set; } = string.Empty;
}

internal sealed class TestQueryHandler : IQueryHandler<TestQuery, string>
{
    public Task<string> Handle(TestQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult($"handled:{query.Input}");
    }
}

internal sealed class RecordingEventHandler : IEventHandler<TestEvent>
{
    private readonly List<string> _log;
    private readonly string _name;

    internal RecordingEventHandler(List<string> log, string name)
    {
        _log = log;
        _name = name;
    }

    public Task Handle(TestEvent @event, CancellationToken cancellationToken)
    {
        _log.Add(_name);
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

internal sealed class RecordingQueryMiddleware : IQueryMiddleware<TestQuery, string>
{
    private readonly List<string> _log;
    private readonly string _name;

    internal RecordingQueryMiddleware(List<string> log, string name)
    {
        _log = log;
        _name = name;
    }

    public async Task<string> Invoke(TestQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<string> next)
    {
        _log.Add($"{_name}:before");
        string result = await next();
        _log.Add($"{_name}:after");
        return result;
    }
}

internal sealed class RecordingEventMiddleware : IEventMiddleware<TestEvent>
{
    private readonly List<string> _log;
    private readonly string _name;

    internal RecordingEventMiddleware(List<string> log, string name)
    {
        _log = log;
        _name = name;
    }

    public async Task Invoke(TestEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
    {
        _log.Add($"{_name}:before");
        await next();
        _log.Add($"{_name}:after");
    }
}

internal sealed class FakeHandlerProvider : IHandlerProvider
{
    private readonly IQueryHandler<TestQuery, string> _queryHandler;
    private readonly IEnumerable<IEventHandler<TestEvent>> _eventHandlers;

    internal FakeHandlerProvider(
        IQueryHandler<TestQuery, string> queryHandler = null,
        IEnumerable<IEventHandler<TestEvent>> eventHandlers = null)
    {
        _queryHandler = queryHandler;
        _eventHandlers = eventHandlers ?? Enumerable.Empty<IEventHandler<TestEvent>>();
    }

    public Task<IQueryHandler<TQuery, TResult>> GetHandler<TQuery, TResult>()
        where TQuery : IQuery<TResult>
    {
        return Task.FromResult((IQueryHandler<TQuery, TResult>)(object)_queryHandler);
    }

    public Task<IEnumerable<IEventHandler<TEvent>>> GetHandlers<TEvent>()
        where TEvent : IEvent
    {
        return Task.FromResult((IEnumerable<IEventHandler<TEvent>>)(object)_eventHandlers);
    }
}

internal sealed class FakeMiddlewareProvider : IMiddlewareProvider
{
    private readonly IEnumerable<IQueryMiddleware<TestQuery, string>> _queryMiddlewares;
    private readonly IEnumerable<IEventMiddleware<TestEvent>> _eventMiddlewares;

    internal FakeMiddlewareProvider(
        IEnumerable<IQueryMiddleware<TestQuery, string>> queryMiddlewares = null,
        IEnumerable<IEventMiddleware<TestEvent>> eventMiddlewares = null)
    {
        _queryMiddlewares = queryMiddlewares ?? Enumerable.Empty<IQueryMiddleware<TestQuery, string>>();
        _eventMiddlewares = eventMiddlewares ?? Enumerable.Empty<IEventMiddleware<TestEvent>>();
    }

    public Task<IEnumerable<IQueryMiddleware<TQuery, TResult>>> GetMiddlewares<TQuery, TResult>()
        where TQuery : IQuery<TResult>
    {
        return Task.FromResult((IEnumerable<IQueryMiddleware<TQuery, TResult>>)(object)_queryMiddlewares);
    }

    public Task<IEnumerable<IEventMiddleware<TEvent>>> GetMiddlewares<TEvent>()
        where TEvent : IEvent
    {
        return Task.FromResult((IEnumerable<IEventMiddleware<TEvent>>)(object)_eventMiddlewares);
    }
}

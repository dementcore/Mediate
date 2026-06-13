// File: MediatorTests.cs
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
public sealed class MediatorTests
{
    private static Mediator CreateMediator(
        IHandlerProvider handlerProvider,
        IMiddlewareProvider middlewareProvider = null)
    {
        return new Mediator(
            handlerProvider,
            middlewareProvider ?? new FakeMiddlewareProvider(),
            new SequentialEventDispatchStrategy());
    }

    [Test]
    public void Send_NullQuery_ThrowsArgumentNullException()
    {
        Mediator mediator = CreateMediator(new FakeHandlerProvider());

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await mediator.Send<string>(null));
    }

    [Test]
    public void Send_WithCancellationToken_NullQuery_ThrowsArgumentNullException()
    {
        Mediator mediator = CreateMediator(new FakeHandlerProvider());

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await mediator.Send<string>(null, CancellationToken.None));
    }

    [Test]
    public void Dispatch_NullEvent_ThrowsArgumentNullException()
    {
        Mediator mediator = CreateMediator(new FakeHandlerProvider());

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await mediator.Dispatch<TestEvent>(null));
    }

    [Test]
    public void Dispatch_WithCancellationToken_NullEvent_ThrowsArgumentNullException()
    {
        Mediator mediator = CreateMediator(new FakeHandlerProvider());

        Assert.ThrowsAsync<ArgumentNullException>(
            async () => await mediator.Dispatch<TestEvent>(null, CancellationToken.None));
    }

    [Test]
    public void Send_NoHandler_ThrowsInvalidOperationException()
    {
        Mediator mediator = CreateMediator(new FakeHandlerProvider(queryHandler: null));

        Assert.ThrowsAsync<InvalidOperationException>(
            async () => await mediator.Send(new TestQuery { Input = "x" }));
    }

    [Test]
    public void Dispatch_NoHandlers_ThrowsInvalidOperationException()
    {
        Mediator mediator = CreateMediator(
            new FakeHandlerProvider(eventHandlers: Enumerable.Empty<IEventHandler<TestEvent>>()));

        Assert.ThrowsAsync<InvalidOperationException>(
            async () => await mediator.Dispatch(new TestEvent()));
    }

    [Test]
    public async Task Send_WithHandler_ReturnsResult()
    {
        Mediator mediator = CreateMediator(
            new FakeHandlerProvider(queryHandler: new TestQueryHandler()));

        string result = await mediator.Send(new TestQuery { Input = "hello" });

        Assert.That(result, Is.EqualTo("handled:hello"));
    }

    [Test]
    public async Task Send_WithQueryMiddleware_ExecutesPipelineAroundHandler()
    {
        List<string> log = new List<string>();
        RecordingQueryMiddleware middleware = new RecordingQueryMiddleware(log, "m1");

        Mediator mediator = CreateMediator(
            new FakeHandlerProvider(queryHandler: new TestQueryHandler()),
            new FakeMiddlewareProvider(queryMiddlewares: new[] { middleware }));

        string result = await mediator.Send(new TestQuery { Input = "hi" });

        Assert.That(result, Is.EqualTo("handled:hi"));
        Assert.That(log, Is.EqualTo(new[] { "m1:before", "m1:after" }));
    }

    [Test]
    public async Task Send_WithMultipleQueryMiddlewares_ExecutesPipelineInRegistrationOrder()
    {
        List<string> log = new List<string>();
        RecordingQueryMiddleware m1 = new RecordingQueryMiddleware(log, "m1");
        RecordingQueryMiddleware m2 = new RecordingQueryMiddleware(log, "m2");

        Mediator mediator = CreateMediator(
            new FakeHandlerProvider(queryHandler: new TestQueryHandler()),
            new FakeMiddlewareProvider(queryMiddlewares: new[] { m1, m2 }));

        await mediator.Send(new TestQuery { Input = "hi" });

        Assert.That(log, Is.EqualTo(new[] { "m1:before", "m2:before", "m2:after", "m1:after" }));
    }

    [Test]
    public async Task Dispatch_WithHandlers_CallsAll()
    {
        List<string> log = new List<string>();
        IEventHandler<TestEvent>[] handlers = new IEventHandler<TestEvent>[]
        {
            new RecordingEventHandler(log, "h1"),
            new RecordingEventHandler(log, "h2"),
        };

        Mediator mediator = CreateMediator(new FakeHandlerProvider(eventHandlers: handlers));

        await mediator.Dispatch(new TestEvent());

        Assert.That(log, Has.Count.EqualTo(2));
        Assert.That(log, Contains.Item("h1"));
        Assert.That(log, Contains.Item("h2"));
    }

    [Test]
    public async Task Dispatch_WithEventMiddleware_ExecutesPipelineAroundHandlers()
    {
        List<string> log = new List<string>();
        RecordingEventMiddleware middleware = new RecordingEventMiddleware(log, "m1");
        RecordingEventHandler handler = new RecordingEventHandler(log, "h1");

        Mediator mediator = CreateMediator(
            new FakeHandlerProvider(eventHandlers: new[] { handler }),
            new FakeMiddlewareProvider(eventMiddlewares: new[] { middleware }));

        await mediator.Dispatch(new TestEvent());

        Assert.That(log, Is.EqualTo(new[] { "m1:before", "h1", "m1:after" }));
    }

    [Test]
    public async Task Send_WithSameQueryType_ReusesCachedWrapper()
    {
        int callCount = 0;
        CountingQueryHandler countingHandler = new CountingQueryHandler(() => callCount++);

        Mediator mediator = CreateMediator(new FakeHandlerProvider(queryHandler: countingHandler));

        await mediator.Send(new TestQuery { Input = "a" });
        await mediator.Send(new TestQuery { Input = "b" });

        Assert.That(callCount, Is.EqualTo(2));
    }

    private sealed class CountingQueryHandler : IQueryHandler<TestQuery, string>
    {
        private readonly Action _onHandle;

        internal CountingQueryHandler(Action onHandle)
        {
            _onHandle = onHandle;
        }

        public Task<string> Handle(TestQuery query, CancellationToken cancellationToken)
        {
            _onHandle();
            return Task.FromResult($"handled:{query.Input}");
        }
    }
}

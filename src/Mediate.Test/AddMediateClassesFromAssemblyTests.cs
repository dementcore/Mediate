// File: AddMediateClassesFromAssemblyTests.cs
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
using Mediate.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Mediate.Test;

[TestFixture]
public sealed class AddMediateClassesFromAssemblyTests
{
    private static readonly Assembly TestAssembly = typeof(AddMediateClassesFromAssemblyTests).Assembly;

    [Test]
    public void AddMediateClassesFromAssembly_NullAssembly_ThrowsArgumentNullException()
    {
        ServiceCollection services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddMediateClassesFromAssembly(null));
    }

    [Test]
    public void AddMediateClassesFromAssembly_OpenGenericEventHandler_RegisteredAsOpenGenericService()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IEventHandler<>) &&
            s.ImplementationType == typeof(OpenGenericEventHandler<>));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void OpenGenericEventHandler_WhenRegisteredViaOpenGenericBinding_ResolvesForConcreteEventType()
    {
        // Verifies the registration pattern used by AddMediateClassesFromAssembly for open-generic handlers
        ServiceCollection services = new ServiceCollection();
        services.AddTransient(typeof(IEventHandler<>), typeof(OpenGenericEventHandler<>));
        IServiceProvider provider = services.BuildServiceProvider();

        IEnumerable<IEventHandler<TestEvent>> handlers = provider.GetServices<IEventHandler<TestEvent>>();

        Assert.That(handlers.Any(h => h is OpenGenericEventHandler<TestEvent>), Is.True);
    }

    [Test]
    public void AddMediateClassesFromAssembly_OpenGenericEventMiddleware_RegisteredAsOpenGenericService()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IEventMiddleware<>) &&
            s.ImplementationType == typeof(OpenGenericEventMiddleware<>));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void OpenGenericEventMiddleware_WhenRegisteredViaOpenGenericBinding_ResolvesForConcreteEventType()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddTransient(typeof(IEventMiddleware<>), typeof(OpenGenericEventMiddleware<>));
        IServiceProvider provider = services.BuildServiceProvider();

        IEnumerable<IEventMiddleware<TestEvent>> middlewares = provider.GetServices<IEventMiddleware<TestEvent>>();

        Assert.That(middlewares.Any(m => m is OpenGenericEventMiddleware<TestEvent>), Is.True);
    }

    [Test]
    public void AddMediateClassesFromAssembly_OpenGenericQueryMiddleware_RegisteredAsOpenGenericService()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IQueryMiddleware<,>) &&
            s.ImplementationType == typeof(OpenGenericQueryMiddleware<,>));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void OpenGenericQueryMiddleware_WhenRegisteredViaOpenGenericBinding_ResolvesForConcreteQueryType()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddTransient(typeof(IQueryMiddleware<,>), typeof(OpenGenericQueryMiddleware<,>));
        IServiceProvider provider = services.BuildServiceProvider();

        IEnumerable<IQueryMiddleware<TestQuery, string>> middlewares =
            provider.GetServices<IQueryMiddleware<TestQuery, string>>();

        Assert.That(middlewares.Any(m => m is OpenGenericQueryMiddleware<TestQuery, string>), Is.True);
    }

    [Test]
    public void AddMediateClassesFromAssembly_OpenGenericQueryHandler_NotRegistered()
    {
        // Query handlers use allowGeneric=false: open-generic query handlers must not be registered.
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IQueryHandler<,>) &&
            s.ImplementationType == typeof(OpenGenericQueryHandler<,>));

        Assert.That(registered, Is.False);
    }

    [Test]
    public void AddMediateClassesFromAssembly_ClosedQueryHandler_Registered()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IQueryHandler<TestQuery, string>) &&
            s.ImplementationType == typeof(TestQueryHandler));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddMediateClassesFromAssembly_ClosedEventHandler_RegisteredOnFirstScan()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);

        int count = services.Count(s => s.ServiceType == typeof(IEventHandler<TestEvent>));

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void AddMediateClassesFromAssembly_CalledTwice_OpenGenericEventHandlerNotDuplicated()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);
        services.AddMediateClassesFromAssembly(TestAssembly);

        int count = services.Count(s =>
            s.ServiceType == typeof(IEventHandler<>) &&
            s.ImplementationType == typeof(OpenGenericEventHandler<>));

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void AddMediateClassesFromAssembly_CalledTwice_OpenGenericEventMiddlewareNotDuplicated()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);
        services.AddMediateClassesFromAssembly(TestAssembly);

        int count = services.Count(s =>
            s.ServiceType == typeof(IEventMiddleware<>) &&
            s.ImplementationType == typeof(OpenGenericEventMiddleware<>));

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void AddMediateClassesFromAssembly_CalledTwice_OpenGenericQueryMiddlewareNotDuplicated()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);
        services.AddMediateClassesFromAssembly(TestAssembly);

        int count = services.Count(s =>
            s.ServiceType == typeof(IQueryMiddleware<,>) &&
            s.ImplementationType == typeof(OpenGenericQueryMiddleware<,>));

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void AddMediateClassesFromAssembly_CalledTwice_ClosedQueryHandlerNotDuplicated()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);
        services.AddMediateClassesFromAssembly(TestAssembly);

        int count = services.Count(s =>
            s.ServiceType == typeof(IQueryHandler<TestQuery, string>) &&
            s.ImplementationType == typeof(TestQueryHandler));

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void AddMediateClassesFromAssembly_CalledTwice_ClosedEventHandlerNotDuplicated()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(TestAssembly);
        services.AddMediateClassesFromAssembly(TestAssembly);

        int count = services.Count(s => s.ServiceType == typeof(IEventHandler<TestEvent>));

        Assert.That(count, Is.EqualTo(1));
    }

    [Test]
    public void AddMediateClassesFromAssembly_AssemblyContainingInterfaces_SkipsInterfaceTypes()
    {
        // Scanning an assembly that contains interfaces exercises the IsNotAbstract → false path
        ServiceCollection services = new ServiceCollection();
        services.AddMediateClassesFromAssembly(typeof(IMediator).Assembly);

        bool hasEventHandlers = services.Any(s => s.ServiceType == typeof(IEventHandler<>));
        bool hasQueryHandlers = services.Any(s => s.ServiceType.IsGenericType &&
            s.ServiceType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));

        Assert.That(hasEventHandlers, Is.False);
        Assert.That(hasQueryHandlers, Is.False);
    }

    // Open-generic event handler — registered as open generic (IEventHandler<>)
    private sealed class OpenGenericEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public Task Handle(TEvent @event, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    // Open-generic event middleware — registered as open generic (IEventMiddleware<>)
    private sealed class OpenGenericEventMiddleware<TEvent> : IEventMiddleware<TEvent>
        where TEvent : IEvent
    {
        public Task Invoke(TEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
        {
            return next();
        }
    }

    // Open-generic query middleware — registered as open generic (IQueryMiddleware<,>)
    private sealed class OpenGenericQueryMiddleware<TQuery, TResult> : IQueryMiddleware<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public Task<TResult> Invoke(TQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<TResult> next)
        {
            return next();
        }
    }

    // Open-generic query handler — must NOT be registered (allowGeneric=false for IQueryHandler<,>)
    private sealed class OpenGenericQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public Task<TResult> Handle(TQuery query, CancellationToken cancellationToken)
        {
            return Task.FromResult(default(TResult));
        }
    }
}
// File: ServiceCollectionExtensionsTests.cs
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
using Mediate.Configuration;
using Mediate.DispatchStrategies;
using Microsoft.Extensions.DependencyInjection;

namespace Mediate.Test;

[TestFixture]
public sealed class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddMediateCore_WhenMediatorAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCore();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateCore());
    }

    [Test]
    public void AddMediateCore_RegistersIMediator()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCore();

        bool registered = services.Any(s => s.ServiceType == typeof(IMediator));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddMediateParallelEventDispatchStrategy_WhenStrategyAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateParallelEventDispatchStrategy();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateParallelEventDispatchStrategy());
    }

    [Test]
    public void AddMediateParallelEventDispatchStrategy_RegistersParallelStrategy()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateParallelEventDispatchStrategy();

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IEventDispatchStrategy) &&
            s.ImplementationType == typeof(ParallelEventDispatchStrategy));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddMediateSequentialEventDispatchStrategy_WhenStrategyAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateSequentialEventDispatchStrategy();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateSequentialEventDispatchStrategy());
    }

    [Test]
    public void AddMediateSequentialEventDispatchStrategy_RegistersSequentialStrategy()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateSequentialEventDispatchStrategy();

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IEventDispatchStrategy) &&
            s.ImplementationType == typeof(SequentialEventDispatchStrategy));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddMediateCustomDispatchStrategy_WhenStrategyAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCustomDispatchStrategy<NoOpDispatchStrategy>();

        Assert.Throws<InvalidOperationException>(() =>
            services.AddMediateCustomDispatchStrategy<NoOpDispatchStrategy>());
    }

    [Test]
    public void AddMediateCustomDispatchStrategy_RegistersCustomStrategy()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCustomDispatchStrategy<NoOpDispatchStrategy>();

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IEventDispatchStrategy) &&
            s.ImplementationType == typeof(NoOpDispatchStrategy));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddMediateCustomDispatchStrategy_WithLifetime_WhenStrategyAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCustomDispatchStrategy<NoOpDispatchStrategy>(ServiceLifetime.Singleton);

        Assert.Throws<InvalidOperationException>(() =>
            services.AddMediateCustomDispatchStrategy<NoOpDispatchStrategy>(ServiceLifetime.Singleton));
    }

    [Test]
    public void AddMediateCustomDispatchStrategy_WithLifetime_RegistersWithSpecifiedLifetime()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCustomDispatchStrategy<NoOpDispatchStrategy>(ServiceLifetime.Singleton);

        ServiceDescriptor descriptor = services.First(s => s.ServiceType == typeof(IEventDispatchStrategy));

        Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
        Assert.That(descriptor.ImplementationType, Is.EqualTo(typeof(NoOpDispatchStrategy)));
    }

    [Test]
    public void AddMediate_RegistersMediatorHandlerProviderAndMiddlewareProvider()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediate();

        bool mediatorRegistered = services.Any(s => s.ServiceType == typeof(IMediator));
        bool handlerProviderRegistered = services.Any(s => s.ServiceType == typeof(IHandlerProvider));
        bool middlewareProviderRegistered = services.Any(s => s.ServiceType == typeof(IMiddlewareProvider));

        Assert.That(mediatorRegistered, Is.True);
        Assert.That(handlerProviderRegistered, Is.True);
        Assert.That(middlewareProviderRegistered, Is.True);
    }

    [Test]
    public void AddServiceProviderHandlerProvider_WhenAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateBuilder builder = services.AddMediateCore();
        builder.AddServiceProviderHandlerProvider();

        Assert.Throws<InvalidOperationException>(() => builder.AddServiceProviderHandlerProvider());
    }

    [Test]
    public void AddServiceProviderHandlerProvider_RegistersHandlerProvider()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCore().AddServiceProviderHandlerProvider();

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IHandlerProvider) &&
            s.ImplementationType == typeof(ServiceProviderHandlerProvider));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddCustomHandlerProvider_WhenAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateBuilder builder = services.AddMediateCore();
        builder.AddCustomHandlerProvider<NoOpHandlerProvider>();

        Assert.Throws<InvalidOperationException>(() => builder.AddCustomHandlerProvider<NoOpHandlerProvider>());
    }

    [Test]
    public void AddCustomHandlerProvider_RegistersCustomHandlerProvider()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCore().AddCustomHandlerProvider<NoOpHandlerProvider>();

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IHandlerProvider) &&
            s.ImplementationType == typeof(NoOpHandlerProvider));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddServiceProviderMiddlewareProvider_WhenAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateBuilder builder = services.AddMediateCore();
        builder.AddServiceProviderMiddlewareProvider();

        Assert.Throws<InvalidOperationException>(() => builder.AddServiceProviderMiddlewareProvider());
    }

    [Test]
    public void AddServiceProviderMiddlewareProvider_RegistersMiddlewareProvider()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCore().AddServiceProviderMiddlewareProvider();

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IMiddlewareProvider) &&
            s.ImplementationType == typeof(ServiceProviderMiddlewareProvider));

        Assert.That(registered, Is.True);
    }

    [Test]
    public void AddCustomMiddlewareProvider_WhenAlreadyRegistered_ThrowsInvalidOperationException()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateBuilder builder = services.AddMediateCore();
        builder.AddCustomMiddlewareProvider<NoOpMiddlewareProvider>();

        Assert.Throws<InvalidOperationException>(() => builder.AddCustomMiddlewareProvider<NoOpMiddlewareProvider>());
    }

    [Test]
    public void AddCustomMiddlewareProvider_RegistersCustomMiddlewareProvider()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddMediateCore().AddCustomMiddlewareProvider<NoOpMiddlewareProvider>();

        bool registered = services.Any(s =>
            s.ServiceType == typeof(IMiddlewareProvider) &&
            s.ImplementationType == typeof(NoOpMiddlewareProvider));

        Assert.That(registered, Is.True);
    }

    private sealed class NoOpDispatchStrategy : IEventDispatchStrategy
    {
        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers)
            where TEvent : IEvent
        {
            return Task.CompletedTask;
        }

        public Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken)
            where TEvent : IEvent
        {
            return Task.CompletedTask;
        }
    }

    private sealed class NoOpHandlerProvider : IHandlerProvider
    {
        public Task<IQueryHandler<TQuery, TResult>> GetHandler<TQuery, TResult>()
            where TQuery : IQuery<TResult>
        {
            return Task.FromResult<IQueryHandler<TQuery, TResult>>(null);
        }

        public Task<IEnumerable<IEventHandler<TEvent>>> GetHandlers<TEvent>()
            where TEvent : IEvent
        {
            return Task.FromResult(Enumerable.Empty<IEventHandler<TEvent>>());
        }
    }

    private sealed class NoOpMiddlewareProvider : IMiddlewareProvider
    {
        public Task<IEnumerable<IEventMiddleware<TEvent>>> GetMiddlewares<TEvent>()
            where TEvent : IEvent
        {
            return Task.FromResult(Enumerable.Empty<IEventMiddleware<TEvent>>());
        }

        public Task<IEnumerable<IQueryMiddleware<TQuery, TResult>>> GetMiddlewares<TQuery, TResult>()
            where TQuery : IQuery<TResult>
        {
            return Task.FromResult(Enumerable.Empty<IQueryMiddleware<TQuery, TResult>>());
        }
    }
}

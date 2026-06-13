// File: ServiceCollectionExtensionsTests.cs
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
using Mediate.BackgroundEventDispatch.Configuration;
using Mediate.BackgroundEventDispatch.HostedService;
using Mediate.BackgroundEventDispatch.Queue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Mediate.BackgroundEventDispatch.Test;

[TestFixture]
public sealed class ServiceCollectionExtensionsTests
{
    private static ServiceCollection CreateServices() => new ServiceCollection();

    [Test]
    public void AddMediateEventQueueDispatchStrategy_RegistersEventDispatchStrategy()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategy();

        Assert.That(services.Any(s =>
            s.ServiceType == typeof(IEventDispatchStrategy) &&
            s.ImplementationType == typeof(EventQueueDispatchStrategy)), Is.True);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_RegistersEventQueue()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategy();

        Assert.That(services.Any(s => s.ServiceType == typeof(EventQueue)), Is.True);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_RegistersDefaultExceptionHandler()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategy();

        Assert.That(services.Any(s =>
            s.ServiceType == typeof(IEventQueueExceptionHandler) &&
            s.ImplementationType == typeof(DefaultEventQueueExceptionHandler)), Is.True);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_RegistersEventDispatcherService()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategy();

        Assert.That(services.Any(s =>
            s.ServiceType == typeof(IHostedService) &&
            s.ImplementationType == typeof(EventDispatcherService)), Is.True);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_EventDispatchStrategyRegisteredAsScoped()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategy();

        ServiceDescriptor descriptor = services.First(s => s.ServiceType == typeof(IEventDispatchStrategy));
        Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_EventQueueRegisteredAsSingleton()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategy();

        ServiceDescriptor descriptor = services.First(s => s.ServiceType == typeof(EventQueue));
        Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_WhenEventDispatchStrategyAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddScoped<IEventDispatchStrategy, EventQueueDispatchStrategy>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategy());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_WhenEventQueueAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddSingleton<EventQueue>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategy());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_WhenExceptionHandlerAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddSingleton<IEventQueueExceptionHandler, DefaultEventQueueExceptionHandler>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategy());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategy_WhenEventDispatcherServiceAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddHostedService<EventDispatcherService>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategy());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_RegistersEventDispatchStrategy()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategyCore();

        Assert.That(services.Any(s =>
            s.ServiceType == typeof(IEventDispatchStrategy) &&
            s.ImplementationType == typeof(EventQueueDispatchStrategy)), Is.True);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_RegistersEventQueue()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategyCore();

        Assert.That(services.Any(s => s.ServiceType == typeof(EventQueue)), Is.True);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_RegistersEventDispatcherService()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategyCore();

        Assert.That(services.Any(s =>
            s.ServiceType == typeof(IHostedService) &&
            s.ImplementationType == typeof(EventDispatcherService)), Is.True);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_DoesNotRegisterExceptionHandler()
    {
        ServiceCollection services = CreateServices();

        services.AddMediateEventQueueDispatchStrategyCore();

        Assert.That(services.Any(s => s.ServiceType == typeof(IEventQueueExceptionHandler)), Is.False);
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_ReturnsIMediateEventQueueBuilder()
    {
        ServiceCollection services = CreateServices();

        IMediateEventQueueBuilder builder = services.AddMediateEventQueueDispatchStrategyCore();

        Assert.That(builder, Is.Not.Null);
        Assert.That(builder, Is.InstanceOf<IMediateEventQueueBuilder>());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_WhenEventDispatchStrategyAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddScoped<IEventDispatchStrategy, EventQueueDispatchStrategy>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategyCore());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_WhenEventQueueAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddSingleton<EventQueue>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategyCore());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_WhenExceptionHandlerAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddSingleton<IEventQueueExceptionHandler, DefaultEventQueueExceptionHandler>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategyCore());
    }

    [Test]
    public void AddMediateEventQueueDispatchStrategyCore_WhenEventDispatcherServiceAlreadyRegistered_Throws()
    {
        ServiceCollection services = CreateServices();
        services.AddHostedService<EventDispatcherService>();

        Assert.Throws<InvalidOperationException>(() => services.AddMediateEventQueueDispatchStrategyCore());
    }
}

// File: MediateEventQueueBuilderTests.cs
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
using Mediate.BackgroundEventDispatch.Configuration;
using Mediate.BackgroundEventDispatch.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.BackgroundEventDispatch.Test;

[TestFixture]
public sealed class MediateEventQueueBuilderTests
{
    private static IMediateEventQueueBuilder CreateBuilder(IServiceCollection? services = null)
    {
        IServiceCollection svc = services ?? new ServiceCollection();
        return svc.AddMediateEventQueueDispatchStrategyCore();
    }

    [Test]
    public void AddDefaultExceptionHandler_RegistersDefaultHandler()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateEventQueueBuilder builder = CreateBuilder(services);

        builder.AddDefaultExceptionHandler();

        Assert.That(services.Any(s =>
            s.ServiceType == typeof(IEventQueueExceptionHandler) &&
            s.ImplementationType == typeof(DefaultEventQueueExceptionHandler)), Is.True);
    }

    [Test]
    public void AddDefaultExceptionHandler_ReturnsBuilderInstance()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateEventQueueBuilder builder = CreateBuilder(services);

        IMediateEventQueueBuilder result = builder.AddDefaultExceptionHandler();

        Assert.That(result, Is.SameAs(builder));
    }

    [Test]
    public void AddDefaultExceptionHandler_WhenCalledTwice_Throws()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateEventQueueBuilder builder = CreateBuilder(services);
        builder.AddDefaultExceptionHandler();

        Assert.Throws<InvalidOperationException>(() => builder.AddDefaultExceptionHandler());
    }

    [Test]
    public void AddCustomExceptionHandler_RegistersCustomHandler()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateEventQueueBuilder builder = CreateBuilder(services);

        builder.AddCustomExceptionHandler<CustomExceptionHandler>();

        Assert.That(services.Any(s =>
            s.ServiceType == typeof(IEventQueueExceptionHandler) &&
            s.ImplementationType == typeof(CustomExceptionHandler)), Is.True);
    }

    [Test]
    public void AddCustomExceptionHandler_ReturnsBuilderInstance()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateEventQueueBuilder builder = CreateBuilder(services);

        IMediateEventQueueBuilder result = builder.AddCustomExceptionHandler<CustomExceptionHandler>();

        Assert.That(result, Is.SameAs(builder));
    }

    [Test]
    public void AddCustomExceptionHandler_RegisteredAsSingleton()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateEventQueueBuilder builder = CreateBuilder(services);

        builder.AddCustomExceptionHandler<CustomExceptionHandler>();

        ServiceDescriptor descriptor = services.First(s => s.ServiceType == typeof(IEventQueueExceptionHandler));
        Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Singleton));
    }

    [Test]
    public void AddDefaultExceptionHandler_WhenCustomHandlerAlreadyRegistered_Throws()
    {
        ServiceCollection services = new ServiceCollection();
        IMediateEventQueueBuilder builder = CreateBuilder(services);
        builder.AddCustomExceptionHandler<CustomExceptionHandler>();

        Assert.Throws<InvalidOperationException>(() => builder.AddDefaultExceptionHandler());
    }

    private sealed class CustomExceptionHandler : IEventQueueExceptionHandler
    {
        public Task Handle(AggregateException aggregateException, string eventName)
            => Task.CompletedTask;
    }
}

// File: ServiceProviderMiddlewareProviderTests.cs
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

namespace Mediate.Test;

[TestFixture]
public sealed class ServiceProviderMiddlewareProviderTests
{
    private static ServiceProviderMiddlewareProvider BuildProvider(Action<IServiceCollection> configure)
    {
        ServiceCollection services = new ServiceCollection();
        configure(services);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return new ServiceProviderMiddlewareProvider(serviceProvider);
    }

    [Test]
    public async Task GetMiddlewares_Event_WithRegisteredMiddlewares_ReturnsAll()
    {
        ServiceProviderMiddlewareProvider provider = BuildProvider(s =>
        {
            s.AddTransient<IEventMiddleware<TestEvent>, NoOpEventMiddlewareA>();
            s.AddTransient<IEventMiddleware<TestEvent>, NoOpEventMiddlewareB>();
        });

        IEnumerable<IEventMiddleware<TestEvent>> middlewares = await provider.GetMiddlewares<TestEvent>();

        Assert.That(middlewares.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetMiddlewares_Event_WithNone_ReturnsEmpty()
    {
        ServiceProviderMiddlewareProvider provider = BuildProvider(_ => { });

        IEnumerable<IEventMiddleware<TestEvent>> middlewares = await provider.GetMiddlewares<TestEvent>();

        Assert.That(middlewares, Is.Empty);
    }

    [Test]
    public async Task GetMiddlewares_Query_WithRegisteredMiddlewares_ReturnsAll()
    {
        ServiceProviderMiddlewareProvider provider = BuildProvider(s =>
        {
            s.AddTransient<IQueryMiddleware<TestQuery, string>, NoOpQueryMiddlewareA>();
            s.AddTransient<IQueryMiddleware<TestQuery, string>, NoOpQueryMiddlewareB>();
        });

        IEnumerable<IQueryMiddleware<TestQuery, string>> middlewares =
            await provider.GetMiddlewares<TestQuery, string>();

        Assert.That(middlewares.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetMiddlewares_Query_WithNone_ReturnsEmpty()
    {
        ServiceProviderMiddlewareProvider provider = BuildProvider(_ => { });

        IEnumerable<IQueryMiddleware<TestQuery, string>> middlewares =
            await provider.GetMiddlewares<TestQuery, string>();

        Assert.That(middlewares, Is.Empty);
    }

    [Test]
    public void Dispose_DoesNotThrow()
    {
        ServiceProviderMiddlewareProvider provider = BuildProvider(_ => { });

        Assert.DoesNotThrow(() => provider.Dispose());
    }

    private sealed class NoOpEventMiddlewareA : IEventMiddleware<TestEvent>
    {
        public Task Invoke(TestEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
            => next();
    }

    private sealed class NoOpEventMiddlewareB : IEventMiddleware<TestEvent>
    {
        public Task Invoke(TestEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
            => next();
    }

    private sealed class NoOpQueryMiddlewareA : IQueryMiddleware<TestQuery, string>
    {
        public Task<string> Invoke(TestQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<string> next)
            => next();
    }

    private sealed class NoOpQueryMiddlewareB : IQueryMiddleware<TestQuery, string>
    {
        public Task<string> Invoke(TestQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<string> next)
            => next();
    }
}

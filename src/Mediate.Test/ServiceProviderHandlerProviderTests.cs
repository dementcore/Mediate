// File: ServiceProviderHandlerProviderTests.cs
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
public sealed class ServiceProviderHandlerProviderTests
{
    private static ServiceProviderHandlerProvider BuildProvider(Action<IServiceCollection> configure)
    {
        ServiceCollection services = new ServiceCollection();
        configure(services);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        return new ServiceProviderHandlerProvider(serviceProvider);
    }

    [Test]
    public async Task GetHandlers_WithRegisteredHandlers_ReturnsAll()
    {
        ServiceProviderHandlerProvider provider = BuildProvider(s =>
        {
            s.AddTransient<IEventHandler<TestEvent>, RecordingEventHandlerA>();
            s.AddTransient<IEventHandler<TestEvent>, RecordingEventHandlerB>();
        });

        IEnumerable<IEventHandler<TestEvent>> handlers = await provider.GetHandlers<TestEvent>();

        Assert.That(handlers.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetHandlers_WithNoRegisteredHandlers_ReturnsEmpty()
    {
        ServiceProviderHandlerProvider provider = BuildProvider(_ => { });

        IEnumerable<IEventHandler<TestEvent>> handlers = await provider.GetHandlers<TestEvent>();

        Assert.That(handlers, Is.Empty);
    }

    [Test]
    public async Task GetHandler_WithRegisteredHandler_ReturnsIt()
    {
        ServiceProviderHandlerProvider provider = BuildProvider(s =>
        {
            s.AddTransient<IQueryHandler<TestQuery, string>, TestQueryHandler>();
        });

        IQueryHandler<TestQuery, string> handler = await provider.GetHandler<TestQuery, string>();

        Assert.That(handler, Is.Not.Null);
        Assert.That(handler, Is.InstanceOf<TestQueryHandler>());
    }

    [Test]
    public async Task GetHandler_WithNoRegisteredHandler_ReturnsNull()
    {
        ServiceProviderHandlerProvider provider = BuildProvider(_ => { });

        IQueryHandler<TestQuery, string> handler = await provider.GetHandler<TestQuery, string>();

        Assert.That(handler, Is.Null);
    }

    [Test]
    public void Dispose_DoesNotThrow()
    {
        ServiceProviderHandlerProvider provider = BuildProvider(_ => { });

        Assert.DoesNotThrow(() => provider.Dispose());
    }

    private sealed class RecordingEventHandlerA : IEventHandler<TestEvent>
    {
        public Task Handle(TestEvent @event, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class RecordingEventHandlerB : IEventHandler<TestEvent>
    {
        public Task Handle(TestEvent @event, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

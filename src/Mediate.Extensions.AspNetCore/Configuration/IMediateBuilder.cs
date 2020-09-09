using System;
using System.Collections.Generic;
using System.Text;
using Mediate.Core.Abstractions;
using Mediate.Core;

namespace Mediate.Extensions.AspNetCore
{
    public interface IMediateBuilder
    {
        IMediateBuilder AddDefaultMediator();
        
        IMediateBuilder AddCustomMediator<TMediator>()
            where TMediator:IMediator;

        IMediateBuilder AddSequentialEventDispatchStrategy();

        IMediateBuilder AddParallelEventDispatchStrategy();

        IMediateBuilder AddQueuedEventDispatchStrategy();

        IMediateBuilder AddCustomDispatchStrategy<TDispatchStrategy>()
            where TDispatchStrategy : IEventDispatchStrategy;

        IMediateBuilder AddDefaultHandlerProvider();

        IMediateBuilder AddCustomEventHandlerProvider<TEventHandlerProvider>()
            where TEventHandlerProvider : IEventHandlerProvider;

        IMediateBuilder AddCustomMessageHandlerProvider<TMessageHandlerProvider>()
            where TMessageHandlerProvider : IMessageHandlerProvider;
    }
}

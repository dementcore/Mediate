using System;
using System.Collections.Generic;
using System.Text;
using Mediate.Core.Abstractions;
using Mediate.Core;

namespace Mediate.Extensions.AspNetCore
{
    public interface IMediateBuilder
    {
        /// <summary>
        /// Registers the default mediator implementation
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddDefaultMediator();
        
        /// <summary>
        /// Registers a custom mediator implementation
        /// </summary>
        /// <typeparam name="TMediator">Mediator implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomMediator<TMediator>()
            where TMediator:IMediator;

        /// <summary>
        /// Registers the SequentialEventDispatchStrategy that executes event handlers after one another
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddSequentialEventDispatchStrategy();

        /// <summary>
        /// Registers the ParallelEventDispatchStrategy that executes events handlers in parallel
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddParallelEventDispatchStrategy();

        /// <summary>
        /// Registers the ParallelEventDispatchStrategy that enqueues events to be handled by a background job.
        /// </summary>
        /// <returns></returns>

        IMediateBuilder AddEventQueueDispatchStrategy();

        /// <summary>
        /// Registers a custom event dispatch strategy implementation
        /// </summary>
        /// <typeparam name="TDispatchStrategy">Event dispatch strategy implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomDispatchStrategy<TDispatchStrategy>()
            where TDispatchStrategy : IEventDispatchStrategy;

        /// <summary>
        /// Registers an event and message handler provider where the handlers are retrieved from AspNetCore Service Provider.
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddServiceProviderHandlerProvider();

        /// <summary>
        /// Registers a custom event handler provider
        /// </summary>
        /// <typeparam name="TEventHandlerProvider">Event handler provider implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomEventHandlerProvider<TEventHandlerProvider>()
            where TEventHandlerProvider : IEventHandlerProvider;

        /// <summary>
        /// Registers a custom message handler provider
        /// </summary>
        /// <typeparam name="TMessageHandlerProvider">Message handler provider implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomMessageHandlerProvider<TMessageHandlerProvider>()
            where TMessageHandlerProvider : IMessageHandlerProvider;
    }
}

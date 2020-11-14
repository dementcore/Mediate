using System;
using System.Collections.Generic;
using System.Text;
using Mediate.Core.Abstractions;
using Mediate.Core;

namespace Mediate.AspNetCore
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
            where TMediator : IMediator;

        /// <summary>
        /// Registers an event and query handler provider where the handlers are retrieved from AspNetCore Service Provider.
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddServiceProviderHandlerProvider();

        /// <summary>
        /// Registers an event and query middleware provider where the middlewares are retrieved from AspNetCore Service Provider.
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddServiceProviderMiddlewareProvider();

        /// <summary>
        /// Registers a custom handler provider
        /// </summary>
        /// <typeparam name="THandlerProvider">Handler provider implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomHandlerProvider<THandlerProvider>()
                    where THandlerProvider : IHandlerProvider;

        /// <summary>
        /// Registers a custom middleware provider
        /// </summary>
        /// <typeparam name="TMiddlewareProvider">Middleware provider implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomMiddlewareProvider<TMiddlewareProvider>()
                    where TMiddlewareProvider : IMiddlewareProvider;
    }
}

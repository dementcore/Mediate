using Mediate.Abstractions;

namespace Mediate.Configuration
{
    /// <summary>
    /// Helper methods to configure Mediate
    /// </summary>
    public interface IMediateBuilder
    {
        /// <summary>
        /// Registers an handler provider for queries and events that are retrieved from AspNetCore Service Provider.
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddServiceProviderHandlerProvider();

        /// <summary>
        /// Registers an middleware provider for queries and events that are retrieved from AspNetCore Service Provider.
        /// </summary>
        /// <returns></returns>
        IMediateBuilder AddServiceProviderMiddlewareProvider();

        /// <summary>
        /// Registers a custom handler provider for queries and events
        /// </summary>
        /// <typeparam name="THandlerProvider">Handler provider implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomHandlerProvider<THandlerProvider>()
                    where THandlerProvider : IHandlerProvider;

        /// <summary>
        /// Registers a custom middleware provider for queries and events
        /// </summary>
        /// <typeparam name="TMiddlewareProvider">Middleware provider implementation type</typeparam>
        /// <returns></returns>
        IMediateBuilder AddCustomMiddlewareProvider<TMiddlewareProvider>()
                    where TMiddlewareProvider : IMiddlewareProvider;
    }
}

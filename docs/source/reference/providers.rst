.. _refProviders:

#########
Providers
#########

In Mediate a provider is the responsible for provide the message handlers and the middlewares.

Default Providers
=================

Mediate includes the following providers out-of-the-box.

.. _refHandlerProviders:

Service Provider Handler Provider
---------------------------------

Provides the events and queries handlers from the DI Container.

.. note:: 
 The handlers are retrieved in the same registration order.

.. _refMiddlewareProviders:

Service Provider Middleware Provider
------------------------------------

Provides the events and queries middlewares from the DI Container.

.. note:: 
 The middlewares are retrieved in the same registration order.

.. _refCustomHandlerProviders:

Custom Handler Provider
=======================

You can create a custom provider for the handlers to provide handlers in any form you want.
For this purpose you have to implement the ``IHandlerProvider`` interface with your custom logic.

.. sourcecode:: csharp

 /// <summary>
 /// Defines a provider that encapsulates event and query handlers provider
 /// </summary>
 public interface IHandlerProvider : IEventHandlerProvider, IQueryHandlerProvider
 {
 }

The above interface is segregated in ``IEventHandlerProvider`` and ``IQueryHandlerProvider`` for more flexibility.

.. sourcecode:: csharp

 /// <summary>
 /// Interface for implement an event handler provider
 /// </summary>
 public interface IEventHandlerProvider
 {
        /// <summary>
        /// Gets all event handlers from an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <returns>All registered handlers for that event</returns>
        Task<IEnumerable<IEventHandler<TEvent>>> GetHandlers<TEvent>() where TEvent : IEvent;
 }

 /// <summary>
 /// Interface for implement a query handler provider
 /// </summary>
 public interface IQueryHandlerProvider
 {
        /// <summary>
        /// Gets a query handler from a concrete query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <returns>Registered handler for that query</returns>
        Task<IQueryHandler<TQuery, TResult>> GetHandler<TQuery, TResult>() where TQuery : IQuery<TResult>;
 }

To register your custom implementation you can use the ``AddCustomHandlerProvider`` advanced configuration method.
See :ref:`Advanced configuration <refAdvancedConfiguration>`

.. _refCustomMiddlewareProviders:

Custom Middleware Provider
==========================

You can create a custom provider for the middlewares to provide middlewares in any form you want.
For this purpose you have to implement the ``IMiddlewareProvider`` interface with your custom logic.

.. sourcecode:: csharp

 /// <summary>
 /// Defines a provider that encapsulates event and query middlewares provider
 /// </summary>
 public interface IMiddlewareProvider : IEventMiddlewareProvider, IQueryMiddlewareProvider
 {

 }

The above interface is segregated in ``IEventMiddlewareProvider`` and ``IQueryMiddlewareProvider`` for more flexibility.

.. sourcecode:: csharp

 /// <summary>
 /// Interface for implement an event middleware provider
 /// </summary>
 public interface IEventMiddlewareProvider
 {
        /// <summary>
        /// Gets all event middlewares from an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <returns>All registered middlewares for that event</returns>
        Task<IEnumerable<IEventMiddleware<TEvent>>> GetMiddlewares<TEvent>() where TEvent : IEvent;
 }

 /// <summary>
 /// Interface for implement a query middleware provider
 /// </summary>
 public interface IQueryMiddlewareProvider
 {
        /// <summary>
        /// Gets all query middlewares from a query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <typeparam name="TResult">Query response type</typeparam>
        /// <returns>All registered middlewares for that query</returns>
        Task<IEnumerable<IQueryMiddleware<TQuery, TResult>>> GetMiddlewares<TQuery, TResult>() where TQuery : IQuery<TResult>;
 }

To register your custom implementation you can use the ``AddCustomMiddlewareProvider`` advanced configuration method.
See :ref:`Advanced configuration <refAdvancedConfiguration>`
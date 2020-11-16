Middlewares
===========

In Mediate a Middleware is a piece of logic that is executed between the Mediator
and the message handler. It has been designed to be very similar to Asp.Net Core middlewares.
You can use middlewares to create any pipeline that you need for your querys and events.

.. image:: /images/mediate_middleware_flow.png
   :align: center

Each middleware can do the following things:

- Choose to invoke the next element in the pipeline.
- Do logic before and after calling the next element in the pipeline.

Query Middleware
^^^^^^^^^^^^^^^^

A query middleware allows you to create a specific pipeline for a query.

Query middleware interface
--------------------------
Is defined by the ``IQueryMiddleware`` generic interface.

.. sourcecode:: csharp

 /// <summary>
 /// Interface to implement a middleware to process a query before it reaches it's handler.
 /// <typeparam name="TQuery">Query type</typeparam>
 /// <typeparam name="TResult">Query response type</typeparam>
 /// </summary>
 public interface IQueryMiddleware<in TQuery, TResult> where TQuery : IQuery<TResult>
 {
        /// <summary>
        /// Invoke the middleware logic
        /// </summary>
        /// <param name="query">Query object</param>
        /// <param name="cancellationToken"></param>
        /// <param name="next">Delegate that encapsulates a call to the next element in the pipeline</param>
        /// <returns></returns>
        Task<TResult> Invoke(TQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<TResult> next);
 }

.. note:: The ``NextMiddlewareDelegate<TResult> next`` parameter is a delegate 
 that encapsulates the call to the next element in the pipeline.

Query middleware pipeline
-------------------------
The following diagram demonstrates the concept to understand how middleware pipelines works.

.. image:: /images/query_middleware_flow.png
   :align: center

.. important:: You have to invoke ``next`` to execute the next element in the pipeline. 
 You can short circuit the pipeline simply don't invoking ``next``.


Event Middleware
^^^^^^^^^^^^^^^^

An event middleware allows you to create a specific pipeline for an event.

Event middleware interface
--------------------------
Is defined by the ``IEventMiddleware`` generic interface.

.. sourcecode:: csharp

    /// <summary>
    /// Interface to implement a middleware to process an event before it reaches it's handlers.
    /// <typeparamref name="TEvent">Event type</typeparam>
    /// </summary>
    public interface IEventMiddleware<in TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Invoke the middleware logic
        /// </summary>
        /// <param name="event">Event object</param>
        /// <param name="cancellationToken"></param>
        /// <param name="next">Delegate that encapsulates a call to the next element in the pipeline</param>
        /// <returns></returns>
        Task Invoke(TEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next);
    }

.. note:: The ``NextMiddlewareDelegate next`` parameter is a delegate 
 that encapsulates the call to the next element in the pipeline.

Event middleware pipeline
-------------------------
The following diagram demonstrates the concept to understand how middleware pipelines works.

.. image:: /images/event_middleware_flow.png
   :align: center

.. important:: You have to invoke ``next`` to execute the next element in the pipeline. 
 You can short circuit the pipeline simply don't invoking ``next``.
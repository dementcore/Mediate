Event Dispatch Strategy
=======================

An event dispatch strategy defines how the event handlers are invoked.

Event dispatch strategy interface
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Is defined by the ``IEventDispatchStrategy`` interface.

.. sourcecode:: csharp

    /// <summary>
    /// Interface for implement an event dispatch strategy
    /// </summary>
    public interface IEventDispatchStrategy
    {
        /// <summary>
        /// Executes this strategy to dispatch an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="handlers">Event handlers</param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers) where TEvent : IEvent;

        /// <summary>
        /// Executes this strategy to dispatch an event
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event data</param>
        /// <param name="handlers">Event handlers</param>
        /// <returns></returns>
        Task Dispatch<TEvent>(TEvent @event, IEnumerable<IEventHandler<TEvent>> handlers, CancellationToken cancellationToken) where TEvent : IEvent;
    }

Included event dispatch strategies
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Mediate have 3 event dispatch strategies available out-of-the-box.

SequentialEventDispatchStrategy
-------------------------------
This strategy executes event handlers after one another, sequentially.

.. important:: In case of exception in one event handler the rest of the handlers will be executed and then an AggregateException will be thrown when all handlers finish.

ParallelEventDispatchStrategy
-----------------------------
This strategy executes event handlers in parallel.

.. important:: In case of exception in one event handler the rest of the handlers will be executed and then an AggregateException will be thrown when all handlers finish.

EventQueueDispatchStrategy
--------------------------
This strategy enqueues event handlers to be executed by an Asp.Net Core hosted service. This strategy is intented to fire and forget an event.

.. important:: In case of exception in one event handler the rest of the handlers will be executed and then an AggregateException will be logged in the registered ILogger service.

.. tip:: As good practices, try to control the exceptions in the event handler.
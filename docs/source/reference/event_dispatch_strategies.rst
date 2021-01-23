#########################
Event Dispatch Strategies
#########################

An event dispatch strategy defines how the event handlers are invoked.

Default event dispatch strategies
=================================
Mediate has two event dispatch strategies available out-of-the-box.

.. note:: 
 The event handlers are invoked in the same registration order.

.. _refSequentialEventDispatchStrategy:

Sequential Event Dispatch Strategy
----------------------------------
This strategy executes event handlers after one another, sequentially.

.. important:: In case of exception in one event handler the rest of the handlers will be executed and then an AggregateException will be thrown when all handlers finish.

.. _refParallelEventDispatchStrategy:

Parallel Event Dispatch Strategy
--------------------------------
This strategy executes event handlers in parallel.

.. important:: In case of exception in one event handler the rest of the handlers will be executed and then an AggregateException will be thrown when all handlers finish.

.. _refCustomEventDispatchStrategy:

Custom event dispatch strategy 
==============================

You can execute the event handlers in any form that you want. 
For this purpose you have to implement the ``IEventDispatchStrategy`` interface with your custom logic.

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

To register your custom implementation you can use the ``AddMediateCustomDispatchStrategy`` extension method.
See :ref:`Configuration <refAddCustomEventDispatchStrategy>`
.. _refEvents:

#############
Events usage
#############

To send events, is the same that you have already seen in the previous page.

Event creation
==============

First, to create an event you have to implement the ``IEvent`` interface.

For example:

.. sourcecode:: csharp
 
 public class MyEvent:IEvent
 {
    public string EventData { get; set; }
 }

.. note:: In this class you can put any info that you need for your event.

Handler creation
================

Second, you have to create a handler for the above event. 
For this, you have to implement the ``IEventHandler<TEvent>`` generic interface.

.. sourcecode:: csharp

    /// <summary>
    /// Interface for implement an event handler for an event
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        /// <summary>
        /// Handle the event
        /// </summary>
        /// <param name="event">Event data</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Handle(TEvent @event, CancellationToken cancellationToken);
    }

For example:

.. sourcecode:: csharp
 
 public class MyEventHandler : IEventHandler<MyEvent>
 {
     public Task Handle(MyEvent @event, CancellationToken cancellationToken)
     {
         //Example operation
         Console.WriteLine("MyEvent Event handler " + @event.EventData );
 
         return Task.CompletedTask;
     }
 }

.. tip:: 
 Remember, you can have multiple handlers for an event.

Dispatching through the mediator
================================

Third, dispatch the event through the mediator:

.. sourcecode:: csharp

 MyEvent @event = new MyEvent() { EventData = "SomeData" };

 await _mediator.Dispatch(@event, cancellationToken);
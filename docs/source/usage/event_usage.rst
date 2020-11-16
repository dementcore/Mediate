Events usage
============


Defining an event
^^^^^^^^^^^^^^^^^
To create an event we have to create a class that implements the ``IEvent`` interface.

.. sourcecode:: csharp
 
 public class MyEvent:IEvent
 {
    public string TestData { get; set; }
 }

.. note:: In this class you can put any info that you need for your event.

Defining an event handler
^^^^^^^^^^^^^^^^^^^^^^^^^

An event handler is defined by the ``IEventHandler`` generic interface.

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

So, to create a handler for the above event we should create a class that implements ``IEventHandler``:


.. sourcecode:: csharp
 
 public class MyEventHandler : IEventHandler<MyEvent>
 {
     public Task Handle(MyEvent @event, CancellationToken cancellationToken)
     {
         //Example operation
         Console.WriteLine("MyEvent Event handler " + @event.TestData );
 
         return Task.CompletedTask;
     }
 }
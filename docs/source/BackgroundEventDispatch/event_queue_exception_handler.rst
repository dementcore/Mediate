.. _refEventQueueExceptionHandler:

#############################
Event Queue Exception Handler
#############################

A Event Queue Exception Handler is a piece of logic that handles the errors produced 
by event handlers when are executed in background by the event queue. You can use it to handle the exceptions in any form you want.

.. important:: In case of exception in one event handler the rest of the handlers will be executed and then an AggregateException will be generated.

.. tip:: As good practices, try to control the exceptions in the event handler.

Default Event Queue Exception Handler
=====================================

This exception handler logs the errors using the ILogger based logging system. 

.. sourcecode:: csharp

 /// <summary>
 /// Default event dispatch exception handler that logs the exceptions
 /// </summary>
 public sealed class DefaultEventQueueExceptionHandler : IEventQueueExceptionHandler
 {
     private readonly ILogger<DefaultEventQueueExceptionHandler> _logger;

     /// <summary>
     /// Constructor
     /// </summary>
     /// <param name="logger"></param>
     public DefaultEventQueueExceptionHandler(ILogger<DefaultEventQueueExceptionHandler> logger)
     {
          _logger = logger;
     }

     /// <summary>
     /// Handles event dispatch exception
     /// </summary>
     /// <param name="aggregateException">Aggregate exception with all handlers errors</param>
     /// <param name="eventName">Name of the event</param>
     /// <returns></returns>
     public Task Handle(AggregateException aggregateException, string eventName)
     {
          _logger.LogError(aggregateException, $"Errors occurred executing event {eventName}");

          return Task.CompletedTask;
     }
 }

Custom Event Queue Exception Handler
====================================

To create a Custom Event Queue Exception Handler you have to implement the ``IEventQueueExceptionHandler`` interface and 
then register it using ``AddCustomExceptionHandler`` method. See :ref:`Advanced configuration <refQueueAdvancedConfiguration>` for details.

.. sourcecode:: csharp

 /// <summary>
 /// Interface for implement an exception handler for the event queue dispatch strategy
 /// </summary>
 public interface IEventQueueExceptionHandler
 {
      /// <summary>
      /// Handles event queue dispatch strategy
      /// </summary>
      /// <param name="aggregateException">Aggregate exception with all handlers errors</param>
      /// <param name="eventName">Name of the event</param>
      /// <returns></returns>
      Task Handle(AggregateException aggregateException, string eventName);
 }
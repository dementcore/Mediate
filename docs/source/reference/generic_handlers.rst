######################
Generic event handlers
######################

You can create handlers that are invoked for all events. 

To do that you have to create an open generic class implementing the ``IEventHandler<TEvent>`` interface. 
See :ref:`Events usage <refEvents>` for details.

For example this event handler will be invoked for all events:

.. sourcecode:: csharp

    /// <summary>
    /// This class catchs all events
    /// </summary>
    public class GenericEventHandler<T> : IEventHandler<T> where T : IEvent
    {
        private readonly ILogger<GenericEventHandler<T>> _logger;
        public GenericEventHandler(ILogger<GenericEventHandler<T>> logger)
        {
            _logger = logger;
        }

        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received event: ", @event);

            return Task.CompletedTask;
        }
    }

Another example. Imagine that you have an abstract class called ``BaseEvent`` for some events in your app, 
and you want to create a handler that is only invoked for all events that are derived from BaseEvent.

This will do the trick:

.. sourcecode:: csharp

 public abstract class BaseEvent : IEvent
 {
    public Guid EventId { get; }

    public BaseEvent()
    {
            EventId = Guid.NewGuid();
    }
 }

    /// <summary>
    /// This class catchs all BaseEvent derived events
    /// </summary>
    public class BaseEventGenericHandler<T> : IEventHandler<T> where T : BaseEvent
    {
        private readonly ILogger<BaseEventGenericHandler<T>> _logger;
        public BaseEventGenericHandler(ILogger<BaseEventGenericHandler<T>> logger)
        {
            _logger = logger;
        }

        public Task Handle(T @event, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Received base event derived event: ", @event);

            return Task.CompletedTask;
        }
    }

.. important:: 
 This depends on the DI container support for generic variance.
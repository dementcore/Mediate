###################
Generic middlewares
###################

You can create middlewares that are invoked for all events or queries. 
To do that you have to create an open generic class implementing the middleware interfaces. 
See :ref:`Middlewares <refMiddlewares>` for details about middlewares.

For example this middleware will be invoked for all events:

.. sourcecode:: csharp

 public class EventGenericMiddleware<TEvent> : IEventMiddleware<TEvent> where TEvent : IEvent
 {
    public async Task Invoke(TEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
    {

            //example validation
            if (@event == null)
            {
                //example exception for this example
                throw new InvalidOperationException("The event must be not null");
            }

            await next();
    }
 }

Another example. Imagine that you have an abstract class called ``BaseEvent`` for some events in your app, 
and you want to create a middleware that is only invoked for all events that are derived from BaseEvent.

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

 //our middleware for all BaseEvent derived events
 public class BaseEventGenericMiddleware<TEvent> : IEventMiddleware<TEvent> where TEvent : BaseEvent
 {
    public async Task Invoke(TEvent @event, CancellationToken cancellationToken, NextMiddlewareDelegate next)
    {

        //example validation
        if (@event.EventId == Guid.Empty)
        {
                //example exception for this example
                throw new InvalidOperationException("The event id must be not null");
        }

        await next();
    }
 }

.. important:: 
 This depends on the DI container support for generic variance.

.. note::
 For queries is the same concept. 

.. sourcecode:: csharp

    public abstract class BaseQuery<TResult>:IQuery<TResult>
    {
            public Guid QueryId { get; }

            public BaseQuery()
            {
                QueryId = Guid.NewGuid();
            }
    }

    //middleware for all BaseQuery derived queries
    public class BaseQueryGenericMiddleware<TQuery, TResult> : IQueryMiddleware<TQuery, TResult> where TQuery : BaseQuery<TResult>
    {
            public async Task<TResult> Invoke(TQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<TResult> next)
            {
                //example validation
                if (query.QueryId != Guid.Empty)
                {
                    return await next();
                }

                //example exception for this example
                throw new InvalidOperationException("The query id must be not null");
            }
    }
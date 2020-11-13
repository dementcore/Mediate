# Mediate
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/1d4f09d9989e4fb788dfe05af01e8fbb)](https://app.codacy.com/manual/dementcore/Mediate?utm_source=github.com&utm_medium=referral&utm_content=dementcore/Mediate&utm_campaign=Badge_Grade_Settings)
![.NET Core](https://github.com/dementcore/Mediate/workflows/.NET%20Core/badge.svg?branch=master)
[![Documentation Status](https://readthedocs.org/projects/mediate/badge/?version=latest)](https://mediate.readthedocs.io/en/latest/?badge=latest)

![Mediate](logo.png)

Mediate is another simple and little in-process messaging and event dispatching system based in mediator pattern.

## What attempts to provide this project?

This project is mostly developed for learn and fun, but also attempts 
to provide an easy communication mechanism to develop decoupled communication between code layers in ASP.NET Core applications.


## [Documentation](https://mediate.readthedocs.io/en/docs/)

## Basic usage
### Querys

Querys in Mediate are just request->response. 
A specific query can only have one handler.

```csharp
public interface IQuery<out TResult>
{

}

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}
````

To create a query we have to implement the `` IQuery<out TResult> `` interface.

Example:

```csharp
public class TestQuery:IQuery<TestQueryReply>
{
    public string Data { get; set; }
}

public class TestQueryReply{
    public string Reply { get; set; }
}
```

To create a handler to process the above message 
we have to implement the `` IQueryHandler<in TMessage, TResult> `` interface.

Example:
```csharp
public class TestQueryHandler : IQueryHandler<TestQuery, TestQueryReply>
{
    public Task<TestQueryReply> Handle(TestQuery query, CancellationToken cancellationToken)
    {
        //Example operation
        return Task.FromResult(new TestQueryReply()
        {
            Reply = "Reply from test query handler with requested data: "
            + query.Data + " " + Guid.NewGuid()
        });
    }
}
```

### Events
Events in Mediate are just requests without response. E.g. to inform a module that a customer has been created.
A specific event can have multiple handlers.

```csharp
public interface IEvent
{

}

public interface IEventHandler<in TEvent> where TEvent : IEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}
````

To create an event we have to implement the `` IEvent `` interface.

Example:

```csharp
public class OnHomeInvoked:IEvent
{
    public string TestData { get; set; }
}
```
To create a handler to handle the above event 
we have to implement the `` IEventHandler<in TEvent> `` interface.

Example:
```csharp
public class OnHomeInvokedEventHandler : IEventHandler<OnHomeInvoked>
{
    public Task Handle(OnHomeInvoked @event, CancellationToken cancellationToken)
    {
        Console.WriteLine($"OnHomeInvoked Event handler {@event.TestData}");

        return Task.CompletedTask;
    }
}
```
### Dispatching
We can dispatch messages and events via the Mediator default implementation or implementing the
``IMediator`` interface.

Message dispatching example:

```csharp
TestMsg test = new TestMsg() { Data = "Test Data" };

TestMsgReply res = await _mediator.Send<TestMsg,TestMsgReply>(test, cancellationToken);
```

Event dispatching example:

```csharp
OnHomeInvoked @event = new OnHomeInvoked() { TestData = Activity.Current.Id };

await _mediator.Dispatch(@event, cancellationToken);
```
### Event Dispatching Strategies
Event dispatching strategies controls how the event handlers are executed.
By default we have two strategies:
  - ParallelEventDispatchStrategy: Executes all handlers from a specific event in parallel.
  - SequentialEventDispatchStrategy: Executes all handlers from a specific event after one another.

## Extensions

In src/Mediate.Extensions.AspNetCore:
- We have one more event dispatch strategy:
  - QueueEventDispatchStrategy: Queues all handlers from a specific event to be executed in background by a AspNetCore HostedService in parallel.

- We have Service Collection extension methods to seamlessly configure Mediate.

## Samples

In samples folder are samples for AspNetCore with Microsoft DI and with Autofac DI.

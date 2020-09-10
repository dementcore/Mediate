# Mediate
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/1d4f09d9989e4fb788dfe05af01e8fbb)](https://app.codacy.com/manual/dementcore/Mediate?utm_source=github.com&utm_medium=referral&utm_content=dementcore/Mediate&utm_campaign=Badge_Grade_Settings)
![.NET Core](https://github.com/dementcore/Mediate/workflows/.NET%20Core/badge.svg?branch=master)

A simple and little in-process messaging and event dispatching system heavily inspired by mediator pattern. Intended, but not only, for AspNetCore 3.1.

This project attempts to provide
a communication mechanism to develop interlayer decoupled communication or modular webapplications 
with event handlers registration modified at runtime, 
e.g. multitenant modular appplications where each tenant have different modules loaded.

## Basic usage
### Messages

Messages in Mediate are just request->response. 
A specific message can only have one handler.

```csharp
public interface IMessage<out TResult>
{

}

public interface IMessageHandler<in TMessage, TResult>
    where TMessage : IMessage<TResult>
{
    Task<TResult> Handle(TMessage message, CancellationToken cancellationToken);
}
````

To create a message we have to implement the `` IMessage<out TResult> `` interface.

Example:

```csharp
public class TestMsg:IMessage<TestMsgReply>
{
    public string Data { get; set; }
}

public class TestMsgReply{
    public string Reply { get; set; }
}
```

To create a handler to process the above message 
we have to implement the `` IMessageHandler<in TMessage, TResult> `` interface.

Example:
```csharp
public class TestMsgHandler : IMessageHandler<TestMsg, TestMsgReply>
{
    public Task<TestMsgReply> Handle(TestMsg message, CancellationToken cancellationToken)
    {
        //Example operation
        return Task.FromResult(new TestMsgReply()
        {
            Reply = "Reply from test msg handler with requested data: "
            + message.Data + " " + Guid.NewGuid()
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

In src/Mediate.Extensions.AspNetCore we have one more event dispatch strategy:
  - QueueEventDispatchStrategy: Queues all handlers from a specific event to be executed in background by a AspNetCore HostedService in parallel.

In src/Mediate.Extensions.AspNetCore.Microsoft.DependencyInjection we have helper servicecollection extension methods to seamlessly configure Mediate.
## Samples

In samples folder are samples for AspNetCore with Microsoft DI and with Autofac DI.

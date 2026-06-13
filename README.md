<p align="center">
  <img src="logo.png" alt="Mediate" width="180" />
</p>

<h1 align="center">Mediate</h1>

<p align="center">
  Lightweight in-process mediator for .NET 10 — queries, events, middleware & pluggable dispatch strategies.
</p>

<p align="center">
  <a href="https://www.nuget.org/packages/Mediate"><img src="https://img.shields.io/nuget/v/Mediate?color=2345ba16&logo=nuget&style=flat" alt="NuGet" /></a>
  <a href="https://mediate.readthedocs.io/en/latest/?badge=latest"><img src="https://readthedocs.org/projects/mediate/badge/?version=latest" alt="Documentation Status" /></a>
</p>

---

## ✨ What is Mediate?

**Mediate** is a simple, zero-dependency in-process messaging library for .NET 10 based on the [Mediator pattern](https://refactoring.guru/design-patterns/mediator). It helps you build loosely coupled applications by routing **queries** (request/response) and **events** (fan-out notifications) through a central hub, with full middleware pipeline support and swappable dispatch strategies.

> Designed to be small, explicit, and easy to extend — no magic, no hidden conventions.

---

## 📦 Packages

| Package | Description | NuGet |
|---|---|---|
| `Mediate` | Core library — mediator, queries, events, middleware | [![NuGet](https://img.shields.io/nuget/v/Mediate?logo=nuget&style=flat)](https://www.nuget.org/packages/Mediate) |
| `Mediate.BackgroundEventDispatch` | ASP.NET Core extension — fire-and-forget background event dispatch | [![NuGet](https://img.shields.io/nuget/v/Mediate.BackgroundEventDispatch?logo=nuget&style=flat)](https://www.nuget.org/packages/Mediate.BackgroundEventDispatch) |

---

## 🚀 Quick Start

### 1. Install

```bash
dotnet add package Mediate
```

### 2. Register

```csharp
// Program.cs
builder.Services.AddMediate();

// Register your handlers and middlewares from an assembly
builder.Services.AddMediateClassesFromAssembly(typeof(Program).Assembly);

// Pick a dispatch strategy (default: sequential)
builder.Services.AddMediateSequentialEventDispatchStrategy();
```

### 3. Define a query

```csharp
public record GetUserQuery(int Id) : IQuery<UserDto>;

public class GetUserHandler : IQueryHandler<GetUserQuery, UserDto>
{
    public Task<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
        => Task.FromResult(new UserDto(query.Id, "Alice"));
}
```

### 4. Define an event

```csharp
public record UserCreatedEvent(int UserId) : IEvent;

public class SendWelcomeEmailHandler : IEventHandler<UserCreatedEvent>
{
    public Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken)
    {
        // send email...
        return Task.CompletedTask;
    }
}
```

### 5. Use the mediator

```csharp
public class UserService(IMediator mediator)
{
    public async Task<UserDto> GetUser(int id)
        => await mediator.Send(new GetUserQuery(id));

    public async Task CreateUser(int id)
    {
        // ... create user ...
        await mediator.Dispatch(new UserCreatedEvent(id));
    }
}
```

---

## 🧩 Features

### 🔁 Query / Handler (Request-Response)

Send a query and get exactly one response. Each query type maps to **exactly one handler**.

```csharp
TResult result = await mediator.Send(new MyQuery(...));
```

### 📢 Event / Handler (Fan-out)

Dispatch an event to **zero or more handlers** simultaneously.

```csharp
await mediator.Dispatch(new MyEvent(...));
```

### 🔗 Middleware Pipelines

Wrap query and event handling with middleware — great for logging, validation, tracing, and cross-cutting concerns. Middleware runs in **registration order**.

**Query middleware:**
```csharp
public class LoggingMiddleware<TQuery, TResult> : IQueryMiddleware<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<TResult> Execute(
        TQuery query,
        NextMiddlewareDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling {typeof(TQuery).Name}");
        TResult result = await next();
        Console.WriteLine($"Done handling {typeof(TQuery).Name}");
        return result;
    }
}
```

**Event middleware:**
```csharp
public class LoggingEventMiddleware<TEvent> : IEventMiddleware<TEvent>
    where TEvent : IEvent
{
    public async Task Execute(
        TEvent @event,
        NextMiddlewareDelegate next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Dispatching {@event.GetType().Name}");
        await next();
    }
}
```

### ⚡ Pluggable Dispatch Strategies

Control how event handlers are invoked by swapping the dispatch strategy:

| Strategy | Behavior |
|---|---|
| `SequentialEventDispatchStrategy` | Handlers run one after another; collects all exceptions into `AggregateException` |
| `ParallelEventDispatchStrategy` | Handlers run concurrently via `Parallel.ForEachAsync` |
| `EventQueueDispatchStrategy` | Fire-and-forget background queue (requires `Mediate.BackgroundEventDispatch`) |

```csharp
// Sequential (default)
services.AddMediateSequentialEventDispatchStrategy();

// Parallel
services.AddMediateParallelEventDispatchStrategy();

// Custom
services.AddMediateCustomDispatchStrategy<MyStrategy>(ServiceLifetime.Scoped);
```

### 🔧 Fluent Builder

For fine-grained control over provider registration:

```csharp
services.AddMediateCore()
    .AddServiceProviderHandlerProvider()
    .AddServiceProviderMiddlewareProvider();
```

You can plug in completely custom handler or middleware providers:

```csharp
services.AddMediateCore()
    .AddCustomHandlerProvider<MyHandlerProvider>()
    .AddCustomMiddlewareProvider<MyMiddlewareProvider>();
```

### 🔍 Assembly Scanning

Auto-register all handlers and middlewares from an assembly in one call:

```csharp
services.AddMediateClassesFromAssembly(typeof(Program).Assembly);
```

---

## 🌐 Background Event Dispatch

The `Mediate.BackgroundEventDispatch` package adds a fire-and-forget dispatch strategy backed by an in-memory queue and an ASP.NET Core `BackgroundService`.

```bash
dotnet add package Mediate.BackgroundEventDispatch
```

```csharp
builder.Services.AddMediate();
builder.Services.AddMediateEventQueueDispatchStrategy();
```

Events dispatched with `mediator.Dispatch(...)` are enqueued immediately and processed asynchronously in the background. The queue has a bounded capacity of **10 items** with semaphore-based backpressure.

**Custom exception handling:**

```csharp
builder.Services.AddMediateEventQueueDispatchStrategyCore()
    .AddCustomExceptionHandler<MyExceptionHandler>();
```

---

## 📖 Documentation

Full documentation, guides, and API reference at:

**[mediate.readthedocs.io](https://mediate.readthedocs.io/en/latest/)**

---

## 🏗️ Architecture Overview

```
IMediator
├── Send<TResult>(IQuery<TResult>)       → one handler + query middleware pipeline
└── Dispatch<TEvent>(TEvent)             → N handlers + event middleware pipeline
                                              └── via IEventDispatchStrategy
```

**Providers** resolve handlers and middlewares from the DI container:
- `IQueryHandlerProvider` / `IEventHandlerProvider`
- `IQueryMiddlewareProvider` / `IEventMiddlewareProvider`

Default implementations (`ServiceProviderHandlerProvider`, `ServiceProviderMiddlewareProvider`) delegate directly to `IServiceProvider`.

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! Feel free to open an issue or submit a pull request.

---

## 📄 License

[MIT](LICENSE) © DementCore
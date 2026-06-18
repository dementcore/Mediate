# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Mediate is a lightweight, in-process mediator library for .NET 10 enabling decoupled communication via query/handler and event/handler patterns with middleware support and pluggable dispatch strategies.

## Build & Development Commands

```powershell
# Restore dependencies
dotnet restore

# Build (debug)
dotnet build --no-restore

# Run tests
dotnet test --no-build --verbosity normal
```

## Solution Structure

```
src/
  Mediate/                              # Core NuGet package (net10.0)
  Mediate.BackgroundEventDispatch/      # ASP.NET Core extension package (net10.0)
  Mediate.Test/                         # Unit tests (net10.0, NUnit 4.x)
  Mediate.BackgroundEventDispatch.Test/ # Unit tests (net10.0, NUnit 4.x)
```

## Architecture

### Core Abstractions (`src/Mediate`)

- **`IMediator`** — central hub; exposes `Send<TResult>(IQuery<TResult>)` and `Dispatch(IEvent)`
- **`IQuery<TResult>` / `IQueryHandler<TQuery, TResult>`** — request-response pattern; exactly one handler per query type
- **`IEvent` / `IEventHandler<TEvent>`** — fan-out notification pattern; zero-to-many handlers
- **`IQueryMiddleware<TQuery, TResult>` / `IEventMiddleware<TEvent>`** — pipeline decorators; applied in registration order; use `NextMiddlewareDelegate<TResult>` / `NextMiddlewareDelegate` to invoke the next element
- **`IEventDispatchStrategy`** — swappable strategy controlling how event handlers are invoked:
  - `SequentialEventDispatchStrategy` (default) — handlers run one after another; collects exceptions into `AggregateException`
  - `ParallelEventDispatchStrategy` — handlers run concurrently via `Parallel.ForEachAsync`
- **Handler providers** — resolve handlers from DI:
  - `IEventHandlerProvider` / `IQueryHandlerProvider` — granular interfaces
  - `IHandlerProvider` — composite of both; default impl: `ServiceProviderHandlerProvider`
- **Middleware providers** — resolve middleware from DI:
  - `IEventMiddlewareProvider` / `IQueryMiddlewareProvider` — granular interfaces
  - `IMiddlewareProvider` — composite of both; default impl: `ServiceProviderMiddlewareProvider`

### Background Dispatch Extension (`src/Mediate.BackgroundEventDispatch`)

Adds `EventQueueDispatchStrategy`, which enqueues event handler invocations for fire-and-forget background processing. Key components:

- **`EventQueue`** — thread-safe `ConcurrentQueue` with semaphore-based backpressure (capacity: 10)
- **`EventDispatcherService`** — ASP.NET Core `BackgroundService` that polls the queue and processes events
- **`IEventQueueExceptionHandler`** — handles exceptions thrown during background dispatch; default impl (`DefaultEventQueueExceptionHandler`) logs via `ILogger`

### DI Registration

Both packages expose extension methods on `IServiceCollection` for registration.

**Core (`Mediate`) extension methods:**
- `AddMediate()` — registers mediator + default providers (scoped); convenience shorthand
- `AddMediateCore()` — registers mediator only; returns `IMediateBuilder` for fluent configuration
  - `.AddServiceProviderHandlerProvider()` / `.AddServiceProviderMiddlewareProvider()` — default DI-backed providers
  - `.AddCustomHandlerProvider<T>()` / `.AddCustomMiddlewareProvider<T>()` — plug in custom providers
- `AddMediateSequentialEventDispatchStrategy()` — registers `SequentialEventDispatchStrategy` (scoped)
- `AddMediateParallelEventDispatchStrategy()` — registers `ParallelEventDispatchStrategy` (scoped)
- `AddMediateCustomDispatchStrategy<T>(ServiceLifetime)` — registers any custom strategy
- `AddMediateClassesFromAssembly(Assembly)` — scans assembly and auto-registers all handlers and middlewares as transient (open generics included)

**BackgroundEventDispatch extension methods:**
- `AddMediateEventQueueDispatchStrategy()` — registers `EventQueueDispatchStrategy` + `DefaultEventQueueExceptionHandler` + `EventDispatcherService`
- `AddMediateEventQueueDispatchStrategyCore()` — same but returns `IMediateEventQueueBuilder` for custom exception handler:
  - `.AddDefaultExceptionHandler()` — registers `DefaultEventQueueExceptionHandler` (singleton)
  - `.AddCustomExceptionHandler<T>()` — registers custom `IEventQueueExceptionHandler` (singleton)

## Code Style

Enforced via `.editorconfig`:

- **Braces**: Allman style; always required, even for single-statement blocks
- **Types**: Explicit types only — no `var`
- **Modifiers**: Accessibility modifiers required everywhere except interface members; order: `public internal private protected override abstract sealed virtual readonly static`
- **File headers**: MIT license comment auto-inserted at top of every `.cs` file
- **Indentation**: Spaces (soft tabs)
- **Initializers**: Object/collection initializers preferred

## Versioning & Packaging

- Package versions are provided explicitly at publish time via workflow inputs
- Symbol packages (`.snupkg`) are produced alongside `.nupkg`
- XML documentation is generated from `///` comments — keep public API documented
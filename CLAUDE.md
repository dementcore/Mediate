# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Mediate is a lightweight, in-process mediator library for .NET 9 enabling decoupled communication via query/handler and event/handler patterns with middleware support and pluggable dispatch strategies.

## Build & Development Commands

```powershell
# Restore dependencies
dotnet restore

# Build (debug)
dotnet build --no-restore

# Run tests
dotnet test --no-build --verbosity normal

# Full release pipeline (clean → build Release → test Release)
./Build.ps1

# Pack and publish to NuGet (requires NUGET_API_KEY env var)
./Push.ps1
```

## Solution Structure

```
src/
  Mediate/                              # Core NuGet package (net9.0)
  Mediate.BackgroundEventDispatch/      # ASP.NET Core extension package (net9.0)
  Mediate.Test/                         # Unit tests (net10.0, NUnit 4.x)
samples/
  Mediate.Samples.Shared/               # Shared query/event models and handlers
  Mediate.Samples.AspNetCore9MVC/       # ASP.NET Core 9 MVC demo
```

## Architecture

### Core Abstractions (`src/Mediate`)

- **`IMediator`** — central hub; exposes `Send<TResult>(IQuery<TResult>)` and `Dispatch(IEvent)`
- **`IQuery<TResult>` / `IQueryHandler<TQuery, TResult>`** — request-response pattern; exactly one handler per query type
- **`IEvent` / `IEventHandler<TEvent>`** — fan-out notification pattern; zero-to-many handlers
- **`IQueryMiddleware<TQuery, TResult>` / `IEventMiddleware<TEvent>`** — pipeline decorators; applied in registration order
- **`IEventDispatchStrategy`** — swappable strategy controlling how event handlers are invoked:
  - `SequentialEventDispatchStrategy` (default) — handlers run one after another
  - `ParallelEventDispatchStrategy` — handlers run concurrently
- **`IHandlerProvider` / `IMiddlewareProvider`** — resolve handlers and middleware from DI; default implementations use `IServiceProvider`

### Background Dispatch Extension (`src/Mediate.BackgroundEventDispatch`)

Adds `EventQueueDispatchStrategy`, which enqueues event handler invocations into an `IBackgroundTaskQueue` processed by an ASP.NET Core `IHostedService`. This decouples event processing from the HTTP request lifecycle (fire-and-forget).

### DI Registration

Both packages expose extension methods on `IServiceCollection` for registration. There is a documented workaround in place for Microsoft DI issue #57333 affecting open-generic resolution.

## Code Style

Enforced via `.editorconfig`:

- **Braces**: Allman style; always required, even for single-statement blocks
- **Types**: Explicit types only — no `var`
- **Modifiers**: Accessibility modifiers required everywhere except interface members; order: `public internal private protected override abstract sealed virtual readonly static`
- **File headers**: MIT license comment auto-inserted at top of every `.cs` file
- **Indentation**: Spaces (soft tabs)
- **Initializers**: Object/collection initializers preferred

## Versioning & Packaging

- **MinVer** derives package versions from git tags (prefix `v`, e.g., `v1.1.2`)
- **SourceLink** embeds GitHub source references in packages
- Symbol packages (`.snupkg`) are produced alongside `.nupkg`
- XML documentation is generated from `///` comments — keep public API documented
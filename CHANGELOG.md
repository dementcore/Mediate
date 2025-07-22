# Changelog

## 1.1.2

### Changes

- Library and examples migrated to .NET 9

## 1.1.0

### Changes

- Migrated to .NET 6

## 1.0.7

### Changes

- Removed obsolete method `IMediator.Send<TQuery, TResult>(TQuery)`
- Removed obsolete method `IMediator.Send<TQuery, TResult>(TQuery, CancellationToken)`
- Added Mediate symbol package.

## 1.0.6

### Changes

-   `IMediator.Dispatch` and `IMediator.Send` methods now throws an
    `InvalidOperationException` is there isn't any handlers registered.

-   `IMediator.Send<TQuery, TResult>(TQuery)` and
    `IMediator.Send<TQuery, TResult>(TQuery, CancellationToken)` are now
    deprecated and will be removed in 1.0.8.

    Instead use `IMediator.Send<TResult>(IQuery<TResult>)` or
    `IMediator.Send<TResult>(IQuery<TResult>, CancellationToken)`.

    This new methods will infer the result type, so there is no need to
    pass the query type and the result type in the method call.

### Breaking Changes

-   The Event Queue Dispatch Strategy functionality has been moved to
    Mediate.BackgroundEventDispatch package to decouple Mediate from
    Asp.Net Core. This will allow using Mediate in non Asp.Net Core
    apps.

    Sorry for the inconveniences !

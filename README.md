# Mediate
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/1d4f09d9989e4fb788dfe05af01e8fbb)](https://app.codacy.com/manual/dementcore/Mediate?utm_source=github.com&utm_medium=referral&utm_content=dementcore/Mediate&utm_campaign=Badge_Grade_Settings)
![.NET Core](https://github.com/dementcore/Mediate/workflows/.NET%20Core/badge.svg?branch=master)
![Nuget](https://img.shields.io/nuget/v/Mediate?color=2345ba16&logo=nuget&style=flat)
[![Documentation Status](https://readthedocs.org/projects/mediate/badge/?version=latest)](https://mediate.readthedocs.io/en/latest/?badge=latest)

![Mediate](logo.png)

Mediate is another simple and little in-process messaging and event dispatching system based in mediator pattern for Asp.Net Core.

## What attempts to provide this project?

This project is mostly developed for learn and fun, but also attempts 
to provide an easy communication mechanism to develop decoupled communication between code layers.

## [Documentation](https://mediate.readthedocs.io/en/latest/)

## Samples

In samples folder are samples for AspNetCore with Microsoft DI and with Autofac DI.

## Changelog

### 1.0.6

#### Changes

-   `IMediator.Dispatch` and `IMediator.Send` methods now throws an
    `InvalidOperationException` is there isn't any handlers registered.

-   `IMediator.Send<TQuery, TResult>(TQuery)` and
    `IMediator.Send<TQuery, TResult>(TQuery, CancellationToken)` are now
    deprecated and will be removed in 1.0.8.

    Instead use `IMediator.Send<TResult>(IQuery<TResult>)` or
    `IMediator.Send<TResult>(IQuery<TResult>, CancellationToken)`.

    This new methods will infer the result type, so there is no need to
    pass the query type and the result type in the method call.

#### Breaking Changes

-   The Event Queue Dispatch Strategy functionality has been moved to
    Mediate.BackgroundEventDispatch package to decouple Mediate from
    Asp.Net Core. This will allow using Mediate in non Asp.Net Core
    apps.

    Sorry for the inconveniences !
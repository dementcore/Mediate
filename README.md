# Mediate
![Nuget](https://img.shields.io/nuget/v/Mediate?color=2345ba16&logo=nuget&style=flat)
[![Documentation Status](https://readthedocs.org/projects/mediate/badge/?version=latest)](https://mediate.readthedocs.io/en/latest/?badge=latest)

![Mediate](logo.png)

Mediate is another simple and little in-process messaging and event dispatching system based in mediator pattern for Asp.Net Core.

## What attempts to provide this project?

This project is mostly developed for learn and fun, but also attempts 
to provide an easy communication mechanism to develop decoupled communication between code layers.

## [Documentation](https://mediate.readthedocs.io/en/latest/)

## Samples

In samples folder are samples for AspNetCore with .Net DI.

## Fully compatible with .NET 9

The migration to .NET9 is completed and fully tested. 
There is a workaround for an issue with Microsoft DI Container [Microsoft DI Issue](https://github.com/dotnet/runtime/issues/57333) that has been implemented and works as intended.
This workaround not affects to other DI containers so it's safe to use.
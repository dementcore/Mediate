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

In samples folder are samples for AspNetCore with Microsoft DI.

## Fully compatible with .NET 6

The migration to .NET6 is completed and fully tested. 
There is a workaround for an issue with Microsoft DI Container [Microsoft DI Issue](https://github.com/dotnet/runtime/issues/57333) that has been implemented and works as intended.
This workaround not affects to other DI containers so is safe to use.
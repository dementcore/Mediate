#########################
Background Event Dispatch
#########################

Mediate also implements an event dispath strategy called ``EventQueueDispatchStrategy``.
This strategy enqueues event handlers to be executed in background by an Asp.Net Core hosted service.

To use this strategy you will need to install Mediate.BackgroundEventDispatch NuGet package.

Using the NuGet package manager console within Visual studio, run::
    
    Install-Package Mediate.BackgroundEventDispatch

Or using the dotnet CLI::

    dotnet add package Mediate.BackgroundEventDispatch
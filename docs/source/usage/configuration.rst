#############
Configuration
#############

Once installed, you will need to configure Mediate in your app's Startup class inside the ``ConfigureServices`` method.
For this, Mediate provides some helper methods as ``IServiceCollection`` extension methods.

Mediate services configuration
==============================

First, you have to configure Mediate's basic services. 

You can use one of the following methods:

* ``AddMediate``
    Registers the mediator service with the default providers as scoped services.
    See :ref:`Providers <refHandlerProviders>` for details.

.. _refAddMediateCore:

* ``AddMediateCore``
    Registers the mediator service as a scoped service and returns a builder object that has a number of helper methods to configure Mediate.
    See :ref:`Advanced configuration <refAdvancedConfiguration>` for details.

Event dispatching strategy configuration
========================================
Second, you have to configure the event dispatch strategy that you want to use. 

You can use one of the following methods:

* ``AddMediateSequentialEventDispatchStrategy``
    Registers the sequential event dispatch strategy as a scoped service.
    See :ref:`SequentialEventDispatchStrategy <refSequentialEventDispatchStrategy>`
    for details.

* ``AddMediateParallelEventDispatchStrategy``
    Registers the parallel event dispatch strategy as a scoped service.
    See :ref:`ParallelEventDispatchStrategy <refParallelEventDispatchStrategy>`
    for details.

* ``AddMediateEventQueueDispatchStrategy``
    Registers the background queue event dispatch strategy as a scoped service.
    This method also registers a hosted service called ``EventDispatcherService`` 
    and a queue called ``EventQueue`` as a singleton service.
    See :ref:`EventQueueDispatchStrategy <refEventQueueDispatchStrategy>`
    for details.

.. _refAddCustomEventDispatchStrategy:

* ``AddMediateCustomDispatchStrategy``
    Registers a custom event dispatch strategy as a scoped service. 
    You can also use ``AddMediateCustomDispatchStrategy(ServiceLifetime)`` 
    to control the registration lifetime in the DI container. 
    See :ref:`Custom Event Dispatch Strategy <refCustomEventDispatchStrategy>` for details.

Automatic handlers and middlewares registration
===============================================

Third, you have to register your handlers and middlewares into the container.

You can do this manually or use the helper method ``AddMediateClassesFromAssembly(Assembly)``.

This method scans the given assembly and registers the found classes as transient services. 
(open generics handlers and middlewares included).
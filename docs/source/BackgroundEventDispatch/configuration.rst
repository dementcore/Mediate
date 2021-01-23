#############
Configuration
#############

Once installed, you will need to configure Mediate to use the ``EventQueueDispatchStrategy``.
For this, Mediate.BackgroundEventDispatch provides helper methods as ``IServiceCollection`` extension methods.

You can use one of the following methods:

* ``AddMediateEventQueueDispatchStrategy``
    Registers ``EventQueueDispatchStrategy`` as a scoped service.
    This method also registers a hosted service called ``EventDispatcherService`` 
    a singleton queue called ``EventQueue`` and a singleton exception handler called ``DefaultEventQueueExceptionHandler``.
    See :ref:`EventQueueExceptionHandler <refEventQueueExceptionHandler>`
    for details.

.. _refAddMediateEventQueueDispatchStrategyCore:

* ``AddMediateEventQueueDispatchStrategyCore``
    Registers ``EventQueueDispatchStrategy`` as a scoped service and returns a builder object that has some helper methods to configure the strategy.
    See :ref:`Advanced configuration <refQueueAdvancedConfiguration>` for details.

    This method also registers a hosted service called ``EventDispatcherService`` 
    and a singleton queue called ``EventQueue``.
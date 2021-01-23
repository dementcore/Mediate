.. _refQueueAdvancedConfiguration:

Advanced configuration
======================

If you want to manually configure the strategy you can use the ``AddMediateEventQueueDispatchStrategyCore`` extension method.
See :ref:`Configuration <refAddMediateEventQueueDispatchStrategyCore>` for details.

This method returns a builder object that has the following methods:

* ``AddDefaultExceptionHandler``
    Registers the DefaultEventQueueExceptionHandler that logs the error using the ILogger based logging system. .
    See :ref:`EventQueueExceptionHandler <refEventQueueExceptionHandler>`

* ``AddCustomExceptionHandler``
    Registers a custom EventQueueExceptionHandler
    See :ref:`EventQueueExceptionHandler <refEventQueueExceptionHandler>`


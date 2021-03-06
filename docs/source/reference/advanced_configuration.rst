.. _refAdvancedConfiguration:

Advanced configuration
======================

If you want to manually configure each Mediate service you can use the ``AddMediateCore`` extension method.
See :ref:`Configuration <refAddMediateCore>` for details.

This method returns a builder object that has the following methods:

* ``AddServiceProviderHandlerProvider``
    Registers a handler provider as a scoped service for queries and events that are retrieved from the Service Provider.
    See :ref:`Providers <refHandlerProviders>` for details.

* ``AddServiceProviderMiddlewareProvider``
    Registers a middleware provider as a scoped service for queries and events that are retrieved from the Service Provider.
    See :ref:`Providers <refMiddlewareProviders>` for details.

* ``AddCustomHandlerProvider``
    Registers a custom handler provider as a scoped service for queries and events.
    See :ref:`Custom Handler Provider <refCustomHandlerProviders>` for details.

* ``AddCustomMiddlewareProvider``
    Registers a custom middleware provider as a scoped service for queries and events.
    See :ref:`Custom Middleware Provider <refCustomMiddlewareProviders>` for details.


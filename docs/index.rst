.. Mediate documentation master file, created by
   sphinx-quickstart on Tue Oct 27 20:55:59 2020.
   You can adapt this file completely to your liking, but it should at least
   contain the root `toctree` directive.

Welcome to Mediate!
*******************

.. image:: images/logo.png
   :align: center

Mediate is another simple and little in-process communication system based 
in mediator pattern.

What attempts to provide this project?
======================================

This project is mostly developed for learn and fun, but also attempts 
to provide an easy communication mechanism to develop decoupled communication between code layers.

Changelog
=========

1.0.6
-----

Changes
^^^^^^^

- ``IMediator.Dispatch`` and ``IMediator.Send`` methods now throws 
  an ``InvalidOperationException`` is there isn't any handlers registered.

- ``IMediator.Send<TQuery, TResult>(TQuery)`` and 
  ``IMediator.Send<TQuery, TResult>(TQuery, CancellationToken)`` are 
  now deprecated and will be removed in 1.0.8. 
     
  Instead use ``IMediator.Send<TResult>(IQuery<TResult>)`` or 
  ``IMediator.Send<TResult>(IQuery<TResult>, CancellationToken)``. 
     
  This new methods will infer the result type, so there is no need
  to pass the query type and the result type in the method call.

Breaking Changes
^^^^^^^^^^^^^^^^

-  The Event Queue Dispatch Strategy functionality has been moved to 
   Mediate.BackgroundEventDispatch package to decouple Mediate from Asp.Net Core. 
   This will allow using Mediate in non Asp.Net Core apps. 
   
   Sorry for the inconveniences |:slight_smile:|!

Contact
=======
You can contact me in the project GitHub_.

If you want request a feature, please create a `Feature Request`__.

.. _GitHub: https://github.com/dementcore/Mediate

__ FeatureRequest_

.. _FeatureRequest: https://github.com/dementcore/Mediate/issues/new?assignees=&labels=feature+request&template=feature_request.md&title=

.. toctree::
   :maxdepth: 2
   :hidden:
   :caption: Introduction

   source/basic_concepts/mediator_pattern
   source/basic_concepts/com_types


.. toctree::
   :maxdepth: 2
   :hidden: 
   :caption: Basic usage
   
   source/usage/installation
   source/usage/configuration
   source/usage/query_usage
   source/usage/event_usage   

.. toctree::
   :maxdepth: 2
   :hidden: 
   :caption: Reference

   source/reference/advanced_configuration
   source/reference/event_dispatch_strategies
   source/reference/middlewares
   source/reference/providers
   source/reference/generic_handlers
   source/reference/generic_middlewares

.. toctree::
   :maxdepth: 2
   :hidden: 
   :caption: Background Event Dispatch

   source/BackgroundEventDispatch/background_event_dispatch
   source/BackgroundEventDispatch/configuration
   source/BackgroundEventDispatch/advanced_configuration
   source/BackgroundEventDispatch/event_queue_exception_handler
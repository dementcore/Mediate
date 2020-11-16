.. Mediate documentation master file, created by
   sphinx-quickstart on Tue Oct 27 20:55:59 2020.
   You can adapt this file completely to your liking, but it should at least
   contain the root `toctree` directive.

Welcome to Mediate
==================

.. image:: images/logo.png
   :align: center

Mediate is another simple and little in-process communication system based in mediator pattern.

Docs WIP!!

What attempts to provide this project?
--------------------------------------

This project is mostly developed for learn and fun, but also attempts 
to provide an easy communication mechanism to develop decoupled communication between code layers in ASP.NET Core applications.


Contact
-------
You can contact me in the GitHub_.

If you want request a feature, please create a `Feature Request`__.

.. _GitHub: https://github.com/dementcore/Mediate

__ FeatureRequest_

.. _FeatureRequest: https://github.com/dementcore/Mediate/issues/new?assignees=&labels=feature+request&template=feature_request.md&title=

.. toctree::
   :maxdepth: 3
   :hidden:
   :caption: Introduction

   source/introduction/mediator_pattern
   source/introduction/installation

.. toctree::
   :maxdepth: 2
   :hidden:
   :caption: Basic concepts

   source/basic_concepts/com_types
   source/basic_concepts/event_dispatch_strategies
   source/basic_concepts/middlewares

.. toctree::
   :maxdepth: 3
   :hidden: 
   :caption: Basic Usage
   
   source/usage/query_usage
   source/usage/event_usage   
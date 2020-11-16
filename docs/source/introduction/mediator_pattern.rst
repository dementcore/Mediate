Terminology
===========

Mediator pattern
^^^^^^^^^^^^^^^^

.. image:: /images/mediator_pattern.png
   :align: center

Main concept
------------
The mediator pattern defines an object that encapsulates how different objects interacts.

This ensures a low coupling between objects because none of them need to have a direct dependency on each other.

Participants in the pattern
---------------------------

| **Mediator**
| Is an intermediary that controls the communication between Colleagues.

| **Colleagues**
| Represents an object or a component in an application. Each Colleague communicates only with the Mediator.


Mediate implementation of the pattern
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
.. image:: /images/mediate_mediator_pattern.png
   :align: center


Participants in mediate implementation
--------------------------------------

| **Client**
| Is an application component or object that needs to communicate with other part of the application. The client sends a message to the Mediator.

.. note:: In the Mediator pattern the Client is a Colleague.

| **Concrete Message Handler**
| Represents an object or a component in an application that receives a concrete message from the Mediator and processes it. 

.. note:: In the Mediator pattern the Concrete Message Handler is a Colleague.
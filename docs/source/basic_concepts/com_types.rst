Message types
=============

In mediate we have two kinds of messages that represent different things:

Querys
^^^^^^
A query is a message that returns a response. We can think of a query like a question that receives an answer.

.. note:: A concrete query can only have a one concrete handler.

A query is defined by the ``IQuery`` generic interface

.. sourcecode:: csharp

 /// <summary>
 /// Marker interface for defining a query with a response
 /// </summary>
 /// <typeparam name="TResult">Query response type</typeparam>
 public interface IQuery<out TResult>
 {
 }

Events
^^^^^^
An event is a message without response. We can think of an event like a notification that inform someone that something has happened. 

.. note:: A concrete event can have multiple concrete handlers.

An event is defined by the ``IEvent`` interface

.. sourcecode:: csharp

 /// <summary>
 /// Marker interface for defining an event
 /// </summary>
 public interface IEvent
 {
 }
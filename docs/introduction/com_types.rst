Message types
=============

In mediate we have two kinds of messages that represent different things:

Querys
^^^^^^
A query is a request that returns a response. We can think of a query like a question that receives an answer.

.. note:: A concrete query can only have a one concrete handler.

A query is represented by ``IQuery`` interface

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
An event is like a message but without response. We can think of a event like a notification that inform someone that something has happened. 

.. note:: A concrete event can have multiple concrete handlers.

An event is represented by ``IEvent`` interface

.. sourcecode:: csharp

 /// <summary>
 /// Marker interface for defining an event
 /// </summary>
 public interface IEvent
 {
 }
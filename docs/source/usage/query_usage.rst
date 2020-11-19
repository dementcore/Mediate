#############
Queries usage
#############

Once configured, you will need to create the queries that you will send through the mediator.

Query creation
==============

First, to create a query you have to implement the ``IQuery<TResult>`` generic interface.

For example:

.. sourcecode:: csharp
 
 public class MyQuery:IQuery<string>
 {
     public string QueryData { get; set; }
 } 

.. note:: You can use any type that you want for the response. 

Handler creation
================

Second, you have to create a handler for the above query.
For this, you have to implement the ``IQueryHandler<TQuery,TResponse>`` interface.

.. sourcecode:: csharp

    /// <summary>
    /// Interface for implement a query handler for a concrete query
    /// </summary>
    /// <typeparam name="TQuery">Query type</typeparam>
    /// <typeparam name="TResult">Query response type</typeparam>
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handle the message
        /// </summary>
        /// <param name="message">Message data</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Message response</returns>
        Task<TResult> Handle(TQuery message, CancellationToken cancellationToken);

    }

For example:

.. sourcecode:: csharp
 
 public class MyQueryHandler : IQueryHandler<MyQuery, string>
 {
    public Task<MyQueryResponse> Handle(MyQuery query, CancellationToken cancellationToken)
    {
        //Example operation
        return Task.FromResult("Hello: " + query.QueryData);
    }
 }

Sending through the mediator
============================

Third, send the query through the mediator:

.. sourcecode:: csharp

 MyQuery query = new MyQuery() { QueryData = "Dementcore" };

 string res = await _mediator.Send<SampleQuery, string>(query);

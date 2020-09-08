# Mediate
 A simple event and messaging dispatcher library for AspNetCore 3.1 
inspired in mediator pattern.

# Mediator
 The mediator service

# Messages
 Internal messages with return values. Intended for interlayer decoupled communication.

 IMessage&lt;TReturn&gt; Message with return value.
 One Message One Handler

# Events
 Events without return value.
 
The event handlers execution can be queued for background dispatching
 or dispatched inmediately.

 Is posible to have multiple event handlers.

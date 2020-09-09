using Autofac;
using Autofac.Core;
using Mediate.Core;
using Mediate.Core.Abstractions;
using Mediate.Extensions.AspNetCore.HostedService;
using Mediate.Extensions.AspNetCore.Queue;
using Microsoft.Extensions.Hosting;
using System;

namespace Mediate.Extensions.AspNetCore.Autofac
{
    public static class AutofacContainerBuilderExtensions
    {
        public static void AddMediateEventHandler<TEvent, TEventHandler>(this ContainerBuilder builder)
          where TEvent : IEvent
          where TEventHandler : IEventHandler<TEvent>
        {
            builder.RegisterType<TEventHandler>().AsSelf().As<IEventHandler<TEvent>>().OnlyIf(r =>
               !r.IsRegistered(new TypedService(typeof(TEventHandler)))
            ).InstancePerDependency();
        }

        public static void AddMediateMessageHandler<TMessage, TResult, TMessageHandler>(this ContainerBuilder builder)
          where TMessageHandler : IMessageHandler<TMessage, TResult>
            where TMessage : IMessage<TResult>
        {
            builder.RegisterType<TMessageHandler>().AsSelf().As<IMessageHandler<TMessage, TResult>>().OnlyIf(r =>
                !r.IsRegistered(new TypedService(typeof(IMessageHandler<TMessage, TResult>)))
            ).InstancePerDependency();
        }
    }
}

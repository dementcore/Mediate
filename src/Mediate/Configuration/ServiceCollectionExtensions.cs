// File: ServiceCollectionExtensions.cs
// The MIT License
//
// Copyright (c) 2021 DementCore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using Mediate;
using Mediate.Abstractions;
using Mediate.Configuration;
using Mediate.DispatchStrategies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Service collection extension methods
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// This method registers the Mediator and returns a builder to configure Mediate.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMediateBuilder AddMediateCore(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IMediator)))
            {
                throw new InvalidOperationException("You have already registered a IMediator implementation");
            }

            services.TryAddScoped<IMediator, Mediator>();

            return new MediateBuilder(services);
        }

        /// <summary>
        /// This method configures all Mediate default services. Similar to call
        /// <see cref="ServiceCollectionExtensions.AddMediateCore"></see>, 
        /// <see cref="IMediateBuilder.AddServiceProviderHandlerProvider"></see> and
        /// <see cref="IMediateBuilder.AddServiceProviderMiddlewareProvider"></see>
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediate(this IServiceCollection services)
        {
            services.AddMediateCore()
                .AddServiceProviderHandlerProvider()
                .AddServiceProviderMiddlewareProvider();
        }

        /// <summary>
        /// Configures Mediate to use the parallel event dispatch strategy
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediateParallelEventDispatchStrategy(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            services.TryAddScoped<IEventDispatchStrategy, ParallelEventDispatchStrategy>();
        }

        /// <summary>
        /// Configures Mediate to use the sequential event dispatch strategy
        /// </summary>
        /// <param name="services"></param>
        public static void AddMediateSequentialEventDispatchStrategy(this IServiceCollection services)
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            services.TryAddScoped<IEventDispatchStrategy, SequentialEventDispatchStrategy>();
        }

        /// <summary>
        /// Configures Mediate to use a custom event dispatch strategy
        /// </summary>
        /// <typeparam name="TDispatchStrategy"></typeparam>
        /// <param name="services"></param>
        public static void AddMediateCustomDispatchStrategy<TDispatchStrategy>(this IServiceCollection services)
          where TDispatchStrategy : IEventDispatchStrategy
        {
            AddMediateCustomDispatchStrategy<TDispatchStrategy>(services, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Configures Mediate to use a custom event dispatch strategy
        /// </summary>
        /// <typeparam name="TDispatchStrategy"></typeparam>
        /// <param name="services"></param>
        /// <param name="lifetime">ServiceLifetime for the DI container</param>
        public static void AddMediateCustomDispatchStrategy<TDispatchStrategy>(this IServiceCollection services, ServiceLifetime lifetime)
          where TDispatchStrategy : IEventDispatchStrategy
        {
            if (services.Any(s => s.ServiceType == typeof(IEventDispatchStrategy)))
            {
                throw new InvalidOperationException("You have already registered an event dispatch strategy");
            }

            services.TryAdd(new ServiceDescriptor(typeof(IEventDispatchStrategy), typeof(TDispatchStrategy), lifetime));
        }

        /// <summary>
        /// Helper method to register events, querys, handlers and middlewares from an assembly
        /// </summary>
        /// <param name="services">service collection</param>
        /// <param name="assembly">Assembly to scan</param>
        public static void AddMediateClassesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            IEnumerable<Type> assemblyTypes = assembly.DefinedTypes;

            RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IEventHandler<>), assemblyTypes, true, true);
            //RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IEventMiddleware<>), assemblyTypes, true, true);
            //RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IQueryHandler<,>), assemblyTypes, false, false);
            //RegisterHelpers.RegisterClassesFromAssemblyAndType(services, typeof(IQueryMiddleware<,>), assemblyTypes, true, true);
        }
    }
}

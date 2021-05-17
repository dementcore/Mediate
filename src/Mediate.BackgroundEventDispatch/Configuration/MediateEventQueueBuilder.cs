// File: MediateEventQueueBuilder.cs
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

using Mediate.BackgroundEventDispatch.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.BackgroundEventDispatch.Configuration
{
    internal sealed class MediateEventQueueBuilder : IMediateEventQueueBuilder
    {
        private readonly IServiceCollection _services;

        public MediateEventQueueBuilder(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Registers a custom EventQueueExceptionHandler
        /// </summary>
        /// <typeparam name="TExceptionHandler">EventQueueExceptionHandler implementation type</typeparam>
        /// <returns></returns>
        public IMediateEventQueueBuilder AddCustomExceptionHandler<TExceptionHandler>() where TExceptionHandler : IEventQueueExceptionHandler
        {
            if (_services.Any(s => s.ServiceType == typeof(TExceptionHandler)))
            {
                throw new InvalidOperationException("You have already registered an EventQueueExceptionHandler");
            }

            _services.AddSingleton(typeof(IEventQueueExceptionHandler), typeof(TExceptionHandler));

            return this;
        }

        /// <summary>
        /// Registers the DefaultEventQueueExceptionHandler that logs the error.
        /// </summary>
        /// <returns></returns>
        public IMediateEventQueueBuilder AddDefaultExceptionHandler()
        {
            if (_services.Any(s => s.ServiceType == typeof(IEventQueueExceptionHandler)))
            {
                throw new InvalidOperationException("You have already registered an EventQueueExceptionHandler");
            }

            _services.AddSingleton<IEventQueueExceptionHandler, DefaultEventQueueExceptionHandler>();

            return this;
        }
    }
}

// File: MediateBuilder.cs
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

using Mediate.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Mediate.Configuration
{
    internal sealed class MediateBuilder : IMediateBuilder
    {
        private readonly IServiceCollection _services;

        public MediateBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IMediateBuilder AddCustomHandlerProvider<THandlerProvider>()
            where THandlerProvider : IHandlerProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a handler provider");
            }

            _services.AddScoped(typeof(IHandlerProvider), typeof(THandlerProvider));

            return this;
        }

        public IMediateBuilder AddServiceProviderHandlerProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IHandlerProvider)))
            {
                throw new InvalidOperationException("You have already registered a handler provider");
            }

            _services.AddScoped<IHandlerProvider, ServiceProviderHandlerProvider>();

            return this;
        }

        public IMediateBuilder AddServiceProviderMiddlewareProvider()
        {
            if (_services.Any(s => s.ServiceType == typeof(IMiddlewareProvider)))
            {
                throw new InvalidOperationException("You have already registered a middleware provider");
            }

            _services.AddScoped<IMiddlewareProvider, ServiceProviderMiddlewareProvider>();

            return this;
        }

        public IMediateBuilder AddCustomMiddlewareProvider<TMiddlewareProvider>() where TMiddlewareProvider : IMiddlewareProvider
        {
            if (_services.Any(s => s.ServiceType == typeof(IMiddlewareProvider)))
            {
                throw new InvalidOperationException("You have already registered a middleware provider");
            }

            _services.AddScoped(typeof(IMiddlewareProvider), typeof(TMiddlewareProvider));

            return this;
        }
    }
}

// File: RegisterHelpers.cs
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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediate.Configuration
{
    internal static class RegisterHelpers
    {
        internal static void RegisterClassesFromAssemblyAndType(IServiceCollection services, Type openType, IEnumerable<Type> assemblyTypes, bool allowMultiple, bool allowGeneric)
        {
            string OpenTypeName = openType.Name;

            //workaround for the .NET 6 issue https://github.com/dotnet/runtime/issues/57333 register first the open generic types and then the closed types
            foreach (Type assemblyType in assemblyTypes.Where(t => IsNotAbstract(t) && t.GetInterface(OpenTypeName) != null))
            {
                if (IsOpenType(assemblyType) && allowGeneric)
                {
                    if (!services.Any(s => s.ServiceType == openType && s.ImplementationType == assemblyType))
                    {
                        services.AddTransient(openType, assemblyType);
                    }
                }
            }
            
            foreach (Type assemblyType in assemblyTypes.Where(t => IsNotAbstract(t) && t.GetInterface(OpenTypeName) != null))
            {
                if (IsClosedType(assemblyType))
                {

                    Type serviceType = assemblyType.GetInterface(OpenTypeName);

                    if (allowMultiple)
                    {
                        if (!services.Any(s => s.ServiceType == serviceType))
                        {
                            services.AddTransient(serviceType, assemblyType);
                        }
                    }
                    else
                    {
                        if (!services.Any(s => s.ServiceType == serviceType && s.ImplementationType == assemblyType))
                        {
                            services.AddTransient(serviceType, assemblyType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the type is not abstract class and is not an interface
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsNotAbstract(Type t)
        {
            if (!t.IsAbstract && !t.IsInterface)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if the type is closed
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsClosedType(Type t)
        {
            if (!t.IsGenericType && !t.IsGenericTypeDefinition)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if the type is an open generic type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsOpenType(Type t)
        {
            if (t.IsGenericType && t.IsGenericTypeDefinition)
            {
                return true;
            }

            return false;
        }
    }
}

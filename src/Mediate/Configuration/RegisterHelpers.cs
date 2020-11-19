using Microsoft.Extensions.DependencyInjection;
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

            foreach (Type assemblyType in assemblyTypes.Where(t => IsNotAbstract(t) && t.GetInterface(OpenTypeName) != null))
            {
                if (IsClosedType(assemblyType))
                {
                    Type serviceType = assemblyType.GetInterface(OpenTypeName);


                    if (!services.Any(
                            s => s.ServiceType == serviceType &&
                            (allowMultiple ? s.ImplementationType == assemblyType : true)
                        ))
                    {
                        services.AddTransient(serviceType, assemblyType);
                    }

                }

                if (IsOpenType(assemblyType) && allowGeneric)
                {
                    if (!services.Any(s => s.ServiceType == openType && s.ImplementationType == assemblyType))
                    {
                        services.AddTransient(openType, assemblyType);
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

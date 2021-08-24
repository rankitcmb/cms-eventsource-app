using System;
using System.Collections.Generic;
using Scrutor;

namespace CMS.Core.Server.WebApi.Common
{
    public static class RegisterHandlersExtension
    {
        private static HashSet<Type> _decorators;

        static RegisterHandlersExtension()
        {
            _decorators = new HashSet<Type>(new[]
            {
                typeof(RetryDecorator<>)
            });
        }

        public static IImplementationTypeSelector RegisterHandlers(this IImplementationTypeSelector selector, Type type)
        {
            return selector.AddClasses(c => c.AssignableTo(type).Where(t => !_decorators.Contains(t)))
                .UsingRegistrationStrategy(RegistrationStrategy.Append).AsImplementedInterfaces().WithScopedLifetime();
        }
    }
}
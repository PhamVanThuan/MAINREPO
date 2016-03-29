using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            Type baseType = givenType.BaseType;
            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }

        public static IEnumerable<Type> GetImmediateInterfaces(this Type type)
        {
            var interfaces = type.GetInterfaces();
            var result = new HashSet<Type>(interfaces);
            foreach (Type i in interfaces)
            {
                result.ExceptWith(i.GetInterfaces());
            }
            return result;
        }

        public static object GetPropertyValue(this Type type, object source, string propertyName)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (string.IsNullOrWhiteSpace(propertyName)) { throw new ArgumentNullException("propertyName"); }

            var propertyInfo = type.GetProperty(propertyName);
            return propertyInfo == null ? null : propertyInfo.GetValue(source);
        }
    }
}
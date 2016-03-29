using System.Xml;
using SAHL.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.Validation
{
    public class TypeMetaDataLookup : ITypeMetaDataLookup
    {
        protected readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> propertyInfoByType = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
        protected readonly ConcurrentDictionary<Type, ValidationActionType> objectTypeInfoByType = new ConcurrentDictionary<Type, ValidationActionType>();

        protected readonly List<string> memberNamespacesAllowed = new List<string> { "SAHL", "Capitec" };

        public virtual IEnumerable<PropertyInfo> GetPropertyInfo(Type givenType)
        {
            return propertyInfoByType.TryGetValueIfNotPresentThenAdd(givenType, GetNonValueTypeProperties);
        }

        public virtual ValidationActionType GetObjectTypeForGivenType(Type givenType)
        {
            return objectTypeInfoByType.TryGetValueIfNotPresentThenAdd(givenType, GetMapping);
        }

        protected virtual IEnumerable<PropertyInfo> GetNonValueTypeProperties(Type o)
        {
            return o.GetProperties().Where(x => x.DeclaringType != null && !x.DeclaringType.IsValueType);
        }

        protected virtual ValidationActionType GetMapping(Type typeOfObject)
        {
            if (typeOfObject.IsPrimitive)
            {
                return ValidationActionType.IsNotValidatable;
            }
            if (typeof(string).IsAssignableFrom(typeOfObject))
            {
                return ValidationActionType.IsNotValidatable;
            }
            var actionType = IsALoopableObject(typeOfObject);
            if (actionType == ValidationActionType.Unspecified)
            {
                if (IsOfAllowedNamespaces(typeOfObject))
                {
                    return ValidationActionType.IsValidatable;
                }
            }
            else
            {
                return actionType;
            }

            return ValidationActionType.IsNotValidatable;
        }

        protected virtual ValidationActionType IsALoopableObject(Type typeOfObject)
        {
            if (IsArray(typeOfObject))
            {
                return ValidationActionType.IsEnumerable;
            }
            if (IsEnumerable(typeOfObject))
            {
                if (IsAnIDictionary(typeOfObject))
                {
                    return ValidationActionType.IsEnumerableOfKeyValuePair;
                }
                return ValidationActionType.IsEnumerable;
            }
            return ValidationActionType.Unspecified;
        }

        protected virtual ValidationActionType ShouldTypeBeLoopedOver(Type ofObject, ValidationActionType returnActionIfLoopedOver)
        {
            bool isUnderlyingObjectValidatable = GetMapping(ofObject) != ValidationActionType.IsNotValidatable;
            return isUnderlyingObjectValidatable ? returnActionIfLoopedOver : ValidationActionType.IsNotValidatable;
        }

        protected virtual bool IsArray(Type typeOfObject)
        {
            return typeOfObject.IsArray;
        }

        protected virtual bool IsAnIDictionary(Type typeOfObject)
        {
            return typeof(IDictionary).IsAssignableFrom(typeOfObject);
        }

        protected virtual bool IsEnumerable(Type typeOfObject)
        {
            return typeof(IEnumerable).IsAssignableFrom(typeOfObject);
        }

        protected virtual bool IsOfAllowedNamespaces(Type type)
        {
            if (type.Namespace == null)
            {
                return false;
            }
            var indexOfFirstDot = type.Namespace.IndexOf(".");
            if (indexOfFirstDot < 0)
            {
                return false;
            }

            var token = type.Namespace.Substring(0, indexOfFirstDot);
            return memberNamespacesAllowed.Contains(token);
        }
    }
}
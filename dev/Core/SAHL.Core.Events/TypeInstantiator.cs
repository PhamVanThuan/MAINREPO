using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Core.Events
{
    internal class TypeInstantiator : ITypeInstantiator
    {
        private static readonly Dictionary<TypeInstantiatorEnum, Func<Type, XElement, Func<Type, string, object>, object>> transformers =
                            new Dictionary<TypeInstantiatorEnum, Func<Type, XElement, Func<Type, string, object>, object>>
        {
            {TypeInstantiatorEnum.Unknown, Throw()},
            {TypeInstantiatorEnum.IsNull,IsNull()},
            {TypeInstantiatorEnum.String, CreateString()},
            {TypeInstantiatorEnum.DateTime, CreateDateTime()},
            {TypeInstantiatorEnum.Primitive, CreatePrimitive()},
            {TypeInstantiatorEnum.Enum, CreateEnum()},
            {TypeInstantiatorEnum.Nullable, CreateNullable()},
            {TypeInstantiatorEnum.Complex, CreateComplex()},
            {TypeInstantiatorEnum.IEnumarable,CreateIEnumrarble()},
            {TypeInstantiatorEnum.Array,CreateArray()}
        };

        public object Create(Type typeOfValue, XElement valueForType)
        {
            return CreateFromType(typeOfValue, valueForType);
        }

        internal static object CreateFromType(Type typeOfValue, XElement valueForType, Func<Type, string, object> deserialiseObjectCallBack = null)
        {
            var typeEnum = valueForType == null || string.IsNullOrEmpty(valueForType.Value) ? TypeInstantiatorEnum.IsNull : GetInstantiatorTypeForType(typeOfValue);
            var value = transformers[typeEnum](typeOfValue, valueForType, deserialiseObjectCallBack);
            return value;
        }

        private static TypeInstantiatorEnum GetInstantiatorTypeForType(Type typeOfValue)
        {
            return typeOfValue.IsPrimitive
                ? TypeInstantiatorEnum.Primitive
                : GetInstantiatorTypeForNonPrimitiveType(typeOfValue);
        }

        private static TypeInstantiatorEnum GetInstantiatorTypeForNonPrimitiveType(Type typeOfValue)
        {
            return typeOfValue.IsEnum
                ? TypeInstantiatorEnum.Enum
                : GetInstantiatorTypeForNonEnum(typeOfValue);
        }

        private static TypeInstantiatorEnum GetInstantiatorTypeForNonEnum(Type typeOfValue)
        {
            return typeOfValue == typeof(string)
                ? TypeInstantiatorEnum.String
                : GetInstantiatorTypeForComplexType(typeOfValue);
        }

        private static TypeInstantiatorEnum GetInstantiatorTypeForComplexType(Type typeOfValue)
        {
            return Nullable.GetUnderlyingType(typeOfValue) != null
                ? TypeInstantiatorEnum.Nullable
                : GetInstantiatorTypeForNonNullableComplexType(typeOfValue);
        }

        private static TypeInstantiatorEnum GetInstantiatorTypeForNonNullableComplexType(Type typeOfValue)
        {
            return TypeDescriptor.GetConverter(typeOfValue).CanConvertFrom(typeof(string))
                ? TypeInstantiatorEnum.Complex
                : GetInstantiatorTypeForIEnumrable(typeOfValue);
        }

        private static TypeInstantiatorEnum GetInstantiatorTypeForIEnumrable(Type typeOfValue)
        {
            return typeOfValue.IsGenericType && (typeOfValue.GetGenericTypeDefinition() == typeof(List<>) || typeOfValue.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                   ? TypeInstantiatorEnum.IEnumarable
                   : GetInstaantiatorTypeForArray(typeOfValue);
        }

        private static TypeInstantiatorEnum GetInstaantiatorTypeForArray(Type typeOfValue)
        {
            return typeOfValue.IsArray ? TypeInstantiatorEnum.Array : TypeInstantiatorEnum.Unknown;
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> IsNull()
        {
            return (type, value, deserialiseObjectCallBack) => { return null; };
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreateComplex()
        {
            return (type, value, deserialiseObjectCallBack) =>
            {
                var converter = TypeDescriptor.GetConverter(type);
                return converter.ConvertFrom(value.Value);
            };
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreateNullable()
        {
            return (type, value, deserialiseObjectCallBack) =>
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return CreateFromType(underlyingType, value);
            };
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreateEnum()
        {
            return (type, value, deserialiseObjectCallBack) =>
             {
                 Activator.CreateInstance(type);
                 return Enum.Parse(type, value.Value); // Yeah, we're making an assumption
             };
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreateDateTime()
        {
            var cultureInfo = new System.Globalization.CultureInfo("en-ZA", true);
            return (type, value, deserialiseObjectCallBack) => DateTime.ParseExact(value.Value, EventSerialiser.W3CDateTimeFormatString, cultureInfo);
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreatePrimitive()
        {
            return (type, value, deserialiseObjectCallBack) =>
            {
                Activator.CreateInstance(type); //are there side effects? otherwise this is a useless statement
                return Convert.ChangeType(value.Value, type);
            };
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreateString()
        {
            return (type, value, deserialiseObjectCallBack) => value.Value;
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreateIEnumrarble()
        {
            return (type, value, deserialiseObjectCallBack) =>
            {
                var listType = typeof(List<>);
                var genericType = type.GetGenericArguments();
                var genericListType = listType.MakeGenericType(genericType);
                var genericList = Activator.CreateInstance(genericListType);

                if (value == null)
                {
                    return genericList;
                }

                foreach (XElement item in value.Elements())
                {
                    genericListType.GetMethod("Add").Invoke(genericList, new[] {
                        CreateFromType(genericType[0], item,deserialiseObjectCallBack)
                    });
                }
                return genericList;
            };
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> CreateArray()
        {
            return (type, value, deserialiseObjectCallBack) =>
            {
                int count = value.Elements().Count();
                Type arrayType = type.GetElementType();
                var arrayValue = Array.CreateInstance(arrayType, count);
                int index = 0;
                foreach (XElement item in value.Elements())
                {
                    arrayValue.SetValue(item.Value, index);
                    index++;
                }
                return arrayValue;
            };
        }

        private static Func<Type, XElement, Func<Type, string, object>, object> Throw()
        {
            return (type, value, deserialiseObjectCallBack) =>
             {
                 return deserialiseObjectCallBack(type, value.ToString());
             };
        }

        private enum TypeInstantiatorEnum
        {
            Unknown,
            String,
            DateTime,
            Primitive,
            Enum,
            Nullable,
            Complex,
            IEnumarable,
            Array,
            IsNull
        }
    }
}
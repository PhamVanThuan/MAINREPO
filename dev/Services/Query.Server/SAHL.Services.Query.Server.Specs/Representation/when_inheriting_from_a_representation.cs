using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.UI;
using NSubstitute;
using NUnit.Framework;
using SAHL.Config.Services.Query.Server;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Query.Controllers;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Resources;
using SAHL.Services.Query.Resources.OrganisationStructure;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;
using StructureMap.TypeRules;
using WebApi.Hal;

namespace SAHL.Services.Query.Server.Specs.Representation
{
    public class when_inheriting_from_a_representation : WithFakes
    {
        Establish that = () =>
        {
            propertiesThatMustBeOverridden = new[] { "Href", "Rel" };

            representationType = typeof (WebApi.Hal.Representation);

            typesThatImplementRepresentation = Assembly.Load("SAHL.Services.Query")
                .GetTypes()
                .Where(a => a.BaseType != null && a.IsSubclassOf(representationType)
                    && a.CustomAttributes //should not have been excluded from generation
                        .All(b => b.AttributeType != typeof(ServiceGenerationToolExcludeAttribute)))
                .ToList();

            var container = new Container(new LinkRegistry());

            representations = container.GetInstance<ILinkMetadataCollection>();
        };

        private Because of = () =>
        {
            foreach (var item in typesThatImplementRepresentation)
            {
                var type = item;

                if (type.BaseType != representationType)
                {
                    var message = string.Format("The {0} class does not inherit directly from a Representation. "
                        + "Inherit directly from the Representation class and not from a child of Representation."
                        , type.Name
                        );
                    exception = new AssertionException(message);
                    return;
                }

                exception = AssertRepresentationIsValid(type);

                if (exception != null)
                {
                    return;
                }
            }
        };

        private It should_not_have_any_invalid_representations = () =>
        {
            if (exception != null)
            {
                throw exception;
            }
        };

        private static List<PropertyInfo> GetPropertiesToCheck(Type type)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList();
        }

        private static List<PropertyInfo> GetOverriddenPropertiesToCheck(Type type)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(a => propertiesThatMustBeOverridden.Contains(a.Name))
                .ToList();
        }

        private static List<FieldInfo> GetFieldsToCheck(Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .ToList();
        }


        private static Exception AssertRepresentationIsValid(Type type)
        {
            return Catch.Exception(() => AssertAllExpectedPropertiesToBeOverriddenAreDeclaredInContainingType(GetOverriddenPropertiesToCheck(type), type))
                ?? Catch.Exception(() => AssertOverriddenPropertiesAreNotCallingBaseImplementationOnly(GetOverriddenPropertiesToCheck(type), type))
                ?? Catch.Exception(() => AssertNoPublicFields(GetFieldsToCheck(type), type))
                ?? Catch.Exception(() => AssertPublicPropertiesReturnValidTypes(GetPropertiesToCheck(type), type))
                ?? Catch.Exception(() => AssertTypeHasValidLinkToSelf(type))
                ;
        }

        private static void AssertPublicPropertiesReturnValidTypes(List<PropertyInfo> propertiesToCheck, Type type)
        {
            foreach (var item in propertiesToCheck)
            {
                if (item.Name.Equals("Links")) //special property in a representation, should not be checked
                {
                    continue;
                }

                var propertyType = item.PropertyType.IsGenericType && item.PropertyType.IsNullable()
                    ? item.PropertyType.GetGenericArguments()[0]
                    : item.PropertyType;

                if (typeof (WebApi.Hal.Representation).IsAssignableFrom(propertyType))
                {
                    ProcessRepresentation(type, item);
                }

                if (IsSupportedType(propertyType))
                {
                    continue;
                }

                if (propertyType.IsGenericType)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(propertyType))
                    {
                        ProcessIEnumerableType(type, item);
                        continue;
                    }

                    ProcessGenericType(type, item);
                }

                ProcessUnsupportedType(type, item);
            }
        }

        private static bool IsSupportedType(Type propertyType)
        {
            //TODO: convert to action/dictionary if this is touched again
            return propertyType.IsPrimitive
                || propertyType == typeof(string)
                || propertyType == typeof(Guid)
                || propertyType == typeof(decimal)
                || propertyType == typeof(DateTime)
                || typeof(IPagingRepresentation).IsAssignableFrom(propertyType);
        }

        private static void ProcessRepresentation(Type type, PropertyInfo item)
        {
            var message = string.Format(
                "Property {0} in the {1} class is a Representation. "
                    + "Representations need to be represented as an IEnumerable<Representations> to ensure consistent access as "
                    + "embedded resources, even if the REL is a 0..1 or 1..1 relationship.",
                item.Name,
                type.Name
                );

            throw new AssertionException(message);
        }

        private static void ProcessUnsupportedType(Type type, PropertyInfo item)
        {
            var message = string.Format(
                "Property {0} in the {1} class was not a [primitive/string/DateTime/IEnumerable<Representation>]"
                    + ", it is a {2}. Make the field private if serialisation is not required"
                    + ", or change the property type to a supported datatype."
                , item.Name
                , type.Name
                , item.PropertyType.Name
                );

            throw new AssertionException(message);
        }

        private static void ProcessGenericType(Type type, PropertyInfo item)
        {
            string message = string.Format(
                "Property {0} in the {1} class is not an IEnumerable<Representation>. "
                    + "Please ensure any generic types are assignable to an IEnumerable<Representation>"
                , item.Name
                , type.Name
                );

            throw new AssertionException(message);
        }

        private static void ProcessIEnumerableType(Type type, PropertyInfo item)
        {
            var genericParameter = item.PropertyType.GetGenericArguments()[0];
            if (typeof (WebApi.Hal.Representation).IsAssignableFrom(genericParameter))
            {
                return;
            }

            var message = string.Format(
                "Property {0} in the {1} class is an IEnumerable<> but has a generic parameter that is not a Representation."
                    + " Collections within representations must contain representation types. Change the {2} type to a representation."
                , item.Name
                , type.Name
                , genericParameter.Name
                );

            throw new AssertionException(message);
        }

        private static void AssertNoPublicFields(List<FieldInfo> fieldsToCheck, Type type)
        {
            foreach (var item in fieldsToCheck)
            {
                if (item.FieldType.IsPrimitive || item.FieldType == typeof(string))
                {
                    continue;
                }

                var message = string.Format(
                    "Field {0} in the {1} class was not a primitive type or a string."
                        + " Make the field non-public if serialisation is not required, or convert field to a property."
                    , item.Name
                    , type.Name
                    );

                throw new AssertionException(message);
            }
        }

        private static void AssertAllExpectedPropertiesToBeOverriddenAreDeclaredInContainingType(List<PropertyInfo> propertiesToCheck, Type item)
        {
            var declaredOnlyProperties = item.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var property in propertiesToCheck)
            {
                if (declaredOnlyProperties.Contains(property))
                {
                    continue;
                }

                var message = string.Format(
                        "Property {0} in the {1} class was not overridden or calls the base class implementation. Please override the property and provide a suitable value.",
                        property.Name,
                        item.Name
                        );
                throw new AssertionException(message);
            }
        }

        private static void AssertOverriddenPropertiesAreNotCallingBaseImplementationOnly(IEnumerable<PropertyInfo> propertiesToCheck, Type item)
        {
            foreach (var property in propertiesToCheck)
            {
                var getMethod = property.GetGetMethod(false);
                var baseDefinition = getMethod.GetBaseDefinition();
                if (baseDefinition.MethodHandle.Value != getMethod.MethodHandle.Value)
                {
                    continue;
                }
                var message = string.Format(
                    "Property {0} in the {1} class was not overridden or calls the base class implementation. Please override the property and provide a suitable value.",
                    property.Name,
                    item.Name
                    );
                throw new AssertionException(message);
            }
        }

        private static void AssertTypeHasValidLinkToSelf(Type representationType)
        {
            var links = representations[representationType];
            if (links != null)
            {
                return;
            }
            throw new AssertionException(string.Format("There was no self link found for {0}", representationType.Name));
        }

        private static List<Type> typesThatImplementRepresentation;
        private static Type representationType;
        private static string[] propertiesThatMustBeOverridden;
        private static List<Exception> exceptions;
        private static Exception exception;
        private static ILinkMetadataCollection representations;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core;
using SAHL.Services.Query.Server.Specs.Validator;

namespace SAHL.Services.Query.Server.Specs.Representation
{
    public class when_defining_representation : WithFakes
    {
        private static List<Type> typesThatAreRepresentations;

        private static Type representationType;
        private static string[] propertiesThatMustBeOverridden;
        private static List<Exception> exceptions;
        private static Exception nullableTypeException;
        private static Exception idException;
        private static Exception relationshipException;
        private static IIocContainer container;

        private Because of = () =>
        {
            foreach (var type in typesThatAreRepresentations)
            {
                CheckForAnIdProperty(type);
                CheckThatAllTypesAreNullable(type);
            }
        };

        private It should_have_a_property_called_id = () =>
        {
            if (idException != null)
            {
                throw idException;
            }
        };

        private It should_have_all_fields_nullable = () =>
        {
            if (nullableTypeException != null)
            {
                throw nullableTypeException;
            }
        };

        private Establish that = () =>
        {
            representationType = typeof (WebApi.Hal.Representation);

            typesThatAreRepresentations = Assembly.Load("SAHL.Services.Query")
                .GetTypes()
                .Where(b => b.BaseType != null
                    && b.IsSubclassOf(representationType)
                    && b.CustomAttributes
                        .All(c => c.AttributeType != typeof (ServiceGenerationToolExcludeAttribute) 
                            && c.AttributeType != typeof(DoesNotRequireAnIdProperty)))
                .ToList();
        };

        private static void CheckForAnIdProperty(Type type)
        {
            if (idException == null)
            {
                idException = TypeValidator.CheckForAnIdProperty(type, "Representation");
            }
        }

        private static void CheckThatAllTypesAreNullable(Type type)
        {
            if (nullableTypeException == null)
            {
                nullableTypeException = TypeValidator.CheckThatAllTypesAreNullable(type, "Representation");
            }
        }
    }
}

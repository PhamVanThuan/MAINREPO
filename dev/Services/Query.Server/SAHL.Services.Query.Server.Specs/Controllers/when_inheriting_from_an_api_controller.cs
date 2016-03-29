using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Machine.Fakes;
using Machine.Specifications;
using NUnit.Framework;
using SAHL.Config.Services.Query.Server;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Resources.OrganisationStructure;
using StructureMap;

namespace SAHL.Services.Query.Server.Specs.Controllers
{
    public class when_inheriting_from_an_api_controller : WithFakes
    {
        Establish that = () =>
        {
            new OrganisationTypeListRepresentation(null, null); //force load of the dll

            propertiesThatMustBeOverridden = new[] { "Href", "Rel" };

            typesThatAreApiControllers = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.StartsWith("SAHL."))
                .SelectMany(a => a.GetTypes().Where(b => b.BaseType != null && b.IsSubclassOf(typeof (ApiController))))
                .Where(a => a.CustomAttributes.All(b => b.AttributeType != typeof (ServiceGenerationToolExcludeAttribute)))
                .ToList();
        };

        private Because of = () =>
        {
            foreach (var item in typesThatAreApiControllers)
            {
                var type = item;

                var methodsToCheck = type
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .ToList();

                exception = Catch.Exception(() => AssertControllerIsValid(type, methodsToCheck));

                if (exception != null)
                {
                    return;
                }
            }
        };

        private It should_not_have_any_invalid_controllers = () =>
        {
            if (exception != null)
            {
                throw exception;
            }
        };

        private static void AssertControllerIsValid(Type type, List<MethodInfo> methodsToCheck)
        {
            foreach (var item in methodsToCheck)
            {
                AssertApiControllerIsDeclaredPublicly(type);
                AssertReturnTypeIsAnHttpResponseMessage(type, item);
            }
        }

        private static void AssertApiControllerIsDeclaredPublicly(Type type)
        {
            if (type.IsPublic)
            {
                return;
            }
            var message = string.Format(
                "The {0} class has not been declared publicly. "
                    + "MVC will not be able to route a request to this controller if you do not declare it publicly."
                , type.Name
                );

            throw new AssertionException(message);
        }

        private static void AssertReturnTypeIsAnHttpResponseMessage(Type type, MethodInfo item)
        {
            if ((typeof(HttpResponseMessage).IsAssignableFrom(item.ReturnType)))
            {
                return;
            }
            var message = string.Format(
                "The {0} method in the {1} controller does not have a return type of HttpResponseMessage. "
                    + "Change your controller method to return an HttpResponseMessage. "
                    + "Set the content of the response via the ToHttpResponseMessage extension method."
                , item.Name
                , type.Name
                );

            throw new AssertionException(message);
        }

        private static List<Type> typesThatAreApiControllers;
        private static Type representationType;
        private static string[] propertiesThatMustBeOverridden;
        private static List<Exception> exceptions;
        private static Exception exception;
        private static ILinkMetadataCollection representations;
    }
}

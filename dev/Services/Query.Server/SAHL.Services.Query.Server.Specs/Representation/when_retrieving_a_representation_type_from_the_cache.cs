using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Machine.Fakes.Sdk;
using SAHL.Core.Data.Models._2AM;
using SAHL.Services.Query.Controllers.Test;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;
using SAHL.Core.Data.Context;
using SAHL.Core.Data.Configuration;
using SAHL.Core.Data.Context.Configuration;

namespace SAHL.Services.Query.Server.Specs.Representation
{
    public class when_retrieving_a_representation_type_from_the_cache : WithFakes
    {
        Establish that = () =>
        {
            typeDictionary = new Dictionary<Type, WebApi.Hal.Representation>();

            representaton = An<WebApi.Hal.Representation>();

            typeDictionary.Add(representaton.GetType(), representaton);

            cache = new RepresentationTemplateCache(typeDictionary);
        };

        private Because of = () =>
        {
            result = cache.GetType(representaton.GetType());
        };

        private It should_have_retrieved_an_instance = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_be_of_the_expected_type = () =>
        {
            result.ShouldEqual(representaton.GetType());
        };

        private It should_have_called_the_method_to_retrieve_the_type = () =>
        {
            representaton.WasToldTo(a => a.GetType());
        };

        private static RepresentationTemplateCache cache;
        private static WebApi.Hal.Representation representaton;
        private static Dictionary<Type, WebApi.Hal.Representation> typeDictionary;
        private static Type result;
    }
}

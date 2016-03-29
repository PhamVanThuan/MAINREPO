using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Machine.Fakes;
using Machine.Specifications;
using SAHL.Config.Services;
using SAHL.Config.Web.Mvc.Routing.Configuration;
using SAHL.Core;
using SAHL.Core.Testing;
using StructureMap;
using StructureMap.Pipeline;

namespace SAHL.Config.Web.Mvc.Specs
{
    public class when_retrieving_an_instance_of_IRouteRetriever : WithFakes
    {
        Establish that = () =>
        {
            container = new Container(new RouteRegistry());
        };

        private Because of = () =>
        {
            instance1 = container.GetInstance<IRouteRetriever>();
            instance2 = container.GetInstance<IRouteRetriever>();
        };

        private It should_have_retrieved_an_instance = () =>
        {
            instance1.ShouldNotBeNull();
        };

        private It should_be_a_singleton = () =>
        {
            ReferenceEquals(instance1, instance2).ShouldBeTrue();
        };

        private static Container container;
        private static object instance1;
        private static object instance2;
    }
}

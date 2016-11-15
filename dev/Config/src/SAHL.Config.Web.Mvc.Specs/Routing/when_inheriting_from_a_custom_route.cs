﻿using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.Config.Web.Mvc.Specs.Routing.Fakes;
using StructureMap;
using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Testing;

namespace SAHL.Config.Web.Mvc.Specs.Routing
{
    public class when_inheriting_from_a_custom_route : WithFakes
    {
        Establish that = () =>
        {
            routeTemplate = "~/somewhere/{myFirstId}/the/{mySecondId}/way/{myThirdId}/high";
        };

        private Because of = () =>
        {
            route = new CustomRouteForTesting(string.Empty, routeTemplate);
        };

        private It should_not_start_with_a_tilde_or_forward_slash = () =>
        {
            route.Url.ShouldNotContain(routeTemplate.Substring(0, 2));
        };

        private It should_have_a_route_without_numbered_parameters = () =>
        {
            route.Url.ShouldEqual("somewhere/{myFirstId}/the/{mySecondId}/way/{myThirdId}/high");
        };

        private static CustomRouteForTesting route;
        private static string routeTemplate;
    }
}
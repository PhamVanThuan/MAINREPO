using System;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;

namespace SAHL.Services.Query.Controllers
{
    public class DoNotRouteParametersInQueryStringAttribute : Attribute, IControllerConfiguration
    {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            //essentially removes the QueryStringProviderFactory from being used when 
            //determining which controller/action to choose when a request is serviced
            controllerSettings.Services.Replace(typeof(ValueProviderFactory), new RouteDataValueProviderFactory());
        }
    }
}
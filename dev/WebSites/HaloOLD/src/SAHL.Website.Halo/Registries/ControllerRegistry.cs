using SAHL.Website.Halo.Attributes;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SAHL.Website.Halo.Registries
{
	public class ControllerRegistry : Registry
	{
		public ControllerRegistry()
		{
			this.For<IControllerFactory>().Use<StructureMapControllerFactory>();
		}
	}

	public class StructureMapControllerFactory : DefaultControllerFactory
	{
		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			if (controllerType != null)
			{
				return ObjectFactory.GetInstance(controllerType) as IController;
			}
			return base.GetControllerInstance(requestContext, controllerType);
		}

		protected override System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, Type controllerType)
		{
			if (controllerType == null)
				return base.GetControllerSessionBehavior(requestContext, controllerType);

			var action = requestContext.RouteData.Values["action"].ToString();
			var method = controllerType.GetMethods().Where(x => x.Name.ToLower() == action.ToLower()).FirstOrDefault();
			if (method == null)
				return base.GetControllerSessionBehavior(requestContext, controllerType);

			var sessionStateAttribute = method.GetCustomAttributes(typeof(ActionSessionStateAttribute), true).FirstOrDefault() as ActionSessionStateAttribute;
			if (sessionStateAttribute != null)
			{
				return sessionStateAttribute.Behavior;
			}
			return base.GetControllerSessionBehavior(requestContext, controllerType);
		}
	}
}
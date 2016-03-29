using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace SAHL.Website.Halo.App_Start.DependencyResolution
{
	public class StructureMapDependencyResolver : StructureMapDependencyScope, System.Web.Http.Dependencies.IDependencyResolver
	{
		public StructureMapDependencyResolver(IContainer container)
			: base(container)
		{
		}

		public IDependencyScope BeginScope()
		{
			IContainer child = this.Container.GetNestedContainer();
			return new StructureMapDependencyResolver(child);
		}
	}
}
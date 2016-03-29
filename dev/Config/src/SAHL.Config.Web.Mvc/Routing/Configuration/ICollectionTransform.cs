using System.Collections.Generic;

namespace SAHL.Config.Web.Mvc.Routing.Configuration
{
    public interface ICollectionTransform<T>
    {
        IEnumerable<T> Transform(IEnumerable<T> customRoutes);
    }
}

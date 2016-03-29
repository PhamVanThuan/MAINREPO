using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SAHL.Config.Web.Mvc.Routing;

namespace SAHL.Config.Web.Mvc
{
    public class CustomHttpConfiguration : ICustomHttpConfiguration
    {
        public IEnumerable<IRegistrable> Registrations { get; private set; }

        public CustomHttpConfiguration(IEnumerable<IRegistrable> registrations)
        {
            this.Registrations = registrations ?? Enumerable.Empty<IRegistrable>();
        }

        public void PerformRegistrations()
        {
            foreach (var item in this.Registrations)
            {
                item.Register();
            }
        }
    }
}

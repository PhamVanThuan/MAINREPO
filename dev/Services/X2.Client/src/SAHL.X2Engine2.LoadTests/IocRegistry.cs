using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.LoadTests
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            this.For<ITestCase>().Use<ApplicationCaptureTestCase>();
        }
    }
}

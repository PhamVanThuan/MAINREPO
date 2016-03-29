using Managers;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Identity;
using SAHL.Core.Logging;
using SAHL.Core.Services;
using StructureMap.Configuration.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework.Registries
{
    public class SpecificConfig : Registry
    {
        public SpecificConfig()
        {
            For<ILoggerAppSource>().Singleton().Use<LoggerAppSourceFromConfiguration>();
       
        }
    }
}

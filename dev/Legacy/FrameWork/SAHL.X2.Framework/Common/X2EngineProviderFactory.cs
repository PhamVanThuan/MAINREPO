using SAHL.X2.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2.Framework.Common
{
    public class X2EngineProviderFactory
    {
        public IX2Provider GetX2EngineProvider()
        {
            if (Properties.Settings.Default.X2ProviderWeb)
                return new X2WebEngineProvider();
            else
                return new X2RemotingEngineProvider();
        }
    }
}

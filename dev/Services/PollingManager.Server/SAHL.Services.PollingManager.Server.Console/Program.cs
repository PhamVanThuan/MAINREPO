using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.IoC;
using StructureMap;
using System.Configuration;
using SAHL.Config.Services.DomainProcessManager.Client;

namespace SAHL.Services.PollingManager.Server.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IoC.Initialize();

            var startables = ObjectFactory.GetAllInstances<IStartable>();
            foreach (var startable in startables)
            {
                startable.Start();
            }

            System.Console.ReadLine();
        }
    }
}

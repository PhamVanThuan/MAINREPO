using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Providers;
using SAHL.Core.Caching;
using SAHL.X2Engine2.Tests.X2;
using SAHL.X2Engine2.Tests.X2.Services;
using SAHL.X2Engine2.Node;
using SAHL.Core.X2.AppDomain;
using SAHL.X2Engine2.Node.AppDomain;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.Core.Services;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Services;
using SAHL.X2Engine2.Factories;
using SAHL.Core.Logging;
using SAHL.Core.X2.Factories;
using SAHL.X2Engine2.MessageHandlers;
using SAHL.Core.X2.Messages.Management;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data;
using SAHL.Core.Configuration;
using SAHL.Core.Messaging;
using SAHL.Config.Services.Client;
using SAHL.Core;

namespace SAHL.X2Engine2.Tests
{
    public static class SpecificationIoCBootstrapper
    {
        private static IIocContainer _iocContainer;

        public static IIocContainer Initialize()
        {

            try
            {
                var clientBootstrapper = new ClientBootstrapper();
                _iocContainer = clientBootstrapper.Initialise();
                if (_iocContainer == null) { throw new NullReferenceException("Ioc Container not found"); }
                DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
                return _iocContainer;

            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}

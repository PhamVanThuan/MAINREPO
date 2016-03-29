using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Security;
using SAHL.Core.Data;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.SystemMessages;
using SAHL.V3.Framework.DependencyResolution;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework
{
    public class V3ServiceManager : IV3ServiceManager
    {
        private IContainer iocContainer;

        public V3ServiceManager(IContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        public IContainer IOCContainer
        {
            get
            {
                return iocContainer;
            }
        }

        static V3ServiceManager instance;
        static readonly object instanceLock = new object();

        public static V3ServiceManager Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new V3ServiceManager(BootStrapper.Initialize());
                        DbContextConfiguration.Instance.RepositoryFactory = ObjectFactory.GetInstance<ISqlRepositoryFactory>();
                    }
                    return instance;
                }
            }
            set
            {
                lock (instanceLock)
                {
                    instance = value;
                }
            }
        }

        public T Get<T>() where T : IV3Service
        {
            return this.iocContainer.GetInstance<T>();
        }

        public void HandleSystemMessages(ISystemMessageCollection systemMessageCollection)
        {
            foreach (ISystemMessage systemMessage in systemMessageCollection.AllMessages)
            {
                if (systemMessage.Severity == SystemMessageSeverityEnum.Error
                    || systemMessage.Severity == SystemMessageSeverityEnum.Exception)
                {
                    SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent()).DomainMessages.Add(new SAHL.Common.DomainMessages.Error(systemMessage.Message, systemMessage.Message));
                }

                if (systemMessage.Severity == SystemMessageSeverityEnum.Warning)
                {
                    SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent()).DomainMessages.Add(new SAHL.Common.DomainMessages.Warning(systemMessage.Message, systemMessage.Message));
                }

                if (systemMessage.Severity == SystemMessageSeverityEnum.Info)
                {
                    SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent()).DomainMessages.Add(new SAHL.Common.DomainMessages.Information(systemMessage.Message, systemMessage.Message));
                }
            }
        }

    }
}

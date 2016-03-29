using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Service.Interfaces;
using SAHL.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SAHL.Common.Service.LeadImportService
{
    [FactoryType(typeof(ILeadImportPublisherService), LifeTime = FactoryTypeLifeTime.Singleton)]
    public class PersonalLoanImportService : LeadImportPublisherService
    {
        public PersonalLoanImportService()
            : base(new BatchPublisher(HttpContext.Current.Application["IMessagebus"] as IMessageBus, new MessageBusDefaultConfiguration()), new CsvFileParse(), RepositoryFactory.GetRepository<IBatchServiceRepository>())
        {

        }

    }
}

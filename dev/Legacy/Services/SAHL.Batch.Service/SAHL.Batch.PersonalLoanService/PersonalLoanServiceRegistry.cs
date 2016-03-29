using SAHL.Batch.Service.Contracts;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;
using System;

namespace SAHL.Batch.PersonalLoanService
{
    public class PersonalLoanServiceRegistry : Registry
    {
        public PersonalLoanServiceRegistry()
        {
            try
            {
                ObjectFactory.Configure(x =>
                {
                    //For<IApplicationUnsecuredLendingRepository>().Singleton().Use(RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>());
                    //For<IPersonalLoanLeadCreationService>().Singleton().Use<PersonalLoanLeadCreation>();
                });
            }
            catch (Exception ex)
            {
                ILogger logger = ObjectFactory.GetInstance<ILogger>();
                logger.LogErrorMessageWithException(string.Format("{0}", this.GetType()), "IOC Config Error", ex);
            }
        }
    }
}
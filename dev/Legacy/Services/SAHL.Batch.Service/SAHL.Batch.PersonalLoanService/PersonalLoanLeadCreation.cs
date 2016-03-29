using SAHL.Batch.Service.Contracts;
using SAHL.Common.BusinessModel.Config;
using SAHL.Common.BusinessModel.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Configuration;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Batch.PersonalLoanService
{
    public class PersonalLoanLeadCreation : IPersonalLoanLeadCreationService
    {
        private IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;

        public PersonalLoanLeadCreation(IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository)
        {
            this.applicationUnsecuredLendingRepository = applicationUnsecuredLendingRepository;
        }

        public PersonalLoanLeadCreation() 
            : this(RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>())
        {
        }

        public void CreatePersonalLoanLeadFromIdNumber(string IDNumber)
        {
            applicationUnsecuredLendingRepository.CreatePersonalLoanLead(IDNumber);
        }

    }
}
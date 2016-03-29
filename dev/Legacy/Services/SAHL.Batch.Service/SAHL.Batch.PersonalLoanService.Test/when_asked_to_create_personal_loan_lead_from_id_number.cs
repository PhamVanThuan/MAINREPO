using Machine.Fakes;
using Machine.Specifications;
using SAHL.Batch.Service.Contracts;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Batch.PersonalLoanService.Test
{
    [Subject(typeof(PersonalLoanLeadCreation))]
    public class when_asked_to_create_personal_loan_lead_from_id_number : WithFakes
    {
        private static IPersonalLoanLeadCreationService personalLoanLeadCreationService;
        private static IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;

        private Establish context = () =>
        {
            applicationUnsecuredLendingRepository = An<IApplicationUnsecuredLendingRepository>();
            personalLoanLeadCreationService = new PersonalLoanLeadCreation(applicationUnsecuredLendingRepository);
        };

        private Because of = () =>
        {
            personalLoanLeadCreationService.CreatePersonalLoanLeadFromIdNumber(Param.IsAny<string>());
        };

        private It should_create_personal_loan_lead = () =>
        {
            applicationUnsecuredLendingRepository.WasToldTo(x => x.CreatePersonalLoanLead(Param.IsAny<string>()));
        };
    }
}
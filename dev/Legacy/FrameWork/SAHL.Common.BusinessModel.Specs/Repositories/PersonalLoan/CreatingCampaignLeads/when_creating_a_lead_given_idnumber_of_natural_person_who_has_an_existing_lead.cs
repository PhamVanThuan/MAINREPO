using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Specs.Repositories.PersonalLoan.CreatingCampaignLeads
{
    [Subject(typeof(ApplicationUnsecuredLendingRepository))]
    public class when_creating_a_lead_given_idnumber_of_natural_person_who_has_an_existing_lead : WithFakes
    {
        private static ILegalEntityRepository legalEntityRepository;
        private static ILegalEntityNaturalPerson legalEntityNaturalPerson;
        private static IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;
        private static IApplicationRepository applicationRepository;
        private static IRuleService ruleService;

        private Establish context = () =>
        {
            legalEntityNaturalPerson = An<ILegalEntityNaturalPerson>();
            legalEntityNaturalPerson.WhenToldTo(le => le.Key).Return(Param.IsAny<int>());

            legalEntityRepository = An<ILegalEntityRepository>();
            legalEntityRepository.WhenToldTo(le => le.GetNaturalPersonByIDNumber(Param.IsAny<string>())).Return(legalEntityNaturalPerson);

            applicationRepository = An<IApplicationRepository>();

            ruleService = An<IRuleService>();
            ruleService.Stub(_ =>
                _.ExecuteRule(
                      Arg<IDomainMessageCollection>.Is.Anything
                    , Arg.Is<string>("CheckUniquePersonalLoanApplication")
                    , Arg<object[]>.Is.Anything
                    )).Return(0)
                    .WhenCalled(_ =>
                    {
                        var msgCollection = (DomainMessageCollection)_.Arguments[0];
                        msgCollection.Add(new SAHL.Common.DomainMessages.Error("Personal Loan Lead already exists for the selected legal entity", ""));
                    }
            );

            applicationUnsecuredLendingRepository =
                new ApplicationUnsecuredLendingRepository(
                        applicationRepository,
                        An<ILookupRepository>(),
                        An<ICreditCriteriaUnsecuredLendingRepository>(),
                        An<IMarketRateRepository>(),
                        legalEntityRepository,
                        An<ICastleTransactionsService>(),
                        ruleService);
        };

        private Because of = () =>
        {
            applicationUnsecuredLendingRepository.CreatePersonalLoanLead(Param.IsAny<string>());
        };

        private It should_find_the_currently_existing_lead_for_this_natural_person = () =>
        {
             IList<object[]> argumentsSentToExecuteRule = 
                 ruleService.GetArgumentsForCallsMadeOn(rs => rs.ExecuteRule(
                                                                               Arg<IDomainMessageCollection>.Is.Anything
                                                                             , Arg.Is<string>("CheckUniquePersonalLoanApplication")
                                                                             , Arg<object[]>.Is.Anything
                                                                            )
                                                         );

             var messages = (DomainMessageCollection)argumentsSentToExecuteRule[0][0];
             messages.Count.ShouldEqual(1);
        };

        private It should_not_create_any_new_lead = () =>
        {
            applicationRepository.WasNotToldTo(x => x.CreateUnsecuredLendingLead());
        };
    }
}
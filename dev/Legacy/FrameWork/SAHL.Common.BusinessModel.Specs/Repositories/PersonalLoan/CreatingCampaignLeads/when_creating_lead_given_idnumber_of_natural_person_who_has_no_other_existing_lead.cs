using Machine.Fakes;
using Machine.Specifications;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Specs.Repositories.PersonalLoan.CreatingCampaignLeads
{
    [Subject(typeof(ApplicationUnsecuredLendingRepository))]
    public class when_creating_lead_given_idnumber_of_natural_person_who_has_no_other_existing_lead : WithFakes
    {
        private static ILegalEntityRepository legalEntityRepository;
        private static ILegalEntityNaturalPerson legalEntityNaturalPerson;
        private static IApplicationRepository applicationRepository;
        private static IApplicationSource campaignLeadSource;
        private static IEventList<IApplicationSource> applicationSource;
        private static ILookupRepository lookupRepository;
        private static IApplicationUnsecuredLendingRepository applicationUnsecuredLendingRepository;
        private static IApplicationUnsecuredLending personalLoanApplication;
        private static IRuleService ruleService;
        private static int messageCountBeforeRuleExecution;

        private Establish context = () =>
        {
            personalLoanApplication = An<IApplicationUnsecuredLending>();
            personalLoanApplication.WhenToldTo(app => app.Key).Return(Param.IsAny<int>());

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(appRepo => appRepo.CreateUnsecuredLendingLead()).Return(personalLoanApplication);

            legalEntityNaturalPerson = An<ILegalEntityNaturalPerson>();
            legalEntityNaturalPerson.WhenToldTo(le => le.Key).Return(Param.IsAny<int>());
            legalEntityNaturalPerson.WhenToldTo(le => le.IDNumber).Return(Param.IsAny<string>());

            legalEntityRepository = An<ILegalEntityRepository>();
            legalEntityRepository.WhenToldTo(le => le.GetNaturalPersonByIDNumber(Param.IsAny<string>())).Return(legalEntityNaturalPerson);

            ruleService = An<IRuleService>();

            ruleService.WhenToldTo(rsvc =>
                rsvc.ExecuteRule(
                          Param.IsAny<IDomainMessageCollection>()
                        , Arg.Is<string>("CheckUniquePersonalLoanApplication")
                        , Param.IsAny<object[]>())
                        ).Return(1);

            campaignLeadSource = An<IApplicationSource>();
            applicationSource = An<IEventList<IApplicationSource>>();
            applicationSource.WhenToldTo(x => x.ObjectDictionary[Param.IsAny<string>()]).Return(campaignLeadSource);

            lookupRepository = An<ILookupRepository>();
            lookupRepository.WhenToldTo(x => x.ApplicationSources).Return(applicationSource);

            applicationUnsecuredLendingRepository =
                new ApplicationUnsecuredLendingRepository(
                        applicationRepository,
                        lookupRepository,
                        An<ICreditCriteriaUnsecuredLendingRepository>(),
                        An<IMarketRateRepository>(),
                        legalEntityRepository,
                        An<ICastleTransactionsService>(),
                        ruleService);

            // Need this cache object to check the count before and after the rule is executed
            var principalCache = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            var messages = principalCache.DomainMessages;
            messageCountBeforeRuleExecution = messages.Count;
        };

        public class MockApplicationSource : IApplicationSource
        {
            public string Description { get; set; }

            public IGeneralStatus GeneralStatus { get; set; }

            public int Key { get; set; }

            public bool ValidateEntity()
            {
                return false;
            }

            public object Clone()
            {
                return new object();
            }

            public void Refresh()
            {
            }
        }

        private Because of = () =>
        {
            applicationUnsecuredLendingRepository.CreatePersonalLoanLead(legalEntityNaturalPerson.IDNumber);
        };

        private It should_not_find_any_other_lead_for_this_natural_person = () =>
        {
            // NULL is used to indicate it can be any value in RhinoMocks, it's so non intuitive
            IList<object[]> argumentsSentToExecuteRule =
                 ruleService.GetArgumentsForCallsMadeOn(rs =>
                     rs.ExecuteRule(
                                      Arg<IDomainMessageCollection>.Is.Anything
                                    , Arg.Is<string>("CheckUniquePersonalLoanApplication")
                                    , Arg<object[]>.Is.Anything
                                    )
                 );

            var messages = (DomainMessageCollection)argumentsSentToExecuteRule[0][0];
            // get message count before rule is executed and compare with count after execution
            messages.Count.ShouldEqual(messageCountBeforeRuleExecution);
        };

        private It should_create_a_personal_loan_campaign_lead = () =>
        {
            applicationRepository.WasToldTo(x => x.CreateUnsecuredLendingLead());
        };

        private It should_distinctly_mark_the_lead_for_differentiating_it_from_other_types_of_leads = () =>
        {
            personalLoanApplication.AssertWasCalled(x => x.ApplicationSource = campaignLeadSource);
        };

        private It should_store_the_personal_loan_campaign_lead = () =>
        {
            applicationRepository.WasToldTo(x => x.SaveApplication(personalLoanApplication));
        };

        private It should_add_person_as_a_client_on_the_newly_created_lead = () =>
        {
            legalEntityRepository.WasToldTo(x => x.InsertExternalRole(ExternalRoleTypes.Client, personalLoanApplication.Key, GenericKeyTypes.Offer, legalEntityNaturalPerson.Key, false));
        };

        private It should_create_workflow_case = () =>
        {
            applicationRepository.WasToldTo(x => x.CreatePersonalLoanWorkflowCase(personalLoanApplication.Key));
        };
    }
}
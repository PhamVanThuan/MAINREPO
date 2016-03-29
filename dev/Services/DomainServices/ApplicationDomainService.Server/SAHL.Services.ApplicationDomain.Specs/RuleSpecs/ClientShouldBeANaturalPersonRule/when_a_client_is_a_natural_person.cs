using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ClientMustBelongToApplication
{
    public sealed class when_a_client_is_a_natural_person : WithFakes
    {
        private static ClientShouldBeANaturalPersonRule rule;
        private static ISystemMessageCollection messages;
        private static IDomainQueryServiceClient domainQueryService;
        private static ApplicantDeclarationsModel applicantDeclarations;

        private Establish context = () =>
        {
            applicantDeclarations = new ApplicantDeclarationsModel(1234, 67, DateTime.MinValue,
                new DeclaredInsolventDeclarationModel(OfferDeclarationAnswer.No, null),
                new UnderAdministrationOrderDeclarationModel(OfferDeclarationAnswer.No, DateTime.MinValue),
                new CurrentlyUnderDebtCounsellingReviewDeclarationModel(OfferDeclarationAnswer.No, null),
                new PermissionToConductCreditCheckDeclarationModel(OfferDeclarationAnswer.Yes));
            domainQueryService = An<IDomainQueryServiceClient>();
            messages = new SystemMessageCollection();
            rule = new ClientShouldBeANaturalPersonRule(domainQueryService);
            domainQueryService.WhenToldTo(c => c.PerformQuery(Param.IsAny<IsClientANaturalPersonQuery>())).Return<IsClientANaturalPersonQuery>(y =>
            {
                y.Result = new ServiceQueryResult<IsClientANaturalPersonQueryResult>(new IsClientANaturalPersonQueryResult[] {
                            new IsClientANaturalPersonQueryResult{ ClientIsANaturalPerson = true } }
                            );
                return messages;
            });
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicantDeclarations);
        };

        private It should_check_if_the_client_is_a_natural_person = () =>
        {
            domainQueryService.WasToldTo(c => c.PerformQuery(Param.IsAny<IsClientANaturalPersonQuery>()));
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
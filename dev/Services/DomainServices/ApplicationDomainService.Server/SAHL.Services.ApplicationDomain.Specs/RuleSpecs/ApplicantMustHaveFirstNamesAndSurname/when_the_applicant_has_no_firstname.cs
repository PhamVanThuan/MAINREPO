using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantMustHaveFirstNamesAndSurname
{
    public class when_the_applicant_has_no_firstname : WithFakes
    {
        private static ApplicantMustHaveFirstNamesAndSurnameRule rule;
        private static IDomainQueryServiceClient domainQueryClient;
        private static AddLeadApplicantToApplicationCommand command;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            domainQueryClient = An<IDomainQueryServiceClient>();
            rule = new ApplicantMustHaveFirstNamesAndSurnameRule(domainQueryClient);
            command = new AddLeadApplicantToApplicationCommand(CombGuid.Instance.Generate(), 1234, 5678, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            domainQueryClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientDetailsQuery>())).Return<GetClientDetailsQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetClientDetailsQueryResult>(new GetClientDetailsQueryResult[] {
                            new GetClientDetailsQueryResult{ LegalEntityKey = 1234, FirstNames = null, Surname = "Speed", IDNumber = "8211045229080",
                            DateOfBirth = DateTime.Now.AddYears(-30), EmailAddress = "clintons@sahomeloans.com", Cellphone = "0827702555", FaxCode = string.Empty,
                            FaxNumber = string.Empty, HomePhone = "031", HomePhoneCode = "7655528", WorkPhoneCode = string.Empty, WorkPhone = string.Empty,
                            LegalEntityType = (int)LegalEntityType.NaturalPerson } }
                            );
                return SystemMessageCollection.Empty();
            });
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_use_the_domain_query_service_to_get_the_client = () =>
        {
            domainQueryClient.WasToldTo(c => c.PerformQuery(Arg.Is<GetClientDetailsQuery>(y => y.ClientKey == command.ClientKey)));
        };

        private It should_return_a_message_for_the_firstname = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("A natural person applicant must have a first name.");
        };
    }
}
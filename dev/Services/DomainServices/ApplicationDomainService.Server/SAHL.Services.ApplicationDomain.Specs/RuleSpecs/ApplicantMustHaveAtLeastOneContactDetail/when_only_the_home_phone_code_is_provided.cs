using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicantMustHaveAtLeastOneContactDetail
{
    public class when_only_the_home_phone_code_is_provided : WithFakes
    {
        private static ApplicantMustHaveAtLeastOneContactDetailRule rule;
        private static IDomainQueryServiceClient domainQueryClient;
        private static ISystemMessageCollection messages;
        private static AddLeadApplicantToApplicationCommand command;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            command = new AddLeadApplicantToApplicationCommand(Guid.NewGuid(), 1234, 5678, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            domainQueryClient = An<IDomainQueryServiceClient>();
            domainQueryClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientDetailsQuery>())).Return<GetClientDetailsQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetClientDetailsQueryResult>(new GetClientDetailsQueryResult[] {
                            new GetClientDetailsQueryResult{ LegalEntityKey = 1234, FirstNames = null, Surname = string.Empty, IDNumber = "8211045229080",
                            DateOfBirth = DateTime.Now.AddYears(-30), EmailAddress = string.Empty, Cellphone = "    ", FaxCode = string.Empty,
                            FaxNumber = string.Empty, HomePhone = "031", HomePhoneCode = string.Empty, WorkPhoneCode = string.Empty, WorkPhone = string.Empty,
                            LegalEntityType = (int)LegalEntityType.NaturalPerson } }
                            );
                return SystemMessageCollection.Empty();
            });
            rule = new ApplicantMustHaveAtLeastOneContactDetailRule(domainQueryClient);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_return_an_error_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("An applicant requires at least one valid contact detail (An Email Address, Home, Work or Cell Number).");
        };
    }
}
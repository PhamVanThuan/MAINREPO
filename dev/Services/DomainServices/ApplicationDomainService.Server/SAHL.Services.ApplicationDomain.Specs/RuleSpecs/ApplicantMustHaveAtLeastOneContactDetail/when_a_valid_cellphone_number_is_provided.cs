using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
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
    public class when_a_valid_cellphone_number_is_provided : WithFakes
    {
        private static ApplicantMustHaveAtLeastOneContactDetailRule rule;
        private static AddLeadApplicantToApplicationCommand command;
        private static ISystemMessageCollection messages;
        private static int clientKey;
        private static int applicationNumber;
        private static IDomainQueryServiceClient domainQueryClient;

        private Establish context = () =>
        {
            domainQueryClient = An<IDomainQueryServiceClient>();
            domainQueryClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientDetailsQuery>())).Return<GetClientDetailsQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetClientDetailsQueryResult>(new GetClientDetailsQueryResult[] {
                            new GetClientDetailsQueryResult{ LegalEntityKey = 1234, FirstNames = null, Surname = string.Empty, IDNumber = "8211045229080",
                            DateOfBirth = DateTime.Now.AddYears(-30), EmailAddress = string.Empty, Cellphone = "0827702555", FaxCode = string.Empty,
                            FaxNumber = string.Empty, HomePhone = null, HomePhoneCode = null, WorkPhoneCode = string.Empty, WorkPhone = string.Empty,
                            LegalEntityType = (int)LegalEntityType.NaturalPerson } }
                            );
                return SystemMessageCollection.Empty();
            });
            applicationNumber = 12345;
            clientKey = 6789;
            messages = SystemMessageCollection.Empty();
            command = new AddLeadApplicantToApplicationCommand(Guid.NewGuid(), 1234, 5678, LeadApplicantOfferRoleTypeEnum.Lead_MainApplicant);
            rule = new ApplicantMustHaveAtLeastOneContactDetailRule(domainQueryClient);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, command);
        };

        private It should_return_no_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
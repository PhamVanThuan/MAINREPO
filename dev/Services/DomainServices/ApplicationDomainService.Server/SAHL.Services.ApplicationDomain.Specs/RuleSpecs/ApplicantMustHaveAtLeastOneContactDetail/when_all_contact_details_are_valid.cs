using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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
    public class when_all_contact_details_are_valid : WithFakes
    {
        private static ApplicantMustHaveAtLeastOneContactDetailRule rule;
        private static IDomainQueryServiceClient domainQueryClient;
        private static ISystemMessageCollection messages;
        private static AddLeadApplicantToApplicationCommand command;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            command = new AddLeadApplicantToApplicationCommand(Guid.NewGuid(), 1234, 5678, LeadApplicantOfferRoleTypeEnum.Lead_Suretor);
            domainQueryClient = An<IDomainQueryServiceClient>();
            domainQueryClient.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientDetailsQuery>())).Return<GetClientDetailsQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetClientDetailsQueryResult>(new GetClientDetailsQueryResult[] {
                            new GetClientDetailsQueryResult{ LegalEntityKey = 1234, FirstNames = null, Surname = string.Empty, IDNumber = "8211045229080",
                            DateOfBirth = DateTime.Now.AddYears(-30), EmailAddress = "clintons@sahomeloans.com", Cellphone = "0827702555", FaxCode = string.Empty,
                            FaxNumber = string.Empty, HomePhone = "031", HomePhoneCode = "7655528", WorkPhoneCode = "011", WorkPhone = "1122334",
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

        private It should_fetch_the_client_for_the_key_provided = () =>
        {
            domainQueryClient.WasToldTo(x => x.PerformQuery(Arg.Is<GetClientDetailsQuery>(y => y.ClientKey == command.ClientKey)));
        };

        private It should_not_return_error_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Model;
using SAHL.Services.Interfaces.DomainQuery.Queries;

namespace SAHL.Services.ApplicationDomain.Specs.RuleSpecs.ApplicationMailingAddressCannotBeAFreeTextAddress
{
    public class when_client_address_is_in_postnet_format : WithFakes
    {
        private static ApplicationMailingAddressCannotBeAFreeTextAddressRule rule;
        private static ISystemMessageCollection messages;
        private static ApplicationMailingAddressModel model;
        private static IDomainQueryServiceClient domainQueryService;
        private static ISystemMessageCollection clientQueryMessageCollection;

        private Establish context = () =>
        {
            domainQueryService = An<IDomainQueryServiceClient>();
            clientQueryMessageCollection = An<ISystemMessageCollection>();
            messages = An<ISystemMessageCollection>();
            model = new ApplicationMailingAddressModel(1, 1, Core.BusinessModel.Enums.CorrespondenceLanguage.English,
                Core.BusinessModel.Enums.OnlineStatementFormat.PDFFormat, Core.BusinessModel.Enums.CorrespondenceMedium.Post,
                null, true);
            rule = new ApplicationMailingAddressCannotBeAFreeTextAddressRule(domainQueryService);
            domainQueryService.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetClientAddressQuery>())).Return<GetClientAddressQuery>(y =>
            {
                y.Result = new ServiceQueryResult<GetClientAddressQueryResult>(
                        new GetClientAddressQueryResult[] {
                            new GetClientAddressQueryResult{ AddressKey = 1234,  AddressFormatKey = (int)AddressFormat.PostNetSuite, ClientKey = 1234 } }
                            );
                return clientQueryMessageCollection;
            });
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_use_the_domain_query_service_to_get_the_client_address = () =>
        {
            domainQueryService.WasToldTo(x => x.PerformQuery(Arg.Is<GetClientAddressQuery>(y => y.ClientAddressKey == model.ClientAddressKey)));
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}
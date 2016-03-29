using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using SAHL.Services.Interfaces.PropertyDomain;
using SAHL.Services.Interfaces.PropertyDomain.Models;
using SAHL.Services.Interfaces.PropertyDomain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.OpenMortgageLoanForApplicantAndPropertyCannotExistSpecs
{
    public class when_no_property_can_be_found_for_the_addresses_provided : WithFakes
    {
        private static OpenMortgageLoanForApplicantAndPropertyCannotExistRule rule;
        private static IPropertyDomainServiceClient propertyDomainService;
        private static IApplicationDomainServiceClient applicationDomainService;
        private static ISystemMessageCollection messages;
        private static ApplicationCreationModel model;
        private static IEnumerable<GetPropertyByAddressQueryResult> emptyResult;

        private Establish context = () =>
        {
            emptyResult = Enumerable.Empty<GetPropertyByAddressQueryResult>();
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan, EmploymentType.Salaried);
            messages = SystemMessageCollection.Empty();
            propertyDomainService = An<IPropertyDomainServiceClient>();
            GetPropertyByAddressQuery propertyQuery = new GetPropertyByAddressQuery(model.PropertyAddress.UnitNumber, model.PropertyAddress.BuildingName, model.PropertyAddress.BuildingNumber,
                model.PropertyAddress.StreetNumber, model.PropertyAddress.StreetName, model.PropertyAddress.Suburb, model.PropertyAddress.City, model.PropertyAddress.Province,
                model.PropertyAddress.PostalCode, model.PropertyAddress.ErfNumber, model.PropertyAddress.ErfPortionNumber);
            propertyQuery.Result = new ServiceQueryResult<GetPropertyByAddressQueryResult>( emptyResult );
            propertyDomainService.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetPropertyByAddressQuery>()))
                .Return<GetPropertyByAddressQuery>(y =>
                {
                    y.Result = propertyQuery.Result; return SystemMessageCollection.Empty();
                });
            applicationDomainService = An<IApplicationDomainServiceClient>();
            rule = new OpenMortgageLoanForApplicantAndPropertyCannotExistRule(propertyDomainService, applicationDomainService);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_not_return_a_message = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_not_query_the_application_domain_for_any_open_applications = () =>
        {
            applicationDomainService.WasNotToldTo(x => x.PerformQuery(Arg.Any<DoesOpenApplicationExistForPropertyAndClientQuery>()));
        };

    }
}
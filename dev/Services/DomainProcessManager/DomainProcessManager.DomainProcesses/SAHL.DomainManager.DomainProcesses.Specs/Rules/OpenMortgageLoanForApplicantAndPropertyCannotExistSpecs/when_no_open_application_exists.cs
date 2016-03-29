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
    public class when_no_open_application_exists : WithFakes
    {
        private static OpenMortgageLoanForApplicantAndPropertyCannotExistRule rule;
        private static IPropertyDomainServiceClient propertyDomainService;
        private static IApplicationDomainServiceClient applicationDomainService;
        private static ISystemMessageCollection messages;
        private static ApplicationCreationModel model;
        private static ApplicantModel applicant1;
        private static ApplicantModel applicant2;
        private static List<AddressModel> addresses;

        private Establish context = () =>
        {
            addresses = ApplicationCreationTestHelper.PopulateAddresses();
            applicant1 = ApplicationCreationTestHelper.PopulateApplicantModel(addresses);
            applicant2 = ApplicationCreationTestHelper.PopulateApplicantModel(addresses);
            model = ApplicationCreationTestHelper.PopulateApplicationCreationModelWithApplicants(OfferType.RefinanceLoan, new List<ApplicantModel>() { applicant1, applicant2 });
            messages = SystemMessageCollection.Empty();
            propertyDomainService = An<IPropertyDomainServiceClient>();
            GetPropertyByAddressQuery propertyQuery = new GetPropertyByAddressQuery(model.PropertyAddress.UnitNumber, model.PropertyAddress.BuildingName, model.PropertyAddress.BuildingNumber,
                model.PropertyAddress.StreetNumber, model.PropertyAddress.StreetName, model.PropertyAddress.Suburb, model.PropertyAddress.City, model.PropertyAddress.Province,
                model.PropertyAddress.PostalCode, model.PropertyAddress.ErfNumber, model.PropertyAddress.ErfPortionNumber);
            propertyQuery.Result = new ServiceQueryResult<GetPropertyByAddressQueryResult>(new GetPropertyByAddressQueryResult[] { new GetPropertyByAddressQueryResult(123465) });
            propertyDomainService.WhenToldTo(c => c.PerformQuery(Param.IsAny<GetPropertyByAddressQuery>()))
                .Return<GetPropertyByAddressQuery>(y =>
                {
                    y.Result = propertyQuery.Result; return SystemMessageCollection.Empty();
                });
            applicationDomainService = An<IApplicationDomainServiceClient>();
            DoesOpenApplicationExistForPropertyAndClientQuery query = new DoesOpenApplicationExistForPropertyAndClientQuery(1234567, model.Applicants.First().IDNumber);
            query.Result = new ServiceQueryResult<bool>(new bool[] { false });
            applicationDomainService.WhenToldTo(c => c.PerformQuery(Param.IsAny<DoesOpenApplicationExistForPropertyAndClientQuery>()))
                .Return<DoesOpenApplicationExistForPropertyAndClientQuery>(y =>
                {
                    y.Result = query.Result; return SystemMessageCollection.Empty();
                });

            rule = new OpenMortgageLoanForApplicantAndPropertyCannotExistRule(propertyDomainService, applicationDomainService);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, model);
        };

        private It should_return_a_message = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_check_against_the_first_applicant = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformQuery(Arg.Is<DoesOpenApplicationExistForPropertyAndClientQuery>(y => y.ClientIDNumber == applicant1.IDNumber)));
        };

        private It should_check_against_the_second_applicant = () =>
        {
            applicationDomainService.WasToldTo(x => x.PerformQuery(Arg.Is<DoesOpenApplicationExistForPropertyAndClientQuery>(y => y.ClientIDNumber == applicant2.IDNumber)));
        };
    }
}
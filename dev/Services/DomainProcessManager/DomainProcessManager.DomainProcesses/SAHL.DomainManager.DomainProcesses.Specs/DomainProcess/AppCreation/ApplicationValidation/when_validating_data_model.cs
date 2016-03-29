using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.Mocks;
using SAHL.DomainProcessManager.DomainProcesses.Utilities;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.ApplicationDomain.Queries;

using System.Collections.Generic;
using SAHL.Services.Interfaces.PropertyDomain.Models;
using SAHL.Services.Interfaces.PropertyDomain.Queries;
using System;
using System.Linq;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.DomainProcess.AppCreation.ApplicationValidation
{
    public class when_validating_data_model : WithNewPurchaseDomainProcess
    {
        private static NewPurchaseApplicationCreationModel applicationCreationModel;
        private static string suppliedVendorCode;
        private static ISystemMessageCollection systemMessages;
        private static int propertyKey;
        private static string applicantIdNumber;

        private Establish context = () =>
        {
            systemMessages = SystemMessageCollection.Empty();
            propertyKey = 183843;
            suppliedVendorCode = "UnknownVendor";
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.NewPurchaseLoan) as NewPurchaseApplicationCreationModel;
            applicantIdNumber = applicationCreationModel.Applicants.First().IDNumber;
            applicationCreationModel.VendorCode = suppliedVendorCode;
            domainProcess.DataModel = applicationCreationModel;

            var mockPropertyDetail = ApplicationCreationTestHelper.PopulatePropertyAddressModel();
            PropertyDataModel mockProperty = new PropertyDataModel(null, null, null, null, null, mockPropertyDetail.BuildingName, mockPropertyDetail.BuildingNumber, mockPropertyDetail.City, null,
                null, mockPropertyDetail.ErfNumber, mockPropertyDetail.ErfPortionNumber, mockPropertyDetail.BuildingName, mockPropertyDetail.BuildingNumber, null,
                null, null, null);
            mockProperty.PropertyKey = propertyKey;

            var GetPropertyByAddressQueryResult = new ServiceQueryResult<GetPropertyByAddressQueryResult>(
                       new GetPropertyByAddressQueryResult[] { new GetPropertyByAddressQueryResult(mockProperty.PropertyKey) });
            propertyDomainService.WhenToldTo(ads => ads.PerformQuery(Param.IsAny<GetPropertyByAddressQuery>())).
                Return<GetPropertyByAddressQuery>(rqst =>
                {
                    rqst.Result = GetPropertyByAddressQueryResult;
                    return new SystemMessageCollection();
                });
            applicationDataManager.WhenToldTo(adm => adm.DoesSuppliedVendorExist(Param.Is<string>(suppliedVendorCode))).Return(false);            
            applicationStateMachine.WhenToldTo(asm => asm.SystemMessages).Return(systemMessages);
        };

        private Because of = () =>
        {
            domainProcess.ValidateApplication(applicationStateMachine, domainRuleManager);
        };

        private It should_ensure_a_minimum_of_one_lead_applicant = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<ApplicantsMustHaveUniqueIdentityNumbersRule>(), applicationCreationModel.OriginationSource));
        };

        private It should_prevent_duplicate_application_submission = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<OpenMortgageLoanForApplicantAndPropertyCannotExistRule>(), applicationCreationModel.OriginationSource));
        };

        private It should_prevent_processing_application_with_unknown_vendor = () =>
        {
            domainRuleManager.WasToldTo(x => x.RegisterRuleForContext(Param.IsAny<VendorMustExistForVendorCodeRule>(), applicationCreationModel.OriginationSource));
        };

        private It should_add_unknown_vendor_error_message = () =>
        {
            systemMessages.ErrorMessages().Any(m => m.Message == "Supplied vendor does not exist on SAHL database." && m.Severity == SystemMessageSeverityEnum.Error);
        };
    }
}
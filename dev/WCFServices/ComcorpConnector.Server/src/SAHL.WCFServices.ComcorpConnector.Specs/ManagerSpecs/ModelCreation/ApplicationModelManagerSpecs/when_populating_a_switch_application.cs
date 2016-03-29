using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.ApplicationModelManagerSpecs
{
    public class when_populating_a_switch_application : WithCoreFakes
    {
        private static ApplicationModelManager applicationModelManager;
        private static IApplicantModelManager applicantModelManager;
        private static IApplicationDebitOrderModelManager applicationDebitOrderModelManager;
        private static IAddressModelManager addressModelManager;
        private static IPropertyDataModelManager propertyDataModelManager;
        private static IApplicationMailingAddressModelManager applicationMailingAddressModelManager;
        private static Application comcorpApplication;
        private static BankAccountModel bankAccount;
        private static IValidationUtils validationUtils;
        private static SwitchApplicationCreationModel model;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            bankAccount = new BankAccountModel("632005", "ABSA Electronic", "777777", ACBType.Current, "Test", "System", true);
            comcorpApplication = IntegrationServiceTestHelper.SetupBasicApplication(MortgageLoanPurpose.Switchloan);
            applicationDebitOrderModelManager = An<IApplicationDebitOrderModelManager>();
            applicantModelManager = An<IApplicantModelManager>();
            addressModelManager = An<IAddressModelManager>();
            propertyDataModelManager = An<IPropertyDataModelManager>();
            applicationMailingAddressModelManager = An<IApplicationMailingAddressModelManager>();
            applicationDebitOrderModelManager.WhenToldTo(x => x.PopulateApplicationDebitOrder(Param.IsAny<List<ApplicantModel>>())).Return(
                new ApplicationDebitOrderModel(FinancialServicePaymentType.DebitOrderPayment, 1, bankAccount));
            applicationModelManager = new ApplicationModelManager(validationUtils, applicantModelManager, applicationDebitOrderModelManager, addressModelManager,
                propertyDataModelManager, applicationMailingAddressModelManager);
        };

        private Because of = () =>
        {
            model = applicationModelManager.PopulateSwitchApplicationCreationModel(comcorpApplication);  
        };

        private It populate_the_comcorp_property_details = () =>
        {
        };

    }
}
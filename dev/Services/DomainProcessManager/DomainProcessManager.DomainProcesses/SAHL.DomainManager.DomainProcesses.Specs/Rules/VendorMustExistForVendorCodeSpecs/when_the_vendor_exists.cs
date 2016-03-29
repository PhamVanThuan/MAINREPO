using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.SystemMessages;
using SAHL.DomainProcessManager.DomainProcesses.DomainProcess.Rules;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.Models;
using SAHL.Services.Interfaces.DomainProcessManager.Tests;
using System;
using System.Linq;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Rules.VendorMustExistForVendorCodeSpecs
{
    public class when_the_vendor_exists : WithFakes
    {
        private static VendorMustExistForVendorCodeRule rule;
        private static IApplicationDataManager applicationDataManager;
        private static ApplicationCreationModel applicationCreationModel;
        private static ISystemMessageCollection messages;

        private Establish context = () =>
        {
            messages = SystemMessageCollection.Empty();
            applicationDataManager = An<IApplicationDataManager>();
            applicationCreationModel = ApplicationCreationTestHelper.PopulateApplicationCreationModel(OfferType.SwitchLoan, EmploymentType.Salaried);
            applicationDataManager.WhenToldTo(x => x.DoesSuppliedVendorExist(applicationCreationModel.VendorCode)).Return(true);
            rule = new VendorMustExistForVendorCodeRule(applicationDataManager);
        };

        private Because of = () =>
        {
            rule.ExecuteRule(messages, applicationCreationModel);
        };

        private It should_return_no_error_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };
    }
}
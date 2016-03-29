using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.CapitecApplication;
using SAHL.Services.Capitec.Managers.CapitecApplication.Models;
using SAHL.Services.Capitec.Managers.Lookup;
using SAHL.Services.Interfaces.Capitec.Common;
using SAHL.Services.Interfaces.Capitec.ViewModels.Apply;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.CapitecApplicationRepositorySpecs
{
    public class when_creating_a_capitec_app_given_switch_and_has_error_messages : WithFakes
    {
        private static CapitecApplicationManager capitecApplicationRepository;
        private static SwitchLoanApplication switchLoanApplication;
        private static SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication = null;
        private static List<Applicant> applicants;
        private static ILookupManager lookupService;
        private static ICapitecApplicationDataManager dataManager;
        private static int applicationNumber;
        private static Enumerations.ApplicationStatusEnums applicationStatusEnum;
        private static ISystemMessageCollection messages;
        private static Guid applicationId;

        private Establish context = () =>
        {
            applicationId = Guid.NewGuid();
            messages = SystemMessageCollection.Empty();
            messages.AddMessage(new SystemMessage("error message", SystemMessageSeverityEnum.Error));
            messages.AddMessage(new SystemMessage("warning message", SystemMessageSeverityEnum.Warning));
            messages.AddMessage(new SystemMessage("info message", SystemMessageSeverityEnum.Info));
            messages.AddMessage(new SystemMessage("debug message", SystemMessageSeverityEnum.Debug));
            messages.AddMessage(new SystemMessage("exception message", SystemMessageSeverityEnum.Exception));
            lookupService = An<ILookupManager>();
            dataManager = An<ICapitecApplicationDataManager>();
            applicationNumber = 0;
            applicationStatusEnum = Enumerations.ApplicationStatusEnums.Decline;
            applicants = new List<Applicant>();

            switchLoanApplication = new SwitchLoanApplication(new SwitchLoanDetails(Guid.NewGuid(), Guid.NewGuid(), 0, 0, 0, 0, 0, 0, 240, false), applicants, Guid.NewGuid(), "1184050800000-0700");

            capitecApplicationRepository = new CapitecApplicationManager(lookupService, dataManager);
            var userBranchModel = new CapitecUserBranchMappingModel(Guid.Parse("{8579FA90-9108-4C00-918C-28F0D3ECEB7B}"), 
                Guid.Parse("{3C1FCC2F-FCB9-4A0F-A674-2A89ABC6CA83}"), "Sally", "South", 9898);
            dataManager.WhenToldTo(x => x.GetCapitecUserBranchMappingForApplication(applicationId)).Return(userBranchModel);
        };

        private Because of = () =>
        {
            capitecApplication = capitecApplicationRepository.CreateCapitecApplicationFromSwitchLoanApplication(applicationNumber, applicationStatusEnum
                , switchLoanApplication, messages, applicationId);
        };

        private It should_create_a_capitec_application = () =>
        {
            capitecApplication.ShouldNotBeNull();
        };

        private It should_have_a_declined_status = () =>
        {
            capitecApplication.ApplicationStatusKey.ShouldEqual((int)Enumerations.ApplicationStatusEnums.Decline);
        };

        private It should_only_contain_warning_messages = () =>
        {
            capitecApplication.Messages.Where(x => x == "warning message").Count().ShouldEqual<int>(1);
            capitecApplication.Messages.Where(x => x != "warning message").Count().ShouldEqual<int>(0);
        };
    }
}
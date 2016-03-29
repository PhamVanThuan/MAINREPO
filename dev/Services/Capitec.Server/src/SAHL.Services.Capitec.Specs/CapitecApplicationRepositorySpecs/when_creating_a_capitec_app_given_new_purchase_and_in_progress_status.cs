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

namespace SAHL.Services.Capitec.Specs.CapitecApplicationRepositorySpecs
{
    public class when_creating_a_capitec_app_given_new_purchase_and_in_progress_status : WithFakes
    {
        private static CapitecApplicationManager capitecApplicationRepository;
        private static NewPurchaseApplication newPurchaseApplication;
        private static SAHL.Services.Capitec.Models.Shared.CapitecApplication capitecApplication = null;
        private static List<Applicant> applicants;
        private static ILookupManager lookupService;
        private static ICapitecApplicationDataManager dataManager;
        private static Enumerations.ApplicationStatusEnums applicationStatusEnum;
        private static ISystemMessageCollection messages; // = An<ISystemMessageCollection>();
        private static int applicationNumber;
        private static Guid applicationId;

        private Establish context = () =>
        {
            messages = An<ISystemMessageCollection>();
            lookupService = An<ILookupManager>();
            dataManager = An<ICapitecApplicationDataManager>();
            applicationNumber = 0;
            applicationId = Guid.NewGuid();
            applicationStatusEnum = Enumerations.ApplicationStatusEnums.InProgress;
            applicants = new List<Applicant>();

            newPurchaseApplication = new NewPurchaseApplication(new NewPurchaseLoanDetails(Guid.NewGuid(), Guid.NewGuid(), 0, 0, 0, 0, 240, false), applicants, Guid.NewGuid(), "1184050800000-0700");

            capitecApplicationRepository = new CapitecApplicationManager(lookupService, dataManager);
            var userBranchModel = new CapitecUserBranchMappingModel(Guid.Parse("{8579FA90-9108-4C00-918C-28F0D3ECEB7B}"), 
                Guid.Parse("{3C1FCC2F-FCB9-4A0F-A674-2A89ABC6CA83}"), "Sally", "South", 9898);
            dataManager.WhenToldTo(x => x.GetCapitecUserBranchMappingForApplication(applicationId)).Return(userBranchModel);
        };

        private Because of = () =>
        {
            capitecApplication = capitecApplicationRepository.CreateCapitecApplicationFromNewPurchaseApplication(applicationNumber, applicationStatusEnum, newPurchaseApplication, messages, applicationId);
        };

        private It should_create_a_capitec_application = () =>
        {
            capitecApplication.ShouldNotBeNull();
        };

        private It should_have_a_declined_status = () =>
        {
            capitecApplication.ApplicationStatusKey.ShouldEqual((int)Enumerations.ApplicationStatusEnums.InProgress);
        };
    }
}
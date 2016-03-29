using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Specs.HandlerSpecs.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.MarketingOptionsModelManagerSpecs
{
    public class when_populating_a_complete_set_of_marketing_options : WithCoreFakes
    {
        private static MarketingOptionsModelManager modelManager;
        private static Applicant applicant;
        private static List<ApplicantMarketingOptionModel> marketingOptions;

        private Establish context = () =>
        {
            modelManager = new MarketingOptionsModelManager();
            applicant = IntegrationServiceTestHelper.PopulateComcorpApplicant(IntegrationServiceTestHelper.ComcorpApplicantType.MainApplicant,
                MaritalStatus.Single, false, false, false, false, true);
            applicant.MarketingTelemarketing = true;
            applicant.MarketingMarketing = true;
            applicant.MarketingConsumerLists = true;
            applicant.MarketingSMS = true;
            applicant.MarketingEmail = true;
        };

        private Because of = () =>
        {
            marketingOptions = modelManager.PopulateMarketingOptions(applicant);
        };

        private It should_contain_five_active_marketing_options = () =>
        {
            marketingOptions.Where(x => x.GeneralStatus == GeneralStatus.Active).Count().ShouldEqual(5);
        };

        private It should_contain_zero_inactive_marketing_options = () =>
        {
            marketingOptions.Where(x => x.GeneralStatus == GeneralStatus.Inactive).Count().ShouldEqual(0);
        };

        private It should_contain_the_telemarketing_option = () =>
        {
            marketingOptions.Where(x => x.MarketingOption == MarketingOption.Telemarketing && x.GeneralStatus == GeneralStatus.Active)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };

        private It should_contain_the_customer_lists_option = () =>
        {
            marketingOptions.Where(x => x.MarketingOption == MarketingOption.CustomerLists && x.GeneralStatus == GeneralStatus.Active)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };

        private It should_contain_the_email_option = () =>
        {
            marketingOptions.Where(x => x.MarketingOption == MarketingOption.Email && x.GeneralStatus == GeneralStatus.Active)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };

        private It should_contain_the_marketing_lists_option = () =>
        {
            marketingOptions.Where(x => x.MarketingOption == MarketingOption.Marketing && x.GeneralStatus == GeneralStatus.Active)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };

        private It should_contain_the_SMS_option = () =>
        {
            marketingOptions.Where(x => x.MarketingOption == MarketingOption.SMS && x.GeneralStatus == GeneralStatus.Active)
                .FirstOrDefault()
                .ShouldNotBeNull();
        };
    }
}
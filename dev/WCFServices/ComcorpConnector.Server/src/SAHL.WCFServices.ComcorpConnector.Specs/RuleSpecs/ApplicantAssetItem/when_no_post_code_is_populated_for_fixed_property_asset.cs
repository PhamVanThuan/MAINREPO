using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantContactDetails
{
    public sealed class when_no_post_code_is_populated_for_fixed_property_asset : WithCoreFakes
    {
        private static IDomainRuleManager<Applicant> domainRuleManager;
        private static Applicant applicant;
        private static List<AssetItem> assetItems;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<Applicant>();
            validationUtils = new ValidationUtils();
            messages = SystemMessageCollection.Empty();
            domainRuleManager.RegisterRule(new ApplicantAssetItemValidation(validationUtils));

            assetItems = new List<AssetItem>()
            {
                new AssetItem
                {
                    SAHLAssetDesc = "Fixed Property",
                    SAHLAssetValue = 200000,
                    DateAssetAcquired = DateTime.Now.AddYears(-2),
                    AssetPhysicalAddressLine1 = "Line 1",
                    AssetPhysicalAddressLine2 = "Clarendon Drive",
                    AssetPhysicalAddressSuburb = "Suburb",
                    AssetPhysicalAddressCity = "Durban",
                    AssetPhysicalAddressCode = " ",
                    AssetPhysicalProvince = "Kwazulu Natal",
                    AssetPhysicalCountry = "South Africa"
                }
            };

            applicant = new Applicant();
            applicant.FirstName = "craig";
            applicant.Surname = "fraser";
            applicant.AssetItems = assetItems;
        };

        private Because of = () =>
        {
            domainRuleManager.ExecuteRules(messages, applicant);
        };

        private It should_have_error_messages = () =>
        {
            messages.HasErrors.ShouldBeTrue();
        };

        private It should_have_the_expected_messages = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEndWith("Post Code is required for Fixed Property Asset address.");
        };
    }
}

using System;
using System.Collections.Generic;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;

namespace SAHL.WCFServices.ComcorpConnector.Specs.RuleSpecs.ApplicantContactDetails
{
    public sealed class when_all_address_fields_are_populated_for_fixed_property_asset : WithFakes
    {
        private static IDomainRuleManager<Applicant> domainRuleManager;
        private static ISystemMessageCollection messages;
        private static Applicant applicant;
        private static List<AssetItem> assetItems;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            domainRuleManager = new DomainRuleManager<Applicant>();
            validationUtils = An<IValidationUtils>();
            messages = SystemMessageCollection.Empty();
            domainRuleManager.RegisterRule(new ApplicantAssetItemValidation(validationUtils));

            assetItems = new List<AssetItem>()
            {
                new AssetItem
                {
                    SAHLAssetDesc = "Fixed Property",
                    SAHLAssetValue = 200000,
                    DateAssetAcquired = DateTime.Now.AddYears(-2),
                    AssetPhysicalAddressLine1 = "5",
                    AssetPhysicalAddressLine2 = "Clarendon Drive",
                    AssetPhysicalAddressSuburb = "Durban North",
                    AssetPhysicalAddressCity = "Durban",
                    AssetPhysicalAddressCode = "4051",
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

        private It should_have_no_error_messages = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_not_return_messages = () =>
        {
            messages.ErrorMessages().ShouldBeEmpty();
        };
    }
}

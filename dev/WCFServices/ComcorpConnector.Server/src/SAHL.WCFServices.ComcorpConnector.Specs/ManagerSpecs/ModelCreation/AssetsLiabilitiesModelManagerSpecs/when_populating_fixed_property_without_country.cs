using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Testing;
using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.WCFServices.ComcorpConnector.Specs.ManagerSpecs.ModelCreation.AssetsLiabilitiesModelManagerSpecs
{
    public class when_populating_fixed_property_without_country : WithCoreFakes
    {
        private static AssetsLiabilitiesModelManager modelManager;
        private static List<AssetItem> comcorpAssetItems;
        private static ApplicantAssetLiabilityModel result;
        private static AssetItem fixedPropertyAsset;
        private static IValidationUtils validationUtils;

        private Establish context = () =>
        {
            validationUtils = new ValidationUtils();
            comcorpAssetItems = new List<AssetItem>
            {
                new AssetItem
                {
                    SAHLAssetDesc="Fixed Property",
                    SAHLAssetValue=200000,
                    DateAssetAcquired=DateTime.Now.AddYears(-2),
                    AssetPhysicalAddressLine1="5",
                    AssetPhysicalAddressLine2="Clarendon Drive",
                    AssetPhysicalAddressSuburb="Durban North",
                    AssetPhysicalAddressCity="Durban",
                    AssetPhysicalAddressCode="4051",
                    AssetPhysicalProvince = "Kwazulu Natal",
                    AssetPhysicalCountry = ""
                },
                new AssetItem{SAHLAssetDesc="Listed Investments",AssetCompanyName = "Listed Investment Company Name", SAHLAssetValue=10000},
                new AssetItem{SAHLAssetDesc="UnListed Investments",AssetCompanyName = "UnListed Investment Company Name", SAHLAssetValue=4000},
                new AssetItem{SAHLAssetDesc="Other Asset", AssetDescription = "Other Asset Description",SAHLAssetValue=1000, AssetOutstandingLiability=500},
                new AssetItem{SAHLAssetDesc="Life Assurance",AssetCompanyName = "Life Assurance Company Name", SAHLAssetValue=15000}
            };

            fixedPropertyAsset = comcorpAssetItems.Where(x => x.SAHLAssetDesc == "Fixed Property").First();
            modelManager = new AssetsLiabilitiesModelManager(validationUtils);
        };

        private Because of = () =>
        {
            result = modelManager
                .PopulateAssets(comcorpAssetItems)
                .FirstOrDefault(x => x.AssetLiabilityType == AssetLiabilityType.FixedProperty);
        };

        private It should_set_country_to_South_Africa = () =>
        {
            result.Address.Country.ShouldEqual("South Africa");
        };
    }
}

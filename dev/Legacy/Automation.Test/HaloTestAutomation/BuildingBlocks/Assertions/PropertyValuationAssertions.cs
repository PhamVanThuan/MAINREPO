using Automation.DataAccess;
using BuildingBlocks.Services.Contracts;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class PropertyValuationAssertions
    {
        private static IAddressService addressService;
        private static IPropertyService propertyService;
        private static IValuationService valuationService;
        private static ICommonService commonService;
        private static readonly IX2WorkflowService x2Service;

        static PropertyValuationAssertions()
        {
            addressService = ServiceLocator.Instance.GetService<IAddressService>();
            propertyService = ServiceLocator.Instance.GetService<IPropertyService>();
            valuationService = ServiceLocator.Instance.GetService<IValuationService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
            x2Service = ServiceLocator.Instance.GetService<IX2WorkflowService>();
            propertyService = ServiceLocator.Instance.GetService<IPropertyService>();
        }

        public static void AssertProperty(Automation.DataModels.Property expectedProperty, Automation.DataModels.Address expectedAddress = null, Automation.DataModels.Valuation expectedValuation = null)
        {
            var savedProperty = propertyService.GetPropertyByPropertyKey(expectedProperty.PropertyKey);
            Assert.That((savedProperty.CompareTo(expectedProperty) == 1), "SavedProperty: {0}, ExpectedProperty: {1}",
            commonService.GetConcatenatedPropertyNameValues<Automation.DataModels.Property>(savedProperty),
            commonService.GetConcatenatedPropertyNameValues<Automation.DataModels.Property>(expectedProperty));

            if (expectedAddress != null)
            {
                var savedPropertyAddress = addressService.GetAddress(addresskey: savedProperty.AddressKey);
                Assert.That((savedPropertyAddress.CompareTo(expectedAddress) == 1), "SavedPropertyAddress: {0}, ExpectedPropertyAddress: {1}",
               commonService.GetConcatenatedPropertyNameValues<Automation.DataModels.Property>(savedProperty),
               commonService.GetConcatenatedPropertyNameValues<Automation.DataModels.Property>(expectedProperty));
            }

            if (expectedValuation != null)
            {
                var savedValuation = (from val in valuationService.GetValuations(savedProperty.PropertyKey)
                                      orderby val.ValuationKey descending
                                      select val).FirstOrDefault();

                Assert.That((savedValuation.CompareTo(expectedValuation) == 1), "SavedValuation: {0}, ExpectedValuation: {1}",
                commonService.GetConcatenatedPropertyNameValues<Automation.DataModels.Property>(savedProperty),
                commonService.GetConcatenatedPropertyNameValues<Automation.DataModels.Property>(expectedProperty));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="offerKey">OfferKey</param>
        /// <param name="requireValuationIndicator"></param>
        public static void AssertRequireValuationIndicatorValue(int offerKey, bool requireValuationIndicator)
        {
            QueryResults results = x2Service.GetlatestX2DataApplicationManagementRow(offerKey);
            Logger.LogAction(@"Asserting that the ValuationIndicator for ApplicationKey: {0} on the X2.X2data.Application_Managment table is set to {1}", offerKey, requireValuationIndicator);
            Assert.IsTrue(results.HasResults, "No X2.X2data.Application_Managment record exists for ApplicationKey: {0}", offerKey);
            Assert.AreEqual(requireValuationIndicator.ToString(), results.Rows(0).Column("RequireValuation").Value, "ValuationIndicator for ApplicationKey: {0} is not set to {1}", offerKey, requireValuationIndicator);
            results.Dispose();
        }

        /// <summary>
        /// Asserting that the latest valuation is a Lightstone Valuation and that the Valuation Date for for the latest Valuation record matches todays date
        /// </summary>
        /// <param name="offerKey">offer.offerkey</param>
        public static void AssertLatestLightstoneValuationRecord(int offerKey)
        {
            DateTime date = DateTime.Today;
            string expectedDate = date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
            var results = propertyService.GetLatestPropertyValuationData(offerKey);
            try
            {
                Logger.LogAction("Asserting that a Valuation record exists for Offer: {0}", offerKey);
                Assert.True(results.HasResults, "No Lightstone Valuation exists for Offer: {0}", offerKey);

                Logger.LogAction("Asserting that the Latest Valuation for Offer: {0} is a Lightstone valuation", offerKey);
                StringAssert.AreEqualIgnoringCase("Lightstone", results.Rows(0).Column("DataProvider").Value,
                    "The Latest Valuation for Offer: {0} is not a Lightstone valuation", offerKey);

                string actualDate = results.Rows(0).Column("Year").Value + results.Rows(0).Column("Month").Value + results.Rows(0).Column("Day").Value;
                Logger.LogAction("Asserting that the Valuation Date for for the latest Valuation for Offer: {0} matches todays date", offerKey);
                StringAssert.AreEqualIgnoringCase(expectedDate, actualDate,
                    "The Valuation Date for for the latest Valuation for Offer: {0} did not match todays date", offerKey);
            }
            finally
            {
                results.Dispose();
            }
        }

        public static void AssertLatestPhysicalLightstoneValuation(int propertykey, ValuationStatusEnum expectedStatus)
        {
            var valuation = (from val in valuationService.GetValuations(propertyKey: propertykey)
                             orderby val.ValuationKey descending
                             select val).FirstOrDefault();
            Assert.IsNotNull(valuation, "no valuation found for propertykey: {0} ,valuation status: {1}", propertykey, (int)expectedStatus);
            if (expectedStatus == ValuationStatusEnum.Complete)
                Assert.True(valuation.IsActive, "Valuation not set to active");
            Assert.AreEqual((int)valuation.ValuationStatusKey, (int)expectedStatus, "Expected valuation status: {0}, but was {1}", (int)expectedStatus, valuation.ValuationStatusKey);
            Assert.Greater(valuation.ValuationDate.Value, DateTime.Now.Subtract(TimeSpan.FromHours(6)));
        }

        public static void AssertLatestPhysicalLightstoneValuationXmlHistory(int generickey)
        {
            Assert.GreaterOrEqual(valuationService.GetValuationXmlHistory(generickey).Count(), 1);
        }
    }
}
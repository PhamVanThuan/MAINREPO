using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;

namespace BuildingBlocks.Assertions
{
    public static class HOCAssertions
    {
        private static IHOCService hocService;
        private static IValuationService valService;

        static HOCAssertions()
        {
            valService = ServiceLocator.Instance.GetService<IValuationService>();
            hocService = ServiceLocator.Instance.GetService<IHOCService>();
        }

        public static void HOCUpdatedToValuationHOC(Automation.DataModels.Valuation expectedValuation)
        {
            var hoc = hocService.GetHOCAccountByPropertyKey(expectedValuation.PropertyKey);
            Assert.AreEqual(hoc.HOCRoof, expectedValuation.HOCRoofDescription, "expected roof types to match");
            if (expectedValuation.HOCRoofKey == HOCRoofEnum.Thatch)
                Assert.AreEqual(expectedValuation.HOCThatchAmount, hoc.HOCThatchAmount, "Thatch amount on HOC does not match valuation");
            if (expectedValuation.HOCRoofKey == HOCRoofEnum.Conventional)
                Assert.AreEqual(expectedValuation.HOCConventionalAmount, hoc.HOCConventionalAmount, "Conventional amount on HOC does not match valuation");
        }

        public static void HOCNotUpdatedToValuationHOC(Automation.DataModels.Valuation notExpectedValuation)
        {
            var hoc = hocService.GetHOCAccountByPropertyKey(notExpectedValuation.PropertyKey);
            Assert.AreNotEqual(hoc.HOCRoof, notExpectedValuation.HOCRoofDescription, "expected roof type to not match");
            if (hoc.HOCRoof == HOCRoof.Thatch)
                Assert.AreNotEqual(notExpectedValuation.HOCThatchAmount, hoc.HOCThatchAmount, "Thatch amount on HOC matches valuation");
            if (hoc.HOCRoof == HOCRoof.Conventional)
                Assert.AreNotEqual(notExpectedValuation.HOCConventionalAmount, hoc.HOCConventionalAmount, "Conventional amount on HOC matches valuation");
        }
    }
}
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class ConvertStaffLoanView : ConvertStaffLoanControls
    {
        /// <summary>
        /// Clicks the Convert button.
        /// </summary>
        public void Convert()
        {
            base.Convert.Click();
        }

        /// <summary>
        /// Clicks the UnConvert button.
        /// </summary>
        public void UnConvert()
        {
            base.UnConvert.Click();
        }

        public void AssertUnconvertDisabled()
        {
            Assert.That(base.UnConvert.Enabled == false);
        }

        public void AssertConvertDisabled()
        {
            Assert.That(base.Convert.Enabled == false);
        }
    }
}
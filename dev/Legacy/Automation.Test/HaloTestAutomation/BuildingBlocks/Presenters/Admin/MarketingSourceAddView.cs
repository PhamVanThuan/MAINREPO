using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Presenters.Admin;

namespace BuildingBlocks.Presenters.Admin
{
    public class MarketingSourceAddView : MarketingSourceAddControls
    {
        public void AddMarketingSource(string description)
        {
            base.txtMarketingSourceDescription.Value = description;
            base.ddlMarketingSourceStatus.Select(GeneralStatusConst.Active);
            base.btnSubmit.Click();
        }

        public void UpdateMarketingSource(string description, string status)
        {
            if (!string.IsNullOrEmpty(description))
            {
                base.txtMarketingSourceDescription.Clear();
                base.txtMarketingSourceDescription.Value = description;
            }
            base.ddlMarketingSourceStatus.Select(status);
            base.btnSubmit.Click();
        }

        public void SelectMarketingSource(string description)
        {
            var tableRow = gridSelectMarketingSource(description);
            tableRow.Click();
        }

        public void AssertMarketingSourceDetailsDisplayed(string description, string status)
        {
            string actualDescription = base.txtMarketingSourceDescription.Value;
            string actualStatus = base.ddlMarketingSourceStatus.SelectedItem.ToString();
            StringAssert.AreEqualIgnoringCase(description, actualDescription, "Expected marketing description {0} it was {1}", description, actualDescription);
            StringAssert.AreEqualIgnoringCase(status, actualStatus, "Expected marketing description status to be {0} it was {1}", status, actualStatus);
        }

        public void ClickSubmit()
        {
            base.btnSubmit.Click();
        }

        public void AssertValidationDisplayed()
        {
            Assert.That(base.lblValidation.Exists);
        }

        public void ClearMarketingSourceDescription()
        {
            base.txtMarketingSourceDescription.Clear();
        }
    }
}
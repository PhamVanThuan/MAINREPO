using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using WatiN.Core;

namespace BuildingBlocks.Presenters.Life
{
    public class LifePolicyAdmin : LifePolicyAdminControls
    {
        private readonly IWatiNService watinService;

        public LifePolicyAdmin()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        public void ConfirmAddLifeDialog()
        {
            watinService.HandleConfirmationPopup(base.AddLifeButton);
        }

        public void CancelAddLifeDialog()
        {
            watinService.CancelConfirmationPopup(base.AddLifeButton);
        }

        public void ConfirmRemoveLifeDialog()
        {
            watinService.HandleConfirmationPopup(base.RemoveLifeButton);
        }

        public void ClickAddButton()
        {
            watinService.HandleConfirmationPopup(base.AddLifeButton);
            //base.AddLifeButton.Click();
        }

        public void ClickRemoveButton()
        {
            watinService.HandleConfirmationPopup(base.RemoveLifeButton);
            //base.RemoveLifeButton.Click();
        }

        public string SelectFirstLegalEntity(string idpassport = "")
        {
            var row = default(TableRow);
            if (!String.IsNullOrEmpty(idpassport))
            {
                row = base.AssuredLifeLegalEntities.FindRowInOwnTableRows(idpassport, 2);
                Assert.That(row != null, "Now rows found in the LegalEntity Grid on View:{0}, IdNumber:{0}", base.ViewName, idpassport);
            }
            else
            {
                row = base.AssuredLifeLegalEntities.FindRowInOwnTableRows((cell) =>
                {
                    if (!String.IsNullOrEmpty(cell.Text) && !cell.Text.Contains("ID/Passport"))
                        return true;
                    return false;
                }, 2);
            }
            return row.OwnTableCells[2].Text;
        }

        public void AssertInforcedConfirmationDialogMessage()
        {
            string confirmationMessage = watinService.HandleConfirmationPopupAndReturnConfirmationMessage(base.AddLifeButton);
            Assert.True(confirmationMessage.Contains("This Policy is Inforce"), "Expected Life Policy Inforced Message when removing life assured.");
        }

        public void AssertViewDisplayed(string viewname)
        {
            Assert.AreEqual(viewname, base.ViewName.Text, "Current view is not being displayed. Current View Displayed: {0}", base.ViewName.Text);
        }

        public void AssertOneAssuredLifeValidationMessage()
        {
            Assert.True(base.divValidationSummaryBody.Text.Contains("Assured Life must exist"), "Expected a validation message: At least one Assured Life must exist.");
        }

        public void AssertRecalculatePremiumsButton()
        {
            Assert.True(base.RecalculatePremiumsButton.Exists, "Expected Recalculate Premium Button");
        }

        public void AssertRemoveLifeButton()
        {
            Assert.True(base.AddLifeButton.Exists, "Expected Remove Life Button");
        }

        public void AssertAddLifeButton()
        {
            Assert.True(base.AddLifeButton.Exists, "Expected Add Life Button");
        }

        public void AssertPremiumCalculatorButton()
        {
            Assert.True(base.PremiumCalculatorButton.Exists, "Expected Premium Calculator Button");
        }

        public void ClickRecalculatePremium()
        {
            watinService.HandleConfirmationPopup(base.RecalculatePremiumsButton);
        }
    }
}
using BuildingBlocks.Assertions;
using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class LifePolicyClaim : LifePolicyClaimControls
    {
        public void PopulateView(string claimType, string status, DateTime claimDate)
        {
            base.ClaimType.Option(claimType).Select();
            base.ClaimStatus.Option(status).Select();
            base.ClaimStatusDate.Value = claimDate.ToString(Formats.DateFormat);
        }

        public void SelectClaim(string claimType, string status)
        {
            var rowByType = base.PolicyClaimGrid.FindRowInOwnTableRows(x => x.Text == claimType, 0);
            var rowByStatus = base.PolicyClaimGrid.FindRowInOwnTableRows(x => x.Text == status, 1);
            if (rowByType.IdOrName == rowByStatus.IdOrName)
                rowByStatus.Click();
            else
                Assert.Fail("Could not find row that match claim type:{0}, status:{1}", claimType, status);
        }

        public void ClickUpdate()
        {
            base.SubmitButton.Click();
        }

        public void ClickAdd()
        {
            base.SubmitButton.Click();
        }

        public void ClickCancel()
        {
            base.CancelButton.Click();
        }

        public void AssertClaimStatusses()
        {
            WatiNAssertions.AssertSelectListContents(base.ClaimStatus, new List<string>
                                                                {
                                                                    Common.Constants.ClaimStatus.Pending,
                                                                    Common.Constants.ClaimStatus.Settled,
                                                                    Common.Constants.ClaimStatus.Repudiated,
                                                                    Common.Constants.ClaimStatus.Invalid
                                                                });
        }

        public void AssertClaimTypes()
        {
            WatiNAssertions.AssertSelectListContents(base.ClaimType, new List<string>
                                                                {
                                                                    Common.Constants.ClaimType.DeathClaim,
                                                                    Common.Constants.ClaimType.DisabilityClaim,
                                                                    Common.Constants.ClaimType.RetrenchmentClaim
                                                                });
        }

        public void AssertClaimTypeReadonly()
        {
            Assert.True(base.ClaimTypeReadonly.Enabled, "Expected Create Life Policy claim type to be readonly, but was not");
        }

        public void AssertThatFutureDateCannotBeCaptured()
        {
            foreach (var error in base.listErrorMessages)
                Assert.True(error.Text.Contains("A future Claim Date must not be entered."), "Expected an error message future dates not allowed");
        }
    }
}
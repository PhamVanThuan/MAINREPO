using Common.Constants;
using NUnit.Framework;
using ObjectMaps.Pages;
using System;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class UnsecuredLoanSummary : UnsecuredLoanSummaryControls
    {
        public void AssertPendingLifePolicyClaimDisplayed(DateTime claimDate)
        {
            var expectedMessage = String.Format("A pending Life Policy Claim exists - Claim Date: {0}", claimDate.ToString(Formats.DateFormat)); ;
            Assert.True(base.PendingLifePolicyClaimHeading.Exists, "Expected pending life policy claim message to display, but did not.");
            Assert.AreEqual(expectedMessage, base.PendingLifePolicyClaimHeading.Text);
        }
    }
}
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_Form27 : Correspondence_Form27Controls
    {
        public void Populate(string username, string format)
        {
            base.UserName.Value = username;
        }

        public void AssertControlsValid()
        {
            Assert.True(base.UserName.Exists, "UserName does not exist.");
        }

        public void AssertValidationMessageExists()
        {
            // @"UserName eg.SAHL\VasnaR is Required"
            NUnit.Framework.Assert.True(base.divValidationSummaryBody.Text.Contains("UserName eg.SAHL\\VasnaR is Required"), "Username required validation did not display");
        }
    }
}
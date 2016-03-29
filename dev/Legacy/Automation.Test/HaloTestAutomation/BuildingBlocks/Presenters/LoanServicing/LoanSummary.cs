using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing
{
    public class LoanSummary : LoanSummaryControls
    {
        public void AssertSPVDescription(string expectedDescription)
        {
            Assert.AreEqual(base.SPVDescription.Text.TrimEnd().TrimStart(), expectedDescription);
        }
    }
}
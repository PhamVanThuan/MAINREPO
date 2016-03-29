using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_IncomeTaxStatement : Correspondence_IncomeTaxStatementControls
    {
        public void Populate(string taxPeriod)
        {
            base.TaxPeriod.Option(taxPeriod).Select();
        }

        public void AssertControlsValid()
        {
            NUnit.Framework.Assert.True(base.TaxPeriod.Exists, "Expected TaxPeriod dropdown");
        }
    }
}
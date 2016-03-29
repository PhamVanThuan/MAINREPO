using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_CancellationLetterAcknowledge : Correspondence_BondCancellationLetterControls
    {
        public void Populate(string cancellationType)
        {
            base.CancellationType.Option(cancellationType).Select();
        }

        public void AssertControlsValid()
        {
            NUnit.Framework.Assert.True(base.CancellationType.Exists, "Expected CancellationType dropdown");
        }
    }
}
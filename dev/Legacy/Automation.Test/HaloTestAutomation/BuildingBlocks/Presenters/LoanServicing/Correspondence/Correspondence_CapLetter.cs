using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_CapLetter : Correspondence_CapLetterControls
    {
        public void Populate(int accountNumber)
        {
            base.AccountNumber.Value = accountNumber.ToString();
        }

        public void AssertControlsValid()
        {
            var message = "";
            if (!base.AccountNumber.Exists)
                message += "Could not locate AccountNumber field";
            NUnit.Framework.Assert.AreEqual("", message, message);
        }
    }
}
using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.Correspondence
{
    public class Correspondence_SuperLoIntroductionLetter : Correspondence_SuperLoIntroductionLetterControls
    {
        public void Populate(string helpdeskUsername)
        {
            base.HelpDeskUser.Option(helpdeskUsername).Select();
        }

        public void AssertControlsValid()
        {
            Assert.True(base.HelpDeskUser.Exists, "HelpDeskUser does not exist.");
        }

        public void AssertValidationMessagesExist()
        {
            Assert.True(base.HelpDeskUser.Exists, "Could not locate Heldesk User dropdown list");
        }
    }
}
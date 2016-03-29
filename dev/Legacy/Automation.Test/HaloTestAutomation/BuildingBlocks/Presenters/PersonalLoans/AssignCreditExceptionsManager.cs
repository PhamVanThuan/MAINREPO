using BuildingBlocks.Assertions;
using NUnit.Framework;
using ObjectMaps.Presenters.PersonalLoans;
using System.Collections.Generic;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class AssignCreditExceptionsManager : AssignCreditExceptionsManagerControls
    {
        public void SelectCreditExceptionsManager(string user)
        {
            ddlExceptionsManager.Option(user).Select();
        }

        public void ClickSubmit()
        {
            SubmitButton.Click();
        }

        public void ClickCancel()
        {
            CancelButton.Click();
        }

        public void AssertViewDisplayed()
        {
            var expectedView = "WF_PL_AssignCreditExceptionsManager";
            Assert.AreEqual(expectedView, base.ViewName.Text, "Expected {0} to be displayed.", expectedView);
        }

        public void AssertUsersListOptions(List<string> expectedOptions, bool checkCountOfItems)
        {
            WatiNAssertions.AssertSelectListContents(base.ddlExceptionsManager, expectedOptions, checkCountOfItems);
        }
    }
}
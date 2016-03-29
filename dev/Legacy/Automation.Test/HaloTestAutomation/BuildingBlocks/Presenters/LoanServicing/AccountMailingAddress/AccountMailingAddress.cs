using BuildingBlocks.Assertions;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LoanServicing.AccountMailingAddress
{
    public class AccountMailingAddress : AccountMailingAddressControls
    {
        public void AssertCorrespondenceMediumValue(string expectedValue)
        {
            WatiNAssertions.AssertFieldText(expectedValue, base.lblCorrespondenceMedium);
        }

        public void AssertCorrespondenceLanguageValue(string expectedValue)
        {
            WatiNAssertions.AssertFieldText(expectedValue, base.lblCorrespondenceLanguage);
        }

        public void AssertOnlineStatementstatus(bool expectedValue)
        {
            WatiNAssertions.AssertCheckboxValue(expectedValue, base.chkOnlineStatement);
        }

        public void AssertOnlineStatmentFormatValue(string expectedValue)
        {
            WatiNAssertions.AssertFieldText(expectedValue, base.OnlineStatementFormat);
        }

        public void AssertMailingAddressValue(string expectedValue)
        {
            WatiNAssertions.AssertFieldText(expectedValue, base.AddressLineDisp);
        }

        public void AssertEmailAddressValue(string expectedValue)
        {
            WatiNAssertions.AssertFieldTextContains(expectedValue, base.lblCorrespondenceMailAddress);
        }
    }
}
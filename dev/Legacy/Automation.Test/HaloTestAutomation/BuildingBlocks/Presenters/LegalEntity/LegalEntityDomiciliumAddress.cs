using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;
using WatiN.Core;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityDomiciliumAddress : ObjectMaps.Pages.LegalEntityDomiciliumAddressControls
    {
        public void SelectDomiciliumAddress(string formattedAddress)
        {
            var rows = base.DomiciliumAddresses.OwnTableRows.AsEnumerable<TableRow>();
            var matchingRow = (from r in rows
                               where r.Text.Contains(formattedAddress)
                               select r).FirstOrDefault();
            Assert.NotNull(matchingRow, "could not locate formatted address on the view.");
            matchingRow.RadioButtons.FirstOrDefault().Checked = true;
        }

        public void AssertLegalEntityAddressesDisplayed(IEnumerable<Automation.DataModels.LegalEntityAddress> validAddressFormats)
        {
            foreach (var address in validAddressFormats)
            {
                var passed = Assertions.WatiNAssertions.AssertTableRowsContainsText(address.DelimitedAddress, base.DomiciliumAddresses);
                Assert.True(passed, "One or more addresses displayed on the update legal entity domicilium address view was not expected");
            }
        }

        public void AssertFormattedAddressDisplayed(string formattedAddress)
        {
            var passed = Assertions.WatiNAssertions.AssertTableRowsContainsText(formattedAddress, base.DomiciliumAddresses);
            Assert.True(passed, string.Format(@"Address '{0}' was not displayed on the update legal entity domicilium address view when it was expected to.", formattedAddress));
        }

        public void ClickSubmit()
        {
            base.SubmitButton.Click();
        }

        public void AssertLegalEntityAddressesAreNotDisplayed(IEnumerable<Automation.DataModels.LegalEntityAddress> inValidAddressFormats)
        {
            foreach (var address in inValidAddressFormats)
            {
                var failed = !Assertions.WatiNAssertions.AssertTableRowsContainsText(address.DelimitedAddress, base.DomiciliumAddresses);
                Assert.True(failed, "One or more addresses are displayed on the update legal entity domicilium address view was not expected");
            }
        }

        public void AssertFormattedAddressNotDisplayed(string formattedAddress)
        {
            var passed = Assertions.WatiNAssertions.AssertTableRowsContainsText(formattedAddress, base.DomiciliumAddresses);
            Assert.False(passed, string.Format(@"Address '{0}' was displayed on the update legal entity domicilium address view when it was not expected to.", formattedAddress));
        }
    }
}
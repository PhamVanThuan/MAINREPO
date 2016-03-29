using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;

namespace BuildingBlocks.Assertions
{
    public static class AttorneyAssertions
    {
        private static IAttorneyService attorneyService;

        static AttorneyAssertions()
        {
            attorneyService = ServiceLocator.Instance.GetService<IAttorneyService>();
        }

        public static void AssertAttorneyInvoice(int accountkey, string expectedinvoicenumber = "", int attorneyKey = 0,
                decimal expectedamount = 0, string expectedcomments = "", decimal expectedVAT = 0, decimal expectedTotal = 0,
                DateTime? invoiceDate = null)
        {
            Thread.Sleep(2500);
            var accountInvoices = attorneyService.GetAttorneyInvoices(accountkey);

            var invoice = (from inv in accountInvoices
                           where
                               inv.InvoiceNumber == expectedinvoicenumber && inv.AttorneyKey == attorneyKey && inv.Amount == expectedamount
                               && inv.Comment == expectedcomments && inv.VatAmount == expectedVAT && inv.TotalAmount == expectedTotal
                               && inv.InvoiceDate.Date == invoiceDate.Value.Date
                           select inv).FirstOrDefault();

            Assert.That(invoice != null, String.Format(@"There is no attorney invoice record for account:{0}, expected the following:
                                                                InvoiceNumber={1}
                                                                Attorney={2}
                                                                Amount={3}
                                                                Comments={4}", accountkey, expectedinvoicenumber,
                                                           attorneyKey, expectedamount, expectedcomments));
        }

        /// <summary>
        /// Assert an attorney
        /// </summary>
        /// <param name="expectedAttorney"></param>
        public static void AssertAttorneyRecord(Automation.DataModels.Attorney expectedAttorney)
        {
            var savedAttoney = attorneyService.GetAttorneyRecord(expectedAttorney.LegalEntity.RegisteredName);
            //Assert Attorney Record
            Assert.AreEqual(expectedAttorney.ContactName, savedAttoney.ContactName, "Contact Name does not match.");
            Assert.AreEqual(expectedAttorney.DeedsOffice, savedAttoney.DeedsOffice, "Deeds Office does not match.");
            Assert.AreEqual(expectedAttorney.IsLitigationAttorney, savedAttoney.IsLitigationAttorney, "Is Litigation Attorney Indicator does not match.");
            Assert.AreEqual(expectedAttorney.IsRegistrationAttorney, savedAttoney.IsRegistrationAttorney, "Is Registration Attorney Indicator does not match.");
            Assert.AreEqual(expectedAttorney.Mandate, savedAttoney.Mandate, "Mandate Value does not match");
        }

        /// <summary>
        /// Assert an attorney contact
        /// </summary>
        /// <param name="attorneyName"></param>
        /// <param name="expectedAttorneyContact"></param>
        public static void AssertAttorneyContactRecord(Automation.DataModels.AttorneyContacts expectedAttorneyContact)
        {
            var savedAttoneyContactRecord = attorneyService.GetAttorneyContactRecord(expectedAttorneyContact.LegalEntity.FirstNames,
                                                                                               expectedAttorneyContact.LegalEntity.Surname);
            //Assert Attorney Contact Record
            Assert.AreEqual(savedAttoneyContactRecord.LegalEntity.Surname, expectedAttorneyContact.LegalEntity.Surname);
            Assert.AreEqual(savedAttoneyContactRecord.LegalEntity.WorkPhoneCode, expectedAttorneyContact.LegalEntity.WorkPhoneCode);
            Assert.AreEqual(savedAttoneyContactRecord.LegalEntity.WorkPhoneNumber, expectedAttorneyContact.LegalEntity.WorkPhoneNumber);
            Assert.AreEqual(savedAttoneyContactRecord.LegalEntity.FaxCode, expectedAttorneyContact.LegalEntity.FaxCode);
            Assert.AreEqual(savedAttoneyContactRecord.LegalEntity.FaxNumber, expectedAttorneyContact.LegalEntity.FaxNumber);
            Assert.AreEqual(savedAttoneyContactRecord.LegalEntity.EmailAddress, expectedAttorneyContact.LegalEntity.EmailAddress);
            Assert.AreEqual(savedAttoneyContactRecord.ExternalRoleType, expectedAttorneyContact.ExternalRoleType);
        }

        public static void AssertAttorneyInvoiceDoesNotExist
        (
            int accountkey,
            string expectedinvoicenumber = "",
            int attorneyKey = 0,
            decimal expectedamount = 0,
            string expectedcomments = "",
            decimal expectedVAT = 0,
            decimal expectedTotal = 0,
            DateTime? invoiceDate = null
        )
        {
            Thread.Sleep(2500);
            var accountInvoices = attorneyService.GetAttorneyInvoices(accountkey);

            var invoice = (from inv in accountInvoices
                           where
                               inv.InvoiceNumber == expectedinvoicenumber && inv.AttorneyKey == attorneyKey && inv.Amount == expectedamount
                               && inv.Comment == expectedcomments && inv.VatAmount == expectedVAT && inv.TotalAmount == expectedTotal
                               && inv.InvoiceDate.Date == invoiceDate.Value.Date
                           select inv).FirstOrDefault();

            Assert.That(invoice == null, String.Format(@"There an attorney invoice record for account:{0}, we expected the following:
                                                                InvoiceNumber={1}
                                                                Attorney={2}
                                                                Amount={3}
                                                                Comments={4} to be deleted.", accountkey, expectedinvoicenumber,
                                                           attorneyKey, expectedamount, expectedcomments));
        }
    }
}
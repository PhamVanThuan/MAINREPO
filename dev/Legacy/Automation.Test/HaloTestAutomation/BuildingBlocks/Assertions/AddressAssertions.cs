using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// Contains assertions for addresses/legal entity addresses
    /// </summary>
    public static class AddressAssertions
    {
        private static readonly IAddressService addressService;

        static AddressAssertions()
        {
            addressService = ServiceLocator.Instance.GetService<IAddressService>();
        }

        /// <summary>
        /// This assertion will check if a residential address exists when provided with the details required
        /// </summary>
        /// <param name="streetNum">Street Number</param>
        /// <param name="streetName">Street Name</param>
        /// <param name="p">Province</param>
        /// <param name="suburb">Suburb</param>
        public static int AssertResidentialAddressRecordExists(string streetNum, string streetName, string p, string suburb)
        {
            Logger.LogAction(string.Format(@"Asserting that an address record exists for {0},{1},{2},{3}", streetNum, streetName, p, suburb));
            int addressKey = addressService.GetExistingResidentialAddress(streetNum, streetName, p, suburb);
            Assert.That(addressKey > 0, "No Address Record Found");
            return addressKey;
        }

        /// <summary>
        /// This assertion will check if the link between the Address and the LegalEntity has been correctly stored in the
        /// LegalEntityAddress table.
        /// </summary>
        /// <param name="legalEntityKey">LegalEntityKey</param>
        /// <param name="addressKey">AddressKey</param>
        public static void AssertAddressLegalEntityLinkByGeneralStatus(int legalEntityKey, int addressKey, GeneralStatusEnum gsKey)
        {
            Logger.LogAction(string.Format(@"Asserting that a LegalEntityAddress record exists for AddressKey={0} and LegalEntityKey = {1} with a
                                                general status of {2} exists",
               addressKey, legalEntityKey, gsKey.ToString()));
            Assert.True(addressService.IsAddressLegalEntityLinkByGeneralStatus(legalEntityKey, addressKey, gsKey),
                "The LegalEntityAddress relationship does not exist.");
        }

        /// <summary>
        /// This assertion will check if an address record matching the 5 free text lines provided exists in the database, if it does
        /// then it will pass and return the addressKey
        /// </summary>
        /// <param name="line1">Free Text Line 1</param>
        /// <param name="line2">Free Text Line 2</param>
        /// <param name="line3">Free Text Line 3</param>
        /// <param name="line4">Free Text Line 4</param>
        /// <param name="line5">Free Text Line 5</param>
        /// <param name="addressKey"></param>
        public static void AssertFreeTextAddressRecordExists(string line1, string line2, string line3, string line4, string line5,
            out int addressKey)
        {
            Logger.LogAction(string.Format(@"Asserting that a free text address record exists for: {0},{1},{2},{3},{4}",
                    line1, line2, line3, line4, line5));
            Assert.True(addressService.IsFreeTextAddress(line1, line2, line3, line4, line5, out addressKey), "No Address Record Found");
        }

        /// <summary>
        /// Ensures that the correct address type has been saved to the LegalEntityAddress table.
        /// </summary>
        /// <param name="_legalEntityKey">legalEntityKey</param>
        /// <param name="addressKey">addressKey</param>
        /// <param name="expectedAddressType">Expected Address Type</param>
        public static void AssertAddressType(int _legalEntityKey, int addressKey, string expectedAddressType)
        {
            Logger.LogAction(string.Format(@"Asserting that the address type is {0} in the LegalEntityAddress table for AddressKey={1},LegalEntityKey={2}",
               expectedAddressType, addressKey, _legalEntityKey));
            StringAssert.AreEqualIgnoringCase(expectedAddressType, addressService.GetAddressType(_legalEntityKey, addressKey), "Address Type is not as expected");
        }

        /// <summary>
        /// Ensures that a PO Box address record exists
        /// </summary>
        /// <param name="boxNumber"></param>
        /// <param name="postOffice"></param>
        /// <param name="addressKey"></param>
        public static void AssertPOBoxAddressRecordExists(string boxNumber, string postOffice, out int addressKey)
        {
            Logger.LogAction(string.Format(@"Asserting that a postal address record for PO Box {0}, {1} exists", boxNumber, postOffice));
            Assert.True(addressService.IsExistingPOBoxAddress(boxNumber, postOffice, out addressKey), "No Address Record Found");
        }

        /// <summary>
        /// Ensures that a Cluster Box address record exists
        /// </summary>
        /// <param name="clusterBox">Cluster Box Number</param>
        /// <param name="postOffice">Post Office</param>
        /// <param name="addressKey">AddressKey</param>
        public static void AssertClusterBoxAddressRecordExists(string clusterBox, string postOffice, out int addressKey)
        {
            Logger.LogAction(string.Format(@"Asserting that a postal address record for Cluster Box {0}, {1} exists", clusterBox, postOffice));
            Assert.True(addressService.IsExistingClusterBoxAddress(clusterBox, postOffice, out addressKey), "No Address Record Found");
        }

        /// <summary>
        /// Asserts that a Cluster Box address record exists
        /// </summary>
        /// <param name="postNetSuite">PostNet Suite Number</param>
        /// <param name="privateBag">private Bag</param>
        /// <param name="postOffice">Post Office</param>
        /// <param name="addressKey">AddressKey</param>
        public static void AssertPostNetAddressRecordExists(string postNetSuite, string privateBag, string postOffice, out int addressKey)
        {
            Logger.LogAction(string.Format(@"Asserting that a postal address record for PostNet Suite {0}, {1}, {2} exists", privateBag, postNetSuite,
                                            postOffice));
            Assert.True(addressService.IsExistingPostNetAddress(postNetSuite, privateBag, postOffice, out addressKey), "No Address Record Found");
        }

        /// <summary>
        /// Asserts that a private bag address exists
        /// </summary>
        /// <param name="privateBag">Private Bag Number</param>
        /// <param name="postOffice">Post Office</param>
        /// <param name="addressKey">out AddressKey if exists</param>
        public static void AssertPrivateBagAddressRecordExists(string privateBag, string postOffice, out int addressKey)
        {
            Logger.LogAction(string.Format(@"Asserting that a postal address record for Private Bag {0}, {1} exists", privateBag, postOffice));
            Assert.True(addressService.IsExistingPrivateBagAddress(privateBag, postOffice, out addressKey), "No Address Record Found");
        }

        /// <summary>
        /// Assert the address change date
        /// </summary>
        /// <param name="dateTime"></param>
        public static void AssertAddressChangeDateExists(int expectedAddressKey, DateTime expectedChangeDate)
        {
            var actualAddress = addressService.GetAddress(addresskey: expectedAddressKey);
            Assert.NotNull(actualAddress.ChangeDate,"Expected Address ChangeDate to not be null");
            var actualChangeDate = DateTime.Parse(actualAddress.ChangeDate.ToString());
            Assert.True(actualChangeDate >= expectedChangeDate);
        }
        /// <summary>
        /// Assert the UserId for an address
        /// </summary>
        /// <param name="addressKey"></param>
        /// <param name="userId"></param>
        public static void AssertAddressUserID(int expectedAddressKey, string userId)
        {
            var actualAddress = addressService.GetAddress(addresskey: expectedAddressKey);
            Assert.NotNull(actualAddress.UserID, "Expected UserID to not be null");
            Assert.AreEqual(actualAddress.UserID, userId);
        }
    }
}
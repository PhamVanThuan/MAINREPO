using Automation.DataAccess;
using Automation.DataModels;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Linq;
using WatiN.Core.Logging;

namespace BuildingBlocks.Assertions
{
    public static class LegalEntityAssertions
    {
        private static readonly ILegalEntityService legalEntityService;
        private static readonly ILegalEntityAddressService legalEntityAddressService;
        private static readonly IAddressService addressService;
        private static readonly ICommonService commonService;

        static LegalEntityAssertions()
        {
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
            legalEntityAddressService = ServiceLocator.Instance.GetService<ILegalEntityAddressService>();
            addressService = ServiceLocator.Instance.GetService<IAddressService>();
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// This method will assert that the assets or liabilities saved against the legal entity match the provided parameters
        /// Will also assert that assetliability is inactive when deleted.
        /// </summary>
        /// <param name="leIdNumber"></param>
        /// <param name="expectedAssetsAndLiability">see BuildingBlocks.AssetsAndLiabilities class</param>
        /// <param name="assertDelete">true if asserting a deleted asset or liability</param>
        public static void AssertAssetsAndLiabilities(string leIdNumber, Automation.DataModels.LegalEntityAssetLiabilities expectedAssetsAndLiability, bool assertDelete)
        {
            try
            {
                QueryResults savedAssetsAndLiabilities = legalEntityService.GetAssetsAndLiabilitiesByLegalEntityIdNumber(leIdNumber);
                int expectedAssetLiabilityTypeKey = (int)expectedAssetsAndLiability.AssetLiabilityTypeKey;
                var rows = savedAssetsAndLiabilities.FindRowByExpression<int>("AssetLiabilityTypeKey", true, expectedAssetLiabilityTypeKey);

                Assert.That(rows.Count > 0, "No rows returned");

                var row = rows[0];

                if (row == null)
                    throw new NUnit.Framework.AssertionException(String.Format("Could not locate Expected AssetLiabilityType:{0} for legalentity (ID:\"{1}\")", expectedAssetLiabilityTypeKey, leIdNumber));
                if (assertDelete)
                {
                    if (row.Column("GeneralStatusKey").Value.Equals("1"))
                        throw new NUnit.Framework.AssertionException(String.Format("AssetLiabilityType:{0} for legalentity (ID:\"{1}\") was not deleted.", expectedAssetLiabilityTypeKey, leIdNumber));
                    else
                        return;
                }

                switch (expectedAssetsAndLiability.AssetLiabilityTypeKey)
                {
                    #region FixedLongTermInvestment

                    case Common.Enums.AssetLiabilityTypeEnum.FixedLongTermInvestment:
                        {
                            if (row.Column("CompanyName").Value != expectedAssetsAndLiability.CompanyName)
                                throw new NUnit.Framework.AssertionException(String.Format("The FixedLongTermInvestment captured for LegalEntity(ID:\"{0}\") has no or an invalid company name", leIdNumber));
                            if (row.Column("LiabilityValue").Value != expectedAssetsAndLiability.LiabilityValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The FixedLongTermInvestment captured for LegalEntity(ID:\"{0}\") has no or an invalid liability value", leIdNumber));
                            break;
                        }

                    #endregion FixedLongTermInvestment

                    #region FixedProperty

                    case Common.Enums.AssetLiabilityTypeEnum.FixedProperty:
                        {
                            DateTime savedDate = DateTime.Parse(row.Column("Date").Value);
                            if (savedDate.ToShortDateString() != expectedAssetsAndLiability.DateAcquired.ToShortDateString())
                                throw new NUnit.Framework.AssertionException(String.Format("The FixedProperty captured for LegalEntity(ID:\"{0}\") has no or an invalid date acquired.", leIdNumber));

                            QueryResults addressResults = addressService.GetAddressesByStreetName(expectedAssetsAndLiability.Address.StreetName);

                            QueryResultsRow addressRow = addressResults.FindRowByExpression<string>("BuildingName", true, new string[] { expectedAssetsAndLiability.Address.BuildingName })[0];

                            if (addressRow.Count == 0)
                                throw new NUnit.Framework.AssertionException(String.Format("The FixedProperty captured for LegalEntity(ID:\"{0}\") has no or an invalid address", leIdNumber));
                            if (row.Column("AssetValue").Value != expectedAssetsAndLiability.AssetValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The FixedProperty captured for LegalEntity(ID:\"{0}\") has no or an invalid asset value", leIdNumber));
                            if (row.Column("LiabilityValue").Value != expectedAssetsAndLiability.LiabilityValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The FixedProperty captured for LegalEntity(ID:\"{0}\") has no or an invalid liability value", leIdNumber));
                            break;
                        }

                    #endregion FixedProperty

                    #region LiabilityLoan

                    case Common.Enums.AssetLiabilityTypeEnum.LiabilityLoan:
                        {
                            if (int.Parse(row.Column("AssetLiabilitySubTypeKey").Value) != (int)expectedAssetsAndLiability.AssetLiabilitySubTypeKey)
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilityLoan captured for LegalEntity(ID:\"{0}\") has no or an invalid LoanType", leIdNumber));
                            if (row.Column("CompanyName").Value != expectedAssetsAndLiability.CompanyName)
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilityLoan captured for LegalEntity(ID:\"{0}\") has no or an invalid Financial Institution", leIdNumber));
                            DateTime savedDate = DateTime.Parse(row.Column("Date").Value);
                            if (savedDate.ToShortDateString() != expectedAssetsAndLiability.DateRepayable.ToShortDateString())
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilityLoan captured for LegalEntity(ID:\"{0}\") has no or an invalid date repayable.", leIdNumber));
                            if (row.Column("Cost").Value != expectedAssetsAndLiability.Cost.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilityLoan captured for LegalEntity(ID:\"{0}\") has no or an invalid Installment/Cost value", leIdNumber));
                            if (row.Column("LiabilityValue").Value != expectedAssetsAndLiability.LiabilityValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilitySurety captured for LegalEntity(ID:\"{0}\") has no or an invalid liability value", leIdNumber));

                            break;
                        }

                    #endregion LiabilityLoan

                    #region LiabilitySurety

                    case Common.Enums.AssetLiabilityTypeEnum.LiabilitySurety:
                        {
                            if (row.Column("Description").Value != expectedAssetsAndLiability.OtherDescription)
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilitySurety captured for LegalEntity(ID:\"{0}\") has no or an invalid description", leIdNumber));
                            if (row.Column("AssetValue").Value != expectedAssetsAndLiability.AssetValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilitySurety captured for LegalEntity(ID:\"{0}\") has no or an invalid asset value", leIdNumber));
                            if (row.Column("LiabilityValue").Value != expectedAssetsAndLiability.LiabilityValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The LiabilitySurety captured for LegalEntity(ID:\"{0}\") has no or an invalid liability value", leIdNumber));
                            break;
                        }

                    #endregion LiabilitySurety

                    #region LifeAssurance

                    case Common.Enums.AssetLiabilityTypeEnum.LifeAssurance:
                        {
                            if (row.Column("CompanyName").Value != expectedAssetsAndLiability.CompanyName)
                                throw new NUnit.Framework.AssertionException(String.Format("The LifeAssurance captured for LegalEntity(ID:\"{0}\") has no or an invalid company name", leIdNumber));
                            if (row.Column("AssetValue").Value != expectedAssetsAndLiability.AssetValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The LifeAssurance captured for LegalEntity(ID:\"{0}\") has no or an invalid asset/surrender value", leIdNumber));
                            break;
                        }

                    #endregion LifeAssurance

                    #region ListedInvestments

                    case Common.Enums.AssetLiabilityTypeEnum.ListedInvestments:
                        {
                            if (row.Column("CompanyName").Value != expectedAssetsAndLiability.CompanyName)
                                throw new NUnit.Framework.AssertionException(String.Format("The ListedInvestments captured for LegalEntity(ID:\"{0}\") has no or an invalid company name", leIdNumber));
                            if (row.Column("AssetValue").Value != expectedAssetsAndLiability.AssetValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The ListedInvestments captured for LegalEntity(ID:\"{0}\") has no or an invalid asset value", leIdNumber));
                            break;
                        }

                    #endregion ListedInvestments

                    #region OtherAsset

                    case Common.Enums.AssetLiabilityTypeEnum.OtherAsset:
                        {
                            if (row.Column("Description").Value != expectedAssetsAndLiability.OtherDescription)
                                throw new NUnit.Framework.AssertionException(String.Format("The OtherAsset captured for LegalEntity(ID:\"{0}\") has no or an invalid description", leIdNumber));
                            if (row.Column("AssetValue").Value != expectedAssetsAndLiability.AssetValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The OtherAsset captured for LegalEntity(ID:\"{0}\") has no or an invalid asset value", leIdNumber));
                            if (row.Column("LiabilityValue").Value != expectedAssetsAndLiability.LiabilityValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The OtherAsset captured for LegalEntity(ID:\"{0}\") has no or an invalid liability value", leIdNumber));
                            break;
                        }

                    #endregion OtherAsset

                    #region UnlistedInvestments

                    case Common.Enums.AssetLiabilityTypeEnum.UnlistedInvestments:
                        {
                            if (row.Column("CompanyName").Value != expectedAssetsAndLiability.CompanyName)
                                throw new NUnit.Framework.AssertionException(String.Format("The UnlistedInvestments captured for LegalEntity(ID:\"{0}\") has no or an invalid company name", leIdNumber));
                            if (row.Column("AssetValue").Value != expectedAssetsAndLiability.AssetValue.ToString())
                                throw new NUnit.Framework.AssertionException(String.Format("The UnlistedInvestments captured for LegalEntity(ID:\"{0}\") has no or an invalid asset value", leIdNumber));
                            break;
                        }

                    #endregion UnlistedInvestments
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Assert legalentity citizentype match the exepected citizentype.
        /// </summary>
        /// <param name="TestBrowser"></param>
        /// <param name="idNumber"></param>
        /// <param name="exepectedCitizenTypeEnum"></param>
        public static void AssertLegalEntityCitizenType(string idNumber, CitizenTypeEnum exepectedCitizenTypeEnum)
        {
            int _savedCitizenType = int.Parse(legalEntityService.GetLegalEntityCitizenType(idNumber));
            int _exepectedCitizenType = (int)exepectedCitizenTypeEnum;
            if (_savedCitizenType != _exepectedCitizenType)
            {
                string message = String.Format("Legal Entity CitizenType of {0} expected, but was {1}", _exepectedCitizenType, _savedCitizenType);
                throw new NUnit.Framework.AssertionException(message);
            }
        }

        public static void AssertAllLegalEntityDetails(Automation.DataModels.LegalEntity expectedLegalentity)
        {
            var savedLegalEntity = legalEntityService.GetLegalEntity
                (
                    expectedLegalentity.IdNumber,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    expectedLegalentity.LegalEntityKey
                );

            Assert.That(savedLegalEntity != null, "No legalentity found for expected legalentitykey:{0}", expectedLegalentity.LegalEntityKey);

            //Compare the values of each property
            if (expectedLegalentity.CompareTo(savedLegalEntity) == 0)
                Assert.Fail("LegalEntityKey {0}, {1}", expectedLegalentity.LegalEntityKey, String.Join("\n", expectedLegalentity.CompareResults.ToArray()));
        }

        /// <summary>
        /// Check that a legalentity given the idnumber exist on provided account.
        /// </summary>
        /// <param name="expectedLegalEntityIdNumber"></param>
        /// <param name="accountkey"></param>
        public static void AssertLegalEntityRoles(string expectedLegalEntityIdNumber, int accountkey)
        {
            var legalentity = (from le in legalEntityService.GetLegalEntityRoles(accountkey)
                               where le.IDNumber.Equals(expectedLegalEntityIdNumber)
                               select le).FirstOrDefault();
            Assert.That(legalentity != null, "No roles exist for LegalEntity IdNumber:{0} on Account:{1}", expectedLegalEntityIdNumber, accountkey);
        }

        /// <summary>
        /// Check that a legalentity given the idnumber does not exist on provided account.
        /// </summary>
        /// <param name="expectedLegalEntityIdNumber"></param>
        /// <param name="accountkey"></param>
        public static void AssertNoLegalEntityRoles(string expectedLegalEntityIdNumber, int accountkey)
        {
            var legalentity = (from le in legalEntityService.GetLegalEntityRoles(accountkey)
                               where le.IDNumber.Equals(expectedLegalEntityIdNumber)
                               select le).FirstOrDefault();
            Assert.That(legalentity == null, "One or more roles exist for LegalEntity IdNumber:{0} on Account:{1}",
                                                                        expectedLegalEntityIdNumber, accountkey);
        }

        /// <summary>
        /// Checks that a legal entity with the ID number provided does not exist
        /// </summary>
        /// <param name="expectedLegalEntityIdNumber"></param>
        public static void AssertLegalEntityNotExist(string expectedLegalEntityIdNumber)
        {
            var legalentity = legalEntityService.GetLegalEntity(expectedLegalEntityIdNumber, "", "", "", 0);
            Assert.That(legalentity == null, "LegalEntity for IdNumber:{0} does exist.",
                                                                        expectedLegalEntityIdNumber);
        }

        /// <summary>
        /// Checks that the legal entity only plays a single role in the account provided.
        /// </summary>
        /// <param name="accountkey"></param>
        public static void AssertOneLegalEntityRoleLifeAccount(int accountkey)
        {
            var roleCount = (from le in legalEntityService.GetLegalEntityRoles(accountkey)
                             select le).Count();
            Assert.AreEqual(1, roleCount, "More than  legalentity role exist for account {0}", accountkey);
        }

        public static void AssertManyLegalEntityRolesLifeAccount(int accountkey)
        {
            var roleCount = (from le in legalEntityService.GetLegalEntityRoles(accountkey)
                             select le).Count();
            Assert.True(roleCount > 1, "One or no legalentity role exist for account {0}", accountkey);
        }

        /// <summary>
        /// Ensures that the expected subsidy provider has been saved.
        /// </summary>
        /// <param name="expectedSubsidyProvider"></param>
        public static void AssertSubsidyProviderSaved(Automation.DataModels.SubsidyProvider expectedSubsidyProvider)
        {
            var savedSubsidyProvider = legalEntityService.GetSubsidyProvider(expectedSubsidyProvider.LegalEntity.RegisteredName);
            Assert.AreEqual(1, savedSubsidyProvider.CompareTo(expectedSubsidyProvider), "Expected subsidy provider was not saved correctly.");
        }

        /// <summary>
        /// This will assert a legalentity and the address linked to the legalentity.
        /// </summary>
        /// <param name="expectedAddress"></param>
        public static void AssertLegalEntityAddress(Automation.DataModels.LegalEntity expectedLegalentity, Automation.DataModels.Address expectedAddress)
        {
            var savedlegalentityaddress = legalEntityAddressService.GetLegalEntityAddresses(expectedLegalentity.LegalEntityKey)
                                                        .Where(x => x.AddressFormatKey == expectedAddress.AddressFormatKey).FirstOrDefault();
            var savedAddress = addressService.GetAddress(expectedAddress.BuildingName, expectedAddress.BuildingNumber, expectedAddress.StreetName, expectedAddress.StreetNumber,
                                                                       expectedAddress.RRR_CountryDescription, expectedAddress.RRR_ProvinceDescription, expectedAddress.RRR_SuburbDescription, expectedAddress.RRR_CityDescription,
                                                                            expectedAddress.BoxNumber);
            var leAddress = String.Format(@"{0} {1} \n\r
                                            {2} {3} \n\r
                                            {4} \n\r
                                            {5} \n\r
                                            {6} \n\r
                                            {7} \n\r", expectedAddress.BuildingNumber,
                                                     expectedAddress.BuildingName,
                                                     expectedAddress.StreetNumber,
                                                     expectedAddress.StreetName,
                                                     expectedAddress.RRR_SuburbDescription,
                                                     expectedAddress.RRR_CityDescription,
                                                     expectedAddress.RRR_ProvinceDescription,
                                                     expectedAddress.RRR_CountryDescription);
            Assert.AreEqual(savedAddress.AddressKey,
                                savedlegalentityaddress.AddressKey, "expected address {0}, but was {1}", leAddress, savedAddress.FormattedAddress);
            Assert.NotNull(savedlegalentityaddress, "No address of format: {0} was saved for legalentitykey:{1}", expectedAddress.AddressFormatKey, expectedLegalentity.LegalEntityKey);
            Assert.AreEqual(expectedAddress.AddressFormatKey, savedAddress.AddressFormatKey, "");
        }

        public static void AssertLegalEntityPostalAddress(Automation.DataModels.LegalEntity expectedLegalentity, Automation.DataModels.Address expectedAddress)
        {
            var savedlegalentityaddress = legalEntityAddressService.GetLegalEntityAddresses(expectedLegalentity.LegalEntityKey)
                                                        .Where(x => x.AddressFormatKey == expectedAddress.AddressFormatKey).FirstOrDefault();
            var savedAddress = addressService.GetAddress(expectedAddress.BuildingName, expectedAddress.BuildingNumber, expectedAddress.StreetName, expectedAddress.StreetNumber,
                                                                       expectedAddress.RRR_CountryDescription, expectedAddress.RRR_ProvinceDescription, expectedAddress.RRR_SuburbDescription, expectedAddress.RRR_CityDescription,
                                                                            expectedAddress.BoxNumber, expectedAddress.RRR_PostalCode);

            var lePostalAddress = String.Format(@"P O Box {0}, {1}, {2}, {3}, {4}",
                                                     expectedAddress.BoxNumber,
                                                     expectedAddress.RRR_SuburbDescription,
                                                     expectedAddress.RRR_ProvinceDescription,
                                                     expectedAddress.RRR_PostalCode,
                                                     expectedAddress.RRR_CountryDescription);

            Assert.AreEqual(savedAddress.AddressKey, savedlegalentityaddress.AddressKey, "expected address {0}, but was {1}", lePostalAddress, savedAddress.FormattedAddress);
            Assert.NotNull(savedlegalentityaddress, "No address of format: {0} was saved for legalentitykey:{1}", expectedAddress.AddressFormatKey, expectedLegalentity.LegalEntityKey);
            Assert.AreEqual(expectedAddress.AddressFormatKey, savedAddress.AddressFormatKey, "");
        }

        public static void AssertLegalEntityDomicilium(Automation.DataModels.LegalEntityAddress legalEntityAddress, GeneralStatusEnum generalStatus)
        {
            var domiciliumAddresses = legalEntityAddressService.GetLegalEntityDomiciliumAddress(legalEntityAddress.LegalEntityKey, generalStatus);
            //should only be one active
            if (generalStatus == GeneralStatusEnum.Active)
                Assert.That(domiciliumAddresses.Count() == 1, string.Format(@"There are multiple active domicilium addresses for legal entity {0}. Expected only one.",
                    legalEntityAddress.LegalEntityKey));
            //should be the one we selected
            var domiciliumAddress = (from da in domiciliumAddresses select da).FirstOrDefault();
            Assert.That(domiciliumAddress.LegalEntityAddressKey == legalEntityAddress.LegalEntityAddressKey, "The LegalEntityKey on the active domicilium does not match the expected record.");
        }

        public static void AssertNoLegalEntityDomicilium(int legalEntityKey, GeneralStatusEnum generalStatus)
        {
            var domiciliumAddresses = legalEntityAddressService.GetLegalEntityDomiciliumAddress(legalEntityKey, generalStatus);
            Assert.AreEqual(domiciliumAddresses.Count(), 0);
        }

        public static void AssertLegalEntityRegisteredForSecureWebsite(int legalEntityKey)
        {
            var clientSecureWebsiteLogin = legalEntityService.GetClientSecureWebsiteLogin(legalEntityKey);
            Assert.That(clientSecureWebsiteLogin.Count() == 1, string.Format(@"No legal entity login record exists for legal entity {0}.", legalEntityKey));
        }

        public static void AssertLegalEntityLoginHasChanged(int legalEntityKey, string password)
        {
            var clientSecureWebsiteLogin = legalEntityService.GetClientSecureWebsiteLogin(legalEntityKey).FirstOrDefault();
            Assert.That(clientSecureWebsiteLogin.Password != password, string.Format(@"The legal entity login has not changed for legal entity {0}.", legalEntityKey));
        }

        public static void AssertLegalEntityChangeDateExists(int expectedLegalEntityKey, DateTime expecetedChangeDate)
        {
            var actualLegalEntity = legalEntityService.GetLegalEntity(legalentitykey: expectedLegalEntityKey);
            Assert.NotNull(actualLegalEntity.ChangeDate, "Expected Address ChangeDate to not be null");
            Assert.GreaterOrEqual(actualLegalEntity.ChangeDate.ToString(), DateTime.Now.ToString(Formats.DateTimeFormatSQL));
        }
        public static void AssertLegalEntityUserID(int expectedLegalEntityKey, string userId)
        {
            var actualLegalEntity = legalEntityService.GetLegalEntity(legalentitykey: expectedLegalEntityKey);
            Assert.NotNull(actualLegalEntity.UserID, "Expected UserID to not be null");
            Assert.AreEqual(actualLegalEntity.UserID, userId);
        }

        public static void AssertITCRecordExists(int expectedLegalEntityKey)
        {
            var ITCs = legalEntityService.GetITCs(expectedLegalEntityKey);
            var itc = (from i in ITCs select i).FirstOrDefault();
            Logger.LogAction(string.Format(@"Asserting that an ITC record exists against legal entity {0}", expectedLegalEntityKey));
            Assert.That(itc != null, string.Format(@"No ITC records exists against legal entity {0}", expectedLegalEntityKey));
        }

        public static void AssertDirtyLegalEntityUpdated(DirtyLegalEntityDetails dirtyLegalEntityDetails)
        {
            var actualLegalEntity
                = legalEntityService.GetLegalEntity(legalentitykey: dirtyLegalEntityDetails.LegalEntityKey);
            Assert.AreEqual(dirtyLegalEntityDetails.DateOfBirth, actualLegalEntity.DateOfBirth);
            Assert.AreEqual(dirtyLegalEntityDetails.FirstNames, actualLegalEntity.FirstNames);
            Assert.AreEqual(dirtyLegalEntityDetails.Surname, actualLegalEntity.Surname);
            Assert.AreEqual(dirtyLegalEntityDetails.Salutation, actualLegalEntity.SalutationKey);
        }
        public static void AssertDirtyLegalEntityNotUpdated(DirtyLegalEntityDetails dirtyLegalEntityDetails)
        {
            var actualLegalEntity
                = legalEntityService.GetLegalEntity(legalentitykey: dirtyLegalEntityDetails.LegalEntityKey);
            Assert.AreNotEqual(dirtyLegalEntityDetails.DateOfBirth, actualLegalEntity.DateOfBirth);
            Assert.AreNotEqual(dirtyLegalEntityDetails.FirstNames, actualLegalEntity.FirstNames);
            Assert.AreNotEqual(dirtyLegalEntityDetails.Surname, actualLegalEntity.Surname);
            Assert.AreNotEqual(dirtyLegalEntityDetails.Salutation, actualLegalEntity.SalutationKey);
        }
    }
}
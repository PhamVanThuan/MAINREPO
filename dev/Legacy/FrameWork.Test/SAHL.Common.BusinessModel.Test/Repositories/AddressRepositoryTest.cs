using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    /// <summary>
    /// Tests <see cref="IAddressRepository"/> methods.
    /// </summary>
    [TestFixture]
    public class AddressRepositoryTest : TestBase
    {
        private IAddressRepository _addressRepository = RepositoryFactory.GetRepository<IAddressRepository>();

        /// <summary>
        /// Tests the <see cref="IAddressRepository.AddressExists"/> method.
        /// </summary>
        [Test]
        public void AddressExists()
        {
            int addressKey = 0;

            using (new SessionScope())
            {
                string testData = "NUNITTEST";

                try
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

                    // box address
                    AddressBox_DAO addressBoxDao = DAODataConsistancyChecker.GetDAO<AddressBox_DAO>();
                    addressBoxDao.SaveAndFlush();
                    addressKey = addressBoxDao.Key;
                    IAddressBox addressBox = BMTM.GetMappedType<IAddressBox>(addressBoxDao);
                    AddressExistsHelper(addressBox, true);
                    addressBoxDao.DeleteAndFlush();
                    addressBox = BMTM.GetMappedType<IAddressBox>(DAODataConsistancyChecker.GetDAO<AddressBox_DAO>());
                    addressBox.BoxNumber = testData;
                    AddressExistsHelper(addressBox, false);

                    // cluster box address
                    AddressClusterBox_DAO addressClusterDao = DAODataConsistancyChecker.GetDAO<AddressClusterBox_DAO>();
                    addressClusterDao.SaveAndFlush();
                    addressKey = addressClusterDao.Key;
                    IAddressClusterBox addressCluster = BMTM.GetMappedType<IAddressClusterBox>(addressClusterDao);
                    AddressExistsHelper(addressCluster, true);
                    addressClusterDao.DeleteAndFlush();

                    addressCluster = BMTM.GetMappedType<IAddressClusterBox>(DAODataConsistancyChecker.GetDAO<AddressClusterBox_DAO>());
                    addressCluster.ClusterBoxNumber = testData;
                    AddressExistsHelper(addressCluster, false);

                    // free text address
                    AddressFreeText_DAO addressFreeTextDao = DAODataConsistancyChecker.GetDAO<AddressFreeText_DAO>();
                    addressFreeTextDao.SaveAndFlush();
                    addressKey = addressFreeTextDao.Key;
                    IAddressFreeText addressFreeText = BMTM.GetMappedType<IAddressFreeText>(addressFreeTextDao);
                    AddressExistsHelper(addressFreeText, true);
                    addressFreeTextDao.DeleteAndFlush();

                    addressFreeText = BMTM.GetMappedType<IAddressFreeText>(DAODataConsistancyChecker.GetDAO<AddressFreeText_DAO>());
                    addressFreeText.FreeText1 = testData;
                    AddressExistsHelper(addressFreeText, false);

                    // postnet suite address
                    AddressPostnetSuite_DAO addressPostnetDao = DAODataConsistancyChecker.GetDAO<AddressPostnetSuite_DAO>();
                    addressPostnetDao.SaveAndFlush();
                    addressKey = addressPostnetDao.Key;
                    IAddressPostnetSuite addressPostnet = BMTM.GetMappedType<IAddressPostnetSuite>(addressPostnetDao);
                    AddressExistsHelper(addressPostnet, true);
                    addressPostnetDao.DeleteAndFlush();

                    addressPostnet = BMTM.GetMappedType<IAddressPostnetSuite>(DAODataConsistancyChecker.GetDAO<AddressPostnetSuite_DAO>());
                    addressPostnet.PrivateBagNumber = testData;
                    AddressExistsHelper(addressPostnet, false);

                    // private bag address
                    AddressPrivateBag_DAO addressPrivateBagDao = DAODataConsistancyChecker.GetDAO<AddressPrivateBag_DAO>();
                    addressPrivateBagDao.SaveAndFlush();
                    addressKey = addressPrivateBagDao.Key;
                    IAddressPrivateBag addressPrivateBag = BMTM.GetMappedType<IAddressPrivateBag>(addressPrivateBagDao);
                    AddressExistsHelper(addressPrivateBag, true);
                    addressPrivateBagDao.DeleteAndFlush();
                    addressPrivateBag = BMTM.GetMappedType<IAddressPrivateBag>(DAODataConsistancyChecker.GetDAO<AddressPrivateBag_DAO>());
                    addressPrivateBag.PrivateBagNumber = testData;
                    AddressExistsHelper(addressPrivateBag, false);

                    // street address
                    AddressStreet_DAO addressStreetDao = DAODataConsistancyChecker.GetDAO<AddressStreet_DAO>();
                    addressStreetDao.SaveAndFlush();
                    addressKey = addressStreetDao.Key;
                    IAddressStreet addressStreet = BMTM.GetMappedType<IAddressStreet>(addressStreetDao);
                    AddressExistsHelper(addressStreet, true);
                    addressStreetDao.DeleteAndFlush();

                    addressStreet = BMTM.GetMappedType<IAddressStreet>(DAODataConsistancyChecker.GetDAO<AddressStreet_DAO>());
                    addressStreet.StreetName = testData;
                    AddressExistsHelper(addressStreet, false);

                    // reset address key so we don't do the finally deletion
                    addressKey = 0;
                }
                finally
                {
                    // this is just a fail-safe call
                    if (addressKey > 0)
                        DeleteRecord("Address", "AddressKey", addressKey.ToString());
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.AddressExists"/> method, using a null field.
        /// </summary>
        [Test]
        public void AddressExistsWithNullField()
        {
            using (new SessionScope())
            {
                IApplicationRepository repo = RepositoryFactory.GetRepository<IApplicationRepository>();

                string query = @"select top 1 AddressKey from [2AM].dbo.Address where AddressFormatKey = 1 and UnitNumber is null";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);
                int AddressKey = Convert.ToInt32(DT.Rows[0][0]);
                DT.Dispose();
                IAddress address = _addressRepository.GetAddressByKey(AddressKey);
                bool b = _addressRepository.AddressExists(ref address);
                Assert.That(b == true);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetAddressByKey"/> method.
        /// </summary>
        [Test, TestCaseSource(typeof(AddressRepositoryTest), "GetAddressTypesForTest")]
        public void GetAddressByKey(AddressesForTest addressUnderTest)
        {
            using (new SessionScope())
            {
                Type addressDAOType = addressUnderTest.DAOType;
                dynamic first_DAO = SAHL.Common.BusinessModel.DAO.Test.DAODataConsistencyChecker.GetFindFirst(addressDAOType);
                Type interfaceType = addressUnderTest.InterfaceType;
                IAddress address = _addressRepository.GetAddressByKey(first_DAO.Key);
                Assert.AreEqual(first_DAO.Key, address.Key);
                Assert.IsNotNull(address.GetType().GetInterfaces().Where(x => x == interfaceType).FirstOrDefault());
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetAddressFormatByKey"/> method.
        /// </summary>
        [Test]
        public void GetAddressFormatByKey()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IAddressFormat addressFormat = _addressRepository.GetAddressFormatByKey(AddressFormats.Street);
                Assert.IsNotNull(addressFormat);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetAddressTypeByKey"/> method.
        /// </summary>
        [Test]
        public void GetAddressTypeByKey()
        {
            using (new SessionScope(FlushAction.Never))
            {
                IAddressType addressType = _addressRepository.GetAddressTypeByKey((int)AddressTypes.Postal);
                Assert.IsNotNull(addressType);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetEmptyAddress"/> method.
        /// </summary>
        [Test, TestCaseSource(typeof(AddressRepositoryTest), "GetAddressFormatsToTest")]
        public void GetEmptyAddress(Type addressFormat)
        {
            using (new SessionScope(FlushAction.Never))
            {
                IAddress address = _addressRepository.GetEmptyAddress(addressFormat);
                Assert.IsNotNull(address);
                Assert.IsInstanceOf(addressFormat, address);
            }
        }

        private IEnumerable<Type> GetAddressFormatsToTest()
        {
            return new[] { typeof(IAddressBox), typeof(IAddressClusterBox), typeof(IAddressFreeText), typeof(IAddressPostnetSuite), typeof(IAddressPrivateBag), typeof(IAddressStreet) };
        }

        private IEnumerable<AddressesForTest> GetAddressTypesForTest()
        {
            return new[] {
                new AddressesForTest { InterfaceType = typeof(IAddressBox), DAOType = typeof(AddressBox_DAO) },
                new AddressesForTest { InterfaceType = typeof(IAddressClusterBox), DAOType = typeof(AddressClusterBox_DAO) },
                new AddressesForTest { InterfaceType = typeof(IAddressFreeText), DAOType = typeof(AddressFreeText_DAO) },
                new AddressesForTest { InterfaceType = typeof(IAddressPostnetSuite), DAOType = typeof(AddressPostnetSuite_DAO) },
                new AddressesForTest { InterfaceType = typeof(IAddressPrivateBag), DAOType = typeof(AddressPrivateBag_DAO) },
                new AddressesForTest { InterfaceType = typeof(IAddressStreet), DAOType = typeof(AddressStreet_DAO) }
            };
        }

        public class AddressesForTest
        {
            public Type InterfaceType { get; set; }

            public Type DAOType { get; set; }

            public override string ToString()
            {
                return this.InterfaceType.ToString();
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetForeignCountryPostOffice"/> method.
        /// </summary>
        [Test]
        public void GetForeignCountryPostOffice()
        {
            string sql = @"select top 1 c.CountryKey, po.PostOfficeKey
                from Country c
                inner join Province p on c.CountryKey = p.CountryKey
                inner join City ci on ci.ProvinceKey = p.ProvinceKey
                inner join PostOffice po on po.CityKey = ci.CityKey
                where c.CountryKey <> 1";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");

            int countryKey = Convert.ToInt32(dt.Rows[0]["CountryKey"]);
            int postOfficeKey = Convert.ToInt32(dt.Rows[0]["PostOfficeKey"]);
            dt.Dispose();

            using (new SessionScope())
            {
                IPostOffice postOffice = _addressRepository.GetForeignCountryPostOffice(countryKey);
                Assert.IsNotNull(postOffice);
                Assert.AreEqual(postOfficeKey, postOffice.Key);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetFailedLegalEntityAddressByKey"/> method.
        /// </summary>
        [Test]
        public void GetFailedLegalEntityAddressByKey()
        {
            int key;

            using (new SessionScope())
            {
                FailedLegalEntityAddress_DAO dao = FailedLegalEntityAddress_DAO.FindFirst();
                key = dao.Key;
            }

            using (new SessionScope())
            {
                IFailedLegalEntityAddress leAddress = _addressRepository.GetFailedLegalEntityAddressByKey(key);
                Assert.IsNotNull(leAddress);
                Assert.AreEqual(key, leAddress.Key);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetLegalEntityAddressByKey"/> method.
        /// </summary>
        [Test]
        public void GetLegalEntityAddressByKey()
        {
            int key;

            using (new SessionScope())
            {
                LegalEntityAddress_DAO dao = LegalEntityAddress_DAO.FindFirst();
                key = dao.Key;
            }

            using (new SessionScope())
            {
                ILegalEntityAddress leAddress = _addressRepository.GetLegalEntityAddressByKey(key);
                Assert.IsNotNull(leAddress);
                Assert.AreEqual(key, leAddress.Key);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetProvinceByKey"/> method.
        /// </summary>
        [Test]
        public void GetProvinceByKey()
        {
            int key;

            using (new SessionScope())
            {
                Province_DAO dao = Province_DAO.FindFirst();
                key = dao.Key;
            }

            using (new SessionScope())
            {
                IProvince province = _addressRepository.GetProvinceByKey(key);
                Assert.IsNotNull(province);
                Assert.AreEqual(key, province.Key);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetSuburbByKey"/> method.
        /// </summary>
        [Test]
        public void GetSuburbByKey()
        {
            int key;

            using (new SessionScope())
            {
                Suburb_DAO dao = Suburb_DAO.FindFirst();
                key = dao.Key;
            }

            using (new SessionScope())
            {
                ISuburb suburb = _addressRepository.GetSuburbByKey(key);
                Assert.IsNotNull(suburb);
                Assert.AreEqual(key, suburb.Key);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetPostOfficeByKey"/> method.
        /// </summary>
        [Test]
        public void GetPostOfficeByKey()
        {
            int key;

            using (new SessionScope())
            {
                PostOffice_DAO dao = PostOffice_DAO.FindFirst();
                key = dao.Key;
            }

            using (new SessionScope())
            {
                IPostOffice postOffice = _addressRepository.GetPostOfficeByKey(key);
                Assert.IsNotNull(postOffice);
                Assert.AreEqual(key, postOffice.Key);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.GetPostOfficesByPrefix"/> method.
        /// </summary>
        [Test]
        public void GetPostOfficesByPrefix()
        {
            using (new SessionScope())
            {
                string prefix = "a";
                IReadOnlyEventList<IPostOffice> postOffices = _addressRepository.GetPostOfficesByPrefix(prefix, 10);
                foreach (IPostOffice postOffice in postOffices)
                {
                    if (!postOffice.Description.ToLower().StartsWith(prefix))
                        Assert.Fail("PostOffice_DAO.FindByPrefix returned post office " + postOffice.Description + " when searching by prefix " + prefix);
                }

                // now search for a specific item and make sure it comes back
                PostOffice_DAO postOfficeDao = PostOffice_DAO.FindFirst();
                int postOfficeKey = postOfficeDao.Key;
                prefix = postOfficeDao.Description.Substring(0, postOfficeDao.Description.Length - 2);
                postOffices = _addressRepository.GetPostOfficesByPrefix(prefix, -1);
                foreach (IPostOffice postOffice in postOffices)
                {
                    if (postOffice.Key == postOfficeKey)
                        return;
                }

                // if we got here, the postoffice wasn't found and the test fails
                Assert.Fail("GetPostOfficesByPrefix didn't return post office {0} using prefix {1}", postOfficeDao.Description, prefix);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.SearchAddresses"/> method.
        /// </summary>
        [Test]
        public void SearchAddresses()
        {
            using (new SessionScope())
            {
                IAddressSearchCriteria criteria = new AddressSearchCriteria();

                AddressBox_DAO addressBox = AddressBox_DAO.FindFirst();
                criteria.BoxNumber = addressBox.BoxNumber;
                if (addressBox.PostOffice != null)
                    criteria.PostOfficeKey = addressBox.PostOffice.Key;
                SearchAddressesHelper(addressBox, criteria);

                criteria = new AddressSearchCriteria();
                AddressClusterBox_DAO addressCluster = AddressClusterBox_DAO.FindFirst();
                criteria.ClusterBoxNumber = addressCluster.ClusterBoxNumber;
                if (addressCluster.PostOffice != null)
                    criteria.PostOfficeKey = addressCluster.PostOffice.Key;
                SearchAddressesHelper(addressCluster, criteria);

                criteria = new AddressSearchCriteria();
                AddressFreeText_DAO addressFree = AddressFreeText_DAO.FindFirst();
                criteria.FreeTextLine1 = addressFree.FreeText1;
                criteria.FreeTextLine2 = addressFree.FreeText2;
                criteria.FreeTextLine3 = addressFree.FreeText3;
                criteria.FreeTextLine4 = addressFree.FreeText4;
                criteria.FreeTextLine5 = addressFree.FreeText5;
                if (addressFree.PostOffice != null)
                    criteria.PostOfficeKey = addressFree.PostOffice.Key;
                SearchAddressesHelper(addressFree, criteria);

                criteria = new AddressSearchCriteria();
                AddressPostnetSuite_DAO addressPostnet = AddressPostnetSuite_DAO.FindFirst();
                if (addressPostnet.PostOffice != null)
                    criteria.PostOfficeKey = addressPostnet.PostOffice.Key;
                criteria.PrivateBagNumber = addressPostnet.PrivateBagNumber;
                criteria.PostnetSuiteNumber = addressPostnet.SuiteNumber;
                SearchAddressesHelper(addressPostnet, criteria);

                criteria = new AddressSearchCriteria();
                AddressPrivateBag_DAO addressPrivateBag = AddressPrivateBag_DAO.FindFirst();
                if (addressPrivateBag.PostOffice != null)
                    criteria.PostOfficeKey = addressPrivateBag.PostOffice.Key;
                criteria.PrivateBagNumber = addressPrivateBag.PrivateBagNumber;
                SearchAddressesHelper(addressPrivateBag, criteria);

                criteria = new AddressSearchCriteria();
                AddressStreet_DAO addressStreet = AddressStreet_DAO.FindFirst();
                criteria.BuildingName = addressStreet.BuildingName;
                criteria.BuildingNumber = addressStreet.BuildingNumber;
                criteria.StreetName = addressStreet.StreetName;
                criteria.StreetNumber = addressStreet.StreetNumber;
                if (addressStreet.Suburb != null)
                    criteria.SuburbKey = addressStreet.Suburb.Key;
                criteria.UnitNumber = addressStreet.UnitNumber;
                SearchAddressesHelper(addressStreet, criteria);
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.CreateEmptyMailingAddress"/> method.
        /// </summary>
        [Test]
        public void CreateEmptyMailingAddress()
        {
            IMailingAddress mailingAddress = _addressRepository.CreateEmptyMailingAddress();
            Assert.IsNotNull(mailingAddress);
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.SaveMailingAddress"/> method.
        /// </summary>
        [Test]
        public void SaveMailingAddress()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                MailingAddress_DAO maDao = MailingAddress_DAO.FindFirst();
                IMailingAddress ma = null;
                if (maDao != null)
                {
                    ma = new MailingAddress(maDao);
                    _addressRepository.SaveMailingAddress(ma);
                    Assert.IsNotNull(ma);
                }
                else
                {
                    Assert.Fail("No data to test IAddressRepository.SaveMailingAddress");
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.SaveAddress"/> method.
        /// </summary>
        [Test]
        public void SaveAddress()
        {
            IAddressRepository addRepo = RepositoryFactory.GetRepository<IAddressRepository>();

            //set the key to 0 and return true for AddressExists so the data does not get saved
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"select top 1 AddressKey, AddressFormatKey from Address a (nolock) where a.AddressFormatKey = 1
                    union all
                    select top 1 AddressKey, AddressFormatKey from Address a (nolock) where a.AddressFormatKey = 2
                    union all
                    select top 1 AddressKey, AddressFormatKey from Address a (nolock) where a.AddressFormatKey = 3
                    union all
                    select top 1 AddressKey, AddressFormatKey from Address a (nolock) where a.AddressFormatKey = 4
                    union all
                    select top 1 AddressKey, AddressFormatKey from Address a (nolock) where a.AddressFormatKey = 5
                    union all
                    select top 1 AddressKey, AddressFormatKey from Address a (nolock) where a.AddressFormatKey = 6";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string addFormat = dr[1].ToString();
                    Int32 addKey = Convert.ToInt32(dr[0].ToString());
                    IAddress ad = addRepo.GetAddressByKey(addKey);
                    addRepo.SaveAddress(ref ad); //do a default save regardless of the type
                    Assert.That(ad.Key == addKey && String.Compare(ad.AddressFormat.Key.ToString(), addFormat, true) == 0);
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.CreateEmptyApplicationMailingAddress"/> method.
        /// </summary>
        [Test]
        public void CreateEmptyApplicationMailingAddress()
        {
            IApplicationMailingAddress mailingAddress = _addressRepository.CreateEmptyApplicationMailingAddress();
            Assert.IsNotNull(mailingAddress);
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.SaveApplicationMailingAddress"/> method.
        /// </summary>
        [Test]
        public void SaveApplicationMailingAddress()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                ApplicationMailingAddress_DAO amaDao = ApplicationMailingAddress_DAO.FindFirst();
                IApplicationMailingAddress ama = null;
                if (amaDao != null)
                {
                    ama = new ApplicationMailingAddress(amaDao);
                    _addressRepository.SaveApplicationMailingAddress(ama);
                    Assert.IsNotNull(ama);
                }
                else
                {
                    Assert.Fail("No data to test IAddressRepository.SaveApplicationMailingAddress");
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="IAddressRepository.CleanDirtyAddress"/> method.
        /// </summary>
        [Test]
        public void CleanDirtyAddress()
        {
            int failedPostalMigrationKey = 0;
            int failedLeAddressPostalKey = 0;

            int failedStreetMigrationKey = 0;
            int failedLeAddressStreetKey = 0;

            try
            {
                // create postal objects and ensure the PostalIsCleaned values is changed to true
                using (new SessionScope())
                {
                    FailedPostalMigration_DAO failedPostalMigration = DAODataConsistancyChecker.GetDAO<FailedPostalMigration_DAO>();
                    failedPostalMigration.SaveAndFlush();
                    failedPostalMigrationKey = failedPostalMigration.Key;

                    FailedLegalEntityAddress_DAO failedLeAddressPostal = DAODataConsistancyChecker.GetDAO<FailedLegalEntityAddress_DAO>();
                    failedLeAddressPostal.FailedPostalMigration = failedPostalMigration;
                    failedLeAddressPostal.PostalIsCleaned = false;
                    failedLeAddressPostal.SaveAndFlush();
                    failedLeAddressPostalKey = failedLeAddressPostal.Key;

                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IFailedLegalEntityAddress flea = BMTM.GetMappedType<IFailedLegalEntityAddress>(failedLeAddressPostal);
                    _addressRepository.CleanDirtyAddress(flea);
                    Assert.IsTrue(flea.PostalIsCleaned);
                }

                // create street objects and ensure the PostalIsCleaned values is changed to true
                using (new SessionScope())
                {
                    FailedStreetMigration_DAO failedStreetMigration = DAODataConsistancyChecker.GetDAO<FailedStreetMigration_DAO>();
                    failedStreetMigration.SaveAndFlush();
                    failedStreetMigrationKey = failedStreetMigration.Key;

                    FailedLegalEntityAddress_DAO failedLeAddressStreet = DAODataConsistancyChecker.GetDAO<FailedLegalEntityAddress_DAO>();
                    failedLeAddressStreet.FailedStreetMigration = failedStreetMigration;
                    failedLeAddressStreet.IsCleaned = false;
                    failedLeAddressStreet.SaveAndFlush();
                    failedLeAddressStreetKey = failedLeAddressStreet.Key;

                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    IFailedLegalEntityAddress flea = BMTM.GetMappedType<IFailedLegalEntityAddress>(failedLeAddressStreet);
                    _addressRepository.CleanDirtyAddress(flea);
                    Assert.IsTrue(flea.IsCleaned);
                }
            }
            finally
            {
                using (new SessionScope())
                {
                    // clean up
                    if (failedPostalMigrationKey > 0)
                        FailedPostalMigration_DAO.Find(failedPostalMigrationKey).DeleteAndFlush();
                    if (failedLeAddressPostalKey > 0)
                        FailedLegalEntityAddress_DAO.Find(failedLeAddressPostalKey).DeleteAndFlush();
                    if (failedStreetMigrationKey > 0)
                        FailedStreetMigration_DAO.Find(failedStreetMigrationKey).DeleteAndFlush();
                    if (failedLeAddressStreetKey > 0)
                        FailedLegalEntityAddress_DAO.Find(failedLeAddressStreetKey).DeleteAndFlush();
                }
            }
        }

        #region Helper Methods

        /// <summary>
        /// Helper method for testing the <see cref="IAddressRepository.AddressExists"/> method.
        /// </summary>
        /// <param name="address"></param>
        private void AddressExistsHelper(IAddress address, bool expectedResult)
        {
            Assert.AreEqual(_addressRepository.AddressExists(ref address), expectedResult);
        }

        /// <summary>
        /// Helper method for searching addresses.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="criteria"></param>
        private void SearchAddressesHelper(Address_DAO address, IAddressSearchCriteria criteria)
        {
            criteria.ExactMatch = true;
            if (address.AddressFormat != null)
                criteria.AddressFormat = (AddressFormats)address.AddressFormat.Key;

            IEventList<IAddress> addresses = _addressRepository.SearchAddresses(criteria, 10);
            bool found = false;
            foreach (IAddress a in addresses)
            {
                if (a.Key == address.Key)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                Assert.Fail("Search for address type {0} failed using key {1}", address.GetType().FullName, address.Key);
        }

        #endregion Helper Methods
    }
}
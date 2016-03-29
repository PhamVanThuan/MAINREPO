using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.AJAX;

namespace SAHL.Web.Test.AJAX
{
    [TestFixture]
    public class LegalEntityService : TestViewBase
    {
        private LegalEntity _leService;
        private ILegalEntityRepository LERepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

        [SetUp]
        public void Setup()
        {
            _leService = new LegalEntity();
        }

        [TearDown]
        public void TearDown()
        {
            _leService = null;
        }

        [Test]
        public void GetClientSearchLegalEntityDetails()
        {
            using (new SessionScope())
            {
                string clientData = "Test";

                LegalEntityNaturalPerson_DAO leNaturalPerson = LegalEntityNaturalPerson_DAO.FindFirst();
                string[] result = _leService.GetClientSearchLegalEntityDetails(leNaturalPerson.Key.ToString(), clientData, true);
                Assert.AreEqual(result[0], clientData);
                Assert.Greater(result[1].Length, 0);

                LegalEntityCompany_DAO leCompany = LegalEntityCompany_DAO.FindFirst();
                result = _leService.GetClientSearchLegalEntityDetails(leCompany.Key.ToString(), clientData, true);
                Assert.AreEqual(result[0], clientData);
                Assert.Greater(result[1].Length, 0);
            }
        }

        [Test]
        public void GetLegalEntitiesByIDNumber()
        {
            using (new SessionScope())
            {
                LegalEntityNaturalPerson_DAO le = LegalEntityNaturalPerson_DAO.FindFirst();

                SAHLAutoCompleteItem[] items = _leService.GetLegalEntitiesByIDNumber(le.IDNumber);
                Assert.AreEqual(items.Length, 1, "GetLegalEntitiesByIDNumber should only return one record for ID number {0}", le.IDNumber);
                Assert.AreEqual(items[0].Value, le.Key.ToString());
            }
        }

        [Test]
        public void GetLegalEntitiesByPassportNumber()
        {
            using (new SessionScope())
            {
                DataTable dt = GetQueryResults("select top 1 * from LegalEntity where LEN(PassportNumber) > 2");
                if (dt.Rows.Count == 0)
                    Assert.Ignore("No data");
                string passportNumber = dt.Rows[0]["PassportNumber"].ToString();
                dt.Dispose();

                SAHLAutoCompleteItem[] items = _leService.GetLegalEntitiesByPassportNumber(passportNumber);
                Assert.Greater(items.Length, 0);
                foreach (SAHLAutoCompleteItem item in items)
                {
                    if (!item.Text.StartsWith(passportNumber))
                        Assert.Fail("GetLegalEntitiesByPassportNumber using prefix {0} returned  an item with value {1}", passportNumber, item.Text);
                }
            }
        }

        [Test]
        public void GetLegalEntitiesByRegistrationNumber()
        {
            string typeKey = ((int)LegalEntityTypes.Company).ToString();
            LegalEntityCompany_DAO lec = LegalEntityCompany_DAO.FindFirst();
            string prefix = lec.RegistrationNumber.Substring(0, lec.RegistrationNumber.Length - 2);

            // first, call the method and ensure the company comes back
            SAHLAutoCompleteItem[] items = _leService.GetLegalEntitiesByRegistrationNumber(prefix, typeKey);
            bool found = false;
            foreach (SAHLAutoCompleteItem item in items)
            {
                if (item.Value == lec.Key.ToString())
                    found = true;
            }
            if (!found)
                Assert.Fail("Unable to find company {0} using prefix {1} and type key {2}", lec.Key, prefix, typeKey);

            // also test that all companies of the correct type are returned
            prefix = prefix.Substring(0, 1);
            items = _leService.GetLegalEntitiesByRegistrationNumber(prefix, typeKey);
            foreach (SAHLAutoCompleteItem item in items)
            {
                //LegalEntity_DAO leTemp = LegalEntity_DAO.Find(Int32.Parse(item.Value));

                ILegalEntity leTemp = LERepo.GetLegalEntityByKey(Int32.Parse(item.Value));
                Assert.IsNotNull(leTemp);
                Assert.IsTrue(leTemp is ILegalEntityCompany || leTemp is ILegalEntityCloseCorporation || leTemp is ILegalEntityTrust);
            }
        }
    }
}
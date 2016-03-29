using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class RegistrationRepositoryTest : TestBase
    {
        private IRegistrationRepository _regRepo;

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();
        }

        #region Test Signature

        [Test]
        public void GetRegmailByAccountKeyMock()
        {
            IRegistrationRepository regmock = _mockery.StrictMock<IRegistrationRepository>();
            IRegMail regmail = _mockery.StrictMock<IRegMail>();
            Expect.Call(regmock.GetRegmailByAccountKey(1)).Return(regmail).IgnoreArguments();
            _mockery.ReplayAll();
            regmail = regmock.GetRegmailByAccountKey(1);
            Assert.IsNotNull(regmail);
        }

        [Test]
        public void SaveRegmailMock()
        {
            IRegistrationRepository regmock = _mockery.StrictMock<IRegistrationRepository>();
            IRegMail regmail = _mockery.StrictMock<IRegMail>();
            Expect.Call(delegate { regmock.SaveRegmail(regmail); });
            _mockery.ReplayAll();
        }

        [Test]
        public void GetAttorneysByDeedsOfficeKeyMock()
        {
            IRegistrationRepository regmock = _mockery.StrictMock<IRegistrationRepository>();
            IList<IAttorney> attn = _mockery.StrictMock<IList<IAttorney>>();
            Expect.Call(regmock.GetAttorneysByDeedsOfficeKey(1)).Return(attn).IgnoreArguments();
            _mockery.ReplayAll();
            attn = regmock.GetAttorneysByDeedsOfficeKey(1);
            Assert.IsNotNull(attn);
        }

        [Test]
        public void GetAttorneyByLegalEntityKeyMock()
        {
            IRegistrationRepository regmock = _mockery.StrictMock<IRegistrationRepository>();
            IAttorney attn = _mockery.StrictMock<IAttorney>();
            Expect.Call(regmock.GetAttorneyByLegalEntityKey(1)).Return(attn).IgnoreArguments();
            _mockery.ReplayAll();
            attn = regmock.GetAttorneyByLegalEntityKey(1);
            Assert.IsNotNull(attn);
        }

        [Test]
        public void GetAttorneyKeyMock()
        {
            IRegistrationRepository regmock = _mockery.StrictMock<IRegistrationRepository>();
            IAttorney attn = _mockery.StrictMock<IAttorney>();
            Expect.Call(regmock.GetAttorneyByKey(1)).Return(attn).IgnoreArguments();
            _mockery.ReplayAll();
            attn = regmock.GetAttorneyByKey(1);
            Assert.IsNotNull(attn);
        }

        [Test]
        public void GetAttorneysByDeedsOfficeKeyAndOSKeyMock()
        {
            IRegistrationRepository regmock = _mockery.StrictMock<IRegistrationRepository>();
            IList<IAttorney> attn = _mockery.StrictMock<IList<IAttorney>>();
            Expect.Call(regmock.GetAttorneysByDeedsOfficeKeyAndOSKey(1, 1)).Return(attn).IgnoreArguments();
            _mockery.ReplayAll();
            attn = regmock.GetAttorneysByDeedsOfficeKeyAndOSKey(1, 1);
            Assert.IsNotNull(attn);
        }

        #endregion Test Signature

        #region Test The Method

        [Test]
        public void GetAttorneyByKeyTest()
        {
            Attorney_DAO topAttorney = Attorney_DAO.FindFirst();
            Assert.IsNotNull(topAttorney, "No data in Attorney table");
            _regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
            IAttorney attorney = _regRepo.GetAttorneyByKey(topAttorney.Key);
            Assert.IsNotNull(attorney, "Cant find Attorney using method");
        }

        [Test]
        public void GetAttorneyByLegalEntityKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 le.LegalEntityKey
                from [2am].[dbo].Attorney att (nolock)
                join [2am].[dbo].LegalEntity le (nolock)
	                on att.LegalEntityKey = le.LegalEntityKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (o != null)
                {
                    int legalEntityKey = Convert.ToInt32(o);
                    _regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
                    IAttorney att = _regRepo.GetAttorneyByLegalEntityKey(legalEntityKey);
                    Assert.IsNotNull(att);
                }
            }
        }

        [Test]
        public void GetAttorneysByDeedsOfficeKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 de.DeedsOfficeKey
                from [2am].[dbo].Attorney att (nolock)
                join [2am].[dbo].DeedsOffice de (nolock)
	                on att.DeedsOfficeKey = de.DeedsOfficeKey";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (o != null)
                {
                    int deedsOfficeKey = Convert.ToInt32(o);
                    _regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
                    IList<IAttorney> attList = _regRepo.GetAttorneysByDeedsOfficeKey(deedsOfficeKey);
                    Assert.IsNotNull(attList);
                    Assert.IsTrue(attList.Count > 0);
                }
            }
        }

        [Test]
        public void GetAttorneysByDeedsOfficeKeyAndOSKey()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 osAtt.OriginationSourceKey, do.DeedsOfficeKey
                from [2am].[dbo].[Attorney] att (nolock)
                inner join [2am].[dbo].[DeedsOffice] do (nolock)
                    on  att.DeedsOfficeKey = do.DeedsOfficeKey
                inner join [2am].[dbo].[OriginationSourceAttorney] osAtt (nolock)
                    on att.AttorneyKey = osAtt.AttorneyKey";

                DataSet ds = CastleTransactionsServiceHelper.ExecuteQueryOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (ds != null && ds.Tables != null & ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int originationSourceKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int deedsOfficeKey = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                    _regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
                    IList<IAttorney> attList = _regRepo.GetAttorneysByDeedsOfficeKeyAndOSKey(deedsOfficeKey, originationSourceKey);
                    Assert.IsNotNull(attList);
                    Assert.IsTrue(attList.Count > 0);
                }
            }
        }

        [Test]
        public void GetRegmailByAccountKeyTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 LoanNumber from [sahldb].[dbo].[RegMail] (nolock)";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (o != null)
                {
                    int accountKey = Convert.ToInt32(o);
                    _regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
                    IRegMail regMail = _regRepo.GetRegmailByAccountKey(accountKey);
                    Assert.IsNotNull(regMail);

                    //and save
                    _regRepo.SaveRegmail(regMail);
                }
            }
        }

        #endregion Test The Method
    }
}
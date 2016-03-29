using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Factories;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class ITCRepositoryTest : TestBase
    {
        private IITCRepository _itcRepo;

        [Test]
        public void GetITCAddressListByLegalEntityKey()
        {
            using (new SessionScope())
            {
                string query = "select lea.* from [2am].[dbo].[LegalEntityAddress] lea (nolock) "
                    + "join [2am].[dbo].[Address] a (nolock) on a.AddressKey = lea.AddressKey "
                    + "where lea.LegalEntityKey = (select top 1 LegalEntityKey "
                    + "from [2am].[dbo].[LegalEntityAddress] (nolock) "
                    + "group by LegalEntityKey order by count(LegalEntityKey) desc) "
                    + "and a.AddressFormatKey = 1 "
                    + "order by lea.AddressTypeKey, a.ChangeDate desc";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count > 1);

                int LegalEntityKey = Convert.ToInt32(DT.Rows[0][1]);

                IList<IAddressStreet> list = ITCRepo.GetITCAddressListByLegalEntityKey(LegalEntityKey);

                Assert.That(list.Count == 2);
            }
        }

        [Test]
        public void GetITCByAccountKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 itc.* from [2AM].[dbo].[ITC] itc (nolock) ";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int AccountKey = Convert.ToInt32(DT.Rows[0][2]);

                IList<IITC> list = ITCRepo.GetITCByAccountKey(AccountKey);

                query = @"select 1 from [2AM].[dbo].[ITC] itc (nolock) where itc.AccountKey = " + AccountKey.ToString();
                DT = base.GetQueryResults(query);

                Assert.That(list.Count == DT.Rows.Count);
            }
        }

        [Test]
        public void GetArchivedITCByLegalEntityKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 itc.LegalEntityKey, itc.AccountKey from [2AM].[Archive].[ITC] itc (nolock) "
                        + "join [2AM].[dbo].[LegalEntity] le (nolock) on le.LegalEntityKey = itc.LegalEntityKey "
                        + "join [2am].[dbo].[Role] r (nolock) on le.LegalEntityKey = r.LegalEntityKey "
                        + "join [2am].[dbo].[Account] a (nolock) on a.AccountKey = r.AccountKey "
                        + "where le.LegalEntityStatusKey <> 2 and a.AccountStatusKey in (1, 5) ";

                DataTable DT = base.GetQueryResults(query);
                
                if (DT.Rows.Count == 0)
                {
                    Assert.Ignore("No data for test: GetArchivedITCByLegalEntityKey()");
                }

                int leKey = Convert.ToInt32(DT.Rows[0][0]);
                int accountKey = Convert.ToInt32(DT.Rows[0][1]);

                DataTable itcList = ITCRepo.GetArchivedITCByLegalEntityKey(leKey, accountKey);

                query = string.Format("SELECT itc.ITCKey, itc.LegalEntityKey, itc.AccountKey, itc.ChangeDate, itc.ResponseXML, itc.ResponseStatus, itc.UserID, NULL as ArchiveUser, NULL as ArchiveDate, itc.RequestXML  "
                    + "FROM ITC itc "
                    + "WHERE itc.LegalEntityKey = {0} "
                    + "AND itc.AccountKey != {1} "
                    + "UNION ALL "
                    + "SELECT * FROM archive.ITC aitc "
                    + "WHERE aitc.LegalEntityKey = {0} ", leKey, accountKey);

                DT = base.GetQueryResults(query);

                Assert.That(itcList.Rows.Count == DT.Rows.Count);
            }
        }

        [Test]
        public void GetArchivedITCByITCKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2AM].[Archive].[ITC] itc (nolock) ";

                DataTable DT = base.GetQueryResults(query);

                if (DT.Rows.Count == 0)
                {
                    Assert.Ignore("No data for test: GetArchivedITCByITCKey()");
                }

                int itcKey = Convert.ToInt32(DT.Rows[0]["ITCKey"]);
                IITCArchive itc = ITCRepo.GetArchivedITCByITCKey(itcKey);
                Assert.That(itc.Key == itcKey);
            }
        }

        [Test]
        public void GetITCByKey()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2AM].[dbo].[ITC] itc (nolock) ";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int itcKey = Convert.ToInt32(DT.Rows[0]["ITCKey"]);
                IITC itc = ITCRepo.GetITCByKey(itcKey);
                Assert.That(itc.Key == itcKey);
            }
        }

        [Test]
        public void GetEmptyITC()
        {
            using (new SessionScope())
            {
                IITC itc = ITCRepo.GetEmptyITC();
                Assert.IsNotNull(itc);
            }
        }

        [Test]
        public void GetITCXslByDate()
        {
            using (new SessionScope())
            {
                string query = "select top 1 * from [2AM].[dbo].[ITCXSL] itc (nolock) "
                    + " where EffectiveDate < '" + DateTime.Now.ToString("yyyy/MM/dd") + "' "
                    + " order by EffectiveDate desc";

                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                string itcXsl = ITCRepo.GetITCXslByDate(DateTime.Now);
                Assert.That(itcXsl.ToString().Length > 0);// == DT.Rows[0]["ITCXslKey"].ToString());
            }
        }

        [Test]
        public void GetITCOrArchiveITCByITCKey()
        {
            using (new SessionScope(FlushAction.Never))
            {
                string sql = @"select top 1 ITCKey from dbo.ITC";
                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                IITC itc;
                IITCArchive archiveITC;

                if (obj != null)
                {
                    ITCRepo.GetITCOrArchiveITCByITCKey(Convert.ToInt32(obj), out itc, out archiveITC);
                    Assert.IsNotNull(itc);
                    Assert.IsNull(archiveITC);
                }

                sql = @"select top 1 ITCKey from archive.ITC";
                obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);

                if (obj != null)
                {
                    itc = null;
                    archiveITC = null;
                    ITCRepo.GetITCOrArchiveITCByITCKey(Convert.ToInt32(obj), out itc, out archiveITC);
                    Assert.IsNull(itc);
                    Assert.IsNotNull(archiveITC);
                }
            }
        }

        public IITCRepository ITCRepo
        {
            get
            {
                if (_itcRepo == null)
                    _itcRepo = RepositoryFactory.GetRepository<IITCRepository>();

                return _itcRepo;
            }
        }
    }
}
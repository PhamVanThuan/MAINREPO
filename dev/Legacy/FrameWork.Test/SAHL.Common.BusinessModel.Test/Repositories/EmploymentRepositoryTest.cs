using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class EmploymentRepositoryTest : TestBase
    {
        private IEmploymentRepository _repo = RepositoryFactory.GetRepository<IEmploymentRepository>();

        /// <summary>
        /// Tests the check against an employment record to determine how many accounts it has an affect on.
        /// </summary>
        [Test]
        public void GetAccountsForPTI()
        {
            using (new SessionScope())
            {
                GetAccountsForPTIHelper(1, 1);
                GetAccountsForPTIHelper(2, 0);
            }
        }

        [Test]
        public void GetEmployerByKey()
        {
            using (new SessionScope())
            {
                int key = Employer_DAO.FindFirst().Key;
                IEmployer employer = _repo.GetEmployerByKey(key);
                Assert.IsNotNull(employer);
            }
        }

        [Test]
        public void GetEmployersByPrefix()
        {
            using (new SessionScope())
            {
                string prefix = Employer_DAO.FindFirst().Name.Trim();
                int maxRowCount = 1;
                IReadOnlyEventList<IEmployer> employerList = _repo.GetEmployersByPrefix(prefix, maxRowCount);
                Assert.IsNotNull(employerList);
                Assert.IsTrue(employerList.Count > 0);
            }
        }

        [Test]
        public void GetEmploymentByKey()
        {
            using (new SessionScope())
            {
                int key = Employment_DAO.FindFirst().Key;
                IEmployment employment = _repo.GetEmploymentByKey(key);
                Assert.IsNotNull(employment);
            }
        }

        /// <summary>
        /// Tests the retrieval of employment records per application.
        /// </summary>
        [Test]
        public void GetEmploymentByApplicationKey()
        {
            string sql = @"select top 1 o.OfferKey
                from Offer o
                inner join OfferRole ofr on ofr.OfferKey = o.OfferKey
                inner join Employment e on e.LegalEntityKey = ofr.LegalEntityKey";

            DataTable dt = base.GetQueryResults(sql);
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");
            int applicationKey = Convert.ToInt32(dt.Rows[0][0]);
            dt.Dispose();

            // at the moment, just run the query against the application key and ensure it works
            IEventList<IEmployment> lstEmployment = _repo.GetEmploymentByApplicationKey(applicationKey, false);
            Assert.IsNotNull(lstEmployment);
            lstEmployment = _repo.GetEmploymentByApplicationKey(applicationKey, true);
            Assert.IsNotNull(lstEmployment);

            // TODO: Some kind of data validation tests should be done to validate the results of the query
        }

        [Test]
        public void GetEmptyEmployer()
        {
            using (new SessionScope())
            {
                IEmployer employer = _repo.GetEmptyEmployer();
                Assert.IsNotNull(employer);
            }
        }

        [Test]
        public void GetEmptyEmploymentByType()
        {
            using (new SessionScope())
            {
                IEmployment employment = _repo.GetEmptyEmploymentByType(EmploymentTypes.Salaried);
                Assert.IsNotNull(employment);
            }
        }

        [Test]
        public void GetEmptySubsidy()
        {
            using (new SessionScope())
            {
                ISubsidy subsidy = _repo.GetEmptySubsidy();
                Assert.IsNotNull(subsidy);
            }
        }

        [Test]
        public void GetSubsidyProviderByKey()
        {
            using (new SessionScope())
            {
                int key = SubsidyProvider_DAO.FindFirst().Key;
                ISubsidyProvider subsidyProvider = _repo.GetSubsidyProviderByKey(key);
                Assert.IsNotNull(subsidyProvider);
            }
        }

        [Test]
        public void GetSubsidyByKey()
        {
            using (new SessionScope())
            {
                int key = Subsidy_DAO.FindFirst().Key;
                ISubsidy subsidy = _repo.GetSubsidyByKey(key);
                Assert.IsNotNull(subsidy);
            }
        }

        [Test]
        public void GetSubsidyProvidersByPrefix()
        {
            using (new SessionScope())
            {
                string prefix = "a";
                ILegalEntityGenericCompany le;

                // start off with just a start of a, this should return lots of results - make sure it comes
                // back with 10
                ReadOnlyCollection<ISubsidyProvider> subsidyProviders = _repo.GetSubsidyProvidersByPrefix(prefix, 10);
                Assert.AreEqual(subsidyProviders.Count, 10);
                foreach (ISubsidyProvider sp in subsidyProviders)
                {
                    le = (ILegalEntityGenericCompany)sp.LegalEntity;
                    Assert.That(le.RegisteredName.ToLower().StartsWith(prefix));
                }

                // pull out the first item on the list so we can use it for more tests
                le = (ILegalEntityGenericCompany)subsidyProviders[0].LegalEntity;
                string desc = le.RegisteredName;

                // run the exact description through, and limit it to one
                subsidyProviders = _repo.GetSubsidyProvidersByPrefix(desc, 1);
                Assert.AreEqual(subsidyProviders.Count, 1);

                // the legal entity returned now should have a RegisteredName that at least starts with the description provided
                le = (ILegalEntityGenericCompany)subsidyProviders[0].LegalEntity;
                Assert.IsTrue(le.RegisteredName.StartsWith(desc));
            }
        }

        [Test]
        public void CreateEmptySubsidyProvider()
        {
            using (new SessionScope())
            {
                ISubsidyProvider subsidyProvider = _repo.CreateEmptySubsidyProvider();
                Assert.IsNotNull(subsidyProvider);
            }
        }

        [Test]
        public void GetSubsidiesByLegalEntityKey()
        {
            using (new SessionScope())
            {
                int key = Subsidy_DAO.FindFirst().LegalEntity.Key;
                IList<ISubsidy> subsidyList = _repo.GetSubsidiesByLegalEntityKey(key);
                Assert.IsNotNull(subsidyList);
                Assert.IsTrue(subsidyList.Count > 0);
            }
        }

        [Test]
        public void SaveEmployment()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                int key = Employment_DAO.FindFirst().Key;
                IEmployment employment = _repo.GetEmploymentByKey(key);
                _repo.SaveEmployment(employment);
            }
        }

        /// <summary>
        /// Helper method for GetAccountsForPTI.
        /// </summary>
        /// <param name="employmentRecordCount">The number of employment records attached to the account.</param>
        /// <param name="expectedAccountCount">The number of accounts that will be affected.</param>
        private void GetAccountsForPTIHelper(int employmentRecordCount, int expectedAccountCount)
        {
            IEmploymentRepository repo = RepositoryFactory.GetRepository<IEmploymentRepository>();

            string sql = @"Select top 1 accountkey, Count(employmentkey)
                from Employment E
                join legalentity LE
                on LE.Legalentitykey = E.Legalentitykey
                join role r
                on r.Legalentitykey = LE.Legalentitykey
                where employmentstatuskey = 1
                and e.confirmedincome is not null
                and e.confirmedincome > 0
                and accountkey in
                (
                Select top 50 a.accountkey from role r
                join account a
                on a.Accountkey = r.Accountkey
                inner join financialservice fs
                on fs.accountkey = a.accountkey
                --where legalentitykey = 134549
                where roletypekey in (2,3)
                and a.RRR_productkey not in (3, 4)
                and r.generalstatuskey = 1
                and a.AccountStatusKey in(1,3)
                and fs.FinancialServiceTypeKey in (1,2)
                )
                group by accountkey
                having count(employmentkey) = {0}";

            DataTable dt = base.GetQueryResults(String.Format(sql, employmentRecordCount));
            if (dt.Rows.Count == 0)
                Assert.Ignore("No data found");

            int accKey = Convert.ToInt32(dt.Rows[0][0]);
            Account_DAO account = Account_DAO.Find(accKey);
            foreach (Role_DAO role in account.Roles)
            {
                foreach (Employment_DAO emp in role.LegalEntity.Employment)
                {
                    if (emp.EmploymentStatus.Key == (int)EmploymentStatuses.Current)
                    {
                        IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                        IEmployment iEmp = BMTM.GetMappedType<IEmployment>(emp);
                        IList<IAccount> affectedAccounts = repo.GetAccountsForPTI(iEmp);
                        Assert.AreEqual(expectedAccountCount, affectedAccounts.Count);
                        return;
                    }
                }
            }
        }

        [Test]
        public void GetEmployers()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                string employerName = "Freedom Distributors";
                string sql = @"Select top 1 * from Employer where Name = '{0}'";
                DataTable dt = base.GetQueryResults(String.Format(sql, employerName));
                IEmploymentRepository repo = RepositoryFactory.GetRepository<IEmploymentRepository>();
                IReadOnlyEventList<IEmployer> employers = repo.GetEmployers(employerName);
                Assert.AreEqual(dt.Rows.Count, employers.Count);
            }
        }

        [Test]
        public void GetVerificationProcessDTTest()
        {
            using (new SessionScope())
            {
                Employment_DAO emp = Employment_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IEmployment employment = BMTM.GetMappedType<IEmployment, Employment_DAO>(emp);
                DataTable dt = _repo.GetVerificationProcessDT(employment);
                Assert.IsNotNull(dt);
            }
        }
    }
}
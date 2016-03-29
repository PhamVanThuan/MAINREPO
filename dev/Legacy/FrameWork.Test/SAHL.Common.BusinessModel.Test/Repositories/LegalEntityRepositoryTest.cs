using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Text;

namespace SAHL.Common.BusinessModel.Test.Repositories
{
    [TestFixture]
    public class LegalEntityRepositoryTest : TestBase
    {
        private ILegalEntityRepository _repo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

        [SetUp]
        public void Setup()
        {
            // set the strategy to default so we actually go to the database
            //SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            _mockery = new MockRepository();
        }

        [TearDown]
        public void TearDown()
        {
            MockCache.Flush();

            // GC.Collect();
        }

        [Test]
        public void GetEmptyRole()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                IRole emptyRole = _repo.GetEmptyRole();

                Assert.IsNotNull(emptyRole);
            }
        }

        [Test]
        public void CreateEmptyAttorney()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                IAttorney emptyRole = _repo.CreateEmptyAttorney();

                Assert.IsNotNull(emptyRole);
            }
        }

        [Test]
        public void GetEmptyLegalEntityBankAccount()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ILegalEntityBankAccount emptyRole = _repo.GetEmptyLegalEntityBankAccount();

                Assert.IsNotNull(emptyRole);
            }
        }

        [Test]
        public void GetEmptyLegalEntityAssetLiability()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ILegalEntityAssetLiability emptyRole = _repo.GetEmptyLegalEntityAssetLiability();

                Assert.IsNotNull(emptyRole);
            }
        }

        [Test]
        public void CreateLegalEntityRelationshipTest()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ILegalEntityRelationship leRel = _repo.CreateLegalEntityRelationship();
                Assert.IsNotNull(leRel);
            }
        }

        [Test]
        public void GetEmptyLegalEntityAffordability()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                ILegalEntityAffordability emptyRole = _repo.GetEmptyLegalEntityAffordability();

                Assert.IsNotNull(emptyRole);
            }
        }

        /// <summary>
        /// Tests the soft deletion of a legal entity address (GeneralStatus is set to Inactive).
        /// </summary>
        [Test]
        public void DeleteAddress()
        {
            int key;

            // get a LegalEntityAddress object with a status of active
            using (new SessionScope())
            {
                string hql = "from LegalEntityAddress_DAO lea where lea.GeneralStatus.Key = 1";
                SimpleQuery<LegalEntityAddress_DAO> q = new SimpleQuery<LegalEntityAddress_DAO>(hql);
                q.SetQueryRange(1);
                LegalEntityAddress_DAO[] res = q.Execute();
                Assert.Greater(res.Length, 0, "No data found");
                key = res[0].Key;

                // create the object and "delete" it
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntityAddress lea = BMTM.GetMappedType<ILegalEntityAddress>(res[0]);
                _repo.DeleteAddress(lea);
            }

            // load the object and check the status is inactive
            using (new SessionScope())
            {
                LegalEntityAddress_DAO lea = LegalEntityAddress_DAO.Find(key);
                Assert.AreEqual(lea.GeneralStatus.Key, (int)GeneralStatuses.Inactive);

                // reset the general status on the record to active
                lea.GeneralStatus = GeneralStatus_DAO.Find(1);
                lea.SaveAndFlush();
            }
        }

        [Test]
        public void GetRelatedRolesByLegalEntity()
        {
            using (new SessionScope())
            {
                int origSourceCount = CurrentPrincipalCache.UserOriginationSourceKeys.Count;
                if (origSourceCount == 0)
                    AddOriginationSourceToPrincipal(OriginationSources.SAHomeLoans);

                try
                {
                    string osKeys = CurrentPrincipalCache.OriginationSourceKeysStringForQuery;
                    string accountStatusKeys = "1,2,3,4,5";

                    // get a legal entity to play with
                    string sql = String.Format(@"select top 1 r.LegalEntityKey
				from [Role] r
				inner join LegalEntity le on r.LegalEntityKey = le.LegalEntityKey
				inner join Account a on r.AccountKey = a.AccountKey
				where a.AccountStatusKey IN ({0})
				and a.RRR_OriginationSourceKey in ({1})", accountStatusKeys, osKeys);
                    DataTable dt = base.GetQueryResults(sql);
                    if (dt.Rows.Count == 0)
                        Assert.Ignore("No legal entity data found");
                    int legalEntityKey = Convert.ToInt32(dt.Rows[0][0]);
                    dt.Dispose();

                    // get the role count out
                    sql = String.Format(@"select count(r.AccountRoleKey)
				from [role] r
				inner join Account a on r.AccountKey = a.AccountKey
				where r.AccountKey in
					(
					select AccountKey
					from [role] r2
					where r2.LegalEntityKey = {0}
					)
				and a.AccountStatusKey IN ({1})
				and a.RRR_OriginationSourceKey IN ({2})", legalEntityKey, accountStatusKeys, osKeys);
                    dt = base.GetQueryResults(sql);
                    if (dt.Rows.Count == 0)
                        Assert.Ignore("No role data found");
                    int roleCount = Convert.ToInt32(dt.Rows[0][0]);
                    dt.Dispose();

                    IReadOnlyEventList<IRole> roles = _repo.GetRelatedRolesByLegalEntity(base.TestPrincipal, legalEntityKey);
                    Assert.AreEqual(roles.Count, roleCount);
                }
                finally
                {
                    if (origSourceCount == 0)
                        RemoveOriginationSourceFromPrincipal(OriginationSources.SAHomeLoans);
                }
            }
        }

        /// <summary>
        /// Test runs as a check only but isn't much of a test - this method has been marked as obsolete.
        /// </summary>
        [Test]
        public void GetRelatedRolesByAccount()
        {
            using (new SessionScope())
            {
                int origSourceCount = CurrentPrincipalCache.UserOriginationSourceKeys.Count;
                if (origSourceCount == 0)
                    AddOriginationSourceToPrincipal(OriginationSources.SAHomeLoans);

                try
                {
                    Account_DAO account = Account_DAO.FindFirst();
                    _repo.GetRelatedRolesByAccount(TestPrincipal, account.Key);
                }
                finally
                {
                    if (origSourceCount == 0)
                        RemoveOriginationSourceFromPrincipal(OriginationSources.SAHomeLoans);
                }
            }
        }

        /// <summary>
        /// This test doesn't actually do anything as the saving of the role contains no business
        /// logic so is already tested by the DAO save.
        /// </summary>
        [Test]
        public void SaveRole()
        {
        }

        #region SearchForLegalEntities

        [Test]
        public void SearchLegalEntities()
        {
            using (new SessionScope())
            {
                base.AddOriginationSourceToPrincipal(OriginationSources.SAHomeLoans);

                try
                {
                    string sql = "select top 1 FirstNames, Surname, IDNumber from LegalEntity le where le.FirstNames is not null and le.Surname is not null and le.IDNumber is not null";
                    DataTable dt = GetQueryResults(sql);
                    if (dt.Rows.Count == 0)
                        Assert.Ignore("No data to test");
                    string firstNames = dt.Rows[0]["FirstNames"].ToString();
                    string surname = dt.Rows[0]["Surname"].ToString();
                    string idNumber = dt.Rows[0]["IDNumber"].ToString();
                    dt.Dispose();

                    // check by first name
                    IClientSearchCriteria searchCriteria = new ClientSearchCriteria();
                    searchCriteria.FirstNames = firstNames.Substring(0, 1);
                    IEventList<ILegalEntity> results = _repo.SearchLegalEntities(searchCriteria, 5);
                    Assert.Greater(results.Count, 0);
                    foreach (ILegalEntityNaturalPerson le in results)
                    {
                        Assert.That(le.FirstNames.StartsWith(searchCriteria.FirstNames));
                    }

                    // check by surname
                    searchCriteria = new ClientSearchCriteria();
                    searchCriteria.Surname = surname.Substring(0, 1);
                    results = _repo.SearchLegalEntities(searchCriteria, 5);
                    Assert.Greater(results.Count, 0);
                    foreach (ILegalEntityNaturalPerson le in results)
                    {
                        Assert.That(le.Surname.StartsWith(searchCriteria.Surname));
                    }

                    // check by firstname and surname
                    searchCriteria = new ClientSearchCriteria();
                    searchCriteria.FirstNames = firstNames.Substring(0, 1);
                    searchCriteria.Surname = surname.Substring(0, 1);
                    results = _repo.SearchLegalEntities(searchCriteria, 5);
                    Assert.Greater(results.Count, 0);
                    foreach (ILegalEntityNaturalPerson le in results)
                    {
                        Assert.That(le.FirstNames.StartsWith(searchCriteria.FirstNames));
                        Assert.That(le.Surname.StartsWith(searchCriteria.Surname));
                    }

                    // check by id number
                    searchCriteria = new ClientSearchCriteria();
                    searchCriteria.IDNumber = idNumber;
                    results = _repo.SearchLegalEntities(searchCriteria, 5);
                    Assert.Greater(results.Count, 0);
                    foreach (ILegalEntityNaturalPerson le in results)
                    {
                        Assert.That(le.IDNumber.StartsWith(searchCriteria.IDNumber));
                    }

                    // search by account key
                    sql = String.Format(@"select top 1 r.AccountKey
						from Role r
						inner join Account a on a.AccountKey = r.AccountKey
						where a.RRR_OriginationSourceKey = {0}", (int)OriginationSources.SAHomeLoans);
                    dt = GetQueryResults(sql);
                    if (dt.Rows.Count == 0)
                        Assert.Ignore("No data to test");
                    string accountKey = dt.Rows[0]["AccountKey"].ToString();
                    dt.Dispose();

                    searchCriteria = new ClientSearchCriteria();
                    searchCriteria.AccountNumber = accountKey;
                    results = _repo.SearchLegalEntities(searchCriteria, 1);
                    Assert.Greater(results.Count, 0);
                    foreach (ILegalEntityNaturalPerson le in results)
                    {
                        bool found = false;
                        foreach (IRole role in le.Roles)
                        {
                            if (role.Account.Key.ToString() == accountKey)
                            {
                                found = true;
                                break;
                            }
                        }
                        Assert.IsTrue(found, "Search did not return results for account key {0}", accountKey);
                    }

                    // search by salary number
                    sql = "select top 1 SalaryNumber from Subsidy where GeneralStatusKey = 1";
                    dt = GetQueryResults(sql);
                    if (dt.Rows.Count == 0)
                        Assert.Ignore("No data to test");
                    string salaryNumber = dt.Rows[0]["SalaryNumber"].ToString();
                    dt.Dispose();

                    searchCriteria = new ClientSearchCriteria();
                    searchCriteria.SalaryNumber = salaryNumber;
                    results = _repo.SearchLegalEntities(searchCriteria, 1);
                    Assert.Greater(results.Count, 0);
                    foreach (ILegalEntityNaturalPerson le in results)
                    {
                        bool found = false;
                        foreach (IEmployment e in le.Employment)
                        {
                            IEmploymentSubsidised es = e as IEmploymentSubsidised;
                            if (es != null)
                            {
                                Assert.AreEqual(es.Subsidy.SalaryNumber, salaryNumber);
                                found = true;
                            }
                        }
                        Assert.IsTrue(found, "Search did not return a legal entity with existing Salary Number {0}", salaryNumber);
                    }

                    // finally, search on all criteria (don't care that it doesn't return results - just making sure the query doesn't fall over);
                    searchCriteria = new ClientSearchCriteria();
                    searchCriteria.AccountNumber = accountKey;
                    searchCriteria.FirstNames = firstNames;
                    searchCriteria.IDNumber = idNumber;
                    searchCriteria.SalaryNumber = salaryNumber;
                    searchCriteria.Surname = surname;
                    _repo.SearchLegalEntities(searchCriteria, 1);
                }
                finally
                {
                    base.RemoveOriginationSourceFromPrincipal(OriginationSources.SAHomeLoans);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>

        [Test]
        public void SearchForLegalEntitiesWithEmptySearchCriteria()
        {
            IDomainMessageCollection Messages = new DomainMessageCollection();
            IClientSearchCriteria CSC = TypeFactory.CreateType<IClientSearchCriteria>();
            Assert.IsTrue(CSC.IsEmpty);
        }

        [Test]
        public void SearchForLegalEntitiesWithNullSearchCriteria()
        {
            IDomainMessageCollection Messages = new DomainMessageCollection();
            IClientSearchCriteria CSC = null;
            Assert.IsNull(CSC);
        }

        [Test]
        public void SuperSearchForLegalEntitiesWithValidSearchCriteria()
        {
            IDomainMessageCollection Messages = new DomainMessageCollection();
            IClientSuperSearchCriteria CSC = new ClientSuperSearchCriteria("Smith", "", "2", base.TestPrincipal);
            IEventList<ILegalEntity> LEs = _repo.SuperSearchForAllLegalEntities(CSC);
        }

        [Test]
        public void GetCloseCorporationsByRegistrationNumber()
        {
            using (new SessionScope())
            {
                IReadOnlyEventList<ILegalEntityCloseCorporation> les = _repo.GetCloseCorporationsByRegistrationNumber("20", 5);
                Assert.That(les.Count > 0);
            }
        }

        [Test]
        public void GetLegalEntityByKey_ClosedCorporation()
        {
            using (new SessionScope())
            {
                int key = (int)base.GetPrimaryKey("LegalEntity", "LegalEntityKey", "LegalEntityTypeKey = 4");
                ILegalEntity le = _repo.GetLegalEntityByKey(key);
                Assert.That(le != null);
            }
        }

        /// <summary>
        /// Tests the find of a legal entity by identity number.
        /// </summary>
        [Test]
        public void GetNaturalPersonByIDNumberPass()
        {
            using (new SessionScope())
            {
                // Look for the first legal entity Trust with a registration number
                string HQL = "from LegalEntityNaturalPerson_DAO NP where len(NP.IDNumber) != 0";
                SimpleQuery<LegalEntityNaturalPerson_DAO> q = new SimpleQuery<LegalEntityNaturalPerson_DAO>(HQL);
                q.SetQueryRange(1);
                LegalEntityNaturalPerson_DAO[] res = q.Execute();
                if (res.Length <= 0)
                    Assert.Ignore("No LegalEntities with ID Numbers Available");

                ILegalEntityNaturalPerson LeNP = _repo.GetNaturalPersonByIDNumber(res[0].IDNumber);

                Assert.That(LeNP != null);
            }
        }

        /// <summary>
        /// Tests that the GetNaturalPersonByIDNumber method will not find a Legal Entity from an ID Number that doesn't exist.
        /// </summary>
        [Test]
        public void GetNaturalPersonByIDNumberFail()
        {
            using (new SessionScope())
            {
                ILegalEntityNaturalPerson LeNP = _repo.GetNaturalPersonByIDNumber("-1");

                Assert.That(LeNP == null);
            }
        }

        #endregion SearchForLegalEntities

        [Test]
        public void ClearLegalEntityOfferAffordabilityData()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                //find 1st legalentityaffordability record
                LegalEntityAffordability_DAO legalentityaffordability = LegalEntityAffordability_DAO.FindFirst();
                int legalEntityKey = legalentityaffordability.LegalEntity.Key;
                int offerKey = legalentityaffordability.Application.Key;

                // delete the record using repo method
                _repo.ClearLegalEntityOfferAffordabilityData(offerKey, legalEntityKey);

                // check that it no longer exists
                int recCount = -1;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"select count(1) from [2am]..LegalEntityAffordability (nolock) where LegalEntityKey = " + legalEntityKey + " and OfferKey = " + offerKey;
                    recCount = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                // do the checking
                Assert.IsTrue(recCount == 0);
            }
        }

        [Test]
        public void UpdateLegalEntityType()
        {
            using (new TransactionScope(TransactionMode.New, IsolationLevel.ReadUncommitted, OnDispose.Rollback))
            {
                //find 1st legalentity of type company
                LegalEntityCompany_DAO company = LegalEntityCompany_DAO.FindFirst();
                int legalEntityKey = company.Key;

                // change it to close corporation using repo method
                int legalEntityTypeKey = (int)SAHL.Common.Globals.LegalEntityTypes.CloseCorporation;
                _repo.UpdateLegalEntityType(legalEntityKey, legalEntityTypeKey);

                // read it back and check that the type has changed
                int updatedLegalEntityTypeKey = -1;
                using (IDbConnection con = Helper.GetSQLDBConnection())
                {
                    string query = @"select le.LegalEntityTypeKey from [2am]..LegalEntity le (nolock) where le.LegalEntityKey = " + legalEntityKey;
                    updatedLegalEntityTypeKey = Convert.ToInt32(Helper.ExecuteScalar(con, query));
                }

                // do the checking
                Assert.IsTrue(updatedLegalEntityTypeKey == legalEntityTypeKey);
            }
        }

        [Test]
        public void GetAttorneysByDeedsOffice()
        {
            using (new SessionScope())
            {
                // Look for the first attorney with a deeds office key
                string HQL = "from Attorney_DAO attorney where attorney.DeedsOffice.Key > 0";
                SimpleQuery<Attorney_DAO> q = new SimpleQuery<Attorney_DAO>(HQL, null);
                q.SetQueryRange(1);
                Attorney_DAO[] res = q.Execute();
                if (res.Length <= 0)
                    Assert.Ignore("No Attorneys Available");

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IAttorney attorney = BMTM.GetMappedType<IAttorney>(res[0]);

                IDictionary<int, string> dicAttorneys = _repo.GetAttorneysByDeedsOffice(attorney.DeedsOffice.Key);

                Assert.That(dicAttorneys.Count >= 1);
            }
        }

        [Test]
        public void GetRegistrationNumbersForCompanies_ExactMatch()
        {
            using (new SessionScope())
            {
                // Look for the first legal entity Trust with a registration number
                string HQL = "from LegalEntityTrust_DAO trust where trust.RegistrationNumber <> ?";
                SimpleQuery<LegalEntityTrust_DAO> q = new SimpleQuery<LegalEntityTrust_DAO>(HQL, "");
                q.SetQueryRange(1);
                LegalEntityTrust_DAO[] res = q.Execute();
                if (res.Length <= 0)
                    Assert.Ignore("No Trusts Available");
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntityTrust leTrust = BMTM.GetMappedType<ILegalEntityTrust>(res[0]);

                IDictionary<string, string> registrationNumbers = _repo.GetRegistrationNumbersForCompanies(leTrust.RegistrationNumber, 10);

                Assert.That(registrationNumbers.Count == 1);
            }
        }

        [Test]
        public void GetRegistrationNumbersForCompanies_PartialMatch()
        {
            using (new SessionScope())
            {
                // Look for the first legal entity Trust with a registration number
                string HQL = "from LegalEntityTrust_DAO trust where trust.RegistrationNumber <> ?";
                SimpleQuery<LegalEntityTrust_DAO> q = new SimpleQuery<LegalEntityTrust_DAO>(HQL, "");
                q.SetQueryRange(1);
                LegalEntityTrust_DAO[] res = q.Execute();
                if (res.Length <= 0)
                    Assert.Ignore("No Trusts Available");

                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntityTrust leTrust = BMTM.GetMappedType<ILegalEntityTrust>(res[0]);

                string registrationNumber = leTrust.RegistrationNumber.Substring(0, leTrust.RegistrationNumber.Length - 2);
                IDictionary<string, string> registrationNumbers = _repo.GetRegistrationNumbersForCompanies(registrationNumber, 10);

                Assert.That(registrationNumbers.Count > 0);
            }
        }

        /// <summary>
        /// Tests <see cref="ILegalEntityRepository.GetEmptyLegalEntityMarketingOption"/>
        /// </summary>
        [Test]
        public void GetEmptyLegalEntityMarketingOption()
        {
            ILegalEntityMarketingOption lemo = _repo.GetEmptyLegalEntityMarketingOption();
            Assert.IsNotNull(lemo);
        }

        /// <summary>
        /// Tests <see cref="ILegalEntityRepository.GetLegalEntityBankAccountByKey"/>
        /// </summary>
        [Test]
        public void GetLegalEntityBankAccountByKey()
        {
            int key = (int)base.GetPrimaryKey("LegalEntityBankAccount", "LegalEntityBankAccountKey");

            using (new SessionScope())
            {
                ILegalEntityBankAccount leba = _repo.GetLegalEntityBankAccountByKey(key);
                Assert.IsNotNull(leba);
                Assert.AreEqual(leba.Key, key);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void GetLegalEntityAssetLiabilitiesByAssetLiabilityKeyTest()
        {
            using (new SessionScope())
            {
                LegalEntityAssetLiability_DAO leal = LegalEntityAssetLiability_DAO.FindFirst();
                IEventList<ILegalEntityAssetLiability> leaList = _repo.GetLegalEntityAssetLiabilitiesByAssetLiabilityKey(leal.AssetLiability.Key);
                Assert.IsNotNull(leaList);
                Assert.IsTrue(leaList.Count > 0);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void GetLegalEntityAssetLiabilityListTest()
        {
            using (new SessionScope())
            {
                LegalEntityAssetLiability_DAO leal = LegalEntityAssetLiability_DAO.FindFirst();
                IEventList<ILegalEntityAssetLiability> leaList = _repo.GetLegalEntityAssetLiabilityList(leal.LegalEntity.Key);
                Assert.IsNotNull(leaList);
                Assert.IsTrue(leaList.Count > 0);
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void GetLegalEntityEmploymentTypeForApplicationTest()
        {
            using (new SessionScope())
            {
                SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                IStageDefinitionRepository SDR = _mockery.StrictMock<IStageDefinitionRepository>();
                MockCache.Add(typeof(IStageDefinitionRepository).ToString(), SDR);
                IApplication app = _mockery.StrictMock<IApplication>();
                ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
                IEmployment employment = _mockery.StrictMock<IEmployment>();
                IEmploymentType empType = _mockery.StrictMock<IEmploymentType>();
                IEventList<IEmployment> empList = new EventList<IEmployment>();
                IStageDefinitionStageDefinitionGroup sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();

                //
                SetupResult.For(sdsdg.Key).Return(1);
                SetupResult.For(empType.Key).Return((int)EmploymentTypes.Salaried);
                SetupResult.For(employment.ConfirmedBasicIncome).Return(10D);
                SetupResult.For(employment.BasicIncome).Return(10D);
                SetupResult.For(employment.EmploymentType).Return(empType);
                empList.Add(null, employment);
                SetupResult.For(legalEntity.Employment).Return(empList);
                SetupResult.For(app.Key).Return(1);
                SetupResult.For(SDR.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ApplicationCapture, (int)StageDefinitions.ApplicationCaptureSubmitted)).IgnoreArguments().Return(sdsdg);
                SetupResult.For(SDR.CountCompositeStageOccurance(1, 1)).IgnoreArguments().Return(1);
                _mockery.ReplayAll();
                IEmploymentType employmentType = _repo.GetLegalEntityEmploymentTypeForApplication(legalEntity, app);
                Assert.IsNotNull(employmentType);
                SetRepositoryStrategy(TypeFactoryStrategy.Default);
            }
        }

        [Test]
        public void GetLegalEntityIncomeForApplicationTest()
        {
            using (new SessionScope())
            {
                using (new SessionScope())
                {
                    SetRepositoryStrategy(TypeFactoryStrategy.Mock);
                    IStageDefinitionRepository SDR = _mockery.StrictMock<IStageDefinitionRepository>();
                    MockCache.Add(typeof(IStageDefinitionRepository).ToString(), SDR);
                    IApplication app = _mockery.StrictMock<IApplication>();
                    ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
                    IEmployment employment = _mockery.StrictMock<IEmployment>();
                    IEmploymentType empType = _mockery.StrictMock<IEmploymentType>();
                    IEventList<IEmployment> empList = new EventList<IEmployment>();
                    IStageDefinitionStageDefinitionGroup sdsdg = _mockery.StrictMock<IStageDefinitionStageDefinitionGroup>();

                    //
                    SetupResult.For(sdsdg.Key).Return(1);
                    SetupResult.For(empType.Key).Return((int)EmploymentTypes.Salaried);
                    SetupResult.For(employment.MonthlyIncome).Return(10D);
                    SetupResult.For(employment.ConfirmedIncome).Return(10D);
                    SetupResult.For(employment.ConfirmedBasicIncome).Return(10D);
                    SetupResult.For(employment.BasicIncome).Return(10D);
                    SetupResult.For(employment.EmploymentType).Return(empType);
                    empList.Add(null, employment);
                    SetupResult.For(legalEntity.Employment).Return(empList);
                    SetupResult.For(app.Key).Return(1);
                    SetupResult.For(SDR.GetStageDefinitionStageDefinitionGroup((int)StageDefinitionGroups.ApplicationCapture, (int)StageDefinitions.ApplicationCaptureSubmitted)).IgnoreArguments().Return(sdsdg);
                    SetupResult.For(SDR.CountCompositeStageOccurance(1, 1)).IgnoreArguments().Return(1);
                    _mockery.ReplayAll();
                    double income = _repo.GetLegalEntityIncomeForApplication(legalEntity, app);
                    Assert.IsTrue(income > 0);
                    SetRepositoryStrategy(TypeFactoryStrategy.Default);
                }
            }
        }

        [Test]
        public void GetNaturalPersonByIDNumberTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 LegalEntityKey
                            from [2am].dbo.LegalEntity (nolock)
                            where len(IDNumber) > 3
                            and LegalEntityTypeKey = 2";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int legalEntityKey = Convert.ToInt32(o);
                ILegalEntityNaturalPerson le = _repo.GetLegalEntityByKey(legalEntityKey) as ILegalEntityNaturalPerson;

                // Test
                ILegalEntityNaturalPerson leNP = _repo.GetNaturalPersonByIDNumber(le.IDNumber);
                Assert.IsNotNull(leNP);
            }
        }

        [Test]
        public void GetNaturalPersonsByIDNumberTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 LegalEntityKey
                            from [2am].dbo.LegalEntity (nolock)
                            where len(PassportNumber) > 3
                            and LegalEntityTypeKey = 2";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int legalEntityKey = Convert.ToInt32(o);
                ILegalEntityNaturalPerson le = _repo.GetLegalEntityByKey(legalEntityKey) as ILegalEntityNaturalPerson;

                // Test
                IReadOnlyEventList<ILegalEntityNaturalPerson> leNPList = _repo.GetNaturalPersonsByIDNumber(le.IDNumber, 1);
                Assert.IsNotNull(leNPList);
                Assert.IsTrue(leNPList.Count > 0);
            }
        }

        [Test]
        public void GetNaturalPersonsByPassportNumberTest()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 LegalEntityKey
                            from [2am].dbo.LegalEntity (nolock)
                            where len(PassportNumber) > 3
                            and LegalEntityTypeKey = 2";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int legalEntityKey = Convert.ToInt32(o);
                ILegalEntityNaturalPerson le = _repo.GetLegalEntityByKey(legalEntityKey) as ILegalEntityNaturalPerson;

                // Test
                IReadOnlyEventList<ILegalEntityNaturalPerson> leNPList = _repo.GetNaturalPersonsByPassportNumber(le.PassportNumber, 1);
                Assert.IsNotNull(leNPList);
                Assert.IsTrue(leNPList.Count > 0);
            }
        }

        [Test]
        public void GetTrustsByRegistrationNumberTest()
        {
            string sql = @"select top 1 LegalEntityKey
                        from [2am].dbo.LegalEntity (nolock)
			            where len(RegistrationNumber) > 3
			            and LegalEntityTypeKey = 5";

            object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            int legalEntityKey = Convert.ToInt32(o);
            ILegalEntityTrust le = _repo.GetLegalEntityByKey(legalEntityKey) as ILegalEntityTrust;

            // Test
            IReadOnlyEventList<ILegalEntityTrust> leTrustList = _repo.GetTrustsByRegistrationNumber(le.RegistrationNumber, 1);
            Assert.IsNotNull(leTrustList);
            Assert.IsTrue(leTrustList.Count > 0);
        }

        [Test]
        public void GetEmptyLegalEntityAssetLiabilityTest()
        {
            using (new SessionScope())
            {
                ILegalEntityAssetLiability leAL = _repo.GetEmptyLegalEntityAssetLiability();
                Assert.IsNotNull(leAL);
            }
        }

        [Test]
        public void GetCompaniesByRegistrationNumberTest()
        {
            string sql = @"select top 1 le.LegalEntityKey from [2am].dbo.LegalEntity le (nolock)
			where le.RegistrationNumber IS NOT NULL and len(le.RegistrationNumber) > 1
			and le.LegalEntityTypeKey = 3";

            object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
            int legalEntityKey = Convert.ToInt32(o);
            ILegalEntityCompany le = _repo.GetLegalEntityByKey(legalEntityKey) as ILegalEntityCompany;

            // Test
            IReadOnlyEventList<ILegalEntityCompany> leCompList = _repo.GetCompaniesByRegistrationNumber(le.RegistrationNumber, 1);
            Assert.IsNotNull(leCompList);
        }

        [Test]
        public void HasNonLeadRolesTest()
        {
            using (new SessionScope())
            {
                ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
                IRole role = _mockery.StrictMock<IRole>();
                IEventList<IRole> roleList = new EventList<IRole>();
                IAccount account = _mockery.StrictMock<IAccount>();
                IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();

                SetupResult.For(accountStatus.Key).Return((int)SAHL.Common.Globals.AccountStatuses.Open);
                SetupResult.For(account.AccountStatus).Return(accountStatus);
                SetupResult.For(role.Account).Return(account);
                roleList.Add(null, role);
                SetupResult.For(legalEntity.Roles).Return(roleList);
                _mockery.ReplayAll();
                bool test = _repo.HasNonLeadRoles(legalEntity);
                Assert.IsTrue(test);
            }
        }

        [Test]
        public void ActivateRoleTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                Role_DAO r = Role_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IRole role = BMTM.GetMappedType<IRole, Role_DAO>(r);
                _repo.ActivateRole(role);
                Assert.IsTrue(role.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active);
            }
        }

        [Test]
        public void DeactivateRoleTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                Role_DAO r = Role_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                IRole role = BMTM.GetMappedType<IRole, Role_DAO>(r);
                _repo.DeactivateRole(role);
                Assert.IsTrue(role.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Inactive);
            }
        }

        [Test]
        public void GetAttorneyByKeyTest()
        {
            using (new SessionScope())
            {
                Attorney_DAO att = Attorney_DAO.FindFirst();
                IAttorney attorney = _repo.GetAttorneyByKey(att.Key);
                Assert.IsNotNull(attorney);
            }
        }

        [Test]
        public void GetEmptyAssetLiabilityTest()
        {
            using (new SessionScope())
            {
                IAssetLiability al = _repo.GetEmptyAssetLiability(AssetLiabilityTypes.FixedProperty);
                Assert.IsNotNull(al);
            }
        }

        [Test]
        public void DeleteAddressTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                LegalEntityAddress_DAO daoLegalEntityAddress = LegalEntityAddress_DAO.FindFirst();
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                ILegalEntityAddress leadd = BMTM.GetMappedType<ILegalEntityAddress, LegalEntityAddress_DAO>(daoLegalEntityAddress);
                _repo.DeleteAddress(leadd);
                LegalEntityAddress_DAO[] leAddList = LegalEntityAddress_DAO.FindAllByProperty("Key", daoLegalEntityAddress.Key);

                if (leAddList[0].GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                    Assert.IsTrue(true);
                else
                    Assert.Fail();
            }
        }

        [Test]
        public void DeleteLegalEntityBankAccountTest()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                SAHLPrincipal pcp = new SAHLPrincipal(new GenericIdentity("sahl\bcuser"));
                LegalEntityBankAccount_DAO leba = LegalEntityBankAccount_DAO.FindFirst();
                int legalEntityBankAccountKey = leba.Key;
                _repo.DeleteLegalEntityBankAccount(legalEntityBankAccountKey, pcp);
                LegalEntityBankAccount_DAO[] lebaList = LegalEntityBankAccount_DAO.FindAllByProperty("Key", legalEntityBankAccountKey);

                if (lebaList[0].GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                    Assert.IsTrue(true);
                else
                    Assert.Fail();
            }
        }

        [Test]
        public void LegalEntityLoginSavePass()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                ILegalEntity person = _repo.GetLegalEntityByKey(55839);
                ILegalEntityLogin loginDetails = _repo.CreateEmptyLegalEntityLogin();

                loginDetails.Username = "pieterg";
                loginDetails.Password = CalculateMD5Hash("password");
                loginDetails.GeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];

                person.LegalEntityLogin = loginDetails;
                loginDetails.LegalEntity = person;
                _repo.SaveLegalEntityLogin(loginDetails);
                _repo.SaveLegalEntity(person, false);
            }
        }

        [Test]
        public void LegalEntityLoginRetrievePass()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                var lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

                ILegalEntity person = _repo.GetLegalEntityByKey(55839);
                ILegalEntityLogin loginDetails = _repo.GetLegalEntityLogin("testuser", CalculateMD5Hash("testpassword"));
            }
        }

        [Test]
        public void GetWebAccessLegalEntity()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string query = @"select top 1 le.EmailAddress
                                from ExternalRole er
                                join LegalEntity le on er.LegalEntityKey = le.LegalEntityKey
                                where ExternalRoleTypeKey = 10 and er.GeneralStatusKey = 1
                                order by le.LegalEntityKey desc";

                object emailAddress = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), new ParameterCollection());
                ILegalEntityNaturalPerson person = _repo.GetWebAccessLegalEntity((string)emailAddress);
                IExternalRole externalRole = _repo.GetExternalRole(person, ExternalRoleTypes.WebAccess);
                IAttorney attorney = _repo.GetAttorney(externalRole);
            }
        }

        /// <summary>
        /// Calculate MD5 Hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        [Test]
        public void GetExternalRolesByRoleTypeAndStatus()
        {
            using (new SessionScope())
            {
                string sql = @"select top 1 GenericKey from ExternalRole er
                    where er.GenericKeyTypeKey = 2
                    and ExternalRoleTypeKey = 1
                    and GeneralStatusKey = 1";

                object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());

                if (obj != null)
                    _repo.GetExternalRoles((int)obj, GenericKeyTypes.Offer, ExternalRoleTypes.Client, GeneralStatuses.Active);
            }
        }

        [Test]
        public void GetFurtherLendingApplicationsByLegalEntity()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                string sql = @"SELECT   top 1  LegalEntity.LegalEntityKey
							FROM         Offer INNER JOIN
							OfferRole ON Offer.OfferKey = OfferRole.OfferKey INNER JOIN
							LegalEntity ON OfferRole.LegalEntityKey = LegalEntity.LegalEntityKey
							WHERE offerTypekey in (2,3,4) and OfferStatusKey = 1
							ORDER BY 1 DESC";

                object o = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), new ParameterCollection());
                int legalEntityKey = Convert.ToInt32(o);
                ILegalEntity person = _repo.GetLegalEntityByKey(legalEntityKey);
                IReadOnlyEventList<IApplication> FLApps = _repo.GetOpenFurtherLendingApplicationsByLegalEntity(person);
                Assert.IsTrue(FLApps.Count >= 1);
            }
        }
    }
}
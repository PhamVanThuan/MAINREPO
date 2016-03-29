using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Rules.Suretor;
using SAHL.Common.Collections;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.Test.Suretor
{
    [TestFixture]
    public class Suretor : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if no arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void SuretorRemoveCheckConfirmedIncome_NoArgumentsPassed()
        {
            SuretorRemoveCheckConfirmedIncome rule = new SuretorRemoveCheckConfirmedIncome();

            ExecuteRule(rule, 0);
        }

        /// <summary>
        /// Test expects the rule to throw an Exception(ArgumentException) if incorrect arguments are passed.
        /// </summary>
        [NUnit.Framework.Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateAssuredLifeMinimumRequired_IncorrectArgumentsPassed()
        {
            SuretorRemoveCheckConfirmedIncome rule = new SuretorRemoveCheckConfirmedIncome();

            // Setup an incorrect Argument to pass -- the rule should accept an IRole
            IApplication application = _mockery.StrictMock<IApplication>();
            ExecuteRule(rule, 0, application);
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Suretor.SuretorRemoveCheckConfirmedIncome"/> rule.
        /// </summary>
        [Test]
        public void SuretorRemoveCheckConfirmedIncome_Failure()
        {
            IRuleService ruleService = ServiceFactory.GetService<IRuleService>(); ;
            DomainMessageCollection messages = new DomainMessageCollection();

            using (new TransactionScope())
            {
                string sql = @"select r.AccountKey
                            from [2am]..Role r (nolock)
                            left join [2am]..Employment e (nolock) on r.LegalEntityKey = e.LegalEntityKey
                            where r.RoleTypeKey = 3 -- suretor
                            and r.GeneralStatusKey = 1
                            group by r.AccountKey
                            having count(r.AccountRoleKey) = 1 and sum(isnull(e.ConfirmedIncome,0)) > 0
                            except
                            select r.AccountKey
                            from [2am]..Role r (nolock)
                            join [2am]..Employment e (nolock) on r.LegalEntityKey = e.LegalEntityKey
                            where r.RoleTypeKey = 2 -- main applicant
                            group by r.AccountKey
                            having sum(isnull(e.ConfirmedIncome,0)) > 0";

                DataTable DT = base.GetQueryResults(sql);
                Assert.That(DT.Rows.Count > 0, "Test failed because no valid data was found!");
                int accountkey = Convert.ToInt32(DT.Rows[0][0]);

                IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
                IAccount account = accountRepo.GetAccountByKey(accountkey);

                string message = string.Format("Test failed because the GetAccountByKey() method failed to get an Account ({0})", accountkey);
                Assert.That(account != null, message);

                List<IRole> suretors = account.Roles.Where(x => x.RoleType.Key == (int)Globals.RoleTypes.Suretor && x.GeneralStatus.Key == (int)Globals.GeneralStatuses.Active).ToList();

                message = string.Format("Test failed because there is no suretor on the account ({0})", accountkey);
                Assert.That(suretors.Count > 0, message);
                message = string.Format("Test failed because there is more than 1 suretor on the account ({0})", accountkey);
                Assert.That(suretors.Count() < 2, message);

                IRole role = suretors[0];

                ruleService.Enabled = false;
                role.GeneralStatus = new GeneralStatus(GeneralStatus_DAO.Find((int)Globals.GeneralStatuses.Inactive));
                ruleService.Enabled = true;
                int result = -1;
                result = ruleService.ExecuteRule(messages, "SuretorRemoveCheckConfirmedIncome", role);
                ruleService.Enabled = false;

                message = string.Format("Test failed because the rule did not work as intended. AccountRoleKey = {0}, ", role.Key);
                Assert.That(result == 0, message);
            }

            //SuretorRemoveCheckConfirmedIncome rule = new SuretorRemoveCheckConfirmedIncome();

            //IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            //using (new TransactionScope())
            //{
            //    IDbConnection con = Helper.GetSQLDBConnection();

            //    try
            //    {
            //        // find first account that has :
            //        // 1 suretor role with a confirmed income
            //        // 1 main applicant role with no confirmed income
            //        string query =
            //            // CTE to get all the accounts that have 2 active roles - 1 main applicant and 1 suretor
            //            " with Accounts_CTE (AccountKey, LegalEntityKey_Suretor, LegalEntityKey_MainApplicant)" +
            //            " as" +
            //            " (" +
            //            " 	select a.AccountKey,r1.LegalEntityKey,r2.LegalEntityKey" +
            //            " 	from [2am]..Account a (nolock)" +
            //            " 	join [2am]..Role r1 (nolock) on	r1.AccountKey = a.AccountKey and r1.RoleTypeKey = 3 and r1.GeneralStatusKey = 1" +
            //            " 	join [2am]..Role r2 (nolock) on r2.AccountKey = a.AccountKey and r2.RoleTypeKey = 2 and r2.GeneralStatusKey = 1" +
            //            " 	group by a.AccountKey,r1.LegalEntityKey,r2.LegalEntityKey" +
            //            " 	having count(r1.AccountKey) = 1 and count (r2.AccountKey) = 1" +
            //            " )" +
            //            //  Get the first account from the above CTE that has Suretor ConfirmedIncome > 0 and MainApplicant ConfirmedIncome  = 0
            //            " select top 1 cte.AccountKey" +
            //            " from Accounts_CTE cte" +
            //            " join [2am]..Employment e1 (nolock) on e1.LegalEntityKey = cte.LegalEntityKey_Suretor and e1.EmploymentStatusKey = 1" +
            //            " join [2am]..Employment e2 (nolock) on e2.LegalEntityKey = cte.LegalEntityKey_MainApplicant and e2.EmploymentStatusKey = 1" +
            //            " group by cte.AccountKey" +
            //            " having sum(isnull(e1.ConfirmedIncome,0)) > 0 and sum(isnull(e2.ConfirmedIncome,0)) = 0 ";

            //        object obj = Helper.ExecuteScalar(con, query);
            //        if (obj != null)
            //        {
            //            //get the account
            //            IAccount account = accountRepo.GetAccountByKey((int)obj);
            //            if (account != null)
            //            {
            //                // get the legalentitykey of the suretor role
            //                int suretorLegalEntityKey = -1;
            //                foreach (IRole role in account.Roles)
            //                {
            //                    if (role.RoleType.Key == (int)Globals.RoleTypes.Suretor)
            //                    {
            //                        suretorLegalEntityKey = role.LegalEntity.Key;
            //                        break;
            //                    }
            //                }

            //                // we need to mock the suretorrole and set the generalstatuskey to invalid
            //                IRole mockedSuretorRole = _mockery.StrictMock<IRole>();
            //                ILegalEntity mockedLegalEntity = _mockery.StrictMock<ILegalEntity>();
            //                IAccount mockedAccount = _mockery.StrictMock<IAccount>();
            //                IGeneralStatus mockedGeneralStatus = _mockery.StrictMock<IGeneralStatus>();
            //                IRoleType mockedRoleType = _mockery.StrictMock<IRoleType>();

            //                SetupResult.For(mockedRoleType.Key).Return((int)Globals.RoleTypes.Suretor);
            //                SetupResult.For(mockedSuretorRole.RoleType).Return(mockedRoleType);

            //                SetupResult.For(mockedLegalEntity.Key).Return(suretorLegalEntityKey);
            //                SetupResult.For(mockedSuretorRole.LegalEntity).Return(mockedLegalEntity);

            //                SetupResult.For(mockedAccount.Key).Return(account.Key);
            //                SetupResult.For(mockedSuretorRole.Account).Return(mockedAccount);

            //                SetupResult.For(mockedGeneralStatus.Key).Return((int)Globals.GeneralStatuses.Inactive);
            //                SetupResult.For(mockedSuretorRole.GeneralStatus).Return(mockedGeneralStatus);

            //                ExecuteRule(rule, 1, mockedSuretorRole);
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        if (con != null)
            //            con.Dispose();
            //    }
            //}
        }

        /// <summary>
        /// Tests <see cref="SAHL.Rules.Suretor.SuretorRemoveCheckConfirmedIncome"/> rule.
        /// </summary>
        [Test]
        public void SuretorRemoveCheckConfirmedIncome_Success()
        {
            SuretorRemoveCheckConfirmedIncome rule = new SuretorRemoveCheckConfirmedIncome();

            IAccountRepository accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();

            using (new TransactionScope())
            {
                IDbConnection con = Helper.GetSQLDBConnection();

                try
                {
                    // find first account that has :
                    // 1 suretor role with a confirmed income
                    // 1 main applicant role with a confirmed income
                    string query =
                        // CTE to get all the accounts that have 2 active roles - 1 main applicant and 1 suretor
                        " with Accounts_CTE (AccountKey, LegalEntityKey_Suretor, LegalEntityKey_MainApplicant)" +
                        " as" +
                        " (" +
                        " 	select a.AccountKey,r1.LegalEntityKey,r2.LegalEntityKey" +
                        " 	from [2am]..Account a (nolock)" +
                        " 	join [2am]..Role r1 (nolock) on	r1.AccountKey = a.AccountKey and r1.RoleTypeKey = 3 and r1.GeneralStatusKey = 1" +
                        " 	join [2am]..Role r2 (nolock) on r2.AccountKey = a.AccountKey and r2.RoleTypeKey = 2 and r2.GeneralStatusKey = 1" +
                        " 	group by a.AccountKey,r1.LegalEntityKey,r2.LegalEntityKey" +
                        " 	having count(r1.AccountKey) = 1 and count (r2.AccountKey) = 1" +
                        " )" +
                        //  Get the first account from the above CTE that has Suretor ConfirmedIncome > 0 and MainApplicant ConfirmedIncome  > 0
                        " select top 1 cte.AccountKey" +
                        " from Accounts_CTE cte" +
                        " join [2am]..Employment e1 (nolock) on e1.LegalEntityKey = cte.LegalEntityKey_Suretor and e1.EmploymentStatusKey = 1" +
                        " join [2am]..Employment e2 (nolock) on e2.LegalEntityKey = cte.LegalEntityKey_MainApplicant and e2.EmploymentStatusKey = 1" +
                        " group by cte.AccountKey" +
                        " having sum(isnull(e1.ConfirmedIncome,0)) > 0 and sum(isnull(e2.ConfirmedIncome,0)) > 0 ";

                    object obj = Helper.ExecuteScalar(con, query);
                    if (obj != null)
                    {
                        //get the account
                        IAccount account = accountRepo.GetAccountByKey((int)obj);
                        if (account != null)
                        {
                            // get the legalentitykey of the suretor role
                            int suretorLegalEntityKey = -1;
                            foreach (IRole role in account.Roles)
                            {
                                if (role.RoleType.Key == (int)Globals.RoleTypes.Suretor)
                                {
                                    suretorLegalEntityKey = role.LegalEntity.Key;
                                    break;
                                }
                            }

                            // we need to mock the suretorrole and set the generalstatuskey to invalid
                            IRole mockedSuretorRole = _mockery.StrictMock<IRole>();
                            ILegalEntity mockedLegalEntity = _mockery.StrictMock<ILegalEntity>();
                            IAccount mockedAccount = _mockery.StrictMock<IAccount>();
                            IGeneralStatus mockedGeneralStatus = _mockery.StrictMock<IGeneralStatus>();
                            IRoleType mockedRoleType = _mockery.StrictMock<IRoleType>();

                            SetupResult.For(mockedRoleType.Key).Return((int)Globals.RoleTypes.Suretor);
                            SetupResult.For(mockedSuretorRole.RoleType).Return(mockedRoleType);

                            SetupResult.For(mockedLegalEntity.Key).Return(suretorLegalEntityKey);
                            SetupResult.For(mockedSuretorRole.LegalEntity).Return(mockedLegalEntity);

                            SetupResult.For(mockedAccount.Key).Return(account.Key);
                            SetupResult.For(mockedSuretorRole.Account).Return(mockedAccount);

                            SetupResult.For(mockedGeneralStatus.Key).Return((int)Globals.GeneralStatuses.Inactive);
                            SetupResult.For(mockedSuretorRole.GeneralStatus).Return(mockedGeneralStatus);

                            ExecuteRule(rule, 0, mockedSuretorRole);
                        }
                    }
                }
                finally
                {
                    if (con != null)
                        con.Dispose();
                }
            }
        }
    }
}
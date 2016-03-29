using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using NHibernate;
using SAHL.Common;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IAccountRepository))]
    public class AccountRepository : AbstractRepositoryBase, IAccountRepository
    {
        private ICastleTransactionsService castleTransactionService;

        public AccountRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public AccountRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        #region IAccountRepository Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        public void RemoveNonNCACompliantAccountInformation(int accountKey)
        {
            Account_DAO account = Account_DAO.Find(accountKey);

            if (account != null && account.AccountInformations != null)
            {
                int key = (int)AccountInformationTypes.NotNCACompliant;

                for (int i = account.AccountInformations.Count - 1; i > -1; i--)
                {
                    if (account.AccountInformations[i].AccountInformationType.Key == key)
                        account.AccountInformations.RemoveAt(i);
                }

                account.SaveAndFlush();
            }
        }

        public DataSet GetBehaviouralScore(int AccountKey)
        {
            string SQL = UIStatementRepository.GetStatement("Common", "GetBSforAccountKey");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", AccountKey));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(SQL, typeof(GeneralStatus_DAO), prms);

            return ds;
        }

        public IAccount CreateDiscriminatedAccountByProduct(int ProductKey)
        {
            switch (ProductKey)
            {
                case (int)Globals.Products.VariableLoan:
                    return new AccountVariableLoan(new AccountVariableLoan_DAO());
                case (int)Globals.Products.VariFixLoan:
                    return new AccountVariFixLoan(new AccountVariFixLoan_DAO());
                case (int)Globals.Products.HomeOwnersCover:
                    return new AccountHOC(new AccountHOC_DAO());
                case (int)Globals.Products.LifePolicy:
                    return new AccountLifePolicy(new AccountLifePolicy_DAO());
                case (int)Globals.Products.SuperLo:
                    return new AccountSuperLo(new AccountSuperLo_DAO());
                case (int)Globals.Products.DefendingDiscountRate:
                    return new AccountDefendingDiscountRate(new AccountDefendingDiscountRate_DAO());
                case (int)Globals.Products.NewVariableLoan:
                    return new AccountNewVariableLoan(new AccountNewVariableLoan_DAO());

                //case 10: //quick cash
                case (int)Globals.Products.Edge:
                    return new AccountEdge(new AccountEdge_DAO());

                default:
                    throw new Exception("Unsupported ProductKey");
            }
        }

        /// <summary>
        /// Returns an Account object fro the given key.
        /// </summary>
        /// <param name="Key">The key of the account to retrieve.</param>
        /// <returns></returns>
        public IAccount GetAccountByKey(int Key)
        {
            Account_DAO Acc = Account_DAO.TryFind(Key);
            if (Acc != null)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IAccount, Account_DAO>(Acc);
            }
            return null;
        }

        public void UpdateAccountDebitOrderFromApplicationDebitOrder(IApplicationDebitOrder iAppdo)
        {
            if (iAppdo.Application.Account != null && iAppdo.Application.Account.AccountStatus.Key == (int)AccountStatuses.Application)
            {
                foreach (IFinancialService fs in iAppdo.Application.Account.FinancialServices)
                {
                    if (fs.AccountStatus.Key == (int)AccountStatuses.Application && (fs.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan || fs.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan))
                    {
                        foreach (IFinancialServiceBankAccount fsb in fs.FinancialServiceBankAccounts)
                        {
                            if (fsb.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
                            {
                                fsb.BankAccount = iAppdo.BankAccount;
                                fsb.DebitOrderDay = iAppdo.DebitOrderDay;
                                fsb.FinancialServicePaymentType = iAppdo.FinancialServicePaymentType;
                            }
                        }
                    }
                }
            }
        }

        public IAccount GetAccountByCap2InstanceID(long instanceID)
        {
            string sql = @"select acc.* from x2.X2DATA.CAP2_Offers x2Cap (nolock) inner join [2am].[dbo].[account] acc (nolock) on x2cap.AccountKey = acc.AccountKey where instanceid = ?";

            SimpleQuery<Account_DAO> adQ = new SimpleQuery<Account_DAO>(QueryLanguage.Sql, sql, instanceID);
            adQ.AddSqlReturnDefinition(typeof(Account_DAO), "acc");
            Account_DAO[] Accs = adQ.Execute();

            if (Accs != null && Accs.Length == 1)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IAccount, Account_DAO>(Accs[0]);
            }
            return null;
        }

        /// <summary>
        /// Gets an account by the offer key.
        /// </summary>
        /// <param name="applicationKey">The integer application key.</param>
        /// <returns>The <see cref="IAccount">account found using the supplied applicationKey, returns null if no account is found.</see></returns>
        public IAccount GetAccountByApplicationKey(int applicationKey)
        {
            string query = "select AC from Account_DAO AC join AC.Applications A where A.Key = ?";
            SimpleQuery<Account_DAO> SQ = new SimpleQuery<Account_DAO>(query, applicationKey);

            object o = Account_DAO.ExecuteQuery(SQ);
            Account_DAO[] Accs = o as Account_DAO[];
            if (Accs != null && Accs.Length == 1)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IAccount, Account_DAO>(Accs[0]);
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public bool TitleDeedsOnFile(int accountKey)
        {
            string accountString = Convert.ToString(accountKey);
            string query = "select td from TitleDeedCheck_DAO td where td.Key = ?";
            SimpleQuery<TitleDeedCheck_DAO> SQ = new SimpleQuery<TitleDeedCheck_DAO>(query, accountString);

            object o = TitleDeedCheck_DAO.ExecuteQuery(SQ);
            TitleDeedCheck_DAO[] td = o as TitleDeedCheck_DAO[];
            if (td != null && td.Length == 1)
            {
                return true;
            }
            return false;
        }

        #region DetailType Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IDetail> GetDetailByAccountKey(int AccountKey)
        {
            /*string HQL = "from Detail_DAO d where d.Account.Key = ? order by d.DetailDate desc";
            SimpleQuery<Detail_DAO> q = new SimpleQuery<Detail_DAO>(HQL, AccountKey);
            Detail_DAO[] res = q.Execute();
            IEventList<IDetail> list = new DAOEventList<Detail_DAO, IDetail, Detail>(res);
            return new ReadOnlyEventList<IDetail>(list);*/
            IEventList<IDetail> list = null;
            string sql = @"SELECT D.*
			FROM   [2AM].[dbo].[Detail] D (NOLOCK)
			JOIN   [2AM].[dbo].[DetailType] DT (NOLOCK)  ON   D.[DetailTypeKey] = DT.[DetailTypeKey]
			JOIN   [2AM].[dbo].[DetailClass] DC (NOLOCK)  ON   DT.[DetailClassKey] = DC.[DetailClassKey]
			LEFT OUTER JOIN  [2AM].[dbo].[CancellationType] CT (NOLOCK)  ON   D.[LinkID] = CT.[CancellationTypeKey]
			LEFT OUTER JOIN  [SAHLDB].[dbo].[RegMail] R WITH (NOLOCK)  ON     D.AccountKey = R.LoanNumber  AND     (D.DetailTypeKey < 10      OR      D.DetailTypeKey IN (213, 214))
			LEFT OUTER JOIN  [2AM].[dbo].[Attorney] ATT WITH (NOLOCK)  ON     R.AttorneyNumber = ATT.AttorneyKey
			LEFT OUTER JOIN  [2AM].[dbo].[LegalEntity] le WITH (NOLOCK) ON	att.LegalEntityKey = le.LegalEntityKey
			WHERE   D.[AccountKey] = ?  AND     D.[DetailTypeKey] <> 10
			UNION
			SELECT D.*
			FROM   [2AM].[dbo].[Detail] D (NOLOCK)
			JOIN   [2AM].[dbo].[Account] A (NOLOCK)  ON     D.AccountKey = A.AccountKey
			JOIN   [2AM].[dbo].[DetailType] DT (NOLOCK)  ON   D.[DetailTypeKey] = DT.[DetailTypeKey]
			JOIN   [2AM].[dbo].[DetailClass] DC (NOLOCK)  ON   DT.[DetailClassKey] = DC.[DetailClassKey]
			LEFT OUTER JOIN   [2AM].[dbo].[CancellationType] CT (NOLOCK)  ON   D.[LinkID] = CT.[CancellationTypeKey]
			WHERE   D.[AccountKey] = ?  AND     D.[DetailTypeKey] = 10   AND      A.[AccountStatusKey] = 2
			AND      CONVERT(CHAR(10), D.[DetailDate], 121) = CONVERT(CHAR(10), A.[CloseDate], 121)  ORDER BY      D.[DetailDate] DESC; ";

            SimpleQuery<Detail_DAO> q = new SimpleQuery<Detail_DAO>(QueryLanguage.Sql, sql, AccountKey, AccountKey);
            q.AddSqlReturnDefinition(typeof(Detail_DAO), "D");
            Detail_DAO[] res = q.Execute();
            list = new DAOEventList<Detail_DAO, IDetail, Detail>(res);
            return new ReadOnlyEventList<IDetail>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="DetailTypeKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IDetail> GetDetailByAccountKeyAndDetailType(int AccountKey, int DetailTypeKey)
        {
            string SQL = "select d.* from Detail d (nolock) where d.AccountKey = ? and d.DetailTypeKey = ?";

            //string HQL = "from Detail_DAO d where d.Account.Key = ? and d.DetailType.Key = ?";
            SimpleQuery<Detail_DAO> q = new SimpleQuery<Detail_DAO>(QueryLanguage.Sql, SQL, AccountKey, DetailTypeKey);
            q.AddSqlReturnDefinition(typeof(Detail_DAO), "D");
            Detail_DAO[] res = q.Execute();
            IEventList<IDetail> list = new DAOEventList<Detail_DAO, IDetail, Detail>(res);
            return new ReadOnlyEventList<IDetail>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="DetailTypeKey"></param>
        /// <returns></returns>
        public IDetail GetLatestDetailByAccountKeyAndDetailType(int AccountKey, int DetailTypeKey)
        {
            string SQL = "select top 1 * from [2am]..detail d (nolock) where AccountKey = ? and DetailTypeKey = ? order by 1 desc";
            SimpleQuery<Detail_DAO> q = new SimpleQuery<Detail_DAO>(QueryLanguage.Sql, SQL, AccountKey, DetailTypeKey);
            q.AddSqlReturnDefinition(typeof(Detail_DAO), "D");
            Detail_DAO[] res = q.Execute();
            IEventList<IDetail> list = new DAOEventList<Detail_DAO, IDetail, Detail>(res);
            if (list != null && list.Count > 0)
                return list[0];
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IDetail> GetAccountDetailForFurtherLending(int AccountKey)
        {
            //TRAC 9358, all detail types should be shown for FL
            //IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            //IControl control = controlRepo.GetControlByDescription("FurtherLendingDetailTypes");

            //string types = control.ControlText;

            //IAccountRepository AR = RepositoryFactory.GetRepository<IAccountRepository>();
            //IAccount account = AR.GetAccountByKey(AccountKey);

            //string hql = String.Format("from Detail_DAO det " +
            //             "where det.Account.Key = ? " +
            //             "and det.DetailType.Key in ({0})",types);

            string hql = String.Format("from Detail_DAO det " +
                         "where det.Account.Key = ? ");

            SimpleQuery<Detail_DAO> q = new SimpleQuery<Detail_DAO>(hql, AccountKey);

            Detail_DAO[] res = q.Execute();
            IEventList<IDetail> list = new DAOEventList<Detail_DAO, IDetail, Detail>(res);
            return new ReadOnlyEventList<IDetail>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="DetailTypeKey"></param>
        public void RemoveDetailByAccountKeyAndDetailTypeKey(int AccountKey, int DetailTypeKey)
        {
            string query = string.Format("AccountKey={0} and DetailTypeKey={1}", AccountKey, DetailTypeKey);
            Detail_DAO.DeleteAll(query);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="DetailKey"></param>
        public void RemoveDetailByKey(int DetailKey)
        {
            IDetail detail = new Detail(Detail_DAO.Find(DetailKey));
            DeleteDetail(detail);
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IDetail CreateEmptyDetail()
        {
            return base.CreateEmpty<IDetail, Detail_DAO>();

            //return new Detail(new Detail_DAO());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="detail"></param>
        public void DeleteDetail(IDetail detail)
        {
            Detail_DAO dao = (Detail_DAO)(detail as IDAOObject).GetDAOObject();

            dao.DeleteAndFlush();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="detail"></param>
        public void SaveDetail(IDetail detail)
        {
            base.Save<IDetail, Detail_DAO>(detail);

            //IDAOObject dao = detail as IDAOObject;
            //Detail_DAO det = (Detail_DAO)dao.GetDAOObject();
            //det.SaveAndFlush();
            //if (ValidationHelper.PrincipalHasValidationErrors())
            //    throw new DomainValidationException();
        }

        #endregion DetailType Members

        /// <summary>
        /// Used to close a loan account when a detailtype of 10 is loaded against the account.
        /// The stored proc adds the detail type after closing the account correctly, so this function is called instead of loading the detail type.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userid"></param>
        public void CloseLoanAccount(int accountKey, string userid)
        {
            // Update the required fields on the Account
            SAHL.Common.DataAccess.ParameterCollection parameters = new SAHL.Common.DataAccess.ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@UID", userid));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.AccountRepository", "CloseLoanAccount", parameters);
        }

        public void ClosePersonalLoanAccount(int accountKey, string userid)
        {
            // Update the required fields on the Account
            SAHL.Common.DataAccess.ParameterCollection parameters = new SAHL.Common.DataAccess.ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));

            parameters.Add(new SqlParameter("@UID", userid));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.AccountRepository", "ClosePersonalLoanAccount", parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IAccountLifePolicy CreateEmptyLifePolicyAccount()
        {
            return base.CreateEmpty<IAccountLifePolicy, AccountLifePolicy_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Account"></param>
        public void SaveAccount(IAccount Account)
        {
            base.Save<IAccount, Account_DAO>(Account);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ads"></param>
        public void SaveAccountDebtSettlement(IAccountDebtSettlement ads)
        {
            base.Save<IAccountDebtSettlement, AccountDebtSettlement_DAO>(ads);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Account"></param>
        public void CreateAccount(IAccount Account)
        {
            IDAOObject IDAOAccount = Account as IDAOObject;
            Account_DAO DAO = (Account_DAO)IDAOAccount.GetDAOObject();
            DAO.CreateAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        /// Creates and Empty Role object
        /// </summary>
        /// <returns>IRole</returns>
        public IRole CreateEmptyRole()
        {
            return base.CreateEmpty<IRole, Role_DAO>();
        }

        /// <summary>
        /// Implements <see cref="IAccountRepository.GetRoleByKey"/>
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IRole GetRoleByKey(int Key)
        {
            return base.GetByKey<IRole, Role_DAO>(Key);
        }

        /// <summary>
        /// Implements <see cref="IAccountRepository.SaveRole"/>
        /// </summary>
        /// <param name="role"></param>
        public void SaveRole(IRole role)
        {
            base.Save<IRole, Role_DAO>(role);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IAccountSequence GetAccountSequenceByKey(int Key)
        {
            return base.GetByKey<IAccountSequence, AccountSequence_DAO>(Key);
        }

        /// <summary>
        /// Gets an account status object by key.
        /// </summary>
        /// <param name="key">Account Status key</param>
        /// <returns></returns>
        public IAccountStatus GetAccountStatusByKey(AccountStatuses key)
        {
            return base.GetByKey<IAccountStatus, AccountStatus_DAO>((int)key);
        }

        /// <summary>
        /// Implements <see cref="IAccountRepository.SearchAccounts"/>
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IEventList<IAccount> SearchAccounts(IAccountSearchCriteria searchCriteria, int maxRowCount)
        {
            AccountSearchQuery searchQuery = new AccountSearchQuery(searchCriteria, maxRowCount);
            IList<Account_DAO> accounts = Account_DAO.ExecuteQuery(searchQuery) as IList<Account_DAO>;
            return new DAOEventList<Account_DAO, IAccount, Account>(accounts);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IExpenseType GetExpenseTypeByKey(int Key)
        {
            return base.GetByKey<IExpenseType, ExpenseType_DAO>(Key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <param name="RegentStatusKey"></param>
        /// <returns></returns>
        public IRegent GetRegent(int AccountKey, int RegentStatusKey)
        {
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            string HQL = "from Regent_DAO regent where regent.Key = ? and regent.RegentStatus.Key = ?";
            SimpleQuery<Regent_DAO> q = new SimpleQuery<Regent_DAO>(HQL, AccountKey, RegentStatusKey);
            Regent_DAO[] regent = q.Execute();

            if (regent.Length == 0)
                return null;
            else
                return BMTM.GetMappedType<IRegent, Regent_DAO>(regent[0]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public double GetCurrentMonthsInArrears(int AccountKey)
        {
            using (IDbConnection con = Helper.GetSQLDBConnection())
            {
                string query = UIStatementRepository.GetStatement("Account", "GetCurrentMonthsInArrears");
                ParameterCollection parameters = new ParameterCollection();
                Helper.AddIntParameter(parameters, "@LoanNumber", AccountKey);
                object o = Helper.ExecuteScalar(con, query, parameters);

                if (o != null)
                    return Convert.ToDouble(o);
                else
                    return Convert.ToDouble(0);
            }
        }

        public void CreateOfferRolesNotInAccount(IApplication application)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = spc.DomainMessages;
            IAccount account = application.Account;

            DataTable dt = appRepo.GetOfferRolesNotInAccount(application);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ILegalEntity le = leRepo.GetLegalEntityByKey(Convert.ToInt32(dr[0]));
                    IRoleType roleType = base.GetByKey<IRoleType, RoleType_DAO>(Convert.ToInt32(dr[2]));
                    IGeneralStatus openStatus = base.GetByKey<IGeneralStatus, GeneralStatus_DAO>(1);
                    IRole role = base.CreateEmpty<IRole, Role_DAO>();
                    role.LegalEntity = le;
                    role.Account = account;
                    role.RoleType = roleType;
                    role.GeneralStatus = openStatus;
                    role.StatusChangeDate = DateTime.Now;
                    account.Roles.Add(dmc, role);
                }
                accRepo.SaveAccount(account);
            }
        }

        public int GetDetailTypeKeyByDescription(string description)
        {
            string sql = @"Select DetailTypeKey from [2am].[dbo].DetailType (nolock) where Description = @Description";
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@Description", description));
            object obj = castleTransactionService.ExecuteScalarOnCastleTran(sql, typeof(DetailType_DAO), pc);

            int dtKey;
            if (int.TryParse(obj.ToString(), out dtKey))
                return dtKey;

            return -1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="accountStatusKey"></param>
        /// <param name="fixedPayment"></param>
        /// <param name="adUserName"></param>
        public void UpdateAccount(int accountKey, int? accountStatusKey, float? fixedPayment, string adUserName)
        {
            // Update the required fields on the Account
            SAHL.Common.DataAccess.ParameterCollection parameters = new SAHL.Common.DataAccess.ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@AccountStatusKey", accountStatusKey.HasValue ? accountStatusKey.Value : System.Data.SqlTypes.SqlInt32.Null));
            parameters.Add(new SqlParameter("@FixedPayment", fixedPayment.HasValue ? fixedPayment.Value : System.Data.SqlTypes.SqlDouble.Null));
            parameters.Add(new SqlParameter("@UserID", adUserName));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "UpdateAccount", parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="accountStatusKey"></param>
        public void UpdateAccountStatusWithNoValidation(int applicationKey, int accountStatusKey)
        {
            Application_DAO app = Application_DAO.Find(applicationKey);
            if (null != app.Account && app.Account.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Application)
            {
                app.Account.AccountStatus = AccountStatus_DAO.Find(accountStatusKey);
                app.SaveAndFlush();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="fixedPayment"></param>
        /// <param name="userid"></param>
        public void UpdateFixedPayment(int accountkey, double fixedPayment, string userid)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountkey));
            prms.Add(new SqlParameter("@FixedPayment", fixedPayment));
            prms.Add(new SqlParameter("@UserID", userid));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "UpdateFixedPayment", prms);
        }

        public void OptOutNonPerforming(int accountKey, string userID, int CancellationReasonKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));
            prms.Add(new SqlParameter("@UserID", userID));
            prms.Add(new SqlParameter("@Cancellationreasonkey", CancellationReasonKey));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("MortgageLoan", "OptOutNonPerforming", prms);
        }

        public void OptOutNonPerforming(int accountKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountKey));
            prms.Add(new SqlParameter("@UserID", "X2"));
            prms.Add(new SqlParameter("@Cancellationreasonkey", SAHL.Common.Globals.CancellationReasons.CancelNonPerfoming));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("MortgageLoan", "OptOutNonPerforming", prms);
        }

        public void OptInNonPerforming(int accountKey, string userID)
        {
            ParameterCollection prms = new ParameterCollection();

            prms.Add(new SqlParameter("@AccountKey", accountKey));
            prms.Add(new SqlParameter("@UserID", userID));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("MortgageLoan", "OptInNonPerforming", prms);
        }

        public void ReOpenAccount(int accountKey, string userID)
        {
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@UserID", userID));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "ReOpenAccount", parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public bool HasApplicationBeenInCompany2(int accountKey)
        {
            string sql = UIStatementRepository.GetStatement("Repositories.AccountRepository", "GetOfferHasBeenInCompany2");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            object result = castleTransactionService.ExecuteScalarOnCastleTran(sql, Databases.TwoAM, parameters);
            if (result != null)
            {
                return Convert.ToBoolean(result);
            }
            return false;
        }

        public IReadOnlyEventList<IDetail> GetDetailByAccountKeyAndDetailClassKey(int accountKey, int detailClassKey)
        {
            string SQL = @"select d.*
                            from Detail d (nolock)
                            join DetailType dt (nolock) on d.DetailTypeKey = dt.DetailTypeKey
                            where d.AccountKey = ? and dt.DetailClassKey = ?";

            SimpleQuery<Detail_DAO> q = new SimpleQuery<Detail_DAO>(QueryLanguage.Sql, SQL, accountKey, detailClassKey);
            q.AddSqlReturnDefinition(typeof(Detail_DAO), "d");
            Detail_DAO[] res = q.Execute();
            IEventList<IDetail> list = new DAOEventList<Detail_DAO, IDetail, Detail>(res);
            return new ReadOnlyEventList<IDetail>(list);
        }

        #endregion IAccountRepository Members

        #region AccountAttorneyInvoice

        /// <summary>
        ///
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IAccountAttorneyInvoice GetAccountAttorneyInvoiceByKey(int key)
        {
            return base.GetByKey<IAccountAttorneyInvoice, AccountAttorneyInvoice_DAO>(key);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public IEventList<IAccountAttorneyInvoice> GetAccountAttorneyInvoiceListByAccountKey(int accountKey)
        {
            IList<AccountAttorneyInvoice_DAO> accAttInvs = AccountAttorneyInvoice_DAO.FindAllByProperty("Key", "AccountKey", accountKey);
            return new DAOEventList<AccountAttorneyInvoice_DAO, IAccountAttorneyInvoice, AccountAttorneyInvoice>(accAttInvs);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accAttInv"></param>
        public void SaveAccountAttorneyInvoice(IAccountAttorneyInvoice accAttInv)
        {
            base.Save<IAccountAttorneyInvoice, AccountAttorneyInvoice_DAO>(accAttInv);
        }

        /// <summary>
        /// Get an empty AccountAttorneyInvoice
        /// </summary>
        /// <returns></returns>
        public IAccountAttorneyInvoice CreateEmptyAccountAttorneyInvoice()
        {
            return base.CreateEmpty<IAccountAttorneyInvoice, AccountAttorneyInvoice_DAO>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accAttInv"></param>
        public void DeleteAccountAttorneyInvoice(IAccountAttorneyInvoice accAttInv)
        {
            AccountAttorneyInvoice_DAO dao = (AccountAttorneyInvoice_DAO)(accAttInv as IDAOObject).GetDAOObject();
            dao.DeleteAndFlush();
        }

        #endregion AccountAttorneyInvoice

        #region Functions On SAHLDB

        /// <summary>
        /// Convert Staff Loan
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        public void ConvertStaffLoan(int accountKey, string userID)
        {
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@UserID", userID));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "ConvertStaffLoan", parameters);
        }

        public void UnConvertStaffLoan(int accountKey, string userID, CancellationReasons cancellationReason)
        {
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));
            parameters.Add(new SqlParameter("@UserID", userID));
            parameters.Add(new SqlParameter("@CancellationReasonKey", (int)cancellationReason));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("COMMON", "UnConvertStaffLoan", parameters);
        }

        #endregion Functions On SAHLDB
    }
}
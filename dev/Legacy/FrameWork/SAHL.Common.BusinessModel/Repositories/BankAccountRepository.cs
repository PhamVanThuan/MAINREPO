using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.DomainMessages;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.Collections;
using System.Security.Principal;
using SAHL.Common.Security;
using SAHL.Common;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using Castle.ActiveRecord.Framework;
using NHibernate;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IBankAccountRepository))]
    public class BankAccountRepository : AbstractRepositoryBase, IBankAccountRepository
    {
        public BankAccountRepository()
        {

        }

        public IReadOnlyEventList<IACBBranch> GetACBBranchesByPrefix( int ACBBankKey, string Prefix, int maxRowCount)
        {
            //SAHLPrincipalCache SPC = SAHLPrincipalCache.GetPrincipalCache(new SAHLPrincipal(WindowsIdentity.GetCurrent()));
            ACBBank bank = new ACBBank(ACBBank_DAO.Find(ACBBankKey));
            return bank.GetACBBranchesByPrefix(Prefix, maxRowCount);
        }

		public IBankAccount GetBankAccountByKey(int Key)
        {
			return base.GetByKey<IBankAccount, BankAccount_DAO>(Key);
			
			//BankAccount_DAO BAD = BankAccount_DAO.Find(BankAccountKey);
			//IBankAccount BA = null;
			//if (BAD != null)
			//    BA = new BankAccount(BAD);
			//return BA;
        }

        public IBankAccount GetBankAccountByACBBranchCodeAndAccountNumber(string acbBranchCode, string bankAccountNumber)
        {
            string HQL = "select ba from BankAccount_DAO ba where ba.ACBBranch.Key = ? and ba.AccountNumber = ?";

            SimpleQuery<BankAccount_DAO> q = new SimpleQuery<BankAccount_DAO>(HQL, acbBranchCode, bankAccountNumber);
            q.SetQueryRange(1); //although there should never be more than 1 anyway...
            BankAccount_DAO[] res = q.Execute();

            if (res != null && res.Length > 0)
                return new BankAccount(res[0]);

            return null;

        }

		public IACBBank GetACBBankByKey(int Key)
        {
			return base.GetByKey<IACBBank, ACBBank_DAO>(Key);
			
			//ACBBank_DAO BAD = ACBBank_DAO.Find(ACBBankKey);
			//IACBBank BA = null;
			//if (BAD != null)
			//    BA = new ACBBank(BAD);
			//return BA;
        }

		public IACBBranch GetACBBranchByKey(string ACBBranchKey)
        {
			ACBBranch_DAO dao = ACBBranch_DAO.TryFind(ACBBranchKey);
            if (dao == null)
                return null;

            IBusinessModelTypeMapper bmtm = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            return bmtm.GetMappedType<IACBBranch>(dao);
        }

		public IACBType GetACBTypeByKey(int Key)
        {
			return base.GetByKey<IACBType, ACBType_DAO>(Key);
			
			//ACBType_DAO dao = ACBType_DAO.Find(ACBTypeKey);
			//IACBType obj = null;

			//if (dao != null)
			//    obj = new ACBType(dao);
			//return obj;
        }

        /// <summary>
        /// Implements <see cref="IBankAccountRepository.GetEmptyBankAccount"></see>.
        /// </summary>
        public IBankAccount GetEmptyBankAccount()
        {
			return base.CreateEmpty<IBankAccount, BankAccount_DAO>();
			//return new BankAccount(new BankAccount_DAO());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccount"></param>
        public void SaveBankAccount(IBankAccount bankAccount)
        {

            //if (bankAccount.Original == null
            //    || bankAccount.Original.ACBBranch.Key == bankAccount.ACBBranch.Key
            //    && bankAccount.Original.ACBType.Key == bankAccount.ACBType.Key
            //    && bankAccount.Original.AccountNumber == bankAccount.AccountNumber)
            //{
                base.Save<IBankAccount, BankAccount_DAO>(bankAccount);
            //    return;
            //}

            //int results = 0;
            //int FSCount = 0;
            //int AppDSCount = 0;
            //int AccDSCount = 0;
            //int LECount = 0;

            //ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();

            ////FinancialServiceBankAccount
            //string query = "select count(*) countRows from FinancialServiceBankAccount fsba where fsba.BankAccountKey = " + bankAccount.Key.ToString();

            //ISession FSBASession = sessionHolder.CreateSession(typeof(FinancialServiceBankAccount_DAO));

            //IQuery sqlQuery = FSBASession.CreateSQLQuery(query).AddScalar("countRows", NHibernateUtil.Int32);

            //FSCount = sqlQuery.UniqueResult<int>();
            //results += FSCount;

            //// ApplicationDebtSettlement
            //query = "select count(*) countRows from OfferDebtSettlement ods where ods.BankAccountKey = " + bankAccount.Key.ToString();

            //ISession ODSSession = sessionHolder.CreateSession(typeof(ApplicationDebtSettlement_DAO));

            //sqlQuery = ODSSession.CreateSQLQuery(query).AddScalar("countRows", NHibernateUtil.Int32);

            //AppDSCount = sqlQuery.UniqueResult<int>();
            //results += AppDSCount;


            //// AccountDebtSettlement
            //query = "select count(*) countRows from [RCS].AccountDebtSettlement ads where ads.BankAccountKey = " + bankAccount.Key.ToString();

            //ISession ADSSession = sessionHolder.CreateSession(typeof(AccountDebtSettlement_DAO));

            //sqlQuery = ADSSession.CreateSQLQuery(query).AddScalar("countRows", NHibernateUtil.Int32);

            //AccDSCount = sqlQuery.UniqueResult<int>();
            //results += AccDSCount; 
             

            ////LegalEntityBankAccount
            //query = "select count(*) countRows from LegalEntityBankAccount leba where leba.BankAccountKey = " + bankAccount.Key.ToString();

            //ISession LESession = sessionHolder.CreateSession(typeof(LegalEntityBankAccount_DAO));

            //sqlQuery = LESession.CreateSQLQuery(query).AddScalar("countRows", NHibernateUtil.Int32);

            //LECount = sqlQuery.UniqueResult<int>();
            //results += LECount;

            //if (results == 0)
            //{
            //    base.Save<IBankAccount, BankAccount_DAO>(bankAccount);
            //    return;
            //}                        
            
            ////create new account and assign all references to this new account
            //IBankAccount newAccount = this.GetEmptyBankAccount();
            //newAccount.ACBBranch = bankAccount.ACBBranch;
            //newAccount.ACBType = bankAccount.ACBType;
            //newAccount.AccountName = bankAccount.AccountName;
            //newAccount.AccountNumber = bankAccount.AccountNumber;
            //newAccount.ChangeDate = bankAccount.ChangeDate;
            //newAccount.UserID = bankAccount.UserID;

            //bankAccount.Refresh();

            
            //base.Save<IBankAccount, BankAccount_DAO>(newAccount);
            //int maxRowCount = 1000;
            //int newAccountKey = newAccount.Key;

            //#region FinancialServiceBankAccounts

            //if (FSCount > 0)
            //{
            //    List<IFinancialServiceBankAccount> list = new List<IFinancialServiceBankAccount>();

            //    string hql = "from FinancialServiceBankAccount_DAO fsba where fsba.BankAccount.Key = ?";

            //    SimpleQuery<FinancialServiceBankAccount_DAO> q = new SimpleQuery<FinancialServiceBankAccount_DAO>(hql, bankAccount.Key );

            //    q.SetQueryRange(maxRowCount);

            //    FinancialServiceBankAccount_DAO[] fsResults = q.Execute();

            //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    for (int i = 0; i < fsResults.Length; i++)
            //    {
            //        list.Add(BMTM.GetMappedType<IFinancialServiceBankAccount, FinancialServiceBankAccount_DAO>(fsResults[i]));
            //    }

            //    IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            //    foreach (IFinancialServiceBankAccount fsba in list)
            //    {
            //        fsba.BankAccount = newAccount;
            //        fsRepo.SaveFinancialServiceBankAccount(fsba);
            //    }

            //}

            //#endregion


            //#region LegalEntityBankAccounts

            //if (LECount > 0)
            //{
            //    List<ILegalEntityBankAccount> list = new List<ILegalEntityBankAccount>();

            //    string hql = "from LegalEntityBankAccount_DAO leba where leba.BankAccount.Key = ?";

            //    SimpleQuery<LegalEntityBankAccount_DAO> q = new SimpleQuery<LegalEntityBankAccount_DAO>(hql, bankAccount.Key);

            //    q.SetQueryRange(maxRowCount);

            //    LegalEntityBankAccount_DAO[] leResults = q.Execute();

            //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    for (int i = 0; i < leResults.Length; i++)
            //    {
            //        list.Add(BMTM.GetMappedType<ILegalEntityBankAccount, LegalEntityBankAccount_DAO>(leResults[i]));
            //    }

            //    ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            //    foreach (ILegalEntityBankAccount leba in list)
            //    {
            //        if (leba.BankAccount.Key != 0)
            //        {
            //            leba.BankAccount = newAccount;

            //            LER.SaveLegalEntityBankAccount(leba);
            //        }
            //    }
            //}

            //#endregion

            //#region ApplicationDebtSettlements

            //if (AppDSCount > 0)
            //{
            //    List<IApplicationDebtSettlement> list = new List<IApplicationDebtSettlement>();

            //    string hql = "from ApplicationDebtSettlement_DAO ads where ads.BankAccount.Key = ?";

            //    SimpleQuery<ApplicationDebtSettlement_DAO> q = new SimpleQuery<ApplicationDebtSettlement_DAO>(hql, bankAccount.Key);

            //    q.SetQueryRange(maxRowCount);

            //    ApplicationDebtSettlement_DAO[] leResults = q.Execute();

            //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    for (int i = 0; i < leResults.Length; i++)
            //    {
            //        list.Add(BMTM.GetMappedType<IApplicationDebtSettlement, ApplicationDebtSettlement_DAO>(leResults[i]));
            //    }

            //    IApplicationRepository AppRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            //    foreach (IApplicationDebtSettlement ads in list)
            //    {
            //        ads.BankAccount = newAccount;

            //        AppRepo.SaveApplicationDebtSettlement(ads);
            //    }
            //}

            //#endregion

            //#region AccountDebtSettlements

            //if (AccDSCount > 0)
            //{
            //    List<IAccountDebtSettlement> list = new List<IAccountDebtSettlement>();

            //    string hql = "from AccountDebtSettlement_DAO ads where ads.BankAccount.Key = ?";

            //    SimpleQuery<AccountDebtSettlement_DAO> q = new SimpleQuery<AccountDebtSettlement_DAO>(hql, bankAccount.Key);

            //    q.SetQueryRange(maxRowCount);

            //    AccountDebtSettlement_DAO[] leResults = q.Execute();

            //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    for (int i = 0; i < leResults.Length; i++)
            //    {
            //        list.Add(BMTM.GetMappedType<IAccountDebtSettlement, AccountDebtSettlement_DAO>(leResults[i]));
            //    }

            //    IAccountRepository AccRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            //    foreach (IAccountDebtSettlement ads in list)
            //    {
            //        ads.BankAccount = newAccount;
            //        AccRepo.SaveAccountDebtSettlement(ads);
            //    }
            //}

            //#endregion

        }

        public void SaveAccountDebtSettlementBankAccount(int accountDebtSettlementKey, IBankAccount bankAccount)
        {
			BankAccount_DAO daoBankAccount = (BankAccount_DAO)(bankAccount as IDAOObject).GetDAOObject();
            // save the bank account if it's new
            if (bankAccount.Key == 0)
            {              
                daoBankAccount.SaveAndFlush();
            }

            AccountDebtSettlement_DAO ads = AccountDebtSettlement_DAO.Find(accountDebtSettlementKey);
            ads.BankAccount = daoBankAccount;
            // save the LegalEntityBankAccount object
            ads.SaveAndFlush();

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

        }

        // Commented out by MattS (20/05/2009) - isn't used anywhere and doesn't work - validation breaks it
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="accountDebtSettlementKey"></param>
        //public void DeleteAccountDebtSettlementBankAccount(int accountDebtSettlementKey)
        //{
        //    AccountDebtSettlement_DAO ads = AccountDebtSettlement_DAO.Find(accountDebtSettlementKey);
        //    ads.BankAccount = null;
        //    // save the LegalEntityBankAccount object
        //    ads.SaveAndFlush();
        //    if (ValidationHelper.PrincipalHasValidationErrors())
        //        throw new DomainValidationException();

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationDebtSettlementKey"></param>
        public void DeleteApplicationDebtSettlementBankAccount(int applicationDebtSettlementKey)
        {
            ApplicationDebtSettlement_DAO ads = ApplicationDebtSettlement_DAO.Find(applicationDebtSettlementKey);
            ads.BankAccount = null;
            // save the LegalEntityBankAccount object
            ads.SaveAndFlush();
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();

        }

        /// <summary>
        /// Implements <see cref="IBankAccountRepository.SearchNonLegalEntityBankAccounts"/>
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IEventList<IBankAccount> SearchNonLegalEntityBankAccounts(IBankAccountSearchCriteria searchCriteria, int maxRowCount)
        {
            List<IBankAccount> list = new List<IBankAccount>();

            string hql = "";

            SimpleQuery<BankAccount_DAO> q = null;            
            
            //if (searchCriteria.AccountName.Length > 0)
            //{
            //    hql = "from BankAccount_DAO as ba " +
            //                 "where ba.ACBBranch.Key = ? " +
            //                 "and ba.AccountName = ? " +
            //                 "and ba.AccountNumber = ? " +
            //                 "and ba.Key not in (select leba.BankAccount.Key from LegalEntityBankAccount_DAO as leba)";
            //    q = new SimpleQuery<BankAccount_DAO>(hql, searchCriteria.ACBBranchKey, searchCriteria.AccountName, searchCriteria.AccountNumber);

            //}
            //else
            //{
                hql = "from BankAccount_DAO as ba " +
                    "where ba.ACBBranch.Key = ? " +
                    "and ba.AccountNumber = ? " +
                    "and ba.Key not in (select leba.BankAccount.Key from LegalEntityBankAccount_DAO as leba)";
                     
                 q = new SimpleQuery<BankAccount_DAO>(hql, searchCriteria.ACBBranchKey, searchCriteria.AccountNumber);

            //}


            q.SetQueryRange(maxRowCount);
            BankAccount_DAO[] results = q.Execute();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            for (int i = 0; i < results.Length; i++)
            {
                list.Add(BMTM.GetMappedType<IBankAccount, BankAccount_DAO>(results[i]));
            }

            return new EventList<IBankAccount>(list);




            //List<IFinancialServiceBankAccount> list = new List<IFinancialServiceBankAccount>();

            //string hql = "SELECT f from FinancialServiceBankAccount_DAO f WHERE f.BankAccount.Key = ?";
            //SimpleQuery<FinancialServiceBankAccount_DAO> q = new SimpleQuery<FinancialServiceBankAccount_DAO>(hql, Key);
            //q.SetQueryRange(maxRecords);
            //FinancialServiceBankAccount_DAO[] results = q.Execute();

            //IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //for (int i = 0; i < results.Length; i++)
            //{
            //    list.Add(BMTM.GetMappedType<IFinancialServiceBankAccount, FinancialServiceBankAccount_DAO>(results[i]));
            //}

            //return new ReadOnlyEventList<IFinancialServiceBankAccount>(list);      





            //BankAccountNonLegalEntitySearchQuery searchQuery = new BankAccountNonLegalEntitySearchQuery(searchCriteria, maxRowCount);
            //IList<BankAccount_DAO> bankAccounts = BankAccount_DAO.ExecuteQuery(searchQuery) as IList<BankAccount_DAO>;
            //return new DAOEventList<BankAccount_DAO, IBankAccount, BankAccount>(bankAccounts);
        }


        /// <summary>
        /// Implements <see cref="IBankAccountRepository.SearchLegalEntityBankAccounts"/>
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="maxRowCount"></param>
        /// <returns></returns>
        public IEventList<ILegalEntityBankAccount> SearchLegalEntityBankAccounts( IBankAccountSearchCriteria searchCriteria, int maxRowCount)
        {
            BankAccountLegalEntitySearchQuery searchQuery = new BankAccountLegalEntitySearchQuery(searchCriteria, maxRowCount);
            IList<LegalEntityBankAccount_DAO> bankAccounts = LegalEntityBankAccount_DAO.ExecuteQuery(searchQuery) as IList<LegalEntityBankAccount_DAO>;
            return new DAOEventList<LegalEntityBankAccount_DAO, ILegalEntityBankAccount, LegalEntityBankAccount>(bankAccounts);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    ///
    /// </summary>
    [FactoryType(typeof(IHOCRepository))]
    public class HOCRepository : AbstractRepositoryBase, IHOCRepository
    {
        public HOCRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public HOCRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <param name="useAccount"></param>
        public IAccountHOC RetrieveHOCByOfferKey(int applicationKey, ref bool useAccount)
        {
            IAccountHOC hocAccount = null;
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication app = appRepo.GetApplicationByKey(applicationKey);

            if (app.ApplicationType.Key == (int)OfferTypes.ReAdvance
                || app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance
                || app.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
            {
                useAccount = true;
                foreach (IAccount account in app.Account.RelatedChildAccounts)
                {
                    if (account.Product.Key == (int)Products.HomeOwnersCover)
                    {
                        hocAccount = account as IAccountHOC;
                        break;
                    }
                }
            }
            else
            {
                foreach (IAccount account in app.RelatedAccounts)
                {
                    if (account.Product.Key == (int)Products.HomeOwnersCover)
                    {
                        hocAccount = account as IAccountHOC;
                        break;
                    }
                }
            }
            return hocAccount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        public IAccountHOC RetrieveHOCByOfferKey(int applicationKey)
        {
            IAccountHOC hocAccount = null;
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication app = appRepo.GetApplicationByKey(applicationKey);

            if (app.ApplicationType.Key == (int)OfferTypes.ReAdvance
                || app.ApplicationType.Key == (int)OfferTypes.FurtherAdvance
                || app.ApplicationType.Key == (int)OfferTypes.FurtherLoan)
            {
                foreach (IAccount account in app.Account.RelatedChildAccounts)
                {
                    if (account.Product.Key == (int)Products.HomeOwnersCover)
                    {
                        hocAccount = account as IAccountHOC;
                        break;
                    }
                }
            }
            else
            {
                foreach (IAccount account in app.RelatedAccounts)
                {
                    if (account.Product.Key == (int)Products.HomeOwnersCover)
                    {
                        hocAccount = account as IAccountHOC;
                        break;
                    }
                }
            }
            return hocAccount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="useAccount"></param>
        public IAccountHOC RetrieveHOCByAccountKey(int accountKey, ref bool useAccount)
        {
            IAccountHOC hocAccount = null;
            IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccount account = accRepository.GetAccountByKey(accountKey);
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
            useAccount = true;
            foreach (IAccount acc in account.RelatedChildAccounts)
            {
                if (acc.Product.Key == (int)Products.HomeOwnersCover)
                {
                    hocAccount = accRepository.GetAccountByKey(acc.Key) as IAccountHOC;
                    break;
                }
            }
            return hocAccount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        public IAccountHOC RetrieveHOCByAccountKey(int accountKey)
        {
            IAccountHOC hocAccount = null;
            IAccountRepository accRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            IAccount account = accRepository.GetAccountByKey(accountKey);
            IMortgageLoanAccount mortgageLoanAccount = account as IMortgageLoanAccount;
            foreach (IAccount acc in account.RelatedChildAccounts)
            {
                if (acc.Product.Key == (int)Products.HomeOwnersCover)
                {
                    hocAccount = accRepository.GetAccountByKey(acc.Key) as IAccountHOC;
                    break;
                }
            }
            return hocAccount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="HOCKey"></param>
        /// <returns></returns>
        public IHOC GetHOCByKey(int HOCKey)
        {
            return base.GetByKey<IHOC, HOC_DAO>(HOCKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="HOCInsurerKey"></param>
        /// <returns></returns>
        public IHOCInsurer GetHOCInsurerByKey(int HOCInsurerKey)
        {
            return base.GetByKey<IHOCInsurer, HOCInsurer_DAO>(HOCInsurerKey);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        public IReadOnlyEventList<IHOCHistoryDetail> GetHOCHistoryDetailByFinancialServiceKey(int FinancialServiceKey)
        {
            string HQL = "from HOCHistoryDetail_DAO h where h.HOCHistory.HOC.Key = ?";
            SimpleQuery<HOCHistoryDetail_DAO> q = new SimpleQuery<HOCHistoryDetail_DAO>(HQL, FinancialServiceKey);
            HOCHistoryDetail_DAO[] res = q.Execute();
            IEventList<IHOCHistoryDetail> list = new DAOEventList<HOCHistoryDetail_DAO, IHOCHistoryDetail, HOCHistoryDetail>(res);
            return new ReadOnlyEventList<IHOCHistoryDetail>(list);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IHOC CreateEmptyHOC()
        {
            return base.CreateEmpty<IHOC, HOC_DAO>();

            //return new HOC(new HOC_DAO());
        }

        /// <summary>
        /// Create empty HOCHistory Detail Record
        /// </summary>
        /// <returns></returns>
        public IHOCHistoryDetail CreateEmptyHOCHistoryDetail()
        {
            return base.CreateEmpty<IHOCHistoryDetail, HOCHistoryDetail_DAO>();

            //return new HOCHistoryDetail(new HOCHistoryDetail_DAO());
        }

        /// <summary>
        /// Create empty HOCHistory Record
        /// </summary>
        /// <returns></returns>
        public IHOCHistory CreateEmptyHOCHistory()
        {
            return base.CreateEmpty<IHOCHistory, HOCHistory_DAO>();

            //return new HOCHistory(new HOCHistory_DAO());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IHOCHistory GetHOCHistoryByKey(int Key)
        {
            return base.GetByKey<IHOCHistory, HOCHistory_DAO>(Key);

            //HOCHistory_DAO hocHistory = HOCHistory_DAO.Find(Key);
            //if (hocHistory != null)
            //{
            //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    return BMTM.GetMappedType<IHOCHistory, HOCHistory_DAO>(hocHistory);

            //}
            //return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="originalInsurerKey"></param>
        /// <param name="hoc"></param>
        /// <param name="updateType"></param>
        public void UpdateHOCWithHistory(IDomainMessageCollection messages, int originalInsurerKey, IHOC hoc, char updateType)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IHOCHistoryDetail hocHistoryDetail = CreateHOCHistoryDetailRecord(hoc, updateType);

            if (originalInsurerKey != hoc.HOCInsurer.Key)
            {
                //update current history
                hoc.HOCHistory.CancellationDate = DateTime.Now;
                SaveHOCHistory(hoc.HOCHistory);

                //create new history
                //use that history key in hoc
                hoc.HOCHistory = CreateHOCHistoryRecord(hoc);

                //historydetail
                hocHistoryDetail.UpdateType = 'I';
            }
            hocHistoryDetail.HOCHistory = hoc.HOCHistory;
            SaveHOCHistory(hoc.HOCHistory);
            hoc.HOCHistory.HOCHistoryDetails.Add(spc.DomainMessages, hocHistoryDetail);
            SaveHOC(hoc);

            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        public void SaveHOC(IHOC hoc)
        {
            if (hoc.HOCInsurer.Key == (int)HOCInsurers.SAHLHOC)
            {
                hoc.HOCPolicyNumber = hoc.FinancialService.Account.Key.ToString();
            }
            base.Save<IHOC, HOC_DAO>(hoc);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hocHistory"></param>
        public void SaveHOCHistory(IHOCHistory hocHistory)
        {
            base.Save<IHOCHistory, HOCHistory_DAO>(hocHistory);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hocHistoryDetail"></param>
        public void SaveHOCHistoryDetail(IHOCHistoryDetail hocHistoryDetail)
        {
            base.Save<IHOCHistoryDetail, HOCHistoryDetail_DAO>(hocHistoryDetail);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <param name="UpdateType"></param>
        /// <returns></returns>
        public IHOCHistoryDetail GetLastestHOCHistoryDetail(int FinancialServiceKey, char UpdateType)
        {
            IHOCHistoryDetail hocHistoryDetail = null;
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            string sql = @"select hhd.*
			from [2am].[dbo].[Hoc] h (nolock)
			inner join
					(select max(hochistoryKey) as hochistoryKey, FinancialServiceKey
					from [2am].[dbo].[HocHistory] (nolock)
					group by FinancialServiceKey) as mhh
				on mhh.FinancialServiceKey = h.FinancialServiceKey
			inner join
					(select max(HocHistoryDetailKey) as HocHistoryDetailKey, hochistoryKey
					from [2am].[dbo].[HocHistoryDetail] (nolock)
					where [HocHistoryDetail].UpdateType = ?
					group by hochistoryKey) as mhhd
				on mhhd.hochistoryKey = mhh.hochistoryKey
			inner join [2am].[dbo].[HocHistoryDetail] hhd (nolock)
				on mhhd.HocHistoryDetailKey = hhd.HocHistoryDetailKey
			where h.FinancialServiceKey = ?";

            SimpleQuery<HOCHistoryDetail_DAO> hhdQ = new SimpleQuery<HOCHistoryDetail_DAO>(QueryLanguage.Sql, sql, UpdateType, FinancialServiceKey);
            hhdQ.AddSqlReturnDefinition(typeof(HOCHistoryDetail_DAO), "hhd");
            HOCHistoryDetail_DAO[] res = hhdQ.Execute();

            if (res != null && res.Length > 0)
                hocHistoryDetail = BMTM.GetMappedType<IHOCHistoryDetail, HOCHistoryDetail_DAO>(res[0]);

            return hocHistoryDetail;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="FinancialServiceKey"></param>
        /// <returns></returns>
        public IHOCHistoryDetail GetLastestHOCHistoryDetail(int FinancialServiceKey)
        {
            IHOCHistoryDetail hocHistoryDetail = null;
            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();

            string sql = UIStatementRepository.GetStatement("Repositories.HOCRepository", "GetLastestHOCHistoryDetail");
            SimpleQuery<HOCHistoryDetail_DAO> hhdQ = new SimpleQuery<HOCHistoryDetail_DAO>(QueryLanguage.Sql, sql, FinancialServiceKey);
            hhdQ.AddSqlReturnDefinition(typeof(HOCHistoryDetail_DAO), "hhd");
            HOCHistoryDetail_DAO[] res = hhdQ.Execute();

            if (res != null && res.Length > 0)
                hocHistoryDetail = BMTM.GetMappedType<IHOCHistoryDetail, HOCHistoryDetail_DAO>(res[0]);

            return hocHistoryDetail;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        public IHOC CreateHOC(int applicationKey)
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplication application = appRepo.GetApplicationByKey(applicationKey);
            IOriginationSourceProduct originationSourceProduct = appRepo.GetOriginationSourceProductBySourceAndProduct((int)OriginationSources.SAHomeLoans, (int)Products.HomeOwnersCover);
            string userID = SAHLPrincipal.GetCurrent().Identity.Name;

            IApplicationProduct appProd = application.CurrentProduct;
            ISupportsVariableLoanApplicationInformation vli = appProd as ISupportsVariableLoanApplicationInformation;
            IApplicationInformationVariableLoan aivl = vli.VariableLoanInformation;

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@OSPKey", originationSourceProduct.Key));
            prms.Add(new SqlParameter("@SPVKey", aivl.SPV.Key));
            prms.Add(new SqlParameter("@UserID", userID));

            SqlParameter AccKeyPrm = new SqlParameter("@AccountKey", SqlDbType.Int);
            AccKeyPrm.Direction = ParameterDirection.Output;
            prms.Add(AccKeyPrm);

            SqlParameter FinKeyPrm = new SqlParameter("@FinancialServiceKey", SqlDbType.Int);
            FinKeyPrm.Direction = ParameterDirection.Output;
            prms.Add(FinKeyPrm);

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.HOCRepository", "CreateHOC", prms);

            int hocAccountKey = Convert.ToInt32(AccKeyPrm.Value);
            int hocFinancialServiceKey = Convert.ToInt32(FinKeyPrm.Value);

            IFinancialService hocFS = base.GetByKey<IFinancialService, FinancialService_DAO>(hocFinancialServiceKey);
            IHOC hoc = CreateEmptyHOC();
            hoc.FinancialService = hocFS;
            return hoc;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hocKey"></param>
        public void UpdateHOCPremium(int hocKey)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@FinancialServiceKey", hocKey));
            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Repositories.HOCRepository", "UpdateHOCPremium", prms);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        /// <returns></returns>
        public IHOCHistoryDetail CreateHOCHistoryDetailRecord(IHOC hoc)
        {
            return CreateHOCHistoryDetailRecordInternal(hoc, Convert.ToChar("I"));
        }

        public IHOCHistoryDetail CreateHOCHistoryDetailRecord(IHOC hoc, char updateType)
        {
            return CreateHOCHistoryDetailRecordInternal(hoc, updateType);
        }

        private IHOCHistoryDetail CreateHOCHistoryDetailRecordInternal(IHOC hoc, char updateType)
        {
            IHOCHistoryDetail hocHistorydetail = CreateEmptyHOCHistoryDetail();
            hocHistorydetail.ChangeDate = DateTime.Now;
            hocHistorydetail.EffectiveDate = DateTime.Now;
            hocHistorydetail.HOCMonthlyPremium = hoc.HOCMonthlyPremium;
            hocHistorydetail.HOCProrataPremium = hoc.HOCProrataPremium;
            hocHistorydetail.HOCConventionalAmount = hoc.HOCConventionalAmount;
            hocHistorydetail.HOCShingleAmount = hoc.HOCShingleAmount;
            hocHistorydetail.HOCThatchAmount = hoc.HOCThatchAmount;
            hocHistorydetail.UpdateType = updateType;
            hocHistorydetail.UserID = SAHLPrincipal.GetCurrent().Identity.Name;
            hocHistorydetail.HOCAdministrationFee = hoc.HOCAdministrationFee;
            hocHistorydetail.HOCBasePremium = hoc.HOCBasePremium;
            hocHistorydetail.SASRIAAmount = hoc.SASRIAAmount;
            return hocHistorydetail;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        /// <returns></returns>
        public IHOCHistory CreateHOCHistoryRecord(IHOC hoc)
        {
            IHOCHistory hocHistory = CreateEmptyHOCHistory();
            hocHistory.HOC = hoc;
            hocHistory.ChangeDate = DateTime.Now;
            hocHistory.HOCInsurer = hoc.HOCInsurer;
            hocHistory.UserID = hoc.UserID;
            hocHistory.CommencementDate = DateTime.Now;
            return hocHistory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        public void CalculatePremium(IHOC hoc)
        {
            int fsKey = hoc.FinancialService != null ? hoc.FinancialService.Key : 0;

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@HOCThatchAmount", hoc.HOCThatchAmount));
            prms.Add(new SqlParameter("@HOCShingleAmount", hoc.HOCShingleAmount));
            prms.Add(new SqlParameter("@HOCConventionalAmount", hoc.HOCConventionalAmount));
            prms.Add(new SqlParameter("@HOCInsurer", hoc.HOCInsurer.Key));
            prms.Add(new SqlParameter("@HOCSubsidence", hoc.HOCSubsidence.Key));
            prms.Add(new SqlParameter("@FinancialServiceKey", fsKey));

            SqlParameter hocMonthlyPremium = new SqlParameter("@HOCMonthlyPremium", SqlDbType.Float);
            hocMonthlyPremium.Direction = ParameterDirection.Output;
            prms.Add(hocMonthlyPremium);

            SqlParameter hocProrataPremium = new SqlParameter("@HOCProrataPremium", SqlDbType.Float);
            hocProrataPremium.Direction = ParameterDirection.Output;
            prms.Add(hocProrataPremium);

            SqlParameter premiumThatch = new SqlParameter("@PremiumThatch", SqlDbType.Float);
            premiumThatch.Direction = ParameterDirection.Output;
            prms.Add(premiumThatch);

            SqlParameter premiumShingle = new SqlParameter("@PremiumShingle", SqlDbType.Float);
            premiumShingle.Direction = ParameterDirection.Output;
            prms.Add(premiumShingle);

            SqlParameter premiumConventional = new SqlParameter("@PremiumConventional", SqlDbType.Float);
            premiumConventional.Direction = ParameterDirection.Output;
            prms.Add(premiumConventional);

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForRead("Repositories.HOCRepository", "CalcHOCPremium", prms);

            hoc.HOCMonthlyPremium = Convert.ToDouble(hocMonthlyPremium.Value);
            hoc.HOCProrataPremium = Convert.ToDouble(hocProrataPremium.Value);
            hoc.PremiumThatch = Convert.ToDouble(premiumThatch.Value);
            hoc.PremiumShingle = Convert.ToDouble(premiumShingle.Value);
            hoc.PremiumConventional = Convert.ToDouble(premiumConventional.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hoc"></param>
        public void CalculatePremiumForUpdate(IHOC hoc)
        {
            int fsKey = 0;// hoc.FinancialService != null ? hoc.FinancialService.Key : 0;

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@HOCThatchAmount", hoc.HOCThatchAmount));
            prms.Add(new SqlParameter("@HOCShingleAmount", hoc.HOCShingleAmount));
            prms.Add(new SqlParameter("@HOCConventionalAmount", hoc.HOCConventionalAmount));
            prms.Add(new SqlParameter("@HOCInsurer", hoc.HOCInsurer.Key));
            prms.Add(new SqlParameter("@HOCSubsidence", hoc.HOCSubsidence.Key));
            prms.Add(new SqlParameter("@FinancialServiceKey", fsKey));

            SqlParameter hocMonthlyPremium = new SqlParameter("@HOCMonthlyPremium", SqlDbType.Float);
            hocMonthlyPremium.Direction = ParameterDirection.Output;
            prms.Add(hocMonthlyPremium);

            SqlParameter hocProrataPremium = new SqlParameter("@HOCProrataPremium", SqlDbType.Float);
            hocProrataPremium.Direction = ParameterDirection.Output;
            prms.Add(hocProrataPremium);

            SqlParameter premiumThatch = new SqlParameter("@PremiumThatch", SqlDbType.Float);
            premiumThatch.Direction = ParameterDirection.Output;
            prms.Add(premiumThatch);

            SqlParameter premiumShingle = new SqlParameter("@PremiumShingle", SqlDbType.Float);
            premiumShingle.Direction = ParameterDirection.Output;
            prms.Add(premiumShingle);

            SqlParameter premiumConventional = new SqlParameter("@PremiumConventional", SqlDbType.Float);
            premiumConventional.Direction = ParameterDirection.Output;
            prms.Add(premiumConventional);

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForRead("Repositories.HOCRepository", "CalcHOCPremium", prms);

            hoc.HOCMonthlyPremium = Convert.ToDouble(hocMonthlyPremium.Value);
            hoc.HOCProrataPremium = Convert.ToDouble(hocProrataPremium.Value);
            hoc.PremiumThatch = Convert.ToDouble(premiumThatch.Value);
            hoc.PremiumShingle = Convert.ToDouble(premiumShingle.Value);
            hoc.PremiumConventional = Convert.ToDouble(premiumConventional.Value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public double GetMonthlyPremium(int accountKey)
        {
            double monthlyPremium = 0;

            DateTime anniversaryDate = System.DateTime.Now;
            string query = UIStatementRepository.GetStatement("HOC", "GetMonthlyPremium");

            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", accountKey));

            object premium = this.castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            if (premium != null)
            {
                monthlyPremium = Convert.ToDouble(premium);
            }

            return monthlyPremium;
        }
    }
}
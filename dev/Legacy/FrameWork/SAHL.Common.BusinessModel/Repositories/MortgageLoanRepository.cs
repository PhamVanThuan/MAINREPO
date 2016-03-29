using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IMortgageLoanRepository))]
    public class MortgageLoanRepository : AbstractRepositoryBase, IMortgageLoanRepository
    {
        public MortgageLoanRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public MortgageLoanRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        private ISPVService _spvServ;

        private ISPVService SPVService
        {
            get
            {
                if (_spvServ == null)
                    _spvServ = ServiceFactory.GetService<ISPVService>();

                return _spvServ;
            }
        }

        /// <summary>
        /// Implements functionality of GetReadvanceComparisonAmountByAccountKey UIStatement
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public double GetReadvanceComparisonAmount(int AccountKey)
        {
            const int dec = 40;
            Control_DAO dao = ActiveRecordBase<Control_DAO>.Find(dec);
            IControl ctrl = new Control(dao);
            double factor = ctrl.ControlNumeric == null ? 1 : 1 + ((double)ctrl.ControlNumeric / 100);

            string HQL = "SELECT DISTINCT b from Bond_DAO b join b.MortgageLoans ml join ml.Account a where a.Key = ? AND ml.AccountStatus.Key in (1,5)";
            SimpleQuery<Bond_DAO> q = new SimpleQuery<Bond_DAO>(HQL, AccountKey);
            Bond_DAO[] res = q.Execute();

            double sumBLA = 0;

            for (int i = 0; i < res.Length; i++)
            {
                sumBLA += ((double)res[i].BondLoanAgreementAmount * factor);//(res[i].BondLoanAgreementAmount == null) ? 0 :
            }
            HQL = "SELECT DISTINCT ml from MortgageLoan_DAO ml join ml.Account a where a.Key = ? AND ml.AccountStatus.Key in (1,5)";
            SimpleQuery<MortgageLoan_DAO> q2 = new SimpleQuery<MortgageLoan_DAO>(HQL, AccountKey);
            MortgageLoan_DAO[] res2 = q2.Execute();

            double sumCB = 0;

            for (int i = 0; i < res2.Length; i++)
            {
                sumCB += res2[i].Balance.Amount;
            }

            return sumCB - sumBLA;
        }

        /// <summary>
        /// Perform Installment Change given AccountKey with a reference
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="userid"></param>
        /// <param name="reference"></param>
        public void InstallmentChange(int accountkey, string userid, string reference)
        {
            ParameterCollection Parameters = new ParameterCollection();
            Parameters.Add(new SqlParameter("@AccountKey", accountkey));
            Parameters.Add(new SqlParameter("@UserID", userid));
            Parameters.Add(new SqlParameter("@Reference", reference));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "UpdateInstalment", Parameters);
        }

        /// <summary>
        /// Perform Installment Change given AccountKey without a reference
        /// </summary>
        /// <param name="accountkey">Account Key</param>
        /// <param name="userid">User ID</param>
        public void InstallmentChange(int accountkey, string userid)
        {
            ParameterCollection Parameters = new ParameterCollection();
            Parameters.Add(new SqlParameter("@AccountKey", accountkey));
            Parameters.Add(new SqlParameter("@UserID", userid));

            SqlParameter reference = new SqlParameter("@Reference", DBNull.Value);
            reference.IsNullable = true;
            Parameters.Add(reference);

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "UpdateInstalment", Parameters);
        }

        public void LookUpPendingTermChangeDetailFromX2(out int NewTerm, long instanceID)
        {
            NewTerm = 0;
            int Accountkey = 0;
            ParameterCollection prms = new ParameterCollection();
            Helper.AddFloatParameter(prms, "@InstanceID", instanceID);

            string sqlQuery = UIStatementRepository.GetStatement("COMMON", "LookUpPendingTermChangeDetailFromX2");
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(sqlQuery, typeof(Fees_DAO), prms);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    NewTerm = (int)dr[0];
                    Accountkey = (int)dr[2];
                }
            }

            IAccount acc = AccRepo.GetAccountByKey(Accountkey);
        }

        public int LookUpPendingTermChangeFromX2(long instanceId)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "LookUpPendingTermChangeFromX2");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@InstanceID", instanceId));

            object obj = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            int result = Convert.ToInt32(obj);

            return result;
        }

        public bool LookUpPendingTermChangeByAccount(int AccountKey)
        {
            string query = UIStatementRepository.GetStatement("COMMON", "LookUpPendingTermChangeByAccountKey");
            ParameterCollection parameters = new ParameterCollection();
            parameters.Add(new SqlParameter("@AccountKey", AccountKey));

            object obj = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(GeneralStatus_DAO), parameters);
            int result = Convert.ToInt32(obj);

            if (result > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Perform Term Change
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="term"></param>
        /// <param name="userid"></param>
        public void TermChange(int accountkey, int term, string userid)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountkey));
            prms.Add(new SqlParameter("@NewTerm", term));
            prms.Add(new SqlParameter("@UserID", userid));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "UpdateTerm", prms);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountkey"></param>
        /// <param name="newSPV"></param>
        /// <param name="userid"></param>
        public void TransferSPV(int accountkey, int newSPV, string userid)
        {
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@AccountKey", accountkey));
            prms.Add(new SqlParameter("@ToSPVKey", newSPV));
            prms.Add(new SqlParameter("@UserID", userid));

            this.castleTransactionService.ExecuteHaloAPI_uiS_OnCastleTranForUpdate("Account", "TransferSPV", prms);
        }

        //public bool IsSPVTermWithinMax(int term, int spv)
        //{
        //    return SPVService.IsSPVWithinMaxTerm(1, term, spv, 1);
        //}

        public string GetNewSPVDescription(int spv)
        {
            ISPV SPVDetails = SPVService.GetSPVDetails(spv);
            return SPVDetails.Description;
        }

        public bool IsThereSPVMovementInProgress(int Offerkey)
        {
            IStageDefinitionRepository SDR = RepositoryFactory.GetRepository<IStageDefinitionRepository>();

            //Check for Decline term change
            int cntDeclineTermChange = SDR.CountCompositeStageOccurance(Offerkey, 4004);

            //Check for Approve Term Change
            int cntApproveTermChange = SDR.CountCompositeStageOccurance(Offerkey, 4003);

            //Check for Archive Term Change
            int cntArchiveTermChange = SDR.CountCompositeStageOccurance(Offerkey, 4005);

            if (cntDeclineTermChange > 0 || cntApproveTermChange > 0)
                /* if there is a decline term request or a approve term request
                 and the case has not passed archive term change then there is
                 an SPV movement in progress */

                if (cntArchiveTermChange == 0)
                    return true;
                else
                    return true;
            else
                return false;
        }

        //public string GetNewSPVTermChange(int spvCurrent)
        //{
        //    // Current Parent SPV 33
        //    int newSPV = SPVService.DetermineSPVForTermChange(spvCurrent);

        //    return GetNewSPVDescription(newSPV);
        //}

        //public int GetNewSPVKeyTermChange(int spvCurrent)
        //{
        //    int newSPV = SPVService.DetermineSPVForTermChange(spvCurrent);
        //    return newSPV;
        //}

        #region DoNotDelete GaryD

        //Need to check about reading dirty data from Paul
        //Ideal solution is to run the sp's to update the db
        //read the dirty data to determine LTV and PTI to get CC and Category
        //update the category and commit

        //public void RateChange(IAccount account, IMargin margin, string userID)
        //{
        //    IMortgageLoanAccount mla = account as IMortgageLoanAccount;
        //    //Get the variable ML
        //    IMortgageLoan vml = mla.SecuredMortgageLoan;
        //    IMortgageLoan fml = null;

        //    int employmentTypeKey = account.GetEmploymentTypeKey();
        //    //default to Salaried if Unemployed or Unknown
        //    if (employmentTypeKey > (int)EmploymentTypes.Unemployed)
        //        employmentTypeKey = (int)EmploymentTypes.Salaried;

        //    //Get the Life instalment
        //    double lifeInstalment = 0;
        //    if (mla.LifePolicyAccount != null)
        //    {
        //        foreach (IFinancialService fs in mla.LifePolicyAccount.FinancialServices)
        //        {
        //            if (fs.AccountStatus.Key == (int)AccountStatuses.Open || fs.AccountStatus.Key == (int)AccountStatuses.Dormant)
        //            {
        //                lifeInstalment = fs.Payment;
        //                break;
        //            }
        //        }
        //    }

        //    #region Variable

        //     //Get the property value
        //    double valuationAmount = vml.GetLatestPropertyValuationAmount();
        //    //Var balance NB: remove the capitalised life
        //    double varBalance = vml.CurrentBalance - mla.CapitalisedLife;
        //    //Get new rate (Base + Margin + Discount)
        //    double varRate = vml.BaseRate + margin.Value + (vml.Discount.HasValue ? vml.Discount.Value : 0);
        //    //Calc new Instalment
        //    double varInstalment = 0;
        //    if (vml.CurrentBalance > 0)
        //        varInstalment = lifeInstalment + LoanCalculator.CalculateInstallment(varBalance, varRate, vml.RemainingInstallments, false);

        //    #endregion

        //    #region Fixed

        //    double fixBalance = 0;
        //    double fixRate = 0;
        //    double fixInstalment = 0;
        //    //Get the Fixed ML
        //    if ((_account as IAccountVariFixLoan) != null)
        //    {
        //        IAccountVariFixLoan faccount = account as IAccountVariFixLoan;
        //        fml = faccount.FixedSecuredMortgageLoan;
        //        //Fix current balance
        //        fixBalance = fml.CurrentBalance;
        //        //Get new rate (Base + Margin + Discount)
        //        fixRate = fml.BaseRate + margin.Value + (fml.Discount.HasValue ? fml.Discount.Value : 0);
        //        //Calc new Instalment
        //        if (fML.CurrentBalance > 0)
        //            fixInstalment = LoanCalculator.CalculateInstallment(fixBalance, fixRate, fML.RemainingInstallments, false);
        //    }

        //    #endregion

        //    //calc LTV
        //    double ltv = 0;
        //    if (valuationAmount > 0)
        //        ltv = (varBalance + fixBalance + mla.CapitalisedLife) / valuationAmount;
        //    //calc PTI - always on amortising
        //    double pti = 0;
        //    if (account.GetHouseholdIncome() > 0)
        //        pti = (varInstalment + fixInstalment + lifeInstalment) / account.GetHouseholdIncome();

        //    //GetCMByOSP
        //    //use LTV and PTI to get best CC and use Category

        //    //Save the Category Key

        //    //Process the rate change
        //}

        #endregion DoNotDelete GaryD

        // Perform rate change
        //[Obsolete("Please use the Disbursement repository methods instead.")]
        //public void RateChange(int fskey, int rateConfigurationKey, string userid)
        //{
        //    const string query = "EXEC [2AM].[dbo].[pLoanUpdateRate] @FinancialServiceKey, @RateConfigurationKey, @UserId, null, 0";

        //    // Create a collection
        //    ParameterCollection Parameters = new ParameterCollection();
        //    //Add the required parameters

        //    Parameters.Add(new SqlParameter("@FinancialServiceKey", fskey));
        //    Parameters.Add(new SqlParameter("@RateConfigurationKey", rateConfigurationKey));
        //    Parameters.Add(new SqlParameter("@UserId", userid));

        //    // execute
        //    ExecuteNonQueryOnCastleTran(query, typeof(GeneralStatus_DAO), Parameters);
        //}

        /// <summary>
        /// Save Mortgage Loan Account
        /// </summary>
        /// <param name="MortgageLoan"></param>
        public void SaveMortgageLoan(IMortgageLoan MortgageLoan)
        {
            base.Save<IMortgageLoan, MortgageLoan_DAO>(MortgageLoan);
        }

        //TODO: Amend or rip this out when the generic get by key is finalised
        /// <summary>
        ///
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public IMortgageLoanPurpose GetMortgageLoanPurposeByKey(int Key)
        {
            return GetByKey<IMortgageLoanPurpose, MortgageLoanPurpose_DAO>(Key);
        }

        public IMortgageLoan GetMortgageLoanByKey(int Key)
        {
            return GetByKey<IMortgageLoan, MortgageLoan_DAO>(Key);
        }

        /// <summary>
        /// Return a Mortgageloan by account key
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public IMortgageLoan GetMortgageloanByAccountKey(int accountkey)
        {
            IMortgageLoan mortgageloan = CreateEmptyMortgageLoan();

            const string HQL = "from MortgageLoan_DAO ml where ml.Account.Key = ?";
            SimpleQuery<MortgageLoan_DAO> ml = new SimpleQuery<MortgageLoan_DAO>(HQL, accountkey);
            MortgageLoan_DAO[] mlInfo = ml.Execute();

            if (mlInfo != null && mlInfo.Length > 0)
                mortgageloan = new MortgageLoan(mlInfo[0]);

            return mortgageloan;
        }

        /// <summary>
        /// Returns a collection of mortgageloans for an accountkey
        /// </summary>
        /// <param name="accountkey">accountkey</param>
        /// <returns>A collection of IMortgageLoan.</returns>
        public IReadOnlyEventList<IMortgageLoan> GetMortgageLoansByAccountKey(int accountkey)
        {
            List<IMortgageLoan> list = new List<IMortgageLoan>();

            string HQL = String.Format("from MortgageLoan_DAO ml where ml.Account.Key = ?");
            SimpleQuery<MortgageLoan_DAO> q = new SimpleQuery<MortgageLoan_DAO>(HQL, accountkey);
            MortgageLoan_DAO[] Mls = q.Execute();

            for (int i = 0; i < Mls.Length; i++)
            {
                list.Add(new MortgageLoan(Mls[i]));
            }

            return new ReadOnlyEventList<IMortgageLoan>(list);
        }

        /// <summary>
        /// Returns a new Empty MortgageLoan
        /// </summary>
        /// <returns></returns>
        public IMortgageLoan CreateEmptyMortgageLoan()
        {
            return base.CreateEmpty<IMortgageLoan, MortgageLoan_DAO>();

            //return new MortgageLoan(new MortgageLoan_DAO());
        }

        private IAccountRepository _accRepo;

        private IAccountRepository AccRepo
        {
            get
            {
                if (_accRepo == null)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AccountKey"></param>
        /// <returns></returns>
        public IEventList<ICAP> GetCapProducts(int AccountKey)
        {
            string hql = @"select c from CAP_DAO c where c.FinancialServiceAttribute.FinancialService.Account.Key = ? and c.FinancialServiceAttribute.FinancialServiceAttributeType.Key = ?";
            SimpleQuery<CAP_DAO> query = new SimpleQuery<CAP_DAO>(hql, AccountKey, (int)FinancialServiceAttributeTypes.CAP);
            CAP_DAO[] result = query.Execute();
            if (result != null && result.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return new DAOEventList<CAP_DAO, ICAP, CAP>(result);
            }
            return null;
        }
    }
}
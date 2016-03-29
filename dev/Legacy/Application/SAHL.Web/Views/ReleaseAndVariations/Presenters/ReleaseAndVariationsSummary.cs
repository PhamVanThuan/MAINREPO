using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.ReleaseAndVariations.Interfaces;

namespace SAHL.Web.Views.ReleaseAndVariations.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ReleaseAndVariationsSummary : SAHLCommonBasePresenter<IReleaseAndVariationsSummary>
    {
        private IApplicationRepository applicationRepository;
        private IReleaseAndVariationsRepository releaseAndVariationsRepository;
        private DataSet RVDS;
        public CBOMenuNode node;
        private CBONode cboNode; // = null;
        private InstanceNode instanceNode;
        private int accountkey;
        //private int offerkey;
        //private int requestindex = -1;
        //private int applyindex = -1;

        //private IApplicationRepository ApplicationRepository
        //{
        //    get
        //    {
        //        if (applicationRepository == null)
        //            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
        //        return applicationRepository;
        //    }
        //}

        private IReleaseAndVariationsRepository ReleaseAndVariationsRepository
        {
            get
            {
                if (releaseAndVariationsRepository == null)
                    releaseAndVariationsRepository = RepositoryFactory.GetRepository<IReleaseAndVariationsRepository>();
                return releaseAndVariationsRepository;
            }
        }

        private IMortgageLoanRepository mortgageloanrepository;

        private IMortgageLoanRepository MortgageLoanRepository
        {
            get
            {
                if (mortgageloanrepository == null)
                    mortgageloanrepository = RepositoryFactory.GetRepository<IMortgageLoanRepository>();
                return mortgageloanrepository;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ReleaseAndVariationsSummary(IReleaseAndVariationsSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;
            View.btnERFInformationClicked += View_btnERFInformationClicked;
            View.btnPrintRequestClicked += View_btnPrintRequestClicked;
            View.btnUpdateConditionsClicked += View_btnUpdateConditionsClicked;
            View.btnUpdateSummaryClicked += View_btnUpdateSummaryClicked;
            SetupInterface();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            cboNode = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null) throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationMortgageLoan applicationMortgageLoan;
            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            int requiredKey;
            //Get the AccountKey/OfferKey from the CBO
            if (cboNode is InstanceNode)
            {
                instanceNode = cboNode as InstanceNode;
                requiredKey = Convert.ToInt32(instanceNode.GenericKey);
            }
            else
                requiredKey = Convert.ToInt32(cboNode.GenericKey);

            switch (cboNode.GenericKeyTypeKey)
            {
                case (int)GenericKeyTypes.Account:
                    IMortgageLoanAccount account = (IMortgageLoanAccount)accountRepository.GetAccountByKey(requiredKey);
                    if (account != null) accountkey = account.Key;
                    //if (account != null) offerkey = account.Key;
                    break;
                case (int)GenericKeyTypes.Offer:
                    applicationMortgageLoan = applicationRepository.GetApplicationByKey(requiredKey) as IApplicationMortgageLoan;
                    if (applicationMortgageLoan != null) accountkey = applicationMortgageLoan.Account.Key;
                    //if (applicationMortgageLoan != null) offerkey = applicationMortgageLoan.Key;
                    break;
                default:
                    break;
            }

            //NB These variables must be set...
            //accountkey = 1715744;
            //offerkey = 1715744;

            GlobalCacheData.Remove("AccountKey");
            GlobalCacheData.Add("AccountKey", accountkey, new List<ICacheObjectLifeTime>());

            if (!PrivateCacheData.ContainsKey("IsSetup"))
            {
                // Check the global cache to see if a ReleaseAndVariations object has been added
                RVDS = ReleaseAndVariationsRepository.CreateReleaseAndVariationsDataSet();
                RVDS = releaseAndVariationsRepository.GetExistingReleaseAndVariationsByGenericKey(accountkey);
                GlobalCacheData.Remove("ReleaseAndVariationsDataSet");
                GlobalCacheData.Add("ReleaseAndVariationsDataSet", RVDS, new List<ICacheObjectLifeTime>());
            }
            else
                RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;

            PopulateData();
            PrivateCacheData.Remove("IsSetup");
            PrivateCacheData.Add("IsSetup", true);
        }

        private void SetupInterface()
        {
            View.ShowbtnSubmit = false;
            View.ShowbtnCancel = false;
            View.ShowbtnConfirm = false;
            View.ShowlblCaption = false;
            View.ShowbtnERFInformation = true;
            View.ShowbtnPrintRequest = true;
            View.ShowbtnUpdateConditions = true;
            View.ShowbtnUpdateSummary = true;

            View.ShowpnlLoanDetails = true;
            View.ShowpnlBondDetails = true;
            View.ShowpnlConditions = true;
            View.ShowpnlMemo = true;
            View.ShowpnlContact = true;

            View.ShowddlRequestType = false;
            View.ShowlblRequestType = true;

            View.ShowddlApplyChangeTo = false;
            View.ShowlblApplyChangeTo = true;
        }

        // Update the Erf Information
        private void View_btnERFInformationClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ReleaseAndVariationsERFDetails");
        }

        // Print a Report containing the release and variation information
        private void View_btnPrintRequestClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Report");
        }

        // Navigate to the update conditions screen
        private void View_btnUpdateConditionsClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ReleaseAndVariationsConditions");
        }

        // Update the memo and navigate back here
        private void View_btnUpdateSummaryClicked(object sender, EventArgs e)
        {
            RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;
            TransactionScope txn = new TransactionScope();
            try
            {
                // Add a New record, and get back the New Memo Key - for adding to the dataset
                int NewMemoKey = Convert.ToInt32(RVDS.Tables["Release"].Rows[0]["MemoKey"]);
                RVDS.Tables[0].Rows[0]["Notes"] = View.GetSettxtNotes;
                RVDS.AcceptChanges();
                ReleaseAndVariationsRepository.UpdateMemo(NewMemoKey, RVDS);

                GlobalCacheData.Remove("ReleaseAndVariationsDataSet");
                GlobalCacheData.Add("ReleaseAndVariationsDataSet", RVDS, new List<ICacheObjectLifeTime>());
                txn.VoteCommit();
            }

            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        private void PopulateData()
        {
            double releaseInfoArrearBalance = 0;
            double releaseInfoCurrentLoanRate = 0;
            double ltv = 0;
            double pti = 0;
            IMortgageLoan mortgageloan = MortgageLoanRepository.GetMortgageloanByAccountKey(accountkey);
            IAccountRepository accrep = RepositoryFactory.GetRepository<IAccountRepository>();

            IAccount account = accrep.GetAccountByKey(accountkey);
            double householdIncome = account.GetHouseholdIncome();
            double installment = account.InstallmentSummary.TotalLoanInstallment;

            if (mortgageloan != null)
            {
                releaseInfoArrearBalance = mortgageloan.ArrearBalance;
                releaseInfoCurrentLoanRate = Math.Round((mortgageloan.InterestRate) * 100, 2);
                ltv = LoanCalculator.CalculateLTV(mortgageloan.Balance.LoanBalance.InitialBalance, mortgageloan.Property.Valuations[0].ValuationAmount.Value);
                pti = LoanCalculator.CalculatePTI(installment, householdIncome);
                //LoanCalculator.
                //
            }

            View.GetSetlblAccountName = RVDS.Tables["Release"].Rows[0]["AccountName"].ToString();
            View.GetSetlblAccountNumber = RVDS.Tables["Release"].Rows[0]["AccountNumber"].ToString();
            View.GetSetlblLinkedToOffer = RVDS.Tables["Release"].Rows[0]["LinkedOffer"].ToString();
            View.GetSetlblRequestType = RVDS.Tables["Release"].Rows[0]["RequestType"].ToString();
            View.GetSetlblApplyChangeTo = RVDS.Tables["Release"].Rows[0]["ApplyChangeTo"].ToString();
            View.GetSetlblLoanBalance = ReleaseAndVariationsRepository.GetLoanCurrentBalance(accountkey).ToString(SAHL.Common.Constants.CurrencyFormat);
            View.GetSetlblArrears = releaseInfoArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            View.GetSetlblSPV = ReleaseAndVariationsRepository.FindSPVNameByFinancialServicesKey(accountkey);
            View.GetSettxtNotes = RVDS.Tables["Release"].Rows[0]["Notes"].ToString();
            View.GetSetlblCurrentLoan = releaseInfoCurrentLoanRate + " %";
            if (mortgageloan != null) View.GetSetlblProducts = mortgageloan.FinancialServiceType.Description;

            //IList<Application> apps = mlInfo[0].Account.Applications;

            // Get the application key for the mortgage loan
            //int applicationkey = 0;
            //for (int i = 0; i < apps.Count; i++)
            //{
            //    if (apps[i].ApplicationStatus.Key == (int)OfferStatuses.Accepted)
            //    {
            //        applicationkey = apps[i].Key;
            //        break;
            //    }

            //}
            //IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            //IApplicationInformationVariableLoan aiVL = AR.GetApplicationInformationVariableLoan(applicationkey);
            //if (aiVL != null)
            //{
            //    //View.GetSetlblCurrentLTV = aiVL.LTV.Value + " %";
            //    //View.GetSetlblCurrentPTI = aiVL.PTI.Value + " %";
            //}

            View.GetSetlblCurrentLTV = Convert.ToString(ltv) + " %";
            View.GetSetlblCurrentPTI = pti.ToString(SAHL.Common.Constants.NumberFormat) + " %";

            // Setup the Grids
            View.BindgridConditions(RVDS.Tables["Conditions"]);
            View.BindgridBondDetails(GetBondDetails());
        }

        private DataTable GetBondDetails()
        {
            List<IBond> bonds = ReleaseAndVariationsRepository.GetBondByFinancialServiceKey(accountkey);
            DataTable bondDT = new DataTable("Bonds");
            bondDT.Columns.Add("InFavourOf", typeof(string));
            bondDT.Columns.Add("BondRegistrationAmount", typeof(double));
            bondDT.Columns.Add("CoverAmount", typeof(string));
            bondDT.Columns.Add("BondRegistrationDate", typeof(string));

            for (int i = 0; i < bonds.Count; i++)
            {
                DataRow dr = bondDT.NewRow();
                if (bonds[i].Application != null)
                    dr["InFavourOf"] = bonds[i].Application.GetLegalName(LegalNameFormat.Full);
                dr["BondRegistrationAmount"] = bonds[i].BondRegistrationAmount.ToString();
                dr["CoverAmount"] = "";
                dr["BondRegistrationDate"] = bonds[i].BondRegistrationDate.ToString(SAHL.Common.Constants.DateFormat);
                bondDT.Rows.Add(dr);
            }
            return bondDT;
        }
    }
}
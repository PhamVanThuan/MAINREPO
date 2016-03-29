using System;
using System.Collections.Generic;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
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
    public class ReleaseAndVariationsSummaryReadOnly : SAHLCommonBasePresenter<IReleaseAndVariationsSummary>
    {
        private IApplicationRepository applicationRepository;
        private IReleaseAndVariationsRepository releaseAndVariationsRepository;
        private DataSet RVDS;
        public CBOMenuNode node;
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
        public ReleaseAndVariationsSummaryReadOnly(IReleaseAndVariationsSummary view, SAHLCommonBaseController controller)
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

            applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationMortgageLoan applicationMortgageLoan;
            IAccountRepository accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            int requiredKey;
            //Get the AccountKey/OfferKey from the CBO
            if (node is InstanceNode)
            {
                instanceNode = node as InstanceNode;
                requiredKey = Convert.ToInt32(instanceNode.GenericKey);
            }
            else
                requiredKey = Convert.ToInt32(node.GenericKey);

            switch (node.GenericKeyTypeKey)
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
            View.ShowbtnUpdateSummary = false;

            View.ShowpnlLoanDetails = true;
            View.ShowpnlBondDetails = true;
            View.ShowpnlConditions = true;
            View.ShowpnlMemo = true;
            View.ShowpnlContact = true;

            View.ShowddlRequestType = false;
            View.ShowlblRequestType = true;
            View.ShowddlApplyChangeTo = false;
            View.ShowlblApplyChangeTo = true;

            View.SetReadOnlytxtNotes = true;

        }


        // Update the Erf Information
        void View_btnERFInformationClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ReleaseAndVariationsERFDetailsReadOnly");
        }

        // Print a Report containing the release and variation information
        void View_btnPrintRequestClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Report");
        }

        // Navigate to the update conditions screen
        void View_btnUpdateConditionsClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("ReleaseAndVariationsConditionsReadOnly");
        }



        void PopulateData()
        {

            double releaseInfoArrearBalance = 0;
            double releaseInfoCurrentLoanRate = 0;
            IMortgageLoan mortgageloan = MortgageLoanRepository.GetMortgageloanByAccountKey(accountkey);

            if (mortgageloan != null)
            {
                releaseInfoArrearBalance = mortgageloan.ArrearBalance;
                releaseInfoCurrentLoanRate = Math.Round((mortgageloan.InterestRate) * 100, 2);
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


            if (mortgageloan != null)
            {
                IEventList<IApplication> apps = mortgageloan.Account.Applications;
                int refindex = 0;
                for (int i = 0; i < apps.Count; i++)
                {
                    refindex = i;
                    if (apps[i].ApplicationStatus.Key == (int)OfferStatuses.Accepted)
                        break;
                }
                IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
                IApplicationInformationVariableLoan aiVL = AR.GetApplicationInformationVariableLoan(mortgageloan.Account.Applications[refindex].Key);
                if (aiVL != null)
                {
                    View.GetSetlblCurrentLTV = aiVL.LTV.Value + " %";
                    View.GetSetlblCurrentPTI = aiVL.PTI.Value + " %";
                }
            }

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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
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
    public class ReleaseAndVariationsAdd : SAHLCommonBasePresenter<IReleaseAndVariationsSummary>
    {

        private IApplicationRepository applicationRepository;
        private IReleaseAndVariationsRepository releaseAndVariationsRepository;
        private DataSet RVDS;
        private CBONode cboNode; // = null;
        public CBOMenuNode node;
        private InstanceNode instanceNode;
        private int accountkey;
        private int offerkey;
        private int requestindex = -1;
        private int applyindex = -1;


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
        public ReleaseAndVariationsAdd(IReleaseAndVariationsSummary view, SAHLCommonBaseController controller)
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

            // Check the global cache to see if a ReleaseAndVariations object has been added
            if (GlobalCacheData.ContainsKey("ReleaseAndVariationsDataSet"))
                RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;
            else
                RVDS = ReleaseAndVariationsRepository.CreateReleaseAndVariationsDataSet();
 

            View.btnCancelClicked += View_btnCancelClicked;
            View.btnConfirmClicked += View_btnSubmitClicked;
            View.btnSubmitClicked += View_btnConfirmClicked;
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
            int requiredKey = 0;
            //Get the AccountKey/OfferKey from the CBO
            if (cboNode != null)
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
                    if (account != null) offerkey = account.Key;
                    break;
                case (int)GenericKeyTypes.Offer:
                    applicationMortgageLoan = applicationRepository.GetApplicationByKey(requiredKey) as IApplicationMortgageLoan;
                    if (applicationMortgageLoan != null) accountkey = applicationMortgageLoan.Account.Key;
                    if (applicationMortgageLoan != null) offerkey = applicationMortgageLoan.Key;
                    break;
                default:
                    break;
            }

            //NB These variables must be set...
            //accountkey = 1715744;
            //offerkey = 1715744;

            GlobalCacheData.Remove("AccountKey");
            GlobalCacheData.Add("AccountKey", accountkey, new List<ICacheObjectLifeTime>());


            PopulateData();

            PrivateCacheData.Remove("IsSetup");
            PrivateCacheData.Add("IsSetup", true);
        }

        private void SetupInterface()
        {
            View.ShowbtnSubmit = true;
            View.ShowbtnCancel = false;
            View.ShowbtnConfirm = false;
            View.ShowlblCaption = false;

            View.ShowpnlLoanDetails = false;
            View.ShowpnlBondDetails = false;
            View.ShowpnlConditions = false;
            View.ShowpnlMemo = true;
            View.ShowpnlContact = true;

            View.ShowddlRequestType = true;
            View.ShowlblRequestType = false;

            View.ShowddlApplyChangeTo = true;
            View.ShowlblApplyChangeTo = false;
        }


        void View_btnConfirmClicked(object sender, EventArgs e)
        {
            View.ShowbtnSubmit = false;
            View.ShowbtnCancel = true;
            View.ShowbtnConfirm = true;
            View.ShowlblCaption = true;
            requestindex = Convert.ToInt32(View.GetSetddlRequestType);
            applyindex = Convert.ToInt32(View.GetSetddlApplyChangeTo);
        }

        // Add the Release And Variation and Navigate to the Summary Screen
        void View_btnSubmitClicked(object sender, EventArgs e)
        {
            ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
            IADUser ADUser = secRepo.GetADUserByPrincipal(_view.CurrentPrincipal);

            RVDS = GlobalCacheData["ReleaseAndVariationsDataSet"] as DataSet;
            accountkey = (int)GlobalCacheData["AccountKey"];

            DataTable RequestTypesDT = ReleaseAndVariationsRepository.CreateRequestTypesTable();
            requestindex = Convert.ToInt32(View.GetSetddlRequestType);
            string requesttype = "";
            for (int i = 0; i < RequestTypesDT.Rows.Count; i++)
            {
                if (requestindex == Convert.ToInt32(RequestTypesDT.Rows[i]["Key"]))
                {
                    requesttype = (string)RequestTypesDT.Rows[i]["RequestType"];
                    break;
                }
            }

            DataTable ApplychangesDT = SetupDDLApplychangesTo();
            string applychanges = "";
            applyindex = Convert.ToInt32(View.GetSetddlApplyChangeTo);
            for (int i = 0; i < ApplychangesDT.Rows.Count; i++)
            {
                if (applyindex == Convert.ToInt32(ApplychangesDT.Rows[i]["Key"]))
                {
                    applychanges = (string)ApplychangesDT.Rows[i]["LoanType"];
                    break;
                }
            }

            TransactionScope txn = new TransactionScope();
            try
            {
                // Add a New record, and get back the New Memo Key - for adding to the dataset
                int NewMemoKey = ReleaseAndVariationsRepository.AddNewMemo(RVDS, accountkey, ADUser, View.Messages);
                RVDS.Tables[0].Rows[0]["MemoKey"] = NewMemoKey;
                RVDS.Tables[0].Rows[0]["Notes"] = View.GetSettxtNotes;
                RVDS.Tables[0].Rows[0]["RequestType"] = requesttype;
                RVDS.Tables[0].Rows[0]["ApplyChangeTo"] = applychanges;
                RVDS.AcceptChanges();
                ReleaseAndVariationsRepository.UpdateMemo(NewMemoKey, RVDS);

                GlobalCacheData.Remove("ReleaseAndVariationsDataSet");
                GlobalCacheData.Add("ReleaseAndVariationsDataSet", RVDS, new List<ICacheObjectLifeTime>());
                //TODO start a new workflow item here
                Navigator.Navigate("ReleaseAndVariationsSummary");

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

        // Reload new Add Screen
        void View_btnCancelClicked(object sender, EventArgs e)
        {
            //Clear all of the caches and navigate back to the screen.
            GlobalCacheData.Remove("ReleaseAndVariationsDataSet");
        }

        void PopulateData()
        {
            RVDS = ReleaseAndVariationsRepository.CreateReleaseAndVariationsDataSet();
            IMortgageLoan mortgageloan = MortgageLoanRepository.GetMortgageloanByAccountKey(accountkey);
            double releaseInfoArrearBalance = mortgageloan.ArrearBalance;
            string[] releaseInfoLegalEntities = ReleaseAndVariationsRepository.GetLegalEntities(accountkey, View.Messages);

            DataTable RequestTypesDT = ReleaseAndVariationsRepository.CreateRequestTypesTable();
            View.bindddlRequestType(RequestTypesDT);
            if (requestindex > -1)
                View.GetSetddlRequestType = Convert.ToString(requestindex);

            DataTable ApplychangesDT = SetupDDLApplychangesTo();
            View.bindddlApplyChangeTo(ApplychangesDT);
            if (applyindex > -1)
                View.GetSetddlApplyChangeTo = Convert.ToString(applyindex);
            
            View.GetSetlblAccountName = FormatAccountName(releaseInfoLegalEntities);
            View.GetSetlblAccountNumber = Convert.ToString(accountkey);
            View.GetSetlblLinkedToOffer = Convert.ToString(offerkey);
            View.GetSetlblLoanBalance = ReleaseAndVariationsRepository.GetLoanCurrentBalance(accountkey).ToString(SAHL.Common.Constants.CurrencyFormat);
            View.GetSetlblArrears = releaseInfoArrearBalance.ToString(SAHL.Common.Constants.CurrencyFormat);
            View.GetSetlblSPV = ReleaseAndVariationsRepository.FindSPVNameByFinancialServicesKey(accountkey);

            DataRow rr = RVDS.Tables["Release"].NewRow();
            rr["MemoKey"] = 0;
            rr["AccountName"] = FormatAccountName(releaseInfoLegalEntities);
            rr["AccountNumber"] = accountkey;
            rr["LinkedOffer"] = offerkey;
            rr["RequestType"] = "";
            rr["ApplyChangeTo"] = "";
            rr["LoanBalance"] = ReleaseAndVariationsRepository.GetLoanCurrentBalance(accountkey);
            rr["Arrears"] = releaseInfoArrearBalance;
            rr["Notes"] = "";
            RVDS.Tables[0].Rows.Add(rr);
            GlobalCacheData.Remove("ReleaseAndVariationsDataSet");
            GlobalCacheData.Add("ReleaseAndVariationsDataSet", RVDS, new List<ICacheObjectLifeTime>());
        }


        private DataTable SetupDDLApplychangesTo()
        {
            DataTable DT = releaseAndVariationsRepository.CreateGetChangeTypesTable();
            DataRow dr = DT.NewRow();
            dr["LoanType"] = "Account No: " + Convert.ToString(accountkey);
            dr["Key"] = Convert.ToString(DT.Rows.Count + 4);
            DT.Rows.Add(dr);

            dr = DT.NewRow();
            dr["LoanType"] = "Entity Not Captured";
            dr["Key"] = Convert.ToString(DT.Rows.Count + 4);
            DT.Rows.Add(dr);

            string[] legalentities = ReleaseAndVariationsRepository.GetLegalEntities(accountkey, View.Messages);
            for (int i = 0; i < legalentities.Length; i++)
            {
                dr = DT.NewRow();
                dr["LoanType"] = legalentities[i];
                dr["Key"] = Convert.ToString(DT.Rows.Count + 4);
                DT.Rows.Add(dr);
            }

            // Loop through the addresses and add them to the dropdown list
            string address = ReleaseAndVariationsRepository.GetAddressGivenAccountKey(accountkey);
            if (address.Length > 0)
            {
                dr = DT.NewRow();
                dr["LoanType"] = address;
                dr["Key"] = Convert.ToString(DT.Rows.Count + 4);
                DT.Rows.Add(dr);
            }
            return DT;
        }

        private static string FormatAccountName(string[] entities)
        {
            StringBuilder mBuilder = new StringBuilder();
            for (int i = 0; i < entities.Length; i++)
                mBuilder.Append(entities[i] + "  ");
            return mBuilder.ToString();
        }
    }
}


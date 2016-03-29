using System;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using Constants = SAHL.Common.Constants;

using SAHL.Common;
using SAHL.Common.UI;

namespace SAHL.Web.Views.Common.Presenters.CommonReason
{
    /// <summary>
    /// 
    /// </summary>
    public class DeclineReasonsHistory : SAHLCommonBasePresenter<IDeclineReasonsHistory>
    {
        private IApplicationRepository AppRepository;
        private int GenericKey;
        private int GenericKeyTypeKey;
        private CBONode Node;


        private IReasonRepository reasonRepository;

        public DeclineReasonsHistory(IDeclineReasonsHistory view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected IApplicationRepository ApplicationRepository
        {
            get
            {
                if (AppRepository == null)
                    AppRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                return AppRepository;
            }
        }

        protected IReasonRepository ReasonRepository
        {
            get
            {
                if (reasonRepository == null)
                    reasonRepository = RepositoryFactory.GetRepository<IReasonRepository>();
                return reasonRepository;
            }
        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            View.OngrdRevisionHistoryIndexChanged += OngrdRevisionHistoryIndex_Changed;
            View.grdRevisionHistorySelectedIndex = -1;

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            Node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;
            
            if (Node != null) GenericKey = Node.GenericKey;
            PrivateCacheData.Remove("generickey");
            PrivateCacheData.Add("generickey", GenericKey);

            if (Node != null) GenericKeyTypeKey = Node.GenericKeyTypeKey;
            PrivateCacheData.Remove("generickeytypekey");
            PrivateCacheData.Add("generickeytypekey", GenericKeyTypeKey);

            Populateinterface();
        }

        #region Events Handlers

        private void OngrdRevisionHistoryIndex_Changed(object sender, KeyChangedEventArgs e)
        {
            PrivateCacheData.Remove("HistoryIndex");
            PrivateCacheData.Add("HistoryIndex", View.grdRevisionHistorySelectedIndex);
            if (View.IsValid)
                Populateinterface();
        }

        private void Populateinterface()
        {
            // Add the Lookup repository to populate the datasets with display information
            int grdRevisionHistoryKey = PrivateCacheData.ContainsKey("HistoryIndex") ? Convert.ToInt32(PrivateCacheData["HistoryIndex"]) : 0;
            int generickey = PrivateCacheData.ContainsKey("generickey") ? Convert.ToInt32(PrivateCacheData["generickey"]) : 0;
            if (generickey > 0)
            {
                IEventList<IApplicationInformation> appList = ApplicationRepository.GetApplicationRevisionHistory(generickey);


                DataTable revisionHistoryDataTable = GetRevisionHistoryDataTable();
                if (PrivateCacheData.ContainsKey("RevisionHistoryDataTable"))
                    revisionHistoryDataTable = PrivateCacheData["RevisionHistoryDataTable"] as DataTable;
                else
                    PopulateRevisionHistoryTable(appList, revisionHistoryDataTable);

                View.BindgrdRevisionHistory(revisionHistoryDataTable);
                View.grdRevisionHistorySelectedIndex = grdRevisionHistoryKey;


                // create the data table for the grid
                DataTable dtDeclineReasons = GetDeclineReasonsDataTable();
                if (revisionHistoryDataTable != null && revisionHistoryDataTable.Rows.Count > 0)
                {
                    // Populate the DEcline reasons for the Selected, or first ammendment
                    // GenericKeyType is of GenericKeyTypes.OfferInformation in the reasons table

                    //GenericKeyTypeKey = Convert.ToInt32(PrivateCacheData["generickeytypekey"]);
                    //IApplicationInformation latestApplicationInformation = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(_generickey).GetLatestApplicationInformation();
                    //int OfferInformationKey = latestApplicationInformation.Key;
                    //PopulateDeclineReasonsDataTable(OfferInformationKey, DTDeclineReasons);

                    int selectedkey = Convert.ToInt32(revisionHistoryDataTable.Rows[grdRevisionHistoryKey][0]);
                    PopulateDeclineReasonsDataTable(selectedkey, dtDeclineReasons);
                    View.BindgrdDeclineReasons(dtDeclineReasons);


                    IApplicationInformationVariableLoan aiVl = ApplicationRepository.GetApplicationInformationVariableLoan(selectedkey);
                    if (aiVl != null)
                    {
                        if (aiVl.BondToRegister != null) View.SetlblBondToRegister = aiVl.BondToRegister.Value.ToString(Constants.CurrencyFormat);
                        if (aiVl.Category != null) View.SetlblCategory = aiVl.Category.Description;
                        if ((aiVl.MarketRate != null) && (aiVl.RateConfiguration.Margin != null))
                            View.SetlblEffectiveRate = (aiVl.RateConfiguration.MarketRate.Value + aiVl.RateConfiguration.Margin.Value).ToString(Constants.RateFormat);
                        if (aiVl.HouseholdIncome != null) View.SetlblHouseHoldIncome = aiVl.HouseholdIncome.Value.ToString(Constants.CurrencyFormat);
                        if (aiVl.RateConfiguration.Margin != null) View.SetlblLinkRate = aiVl.RateConfiguration.Margin.Description;
                        if (aiVl.LTV != null) View.SetlblLTV = aiVl.LTV.Value.ToString(Constants.RateFormat);
                        if (aiVl.PTI != null) View.SetlblPTI = aiVl.PTI.Value.ToString(Constants.RateFormat);
                        if (aiVl.SPV != null) View.SetlblSPVName = aiVl.SPV.Description;
                        if (aiVl.Term != null) View.SetlblTerm = Convert.ToString(aiVl.Term) + " months";
                        if (aiVl.MonthlyInstalment != null) View.SetlblTotalInstallment = aiVl.MonthlyInstalment.Value.ToString(Constants.CurrencyFormat);
                        if (aiVl.LoanAgreementAmount != null) View.SetlblTotalLoanRequired = aiVl.LoanAgreementAmount.Value.ToString(Constants.CurrencyFormat);
                        // bind the lower grid
                    }
                }
            }
        }

        private static DataTable GetRevisionHistoryDataTable()
        {
            DataTable revisionHistoryDataTable = new DataTable();
            revisionHistoryDataTable.Columns.Add("Key");
            revisionHistoryDataTable.Columns.Add("Revision");
            revisionHistoryDataTable.Columns.Add("DateRevised");
            revisionHistoryDataTable.Columns.Add("ApplicationType");
            revisionHistoryDataTable.Columns.Add("Product");
            return revisionHistoryDataTable;
        }

        private void PopulateRevisionHistoryTable(IEventList<IApplicationInformation> appList, DataTable RevisionHistoryDataTable)
        {
            if (appList.Count > 0)
            {
                int RevisionCount = 0;
                for (int i = 0; i < appList.Count; i++)
                {
                    DataRow rhRow = RevisionHistoryDataTable.NewRow();
                    RevisionCount++;
                    rhRow["Key"] = appList[i].Key;

                    // set up the display for revision count                    
                    if (i == 0)
                        rhRow["Revision"] = "Initial";
                    else
                    {
                        if (i == (appList.Count - 1))
                            rhRow["Revision"] = "Current";
                        else
                            rhRow["Revision"] = "Revision " + Convert.ToString(RevisionCount);
                    }
                    rhRow["DateRevised"] = appList[i].ApplicationInsertDate.ToString(Constants.DateFormat); //.HasValue ? appList[i].ApplicationInsertDate.Value.ToString(SAHL.Common.Constants.DateFormat) : "";
                    rhRow["ApplicationType"] = appList[i].Application.ApplicationType.Description;
                    rhRow["Product"] = appList[i].Product.Description;
                    RevisionHistoryDataTable.Rows.Add(rhRow);
                }
                PrivateCacheData.Remove("RevisionHistoryDataTable");
                PrivateCacheData.Add("RevisionHistoryDataTable", RevisionHistoryDataTable);
            }
        }



        private static DataTable GetDeclineReasonsDataTable()
        {
            DataTable declineReasonsDataTable = new DataTable();
            declineReasonsDataTable.Columns.Add("ReasonType");
            declineReasonsDataTable.Columns.Add("Description");
            declineReasonsDataTable.Columns.Add("Comment");
            return declineReasonsDataTable;
        }

        private static void PopulateDeclineReasonsDataTable(int selectedKey, DataTable declineReasonsDataTable)
        {

            declineReasonsDataTable.Clear();
            //int reasonTypeKey = Convert.ToInt32(_view.ViewAttributes["reasontypekey"]);
            IReasonRepository reasonRepo = RepositoryFactory.GetRepository<IReasonRepository>();
            IReadOnlyEventList<IReason> reasons = reasonRepo.GetReasonByGenericTypeAndKey((int)SAHL.Common.Globals.GenericKeyTypes.OfferInformation, selectedKey);

            if (reasons.Count > 0)
                for (int i = 0; i < reasons.Count; i++)
                {
                    DataRow drRow = declineReasonsDataTable.NewRow();
                    drRow["ReasonType"] = reasons[i].ReasonDefinition.ReasonType.Description;
                    drRow["Description"] = reasons[i].ReasonDefinition.ReasonDescription.Description;
                    drRow["Comment"] = reasons[i].ReasonDefinition.AllowComment ? reasons[i].Comment : "";
                    declineReasonsDataTable.Rows.Add(drRow);
                }
        }

        #endregion
    }
}
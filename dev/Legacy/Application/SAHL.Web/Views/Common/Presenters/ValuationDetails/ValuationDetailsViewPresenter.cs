using System;
using System.Collections;
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
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ValuationDetails
{
    public class ValuationDetailsViewPresenter : SAHLCommonBasePresenter<IValuationDetailsView>
    {
        private IPropertyRepository propertyRepository;
        private IApplicationMortgageLoan applicationMortgageLoan;
        private IApplicationRepository applicationRepository;
        private IAccountRepository accountRepository;
        private DataTable dsValuations = new DataTable();
        private DataTable dtAddress = new DataTable();
        private InstanceNode instanceNode;
        private CBOMenuNode node;

        private IProperty property;
        private int requiredKey;


        public ValuationDetailsViewPresenter(IValuationDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }


        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            if (_view.IsMenuPostBack)
                GlobalCacheData.Remove(ViewConstants.ValuationManual);

            node = CBOManager.GetCurrentCBONode(View.CurrentPrincipal) as CBOMenuNode;
            //Set up the Interface
            View.ShowbtnViewDetail = true;
            View.ShowpnlInspectionContactDetails = false;
            View.ShowbtnSubmit = false;
            View.ShowgrdValuations = true;
            View.ShowpnlValuations = true;
            // set up the eventhandlers
            View.btnViewDetailClicked += btnViewDetailClicked;
            View.grdValuationsGridSelectedIndexChanged += ValuationsGridSelectedIndexChanged;

            //if (!PrivateCacheData.ContainsKey("ValuationsIndex"))
            //    PrivateCacheData.Add("ValuationsIndex", 0);
            GlobalCacheData.Remove("ValuationsIndex");
            GlobalCacheData.Add("ValuationsIndex", 0, new List<ICacheObjectLifeTime>());


        }



        void ValuationsGridSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            //CheckIfPending();
            GlobalCacheData.Remove("ValuationsIndex");
            GlobalCacheData.Add("ValuationsIndex", View.ValuationItemIndex, new List<ICacheObjectLifeTime>());

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
            accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
            IRuleService svcRule = ServiceFactory.GetService<IRuleService>(); 

            // Get the AccountKey/OfferKey from the CBO
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
                    property = account.SecuredMortgageLoan.Property;
                    break;
                case (int)GenericKeyTypes.Property:
                    property = propertyRepository.GetPropertyByKey(requiredKey);

                    IEventList<IApplication> applications = propertyRepository.GetApplicationsForProperty(requiredKey);

                    foreach (IApplicationMortgageLoan app in applications)
                    {
                        svcRule.ExecuteRule(_view.Messages, "ValuationRecentExists", app);
                    }
                    

                    break;
                case (int)GenericKeyTypes.Offer:
                    applicationMortgageLoan = applicationRepository.GetApplicationByKey(requiredKey) as IApplicationMortgageLoan;
                    property = applicationMortgageLoan.Property;
                    svcRule.ExecuteRule(_view.Messages, "ValuationRecentExists", applicationMortgageLoan);

                    break;
                default:
                    break;
            }

            //property = propertyRepository.GetPropertyByKey(527089);

            // Check if no property has been assigned
            if(property != null)
            {
                if (PrivateCacheData.ContainsKey("AddressDS"))
                    View.BindPropertyGrid(PrivateCacheData["AddressDS"] as DataTable);
                else
                    View.BindPropertyGrid((GetPropertyDT(property.Address)));

                if (GlobalCacheData.ContainsKey("ValuationsIndex"))
                    View.ValuationItemIndex = (int)GlobalCacheData["ValuationsIndex"];
                //GlobalCacheData.Remove("ValuationsIndex");


                IEventList<IValuation> valuations = propertyRepository.GetValuationsByPropertyKeyDateSorted(property.Key);

                if (valuations != null)
                    PopulateValuationsDS(valuations);


                if (dsValuations.Rows.Count > 0)
                {
                    View.BindgrdValuations(dsValuations);
                    CheckIfPending();
                }
                else
                {
                    View.ShowbtnViewDetail = false;
                    View.EnablegrdValuations = false;
                } 

            }
            else
            {
                // There are no properties assigned to this offer yet
                View.ShowbtnViewDetail = false;
                View.EnablegrdValuations = false;
            }



        }

        private DataTable GetPropertyDT(IAddress Address)
        {
            // Setup the Valuations Table 
            dtAddress.Reset();
            dtAddress.Columns.Add("Address");
            dtAddress.TableName = "AddressDS";

            DataRow valRow = dtAddress.NewRow();
            valRow["Address"] = Address.GetFormattedDescription(AddressDelimiters.Comma);
            dtAddress.Rows.Add(valRow);

            PrivateCacheData.Remove("AddressDS");
            PrivateCacheData.Add("AddressDS", dtAddress);

            return dtAddress;


        }

        /// <summary>
        /// Check if the record is pending if so hide the View detail button
        /// </summary>
        /// <returns></returns>
        void CheckIfPending()
        {

            GlobalCacheData.Remove("ValuationsIndex");
            GlobalCacheData.Add("ValuationsIndex", View.ValuationItemIndex, new List<ICacheObjectLifeTime>());
            int valuationindex = View.ValuationItemIndex;

            //TODO If valuation  not complete throw domain error, dont navigate
            // ValuationStatusID = 2 Navigate else dont
            if (Convert.ToInt32(dsValuations.Rows[valuationindex][5]) == 2)
                View.ShowbtnViewDetail = true;
            else
                View.ShowbtnViewDetail = false;
        }

        void btnViewDetailClicked(object sender, EventArgs e)
        {

            dsValuations = GlobalCacheData["ValuationDS"] as DataTable;

            GlobalCacheData.Remove("ValuationsIndex");
            GlobalCacheData.Add("ValuationsIndex", View.ValuationItemIndex, new List<ICacheObjectLifeTime>());
            int valuationindex = View.ValuationItemIndex;

            //TODO If valuation  not complete throw domain error, dont navigate
            // ValuationStatusID = 2 Navigate else dont

            int valuationkey = Convert.ToInt32(dsValuations.Rows[valuationindex][0]);
            GlobalCacheData.Remove("ValuationKey");
            GlobalCacheData.Add("ValuationKey", valuationkey, new List<ICacheObjectLifeTime>());

            int dataProviderDataServiceKey = Convert.ToInt32(dsValuations.Rows[valuationindex][1]);

            // Navigate to the correct display type
            switch (dataProviderDataServiceKey)
            {
                case (int)SAHL.Common.Globals.DataProviderDataServices.SAHLManualValuation:
                    View.Navigator.Navigate("Manual");
                    break;
                case (int)SAHL.Common.Globals.DataProviderDataServices.LightstoneAutomatedValuation:
                    View.Navigator.Navigate("Lightstone");
                    break;
                case (int)SAHL.Common.Globals.DataProviderDataServices.LightstonePhysicalValuation:
                    View.Navigator.Navigate("LightstonePhysical");
                    break;
                case (int)SAHL.Common.Globals.DataProviderDataServices.AdCheckPhysicalValuation:   
                    View.Navigator.Navigate("Adcheck");
                    break;
            }
        }

        void PopulateValuationsDS(IEventList<IValuation> Valuations)
        {

            // Setup the Valuations Table 
            dsValuations.Reset();
            dsValuations.Columns.Add("Key");
            dsValuations.Columns.Add("DataServiceProviderKey");
            dsValuations.Columns.Add("Valuer");
            dsValuations.Columns.Add("ValuationDate");
            dsValuations.Columns.Add("ValuationStatus");
            dsValuations.Columns.Add("ValuationStatusKey");
            dsValuations.Columns.Add("IsActive");

            dsValuations.Columns.Add("ValuationAmount");
            dsValuations.Columns.Add("HOCValuation");
            dsValuations.Columns.Add("ValuationPurpose");
            dsValuations.Columns.Add("RequestedBy");
            dsValuations.Columns.Add("ValuationType");
            dsValuations.Columns.Add("XMLData");
            dsValuations.Columns.Add("ValuationSortDate");
            dsValuations.TableName = "Valuations";

            
            //Valuations.Sort((comparer.);

            if (Valuations.Count > 0)
                for (int i = 0; i < Valuations.Count; i++)
                {
                    DataRow valRow = dsValuations.NewRow();
                    valRow["Key"] = Valuations[i].Key;
                    valRow["DataServiceProviderKey"] = Valuations[i].ValuationDataProviderDataService.DataProviderDataService.Key;

                    if (Valuations[i].Valuator == null)
                        valRow["Valuer"] = string.Format("{0} {1}", Valuations[i].ValuationDataProviderDataService.DataProviderDataService.DataProvider.Description, Valuations[i].ValuationDataProviderDataService.DataProviderDataService.DataService.Description);
                    else
                        valRow["Valuer"] = Valuations[i].Valuator.LegalEntity.DisplayName;

                    valRow["ValuationDate"] = Convert.ToString(Valuations[i].ValuationDate.ToShortDateString());
                    valRow["ValuationSortDate"] = Valuations[i].ValuationDate;
                    if (Valuations[i].ValuationAmount != null) valRow["ValuationAmount"] = Convert.ToString(Valuations[i].ValuationAmount.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
                    if (Valuations[i].ValuationHOCValue != null) valRow["HOCValuation"] = Convert.ToString(Valuations[i].ValuationHOCValue.Value.ToString(SAHL.Common.Constants.CurrencyFormat));
                    valRow["ValuationPurpose"] = "";
                    valRow["RequestedBy"] = Valuations[i].ValuationUserID;
                    valRow["ValuationType"] = string.Format("{0} {1}", Valuations[i].ValuationDataProviderDataService.DataProviderDataService.DataProvider.Description, Valuations[i].ValuationDataProviderDataService.DataProviderDataService.DataService.Description);
                    valRow["XMLData"] = Valuations[i].Data;
                    valRow["ValuationStatus"] = Valuations[i].ValuationStatus.Description;
                    valRow["IsActive"] = Valuations[i].IsActive ? "True" : "False";
                    valRow["ValuationStatusKey"] = Valuations[i].ValuationStatus.Key;
                    dsValuations.Rows.Add(valRow);
                }

            //dsValuations.DefaultView.Sort = "ValuationSortDate DESC";
            //dsValuations.DefaultView.Sort = "Key DESC"; //ValuationDate DESC";
            GlobalCacheData.Remove("ValuationDS");
            GlobalCacheData.Add("ValuationDS", dsValuations, new List<ICacheObjectLifeTime>());

        }
    }
}

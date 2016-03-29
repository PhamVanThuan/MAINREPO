using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Views.Common.Presenters.ValuationDetails;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingExtendedBase : SAHLCommonBasePresenter<IValuationManualPropertyDetailsView>
    {
        protected int selectedBuildingType = 1;
        protected IValuationDiscriminatedSAHLManual _valManual;
        protected IProperty _property;
        protected IList<ICacheObjectLifeTime> _lifeTimes;
        protected ILookupRepository _lookupRepo;
        protected IPropertyRepository _propRepo;
        protected List<ValuationExtendedDetails> _lstValuationExtendedDetails;

        public MainDwellingExtendedBase(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!View.ShouldRunPage)
                return;

            if (_view.IsMenuPostBack)
                ClearGlobalCache();

            _view.OnNextButtonClicked += new EventHandler(_view_OnNextButtonClicked);
            _view.OnAddButtonClicked += new EventHandler(_view_OnAddButtonClicked);
            _view.OnRemoveButtonClicked += new KeyChangedEventHandler(_view_OnRemoveButtonClicked);

            _view.ddlBuildingTypeOnSelectedIndexChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_ddlBuildingTypeOnSelectedIndexChanged);
            _view.OnBackButtonClicked += new EventHandler(_view_OnBackButtonClicked);
            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);

            _propRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            if (GlobalCacheData.ContainsKey(ViewConstants.Properties))
            {
                IEventList<IProperty> properties = GlobalCacheData[ViewConstants.Properties] as IEventList<IProperty>;
                _property = properties[0];
            }

            _view.HideAllPanels();
            
            _view.ShowPanelsForAddExtended();

            _view.ShowExtendedDetailsGrid = true;

            _view.BindOutBuildingRoofType(_lookupRepo.ValuationRoofTypes);
            
            _view.BindImprovementType(_lookupRepo.ValuationImprovementType);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!View.ShouldRunPage) 
                return;

            if (selectedBuildingType == (int)SAHL.Common.Globals.ValuationBuildingType.Outbuilding)
                _view.ShowPanelForOutBuildingAdd();
            else
                _view.ShowPanelForImprovementAdd();

            if (GlobalCacheData.ContainsKey(ViewConstants.ValuationManual))
                _valManual = GlobalCacheData[ViewConstants.ValuationManual] as IValuationDiscriminatedSAHLManual;

            _view.BindExtendedDetailsGrid(PopulateValuationExtendedDetailsList(_valManual));
        }

        protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            Navigator.Navigate("Cancel");
        }

        protected virtual void _view_OnBackButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Back");
        }
        
        protected virtual void _view_OnNextButtonClicked(object sender, EventArgs e)
        {
            GlobalCacheData.Remove(ViewConstants.ValuationPresenter);
            GlobalCacheData.Add(ViewConstants.ValuationPresenter, _view.ViewName, _lifeTimes);

            Navigator.Navigate("Next");
        }

        protected void _view_ddlBuildingTypeOnSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            selectedBuildingType = Convert.ToInt32(e.Key);
        }

        protected virtual void _view_OnRemoveButtonClicked(object sender, KeyChangedEventArgs e)
        {
            this.RemoveExtendedDetail(e.Key.ToString(), ref _valManual);

            if (_view.IsValid)
            {
                GlobalCacheData.Remove(ViewConstants.ValuationManual);
                GlobalCacheData.Add(ViewConstants.ValuationManual, _valManual, _lifeTimes);

                _view.SetupViewForAddNewEntry();
            }
        }

        protected virtual void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            if (selectedBuildingType == (int)SAHL.Common.Globals.ValuationBuildingType.Outbuilding)
            {
                IValuationOutbuilding valOutbuilding = _view.GetCapturedOutbuilding(_propRepo.CreateEmptyValuationOutbuilding());
                if (valOutbuilding != null)
                {
                    valOutbuilding.Valuation = _valManual;
                    if (valOutbuilding.ValidateEntity())
                        _valManual.ValuationOutBuildings.Add(_view.Messages, valOutbuilding);
                }
            }
            else
            {
                IValuationImprovement valImprovement = _view.GetValuationImprovement(_propRepo.CreateEmptyValuationImprovement());
                if (valImprovement != null)
                {
                    valImprovement.Valuation = _valManual;
                    if (valImprovement.ValidateEntity())
                        _valManual.ValuationImprovements.Add(_view.Messages, valImprovement);
                }
            }

            if (_view.IsValid)
            {
                GlobalCacheData.Remove(ViewConstants.ValuationManual);
                GlobalCacheData.Add(ViewConstants.ValuationManual, _valManual, _lifeTimes);

                _view.SetupViewForAddNewEntry();
            }
        }

        protected static List<ValuationExtendedDetails> PopulateValuationExtendedDetailsList(IValuationDiscriminatedSAHLManual valManual)
        {
            List<ValuationExtendedDetails> lstValuationExtendedDetails = new List<ValuationExtendedDetails>();

            foreach (IValuationOutbuilding valuationOutbuilding in valManual.ValuationOutBuildings)
            {
                ValuationExtendedDetails itm = new ValuationExtendedDetails();

                itm.BuildingTypeKey = Convert.ToInt32(ValuationBuildingType.Outbuilding);
                itm.Action = SAHL.Common.UpdateAction.Add;
                itm.Key = valuationOutbuilding.Key;
                itm.RoofTypeKey = valuationOutbuilding.ValuationRoofType.Key;
                itm.RoofTypeDesc = valuationOutbuilding.ValuationRoofType.Description;
                itm.Extent = valuationOutbuilding.Extent;
                itm.Rate = valuationOutbuilding.Rate;

                lstValuationExtendedDetails.Add(itm);
            }

            foreach (IValuationImprovement valuationImprovement in valManual.ValuationImprovements)
            {
                ValuationExtendedDetails itm = new ValuationExtendedDetails();

                itm.BuildingTypeKey = Convert.ToInt32(ValuationBuildingType.Improvement);
                itm.Action = SAHL.Common.UpdateAction.Add;
                itm.Key = valuationImprovement.Key;
                itm.ImprovementTypeKey = valuationImprovement.ValuationImprovementType.Key;
                itm.ImprovementTypeDesc = valuationImprovement.ValuationImprovementType.Description;
                itm.ImprovementDate = valuationImprovement.ImprovementDate;
                itm.ImprovementValue = valuationImprovement.ImprovementValue;

                lstValuationExtendedDetails.Add(itm);
            }

            return lstValuationExtendedDetails;
        }

        protected void RemoveExtendedDetail(string selectedGridItem, ref IValuationDiscriminatedSAHLManual valManual)
        {
            string[] selectedItem = selectedGridItem.Split(',');
            int selectedBuildingTypeKey = Convert.ToInt32(selectedItem[2]);
            int removeIndex = -1;
            int i = -1;

            if (selectedBuildingTypeKey == (int)SAHL.Common.Globals.ValuationBuildingType.Outbuilding)
            {
                int selectedRoofTypeKey = Convert.ToInt32(selectedItem[3]);
                double selectedExtent = Convert.ToDouble(selectedItem[4]);
                double selectedRate = Convert.ToDouble(selectedItem[5]);

                // loop thru the outbuildings and find the index of the one to be removed
                foreach (IValuationOutbuilding valuationOutbuilding in valManual.ValuationOutBuildings)
                {
                    i++;
                    if (valuationOutbuilding.ValuationRoofType.Key == selectedRoofTypeKey
                        && valuationOutbuilding.Extent.Value == selectedExtent
                        && valuationOutbuilding.Rate.Value == selectedRate)
                    {
                        //if we have found the one to remove then exit loop
                        removeIndex = i;
                        break;
                    }
                }
                // remove the selected outbuilding
                if (removeIndex > -1)
                    valManual.ValuationOutBuildings.RemoveAt(_view.Messages, removeIndex);
            }
            else
            {
                int selectedImprovementTypeKey = Convert.ToInt32(selectedItem[3]);
                string selectedImprovementDateText = selectedItem[4];
                DateTime? selectedImprovementDate = null;
                if (selectedImprovementDateText.Length > 0 && selectedImprovementDateText != "-")
                    selectedImprovementDate = Convert.ToDateTime(selectedImprovementDateText);
                double selectedImprovementValue = Convert.ToDouble(selectedItem[5]);

                // loop thru the impovements and find the index of the one to be removed
                foreach (IValuationImprovement valuationImprovement in valManual.ValuationImprovements)
                {
                    i++;
                    if (valuationImprovement.ValuationImprovementType.Key == selectedImprovementTypeKey
                        && (valuationImprovement.ImprovementDate == selectedImprovementDate)
                        && valuationImprovement.ImprovementValue == selectedImprovementValue)
                    {
                        //if we have found the one to remove then exit loop
                        removeIndex = i;
                        break;
                    }
                }
                // remove the selected improvement
                if (removeIndex > -1)
                    valManual.ValuationImprovements.RemoveAt(_view.Messages, removeIndex);
            }
        }

        protected void ClearGlobalCache()
        {
            GlobalCacheData.Remove(ViewConstants.ValuationManual);
            GlobalCacheData.Remove(ViewConstants.ValuationClassification);
            GlobalCacheData.Remove(ViewConstants.ValuationEscalationPercentage);
            GlobalCacheData.Remove(ViewConstants.ValuationMainBuilding);
            GlobalCacheData.Remove(ViewConstants.ValuationCottage);
        }
    }
}

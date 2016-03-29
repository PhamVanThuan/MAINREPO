using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Common.Presenters.ManualValuationMainDwellingDetails
{
    public class MainDwellingExtendedDisplay : MainDwellingDetailsBase
    {    
        public MainDwellingExtendedDisplay(IValuationManualPropertyDetailsView view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            
            if (!_view.ShouldRunPage) 
                return;

            _view.Property = _property;
            _view.BindExtendedDetailsGrid(PopulateValuationExtendedDetailsList(_valManual));
            
            _view.ShowExtendedDetailsGrid = true;
            _view.HideAllPanels();
            _view.HidePanelsForAddExtended();

            _view.OnBackButtonClicked+=new EventHandler(_view_OnBackButtonClicked);
            _view.OnCancelButtonClicked+=new EventHandler(_view_OnCancelButtonClicked);
        }

        private static List<ValuationExtendedDetails> PopulateValuationExtendedDetailsList(IValuationDiscriminatedSAHLManual _valManual)
        {
            List<ValuationExtendedDetails> lstValuationExtendedDetails = new List<ValuationExtendedDetails>();

            foreach (IValuationImprovement valuationImprovement in _valManual.ValuationImprovements)
            {
                ValuationExtendedDetails itm = new ValuationExtendedDetails();

                itm.BuildingTypeKey = Convert.ToInt32(ValuationBuildingType.Improvement);
                itm.Key = valuationImprovement.Key;
                itm.ImprovementTypeKey = valuationImprovement.ValuationImprovementType.Key;
                itm.ImprovementTypeDesc = valuationImprovement.ValuationImprovementType.Description;
                itm.ImprovementDate = valuationImprovement.ImprovementDate;
                itm.ImprovementValue = valuationImprovement.ImprovementValue;

                lstValuationExtendedDetails.Add(itm);
            }
            foreach (IValuationOutbuilding valuationOutbuilding in _valManual.ValuationOutBuildings)
            {
                ValuationExtendedDetails itm = new ValuationExtendedDetails();

                itm.BuildingTypeKey = Convert.ToInt32(ValuationBuildingType.Outbuilding);
                itm.Key = valuationOutbuilding.Key;
                itm.RoofTypeKey = valuationOutbuilding.ValuationRoofType.Key;
                itm.RoofTypeDesc = valuationOutbuilding.ValuationRoofType.Description;
                itm.Extent = valuationOutbuilding.Extent;
                itm.Rate = valuationOutbuilding.Rate;

                lstValuationExtendedDetails.Add(itm);
            }

            return lstValuationExtendedDetails;
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            
            if (!_view.ShouldRunPage)
                return;
        }

        protected virtual void _view_OnBackButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Back");
        }

        protected virtual void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            Navigator.Navigate("Cancel");
        }        
     }
}

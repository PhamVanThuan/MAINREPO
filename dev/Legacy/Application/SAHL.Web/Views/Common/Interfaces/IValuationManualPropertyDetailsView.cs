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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common.Interfaces
{
    public class ValuationExtendedDetails
    {
        private int _key;
        private int _buildingTypeKey;
        private UpdateAction _action;
        // improvement properties
        private DateTime? _improvementDate;
        private int _improvementTypeKey;
        private string _improvementTypeDesc;
        private double _improvementValue;
        // outbuilding properties
        private int _roofTypeKey;
        private string _roofTypeDesc;
        private double? _extent;
        private double? _rate;  

        public int BuildingTypeKey
        {
            get { return _buildingTypeKey; }
            set { _buildingTypeKey = value; }
        }
	

        public UpdateAction Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public DateTime? ImprovementDate
        {
            get { return _improvementDate; }
            set { _improvementDate = value; }
        }

        public int ImprovementTypeKey
        {
            get { return _improvementTypeKey; }
            set { _improvementTypeKey = value; }
        }
        public string ImprovementTypeDesc
        {
            get { return _improvementTypeDesc; }
            set { _improvementTypeDesc = value; }
        }
        public double ImprovementValue
        {
            get { return _improvementValue; }
            set { _improvementValue = value; }
        }

        public int RoofTypeKey
        {
            get { return _roofTypeKey; }
            set { _roofTypeKey = value; }
        }

        public string RoofTypeDesc
        {
            get { return _roofTypeDesc; }
            set { _roofTypeDesc = value; }
        }

        public double? Extent
        {
            get { return _extent; }
            set { _extent = value; }
        }
        public double? Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }
    }

    public interface IValuationManualPropertyDetailsView : IViewBase
    {
        void ShowPanelForOutBuildingAdd();

        void ShowPanelForImprovementAdd();

        bool ShowExtendedDetailsGrid { set; }

        void HideAllPanels();

        void ShowReadOnlyFieldsForDisplay();

        IValuationMainBuilding GetValuationMainBuilding(IValuationMainBuilding valMainBuilding);

        IValuationDiscriminatedSAHLManual GetValuationManual(IValuationDiscriminatedSAHLManual valSAHLManual);

        IValuationCottage GetValuationCottage(IValuationCottage valCottage);

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "It's a property, should we call it a cave?")]
        IProperty Property { set;}

        void BindValuationDisplay(IValuationDiscriminatedSAHLManual valManual, IValuationMainBuilding valMainBuilding, IValuationCottage valCottage, double valCombinedThatchValue);

        void HidePanelsForAddExtended();

        void ShowPanelsForAdd();

        void ShowPanelsForAddExtended();

        void BindExtendedDetailsGrid(List<ValuationExtendedDetails> lstValuationExtendedDetails);

        void ShowUpdateButton();

        void ShowBackButton();

        void ShowValuationImprovementDisplayDetails(IValuationImprovement valImprovements);

        void ShowValuationOutBuildingDisplayDetails(IValuationOutbuilding valOutBuilding);

        void BindUpdateableFields(IValuationDiscriminatedSAHLManual valManual, IValuationMainBuilding valMainBuilding, IValuationCottage valCottage);

        event EventHandler OnCancelButtonClicked;

        event EventHandler OnNextButtonClicked;

        event EventHandler OnBackButtonClicked;

        event EventHandler OnAddButtonClicked;

        event KeyChangedEventHandler OnRemoveButtonClicked;

        event KeyChangedEventHandler ddlBuildingTypeOnSelectedIndexChanged;

        void BindCottageRoofTypes(IEventList<IValuationRoofType> hocRoof);

        void BindMainBuildingRoofType(IEventList<IValuationRoofType> hocRoof);

        void BindBuildingClassification( IEventList<IValuationClassification> valuationClassification);

        void BindOutBuildingRoofType(IEventList<IValuationRoofType> valuationRoofTypes);

        void BindImprovementType(IEventList<IValuationImprovementType> valImprovementTypes);

        IValuationImprovement GetValuationImprovement(IValuationImprovement valImprovement);

        IValuationOutbuilding GetCapturedOutbuilding(IValuationOutbuilding valOutbuilding);

        void SetupViewForAddNewEntry();

        string SelectedGridRowItem { get;}
    }
}

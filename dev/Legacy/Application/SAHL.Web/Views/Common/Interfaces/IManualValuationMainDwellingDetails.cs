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

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IManualValuationMainDwellingDetails : IViewBase
    {
        #region Properties

        void ShowPanelForOutBuildingAddUpdate();

        void ShowPanelForImprovementAddUpdate();

        bool ShowImprovementGrid { set; }

        void HideAllPanels();

        string getSelectedFirstRowGrid { get; }

        void ShowReadOnlyFieldsForDisplay();

        IValuationMainBuilding GetValuationMainBuilding(IValuationMainBuilding valMainBuilding);

        IValuationDiscriminatedSAHLManual GetValuationManual(IValuationDiscriminatedSAHLManual valSAHLManual);

        IValuationCottage GetValuationCottage(IValuationCottage valCottage);

        IEventList<SAHL.Common.BusinessModel.Interfaces.IProperty> Properties { set;}

        IValuationDiscriminatedSAHLManual ValuationSAHLManual { set;}

        void BindValuationDisplay(IValuationDiscriminatedSAHLManual val);

        void ShowButtonsForImprovements();

        void ShowPanelsForAdd();

        void ShowPanelsForAddExtended();

        void bindGridImprovements();

        void ShowUpdateButton();

        void ShowBackButton();

        void ShowValuationImprovementDisplayDetails(IValuationImprovement valImprovements);

        void ShowValuationOutBuildingDisplayDetails(IValuationOutbuilding valOutBuilding);

        void BindOutBuildingDataFromGrid(int selectedGridItem);

        void BindImprovementDataFromGrid(int selectedGridItem);

        void BindUpdateableFields(IValuationDiscriminatedSAHLManual val);

        event EventHandler OnCancelButtonClicked;

        event EventHandler OnNextButtonClicked;

        event EventHandler OnBackButtonClicked;

        event KeyChangedEventHandler ddlBuildingTypeOnSelectedIndexChanged;

        event EventHandler OnAddNewButtonClicked;

        event KeyChangedEventHandler OnImprovementsGridSelectedIndexChanged;

        void BindCottageRoofTypes(IEventList<IValuationRoofType> hocRoof);

        event KeyChangedEventHandler OnAddButtonClicked;

        void BindMainBuildingRoofType(IEventList<IValuationRoofType> hocRoof);

        void BindBuildingClassification( IEventList<IValuationClassification> valuationClassification);

        void BindOutBuildingRoofType(IEventList<IValuationRoofType> valuationRoofTypes);

        void BindImprovementType(IEventList<IValuationImprovementType> valImprovementTypes);

        IValuationImprovement GetValuationImprovement(IValuationImprovement valImprovement);

        IValuationOutbuilding GetCapturedOutbuilding(IValuationOutbuilding valOutbuilding);

        void SetButtonsForFirstImprovementAdd(int numberRecs);

        void SetupViewForAddNewEntry();

        string GetSelectedFirstItem { get;}

        #endregion




    }
}

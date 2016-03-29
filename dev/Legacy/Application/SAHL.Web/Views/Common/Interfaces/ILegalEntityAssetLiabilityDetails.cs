using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ILegalEntityAssetLiabilityDetails : IViewBase
    {
        event KeyChangedEventHandler OngrdAssetLiabilitySelectedIndexChanged;
        event KeyChangedEventHandler OnddlTypeSelectedIndexChanged;
        event EventHandler OnAddButtonClicked;
        event EventHandler OnAddAddressButtonClicked;
        event EventHandler OnDeleteButtonClicked;
        event EventHandler OnCancelButtonClicked;
        event KeyChangedEventHandler OndddlAssociateSelectedIndexChanged;

        void SetControlsForAddUpdateFixedProperty();
        void SetControlsForAddInvestment();
        void SetControlsForAddUpdateLiabilityLoan();
        void SetControlsForAddUpdateLiabilitySurtey();
        void SetControlsForAddUpdateLifeAssurance();
        void SetControlsForAddUpdateOther();
        void SetControlsForAddUpdateFixedLongTermInvestment();

        bool ApplicationSummaryMode { set; }
        bool ShowUpdate { set; }
        bool ShowCancelButton { set;}
        bool ShowUpdateButton {set;}
        bool ShowDeleteButton {set;}

        void BindDisplayPanel(ILegalEntityAssetLiability leAssetLiability);
        void BindAssetLiabilitySubTypes(IEventList<IAssetLiabilitySubType> assetLiablilitySubTypes);
        void BindAssetLiabilityTypes(IEventList<IAssetLiabilityType> assetLiablilityTypes);

        // TODO: This viewName parameter is BS!  Change this method signature to do the right thing!
        void BindAssetLiabilityGrid(string viewName,IEventList<ILegalEntityAssetLiability> assetLiabilities);

        void BindlegalEntityAddress(IDictionary<string, string> leAddressLst);
        void BindAssociatedAssets(IEventList<ILegalEntityAssetLiability> leAssets);
        void ShowpnlAssociate(bool ShowDisplay);
        void SetUpdateButtonText(string descriptionText);

        /// <summary>
        /// Trims and checks string values for Zero Length
        /// </summary>
        /// <returns></returns>
        bool CheckStringsForZeroLength(IAssetLiability assetLiability);

        IAssetLiability GetAssetLiablityForAdd(IAssetLiability assetLiability);
        IAssetLiability GetAssetLiablityForUpdate(IAssetLiability assetLiability);

        DateTime DateAcquired { get; set;}
        double LiabilityValue { get; set; }
        double AssetValue { get; set; }
        int GetSelectedIndexOnGrid { get; }
        int GetSelectedAssociatedAsset { get;} 
        void RestoreAssetTypeValueForFixedProperty(int AssetLiabilityType);

        void BindUpdatePanel(ILegalEntityAssetLiability leAssetLiabilityUpdate);
        IEventList<ILegalEntityAssetLiability> AssetLiabilityDuplicates { set;}
        bool IsAddUpdate { set;}
    }
}

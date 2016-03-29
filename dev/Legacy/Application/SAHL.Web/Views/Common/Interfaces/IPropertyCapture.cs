using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IPropertyCapture : IViewBase
    {
        event KeyChangedEventHandler OnSearchButtonClicked;

        event KeyChangedEventHandler OnPropertiesGridSelectedIndexChanged;

        event KeyChangedEventHandler OnPropertiesGridDoubleClick;

        event KeyChangedEventHandler OnExistingAddressSelected;

        event KeyChangedEventHandler OnNewAddressSelected;

        event KeyChangedEventHandler OnPageChanged;

        event KeyChangedEventHandler OnPropertySave;

        event KeyChangedEventHandler OnSavePropertyData;

        void BindPropertiesGrid(DataTable DT);

        void BindPropertyTypes(IDictionary<string, string> propertyTypes);

        void BindTitleTypes(IDictionary<string, string> titleTypes);

        void BindOccupancyTypes(IDictionary<string, string> occupancyTypes);

        void SetAddress(DataRow row);

        string SellerID { set; }

        string Message { set; }

        string PageNo { set; }

        int PropertyIndex { set; }

        int AddressKey { set; }

        DataRow SelectedPropertyData { set; }

        bool ButtonRowVisible { set; }

        string SetOccupancyTypeValue { set; }

        // TODO: change setter to getter as per Microsoft.Usage
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Presenter caches this collection.")]
        Dictionary<string, string> PropertyData { set; }

        bool HasComcorpOfferPropertyDetails { set; }

        void BindComcorpOfferPropertyDetail(string pageNo, IComcorpOfferPropertyDetails comcorpOfferPropertyDetails);
    }
}
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
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Common.Interfaces
{
    public enum PropertyDetailsUpdateMode
    {
        Property, 
        Deeds, 
        Contact,
        Display
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyDetails : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        PropertyDetailsUpdateMode PropertyDetailsUpdateMode { get; set;}

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnUpdateButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnPropertyAddressGridSelectedIndexChanged;

        /// <summary>
        /// Property for setting whether to display the property grid
        /// </summary>
        bool ShowPropertyGrid { set;}

        /// <summary>
        /// Property for setting whether to display the deeds transfers grid
        /// </summary>
        bool ShowDeedsTransfersGrid { get; set;}

        /// <summary>
        /// Property which will tell us if any of the values have been updated
        /// </summary>
        bool ValuesChanged { get; set; }

        /// <summary>
        /// </summary>
        bool TitleDeedNumbersChanged { get; set; }

        /// <summary>
        /// Property for setting/retreiving the selected property 
        /// </summary>
        IProperty SelectedProperty { get; set;}
        
        /// <summary>
        /// Property for setting/retreiving the selected address key
        /// </summary>
        int SelectedAddressKey { get; set;}

        int UpdatedDeedsOfficeKey { get;}
        string UpdatedBondAccountNumber { get; }
        string UpdatedTitleDeedNumbers { get; }

        string UpdatedContactName { get; }
        string UpdatedContactNumber { get; }
        string UpdatedContactName2 { get; }
        string UpdatedContactNumber2 { get; }

        /// <summary>
        /// Binds the Property Address Grid
        /// </summary>
        /// <param name="lstPropertyAddresses"></param>
        void BindPropertyAddressGrid(IEventList<IAddress> lstPropertyAddresses);

        /// <summary>
        /// Binds the Property Details data to the appropriate controls
        /// </summary>
        /// <param name="property"></param>
        /// <param name="bondAccountNumber"></param>
        /// <param name="deedsOfficeKey"></param>
        /// <param name="lightStonePropertyID"></param>
        /// <param name="adCheckPropertyID"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        void BindPropertyDetails(IProperty property, string bondAccountNumber, int deedsOfficeKey, string lightStonePropertyID, string adCheckPropertyID, string currentDataProvider);

        /// <summary>
        /// Binds the Property Owners Grid
        /// </summary>
        /// <param name="dtOwnerDetails"></param>
        void BindPropertyOwnersGrid(DataTable dtOwnerDetails);

        /// <summary>
        /// Binds the Bond Registrations Grid
        /// </summary>
        /// <param name="dtBondRegistrations"></param>
        void BindBondRegistrationsGrid(DataTable dtBondRegistrations);


        /// <summary>
        /// 
        /// </summary>
        void BindDropDownLists();

        /// <summary>
        /// 
        /// </summary>
        bool ButtonRowVisible { set;}
    }
}

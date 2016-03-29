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

namespace SAHL.Web.Views.Life.Interfaces
{

    //public delegate void LegalEntityGridIndexChangedEventHandler();

    /// <summary>
    /// 
    /// </summary>
    public interface IContact : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnNextButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnUpdateAddressButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnAddAddressButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnUpdateContactDetailsButtonClicked;       
        
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnLegalEntityGridSelectedIndexChanged;
                
        /// <summary>
        /// Property for setting whether the grid is displaying Main Applicants or Assured Lives
        /// </summary>
        bool AssuredLivesMode { set;}

        /// <summary>
        /// Sets whether to show/hide workflow header
        /// </summary>
        bool ShowWorkFlowHeader { set;}

        /// <summary>
        /// Property for setting whether the Update Contact button is displayed
        /// </summary>
        bool ShowUpdateContactButton { set;}
        /// <summary>
        /// Property for setting whether the Update Address button is displayed
        /// </summary>
        bool ShowUpdateAddressButton { set;}
        /// <summary>
        /// Property for setting whether the Add Address button is displayed
        /// </summary>
        bool ShowAddAddressButton { set;}
        /// <summary>
        /// Property for setting whether the Add Address button is displayed
        /// </summary>
        bool ShowNextButton { set;}


        /// <summary>
        /// Property for setting/retreiving the selected legalentity key
        /// </summary>
        int SelectedLegalEntityKey { get; set;} 

        /// <summary>
        /// Binds the Legal Entitity Grid data
        /// </summary>
        /// <param name="lstLegalEntities"></param>
        /// <param name="accountKey"></param>
        void BindLegalEntityGrid(IReadOnlyEventList<ILegalEntity> lstLegalEntities, int accountKey);

        /// <summary>
        /// Binds the Assured Lives Details data to the appropriate controls
        /// </summary>
        /// <param name="legalEntity"></param>
        void BindAssuredLivesDetails(ILegalEntity legalEntity);

        /// <summary>
        /// Binds the Legal Entitity Address Grid data
        /// </summary>
        /// <param name="lstLegalEntityAddress"></param>
        void BindAddressData(IEventList<ILegalEntityAddress> lstLegalEntityAddress);

    }
}

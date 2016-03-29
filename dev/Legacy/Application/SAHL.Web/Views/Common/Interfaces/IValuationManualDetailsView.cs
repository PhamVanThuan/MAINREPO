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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValuationManualDetailsView :IViewBase
    {
        #region Events

        /// <summary>
        /// Event to be raised when the Cancel button is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Event to be raised when the Submit button is clicked
        /// </summary>
        event KeyChangedEventHandler OnSubmitButtonClicked;
        /// <summary>
        /// Change event of HOC Roof
        /// </summary>
        event KeyChangedEventHandler OnddlHOCRoofDescriptionIndexChanged;
        /// <summary>
        /// Valuation Details Click Event
        /// </summary>
        event KeyChangedEventHandler OnValuationDetailsClicked;

        event EventHandler OnBackButtonClicked;

        #endregion
        

        #region Properties

        /// <summary>
        /// Show Buttons
        /// </summary>
        bool ShowButtons { set;}

        /// <summary>
        /// Show Labels
        /// </summary>
        bool ShowLabels { set;}
        /// <summary>
        /// Show Navigation Buttons
        /// </summary>
        bool ShowNavigationButtons { set; }
        /// <summary>
        /// Get list of Properties
        /// </summary>
        IEventList<IProperty> Properties { set;}
        /// <summary>
        /// Set the Presenter - used in Grid Control display
        /// </summary>
        string SetPresenter { set; }
        /// <summary>
        /// Set ThatchAmount - for Update
        /// </summary>
        bool SetHOCThatchAmountForUpdate { set; }
        /// <summary>
        /// Set Conventional amount for update
        /// </summary>
        bool SetHOCConventionalAmountForUpdate { set; }

        int GetSelectedValuationKey { get;}

        #endregion


        #region Methods
        /// <summary>
        /// Get Updated Valuation Record
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        IValuation GetUpdatedValuation(IValuationDiscriminatedSAHLManual val);
         /// <summary>
        /// Get HOC Roof value
        /// </summary>
        IHOCRoof GetHOCRoof { get; }
        #endregion


        void HideAllPanels();
    }
}

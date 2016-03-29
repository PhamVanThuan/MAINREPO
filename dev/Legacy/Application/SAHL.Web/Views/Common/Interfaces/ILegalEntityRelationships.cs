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
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ILegalEntityRelationships: IViewBase
    {
        #region Properties

        /// <summary>
        /// Sets whether the Delete Confirmation message is visible.
        /// </summary>
        bool DeleteConfirmationVisible { set; }

        /// <summary>
        /// Sets whether the Add To CBO button is visible.
        /// </summary>
        bool AddToCBOButtonVisible { set; }

        /// <summary>
        /// Sets whether the Cancel button is visible.
        /// </summary>
        bool CancelButtonVisible { set; }

        /// <summary>
        /// Sets whether Submit button is visible.
        /// </summary>
        bool SubmitButtonVisible { set; }

        /// <summary>
        /// Sets whether the Action table/panel is visible.
        /// </summary>
        bool ActionTableVisible { set; }

        /// <summary>
        /// Sets whether the LegalEntityInfo table/panel is visible.
        /// </summary>
        bool LegalEntityInfoTableVisible { set; }

        /// <summary>
        /// Sets whether the Add To CBO button is enabled.
        /// </summary>
        bool AddToCBOButtonEnabled { set; }

        /// <summary>
        /// Sets whether the submit button is enabled.
        /// </summary>
        bool SubmitButtonEnabled { set; }

        /// <summary>
        /// Sets the grid postback type (enum). 
        /// </summary>
        GridPostBackType GridPostBackType { set; }

        /// <summary>
        /// Sets the caption on the Submit button
        /// </summary>
        string SubmitButtonText { set; }

        /// <summary>
        /// Get the selected Legal Entity relationship Type. Used when saving.
        /// </summary>
        int SelectedLegalEntityRelationshipTypeKey { get; }

        /// <summary>
        /// Gets/sets the selected Legal Entity relationship. Used when saving.
        /// </summary>
        int SelectedLegalEntityRelationshipIndex { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Bing a list of RelationShipTypes.
        /// </summary>
        /// <param name="relationshipTypes"></param>
        /// <param name="defaultValue"></param>
        void BindRelationshipTypes(IDictionary<string, string> relationshipTypes, string defaultValue);

        /// <summary>
        /// Binds the caption for an existing relationship.
        /// </summary>
        /// <param name="messageText"></param>
        void BindLabelMessage(string messageText);

        /// <summary>
        /// Bind the list of available 
        /// </summary>
        /// <param name="legalEntityRelationships"></param>
        void BindRelationshipGrid(IEventList<ILegalEntityRelationship> legalEntityRelationships);

        #endregion

        #region Events
        /// <summary>
        /// Triggered when a Legal Entity relationship changes need to be saved.
        /// </summary>
        event KeyChangedEventHandler OnSubmitButtonClick;

        /// <summary>
        /// Triggered when a Legal Entity relationship changes need to be cancelled.
        /// </summary>
        event EventHandler OnCancelButtonClick;

        /// <summary>
        /// Triggered when the selected Legal Entity is added to the DBO Menu.
        /// </summary>
        event KeyChangedEventHandler OnAddToCBO;

        /// <summary>
        /// Triggered when a Legal Entity relationship row is selected.
        /// </summary>
        event KeyChangedEventHandler OnGridItemSelected;

        #endregion



    }
}

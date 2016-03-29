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
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    public interface ILifeNTU : IViewBase
    {
        /// <summary>
        /// An event for handling when the SubmitButton is clicked
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// An event for handling when the Cancel is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// Set whether the cancel button should be visible
        /// </summary>
        bool CancelButtonVisible { set;}

        /// <summary>
        /// Gets/Sets the reason definition key selected by the user
        /// </summary>
        int SelectedReasonDefinitionKey { get; set;}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reasonDefinitions"></param>
        void BindReasonDefinitions(IDictionary<int,string> reasonDefinitions);
    }
}

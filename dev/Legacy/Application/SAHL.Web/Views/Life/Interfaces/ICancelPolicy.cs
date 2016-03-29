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
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life.Interfaces
{
    /// <summary>
    /// Interface for Life Cancel Policy
    /// </summary>
    public interface ICancelPolicy : IViewBase
    {
        /// <summary>
        /// Raised when the cancel button is clicked, pass in the list of search criteria.
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Raised when the submit button is clicked.
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// Called to bind the controls
        /// </summary>
        /// <param name="cancellationTypes"></param>
        /// <param name="reasons"></param>
        /// <param name="daysSinceAcceptedDate"></param>
        /// <param name="policyHasCommenced"></param>
        /// <param name="lifeIsConditionOfLoan"></param>
        void BindControls(IDictionary<string, int> cancellationTypes, IDictionary<int, string> reasons, int daysSinceAcceptedDate, bool policyHasCommenced, bool lifeIsConditionOfLoan);

        /// <summary>
        /// 
        /// </summary>
        string CancellationDisabledMessage { get; set;}

        /// <summary>
        /// Sets whether the CancelFromInception Radio Button is enabled
        /// </summary>
        bool CancelFromInceptionEnabled { set;}

        /// <summary>
        /// Sets whether the CancelWithAuthorization Radio Button is enabled
        /// </summary>                
        bool CancelWithAuthorizationEnabled { set;}

        /// <summary>
        /// Sets whether the CancelWithProRate Radio Button is enabled
        /// </summary>
        bool CancelWithProRateEnabled { set;}

        /// <summary>
        /// Sets whether the CancelWithNoRefund Radio Button is enabled
        /// </summary>
        bool CancelWithNoRefundEnabled { set;}

        /// <summary>
        /// Gets the value of checkbox stating whether a cancellation letter has been received from the client.
        /// </summary>
        bool CancellationLetterReceived { get;}

        /// <summary>
        /// 
        /// </summary>
        int CancellationTypeKey { get;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedReason { get; set; }

        /// <summary>
        /// Boolean to hold whether the user is a manager/admin user or not
        /// </summary>
        bool AdminUser { get; set; }
    }
}

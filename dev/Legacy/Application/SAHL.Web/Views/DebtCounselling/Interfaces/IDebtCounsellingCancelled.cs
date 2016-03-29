using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Data;

namespace SAHL.Web.Views.DebtCounselling.Interfaces
{
    public interface IDebtCounsellingCancelled : IViewBase
    {
        #region Events

        /// <summary>
        /// An event for handling when the SubmitButton is clicked
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// An event for handling when the Cancel is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        #endregion

        #region Methods

       /// <summary>
       /// 
       /// </summary>
       /// <param name="dictreasons"></param>
        void BindReasons(Dictionary<int, string> dictreasons);

        void BindLinkedAccountsGrid(DataTable dtLinkedAccounts);

        #endregion

        #region Properties
        /// <summary>
        /// Set whether the cancel button should be visible
        /// </summary>
        bool CancelButtonVisible { set; }

        int SelectedReasonDefinitionKey { get; }

        #endregion
    }
}

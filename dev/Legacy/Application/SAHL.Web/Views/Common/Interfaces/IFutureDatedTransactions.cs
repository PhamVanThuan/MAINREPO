using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IFutureDatedTransactions : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnAddButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event KeyChangedEventHandler OnUpdateButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event KeyChangedEventHandler OnDeleteButtonClicked;

        /// <summary>
        ///
        /// </summary>
        bool ArrearBalanceRowVisible { get; set; }

        /// <summary>
        ///
        /// </summary>
        bool ControlsVisible { set; }

        /// <summary>
        ///
        /// </summary>
        bool ShowLabels { set; }

        /// <summary>
        ///
        /// </summary>
        int RecordsGridSelectedIndex { get; }

        /// <summary>
        ///
        /// </summary>
        string DeleteButtonText { set; }

        /// <summary>
        ///
        /// </summary>
        string DeleteButtonOnClientClick { set; }

        /// <summary>
        ///
        /// </summary>
        bool SetEffectiveDateToCurrentDate { set; }

        /// <summary>
        ///
        /// </summary>
        GridPostBackType GridPostbackType { get; set; }

        /// <summary>
        /// Gets/sets the account key used on the view.
        /// </summary>
        string AccountKey { get; set; }

        /// <summary>
        ///
        /// </summary>
        bool ShowButtons { get; set; }

        /// <summary>
        /// Gets/sets the visibility of the Add button.
        /// </summary>
        bool ButtonAddVisible { get; set; }

        /// <summary>
        /// Gets/sets the visibility of the Update button.
        /// </summary>
        bool ButtonUpdateVisible { get; set; }

        /// <summary>
        /// Gets/sets the visibility of the Delete button.
        /// </summary>
        bool ButtonDeleteVisible { get; set; }

        /// <summary>
        ///
        /// </summary>
        int? SelectedBankAccountKey { get; }

        /// <summary>
        /// Gets/sets the Arrears Balance to be displayed on the screen if <see cref="ArrearBalanceRowVisible"/> is set to true.
        /// </summary>
        double ArrearBalance { get; set; }

        /// <summary>
        ///
        /// </summary>
        DateTime? EffectiveDate { get; }

        /// <summary>
        ///
        /// </summary>
        double? Amount { get; }

        /// <summary>
        ///
        /// </summary>
        string Reference { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Note { get; }

        /// <summary>
        ///
        /// </summary>
        bool tdRequestedByVisible { set; }

        /// <summary>
        ///
        /// </summary>
        bool lblRequestedByVisible { set; }

        /// <summary>
        ///
        /// </summary>
        string lblRequestedByText { set; }

        /// <summary>
        ///
        /// </summary>
        bool tdProcessedByVisible { set; }

        /// <summary>
        ///
        /// </summary>
        bool lblProcessedByVisible { set; }

        /// <summary>
        ///
        /// </summary>
        string lblProcessedByText { set; }

        IList<ILegalEntityBankAccount> LegalEntityBankAccounts { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="recurringTransactions"></param>
        void BindOrdersToGrid(IEventList<IManualDebitOrder> manualDebitOrders);

        /// <summary>
        ///
        /// </summary>
        /// <param name="recurringTransactionsPrv"></param>
        void BindOrdersToPreviousGrid(IEventList<IManualDebitOrder> manualDebitOrdersPrv);

        /// <summary>
        ///
        /// </summary>
        /// <param name="gridRowIndex"></param>
        int GetFSRTransactionKey(int gridRowIndex);

        /// <summary>
        ///
        /// </summary>
        bool RecordsGridPrvVisible { set; }

        /// <summary>
        ///
        /// </summary>
        bool CaptureMultipleDebitOrders { get; set; }

        /// <summary>
        ///
        /// </summary>
        int NoOfPayments { get; }
    }
}
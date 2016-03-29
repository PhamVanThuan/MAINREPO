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
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.QuickCash.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuickCashDetails : IViewBase
    {
        #region Events

        /// <summary>
        /// Handles the event that is fired when the cancel button is clicked
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Handles the event that is fired when the submit button is clicked
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// QC Payment Details Grid - Selected Index Changed
        /// </summary>
        event KeyChangedEventHandler onGridQuickCashPaymentSelectedIndexChanged;

        event KeyChangedEventHandler onPaymentTypeSelectedIndexChanged;

        event KeyChangedEventHandler ongridThirdPartyPaymentSelectedIndexChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Sets whether the Quick Cash Approved panel is shown
        /// </summary>
        bool ShowApprovedPanel { set;}

        /// <summary>
        /// Sets whether the Bank Account panel is shown
        /// </summary>
        bool ShowBankAccountPanel { set;}

        /// <summary>
        /// Sets whether the Quick Cash Payments panel is shown
        /// </summary>
        bool ShowPaymentsPanel { set;}

        /// <summary>
        /// Sets whether the cancel and submit buttons should be visible
        /// </summary>
        bool ShowButtons { set;}

        int BankAccountKey { get; set;}

        string SetSubmitButtonText { set;}

        int GetSetSelectedGridItem { get; set;}

        int GetSelectedThirPartyPayment { get;}

        bool ShowUpdateButton { set;}

        string SetThirdParyPaymentsGridHeaderText { set;}

        /// <summary>
        /// Pass the QuickCashInformation object to the view
        /// </summary>
        ISupportsQuickCashApplicationInformation QuickCashInformation { set;}

        bool ShowThirdPartyPaymentsGrid { set;}

        #endregion

        # region Methods

        /// <summary>
        /// 
        /// </summary>
        void BindApprovedPanel();
        /// <summary>
        /// 
        /// </summary>
        void BindQuickCashPaymentsGrid(bool postBack);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appExpenses"></param>
        /// <param name="setAutoPostBack"></param>
        void BindThirdPartyPaymentsGrid(IEventList<IApplicationExpense> appExpenses, bool setAutoPostBack);

        void BindBankAccount(IDictionary<string, string> bankAccounts);

        void BindQuickCashPaymentTypes(IEventList<IQuickCashPaymentType> qcPaymentTypes);

        void SetRatesForPaymentType(IRateConfiguration rateConfig);

        IApplicationInformationQuickCashDetail GetUpdatedQuickDetailRecord(IApplicationInformationQuickCashDetail qcDetail);

        void BindBankAccountPanel(IApplicationInformationQuickCashDetail appInformationQCDetail);

        #endregion






    }
}

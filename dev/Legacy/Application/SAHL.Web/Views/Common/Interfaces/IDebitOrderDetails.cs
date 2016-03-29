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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Collections;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDebitOrderDetails : IViewBase
    {
        #region EventHandlers
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
        event KeyChangedEventHandler OnDeleteButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnUpdateButtonClicked;

        event KeyChangedEventHandler OnGridSelectedIndexChanged;

        /// <summary>
        /// Called when the Payment Type changes.
        /// </summary>
        event KeyChangedEventHandler OnDebitOrderPaymentTypeChanged;

        #endregion

        #region Properties

        #region Get Properties
        string BankAccountKey { get;}
        string PaymentTypeKey { get;}
        string DODay { get;}
        DateTime? EffectDate { get;}
        int DetailKey { get;}
        int? FutureDatedChangeKey { get;}
        
        #endregion
        #region Set Properties
        /// <summary>
        /// 
        /// </summary>
        bool Ignore { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ForceShowBankAccountControl { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowLabels { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SetEffectiveDateToCurrentDate { set;}


        bool SetControlsToGrid { set;}
        
        /// <summary>
        /// 
        /// </summary>
        GridPostBackType gridPostBackType { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowButtons { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowControls { set;}

        /// <summary>
        /// 
        /// </summary>
        bool HideEffectiveDate { set;}

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

        #endregion
        #endregion

        #region Binding
        void BindControlsToGrid();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        void BindGrid(IFinancialService fs);


        /// <summary>
        /// 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        void BindGridForApplication(IApplication application);       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccounts"></param>
        void BindBankAccountControl(IEventList<IBankAccount> bankAccounts);

        /// <summary>
        /// 
        /// </summary>
        void BindPaymentTypes();


        /// <summary>
        /// 
        /// </summary>
        void BindDebitOrderDays();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridRowIndex"></param>
        void BindLabels(int gridRowIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicSalaryPaymentDates"></param>
        void BindSalaryPaymentDays(IDictionary<ILegalEntity, string> dicSalaryPaymentDates);
        
        #endregion
    }
}

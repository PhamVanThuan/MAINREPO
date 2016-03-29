using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ITransaction : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnRollbackButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnRollbackConfirmButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnTransactionTypeSelectedIndexChanged;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Transactions"></param>
        /// <param name="isArrears"></param>
        void BindTransactions(DataTable Transactions, bool isArrears);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Transactions"></param>
        /// <param name="isArrears"></param>
        void BindRollbackTransactions(DataTable Transactions, bool isArrears);

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        int FinancialServicesSelectedValue
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        int TransactionDisplayTypeSelectedValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        string TransactionDescriptionSelectedValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        bool FinancialTransactionsOnly
        {
            get;
            set;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //int NextKey
        //{
        //    get ; 
        //    set ; 
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //int PreviousKey
        //{
        //    get ; 
        //    set ; 
        //}
        /// <summary>
        /// 
        /// </summary>
        ButtonStatus.Display ButtonCancel
        {
            set;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //ButtonStatus.Display ButtonNext
        //{
        //    set;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //ButtonStatus.Display ButtonPrevious
        //{
        //    set;
        //}
        /// <summary>
        /// 
        /// </summary>
        ButtonStatus.Display ButtonRollback
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        ButtonStatus.Display ButtonRollbackConfirm
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        int RollbackTransactionNumber
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        GridPostBackType GridViewPostBackType
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        bool ShowTransactions
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        bool ShowRollbackTransactions
        {
            set;
        }
        /// <summary>
        /// Use this to hide empty lists and buttons when no account exists
        /// </summary>
        bool NoTransactions
        {
            set;
        }
        #endregion
    }

    public static class Paging
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PageRequestType
        {
            First = 0,
            Next,
            Previous
        };
    }

    public static class ButtonStatus
    {
        public enum Display
        {
            Hidden = 0,
            Disabled,
            Enabled
        };
    }
}

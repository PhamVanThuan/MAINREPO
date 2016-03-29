using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using System.Data;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ITransactionAccessMaintenance : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groups"></param>
        void BindGroupDropDown(DataTable groups);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        void BindUserDropDown(DataTable users);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionTypes"></param>
        void BindTransactionAccessTree(DataTable transactionTypes);

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        bool ShowUser
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        bool ShowGroup
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        bool ShowButton
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        bool AddGroup
        {
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        string GroupSelectedValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        string UserSelectedValue
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        bool AllowUpdate
        {
            set ; 
        }
        /// <summary>
        /// 
        /// </summary>
        string CheckedTransactions
        {
            get; 
        }
        /// <summary>
        /// 
        /// </summary>
        string NewGroupDescription
        {
            get; 
        }
        #endregion
    }
}

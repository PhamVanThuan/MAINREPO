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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ICallSummary : IViewBase
    {
        #region  View Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryList"></param>
        void BindGrid(IReadOnlyEventList<IHelpDeskQuery> queryList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskQuery"></param>
        void BindLabels(IHelpDeskQuery helpDeskQuery);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskQuery"></param>
        void BindUpdateControls(IHelpDeskQuery helpDeskQuery);

        /// <summary>
        /// 
        /// </summary>
        void ClearUpdateControls();

        /// <summary>
        /// 
        /// </summary>
        void BindStatusDropDown();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskCategoryList"></param>
		void BindCategoryDropDown(IList<IHelpDeskCategory> helpDeskCategoryList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountList"></param>
        void BindAccountDropDown(IList<IAccount> accountList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryTypeList"></param>
        void BindQueryTypeDropDown(IList<IGenericKeyType> queryTypeList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpdeskConsultants"></param>
        void BindConsultantDropDown(IList<IADUser> helpdeskConsultants);

        /// <summary>
        /// 
        /// </summary>
        void SetupControlsForDisplay();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adding"></param>
        /// <param name="routing"></param>
        void SetupControlsForAddOrUpdate(bool adding, bool routing);

        /// <summary>
        /// 
        /// </summary>
        void SetupControlsForRouting();

        /// <summary>
        /// 
        /// </summary>
        void SetDefaultDates();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helpDeskQuery"></param>
        /// <param name="adding"></param>
        void PopulateHeldeskRecord(IHelpDeskQuery helpDeskQuery, bool adding);

        /// <summary>
        /// 
        /// </summary>
        void ResetControls();

        #endregion


        #region View Properties

        /// <summary>
        /// 
        /// </summary>
        bool StatusDropDownVisible { set;}
        
        /// <summary>
        /// 
        /// </summary>
		bool AddOrUpdateButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        string SubmitButtonOnclickAttribute { set;}

        /// <summary>
        /// 
        /// </summary>
        bool AccountDropDownVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedMemoType { set;get;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedAccountNumber { set;get; }

        /// <summary>
        /// 
        /// </summary>
        string SelectedCategory { set;get; }

        /// <summary>
        /// 
        /// </summary>
        string SelectedStatus { set;get;}

        /// <summary>
        /// 
        /// </summary>
        bool CallSummaryGridSelectFirstRow { set;}

        /// <summary>
        /// 
        /// </summary>
        int CallSummaryGridSelectedIndex { get;}

        /// <summary>
        /// 
        /// </summary>
        bool AddOrUpdateButtonClicked { get;}

        /// <summary>
        /// 
        /// </summary>
		int SetLegalEntityKey { set;}

        ///// <summary>
        ///// 
        ///// </summary>
		//Int64 SetInstanceID { set;}

        /// <summary>
        /// 
        /// </summary>
        int SelectedConsultant { get;}

        /// <summary>
        /// 
        /// </summary>
        string DetailDescription { get;}

        /// <summary>
        /// 
        /// </summary>
        string ShortDescription { get;}

        #endregion


        #region View Events

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnAddUpdateClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnGridSelectedIndexChanged;

        #endregion

    }
}

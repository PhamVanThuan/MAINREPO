using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Authentication;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Collections;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Life
{
    public partial class Callback : SAHLCommonBaseView,ICallBack
    {
        #region Private Variables

        private DateTime? _callbackDate;
        private string _callbackComment;
        private int _reasonDefinitionKey;

        private enum GridColumnPositions
        {
            CallBackKey = 0,
            EntryUser = 1,
            EntryDate = 2,
            CallBackDate = 3,
            CompletedDate = 4,
            Reason = 5,
            Comments = 6
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.ShouldRunPage)
                return;

            if (!IsPostBack)
            {
                // Set the initial Date as Today and time as now() + 5 mins
                SetInitialCallbackDateTime(0, 5);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gridCallBack_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the CallBack Row
                ICallback callBack = e.Row.DataItem as ICallback;

                e.Row.Cells[(int)GridColumnPositions.EntryDate].Text = callBack.EntryDate.ToString(SAHL.Common.Constants.DateFormat + " HH:mm");
                e.Row.Cells[(int)GridColumnPositions.CallBackDate].Text = callBack.CallbackDate.ToString(SAHL.Common.Constants.DateFormat + " HH:mm");
                e.Row.Cells[(int)GridColumnPositions.CompletedDate].Text = callBack.CompletedDate.HasValue ? callBack.CompletedDate.Value.ToString(SAHL.Common.Constants.DateFormat + " HH:mm") : "";
                e.Row.Cells[(int)GridColumnPositions.Reason].Text = callBack.Reason.ReasonDefinition.ReasonDescription == null ? "" : callBack.Reason.ReasonDefinition.ReasonDescription.Description;
                e.Row.Cells[(int)GridColumnPositions.Comments].Text = callBack.Reason.Comment == null ? "" : callBack.Reason.Comment;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Perform Validation
            if (dteCallbackDate.Date != null)
               _callbackDate = new DateTime(dteCallbackDate.Date.Value.Year, dteCallbackDate.Date.Value.Month, dteCallbackDate.Date.Value.Day, int.Parse(ddlHour.SelectedValue), int.Parse(ddlMin.SelectedValue), 0);

            // Populate the Callback
            _callbackComment = txtNotes.Text;
            _reasonDefinitionKey = ddlReason.SelectedItem.Value == "-select-" ? -1 : Convert.ToInt32(ddlReason.SelectedItem.Value); 

            // Peform the save on the presenter
            OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        private void SetInitialCallbackDateTime(int numdays, int mins)
        {
            DateTime now = DateTime.Now;
            DateTime then = new DateTime(now.Year, now.Month, now.Day);

            for (int i = 0; i < numdays; i++)
            {
                then = then.AddDays(1);

                if (then.DayOfWeek == DayOfWeek.Saturday)
                    then = then.AddDays(2);
                else if (then.DayOfWeek == DayOfWeek.Sunday)
                    then = then.AddDays(1);
            }

            dteCallbackDate.Date = then;

            DateTime nowplusminutes = now.AddMinutes(mins);

            int iHour = nowplusminutes.Hour;
            int iMin = nowplusminutes.Minute;

            ddlHour.SelectedValue = iHour.ToString();

            int inewmin = 0;
            for (int i = 0; i < 60; i += 5)
            {
                if (i >= iMin)
                {
                    inewmin = i;
                    break;
                }
            }

            ddlMin.SelectedValue = inewmin.ToString();
        }

        #region ICallback Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        public DateTime? CallBackDate
        {
            get { return _callbackDate; }
            set { _callbackDate = value; }
        }

        public string CallBackComment
        {
            get { return _callbackComment; }
            set { _callbackComment = value;  }
        }

        public int ReasonDefinitionKey
        {
            get { return _reasonDefinitionKey; }
            set { _reasonDefinitionKey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstCallbacks"></param>
        public void BindCallBackGrid(IEventList<ICallback> lstCallbacks)
        {
            gridCallBack.AddGridBoundColumn("Key", "", new Unit(0), HorizontalAlign.Left, false);
            gridCallBack.AddGridBoundColumn("EntryUser", "User", new Unit(10, UnitType.Percentage), HorizontalAlign.Left, true);
            gridCallBack.AddGridBoundColumn("EntryDate", "Created", new Unit(15, UnitType.Percentage), HorizontalAlign.Left, true);
            gridCallBack.AddGridBoundColumn("CallbackDate", "Due", new Unit(15, UnitType.Percentage), HorizontalAlign.Left, true);
            gridCallBack.AddGridBoundColumn("", "Completed", new Unit(15, UnitType.Percentage), HorizontalAlign.Left, true);
            gridCallBack.AddGridBoundColumn("", "Reason", new Unit(15, UnitType.Percentage), HorizontalAlign.Left, true);
            gridCallBack.AddGridBoundColumn("", "Notes", new Unit(30, UnitType.Percentage), HorizontalAlign.Left, true);

            gridCallBack.DataSource = lstCallbacks;
            gridCallBack.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reasons"></param>
        /// <param name="enabled"></param>
        public void BindCallBackReasons(IDictionary<int, string> reasons, bool enabled)
        {
            ddlReason.Enabled = enabled;
            ddlReason.PleaseSelectItem = enabled;

            ddlReason.DataSource = reasons;
            ddlReason.DataBind();
        }     

        #endregion



    }
}

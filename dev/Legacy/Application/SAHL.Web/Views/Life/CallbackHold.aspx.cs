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
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common;
using System.Text;
using SAHL.Common.Authentication;

using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life
{
    public partial class CallbackHold : SAHLCommonBaseView,ICallBackHold
    {
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

        #region ICallBackHold Members

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

        #endregion
    }
}
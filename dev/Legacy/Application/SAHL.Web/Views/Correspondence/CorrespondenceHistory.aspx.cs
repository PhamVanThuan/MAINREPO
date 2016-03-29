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
using SAHL.Common.Authentication;
using SAHL.Web.Views.Correspondence.Interfaces;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Security.Principal;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxGridView;

namespace SAHL.Web.Views.Correspondence
{
    public partial class CorrespondenceHistory : SAHLCommonBaseView, ICorrespondenceHistory
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        /// <summary>
        /// Cancel Button Clicked event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            onCancelButtonClicked(sender, e);
        }

        #region ICorrespondenceHistory Members

        /// <summary>
        /// Event handler for Cancel Button
        /// </summary>
        public event EventHandler onCancelButtonClicked;

        public event KeyChangedEventHandler onCallback;


        /// <summary>
        /// Controls visibility of Cancel Button
        /// </summary>
        public bool ShowCancelButton
        {
            set
            {
                btnCancel.Visible = value;
            }
        }

        public string CorrespondenceDetailHTML
        {
            set
            {
                CorrespondenceDetailEditor.Html = value.ToString();
            }
            get
            {
                return CorrespondenceDetailEditor.Html;
            }
        }

        /// <summary>
        /// Controls visibility of Life Workflow Header control
        /// </summary>
        public bool ShowLifeWorkFlowHeader
        {
            set
            {
                WorkFlowHeader1.Visible = value;
            }
        }

        /// <summary>
        /// Bind History Grid
        /// 
        /// </summary>
        public void BindHistoryGrid(IEventList<ICorrespondence> lstCorrespondence)
        {
            List<BindableCorrespondence> bindableCorrespondence = new List<BindableCorrespondence>();
            foreach (ICorrespondence correspondence in lstCorrespondence)
            {
                bindableCorrespondence.Add(new BindableCorrespondence(correspondence));
            }

            // sort by latest correspondence first
            bindableCorrespondence.Sort(delegate(BindableCorrespondence c1, BindableCorrespondence c2) { return c2.Key.CompareTo(c1.Key); });

            gridHistory.DataSource = bindableCorrespondence;
            gridHistory.DataBind();
        }

        #endregion

        protected void gridHistory_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {        
            if (e.DataColumn.Caption == "Details")
            {
                BindableCorrespondence correspondence = gridHistory.GetRow(e.VisibleIndex) as BindableCorrespondence;

                if (correspondence != null && correspondence.HasDetail == false)
                    e.Cell.Controls[0].Visible = false;
            }
        }

        protected void callbackPanel_Callback(object source, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            onCallback(e.Parameter, null);           
        }
    }
}
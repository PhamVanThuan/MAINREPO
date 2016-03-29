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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life
{
    public partial class LifeNTU : SAHLCommonBaseView,ILifeNTU
    {
        private bool _cancelButtonVisible;
        private int _selectedReasonDefinitionKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ShouldRunPage) 
                return;

            btnCancel.Visible = _cancelButtonVisible;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlNTUReason.SelectedItem.Value != "-select-")
                _selectedReasonDefinitionKey = Convert.ToInt32(ddlNTUReason.SelectedItem.Value);
            else
                _selectedReasonDefinitionKey = -1;

            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        #region ILifeNTU Members

        public event EventHandler OnSubmitButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public bool CancelButtonVisible
        {
            set { _cancelButtonVisible = value; }
        }

        public int SelectedReasonDefinitionKey
        {
            get { return _selectedReasonDefinitionKey; }
            set { _selectedReasonDefinitionKey = value; }
        }
	
        public void BindReasonDefinitions(IDictionary<int, string> reasonDefinitions)
        {
            ddlNTUReason.DataSource = reasonDefinitions;
            ddlNTUReason.DataValueField = "Key";
            ddlNTUReason.DataTextField = "Description";
            ddlNTUReason.DataBind();
        }

        #endregion
    }
}

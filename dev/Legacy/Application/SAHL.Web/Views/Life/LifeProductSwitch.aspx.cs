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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life
{
    public partial class LifeProductSwitch : SAHLCommonBaseView, ILifeProductSwitch
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Properties

        public bool SubmitButtonVisible
        {
            set { btnSubmit.Visible = value; }
        }
        
        public int PolicyTypeSelectedValue
        {
            set
            {
                ddlPolicyType.SelectedValue = value.ToString();
            }
            get
            {
                if (PolicyTypeSelectedIndexChanged != null && ddlPolicyType.SelectedValue != null && ddlPolicyType.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlPolicyType.SelectedValue);
                else
                    return -1;
            }
        }

        #endregion

        #region Methods
                
        public void BindLifePolicyTypes(IEventList<ILifePolicyType> lifePolicyTypes)
        {
            ddlPolicyType.DataSource = lifePolicyTypes;
            ddlPolicyType.DataValueField = "Key";
            ddlPolicyType.DataTextField = "Description";
            ddlPolicyType.DataBind();
        }

        #endregion

        #region Events

        public event EventHandler OnSubmitButtonClicked;
        public event EventHandler OnCancelButtonClicked;
        public event KeyChangedEventHandler PolicyTypeSelectedIndexChanged;

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        protected void ddlPolicyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PolicyTypeSelectedIndexChanged != null && ddlPolicyType.SelectedValue != null && ddlPolicyType.SelectedValue != "-select-")
                PolicyTypeSelectedIndexChanged(sender, new KeyChangedEventArgs(ddlPolicyType.SelectedValue));
        }


        #endregion

    }
}

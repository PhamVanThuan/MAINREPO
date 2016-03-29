using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common
{
    public partial class WorkflowEmploymentTypeConfirmation : SAHLCommonBaseView, IWorkflowEmploymentTypeConfirmation
    {
        public event EventHandler OnConfirmButtonClicked
        {
            add { this.btnConfirm.Click += value; }
            remove { this.btnConfirm.Click -= value; }
        }

        public event EventHandler OnCancelButtonClicked
        {
            add { this.btnCancel.Click += value; }
            remove { this.btnCancel.Click -= value; }
        }

        public int SelectedEmploymentTypeKey
        {
            get { return GetSelectedValueFrom(this.ddlSelectedEmploymentType); }
        }

        public void BindEmploymentTypes(IDictionary<int, string> employmentTypes)
        {
            ddlSelectedEmploymentType.DataSource = employmentTypes;
            ddlSelectedEmploymentType.DataValueField = "Key";
            ddlSelectedEmploymentType.DataTextField = "Description";
            ddlSelectedEmploymentType.DataBind();
        }

        private int GetSelectedValueFrom(SAHLDropDownList dropDownList)
        {
            if (!string.IsNullOrEmpty(Request.Form[dropDownList.UniqueID]) && Request.Form[dropDownList.UniqueID] != "-select-")
                return (Convert.ToInt32(Request.Form[dropDownList.UniqueID]));
            else if (!string.IsNullOrEmpty(dropDownList.SelectedValue) && dropDownList.SelectedValue != "-select-")
                return (Convert.ToInt32(dropDownList.SelectedValue));
            else
                return -1;
        }
    }
}
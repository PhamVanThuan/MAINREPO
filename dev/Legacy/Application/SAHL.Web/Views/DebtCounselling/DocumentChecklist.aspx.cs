using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using System.Data;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.AJAX;
using SAHL.Common.DomainMessages;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class DocumentChecklist : SAHLCommonBaseView, IDocumentChecklist
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(dteNewDate.Text) || !dteNewDate.DateIsValid)
            {
                Messages.Add(new Error("Please enter a valid date", "Please enter a valid date"));
                return;
            }

            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        #region IDocumentChecklist Members

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnSubmitButtonClicked;

        public void BindDateGrid(List<BindableDCItem> dates)
        {
            grdDate.Columns.Clear();

            grdDate.KeyFieldName = "Description";

            grdDate.AddGridColumn("Description", "Type", 20, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdDate.AddGridColumn("Date", "Date", 20, GridFormatType.GridString, SAHL.Common.Constants.DateFormat, HorizontalAlign.Right, true, true);
            grdDate.AddGridColumn("Detail", "Comments", 60, GridFormatType.GridString, "", HorizontalAlign.Left, true, true);
            grdDate.AddGridColumn("Type", "DateType", 10, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);
            grdDate.AddGridColumn("CanSave", "CanSave", 1, GridFormatType.GridString, "", HorizontalAlign.Left, false, true);

            grdDate.DataSource = dates;
            grdDate.DataBind();
        }

        public string Comment
        {
            get
            {
                return txtComments.Text;
            }
            set
            {
                txtComments.Text = value;
            }
        }

        public DateTime? NewDate
        {
            get
            {
                if (dteNewDate.DateIsValid)
                    return dteNewDate.Date;

                return null;
            }
            set
            {
                dteNewDate.Date = value;
            }
        }

        public DCItemType ItemType
        {
            get
            {
                if (!String.IsNullOrEmpty(txtType.Text))
                    return (DCItemType)Enum.Parse(typeof(DCItemType), txtType.Text);

                return DCItemType.NONE;
            }
            set
            {
                txtType.Text = ((int)value).ToString();
            }
        }

        #endregion
    }
}
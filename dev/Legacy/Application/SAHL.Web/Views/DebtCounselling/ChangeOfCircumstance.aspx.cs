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

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class ChangeOfCircumstance : SAHLCommonBaseView, IChangeOfCircumstance
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!ShouldRunPage)
                return;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!dte17point3.Date.HasValue || !dte17point3.DateIsValid)
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

        #region IChangeOfCircumstance Members

        public event EventHandler OnCancelButtonClicked;

        public event EventHandler OnSubmitButtonClicked;

        public string Comment
        {
            get { return txtComments.Text; }
        }

        public DateTime? Date
        {
            get
            {
                if (dte17point3.Date.HasValue && dte17point3.DateIsValid)
                    return dte17point3.Date.Value;

                return null;
            }
        }

        #endregion
    }
}
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Comcorp.Interfaces;
using System;
using System.Web;

namespace SAHL.Web.Views.Comcorp
{
    /// <summary>
    /// SendFeedback View
    /// </summary>
    public partial class SendFeedback : SAHLCommonBaseView, ISendFeedback
    {
        public string EventComment
        {
            get
            {
                return HttpUtility.HtmlEncode(txtComment.Text);
            }
        }

        public bool SubmitButtonEnabled
        {
            set
            {
                SubmitButton.Enabled = value;
            }
        }

        public event EventHandler SubmitButtonClicked;

        public event EventHandler CancelButtonClicked;

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (CancelButtonClicked != null)
                CancelButtonClicked(sender, e);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (SubmitButtonClicked != null)
                SubmitButtonClicked(sender, e);
        }
    }
}
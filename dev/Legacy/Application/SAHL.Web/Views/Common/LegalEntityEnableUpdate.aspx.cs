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
using SAHL.Common.Authentication;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common
{
    public partial class LegalEntityEnableUpdate : SAHLCommonBaseView, ILegalEntityEnableUpdate
    {
        #region Private members

        string _labelMessage = String.Empty;
        string _labelQuestion = String.Empty;
        string _cancelButtonText = String.Empty;
        string _submitButtonText = String.Empty;

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!ShouldRunPage)
                return;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage)
                return;

            labelMessage.Text = _labelMessage;
            labelQuestion.Text = _labelQuestion;
            btnCancelButton.Text = _cancelButtonText;
            btnSubmitButton.Text = _submitButtonText;
        }

         protected void btnCancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClick(sender, e);
        }

        protected void btnSubmitButton_Click(object sender, EventArgs e)
        {
            OnSubmitButtonClick(sender, e);
        }

        #region ILegalEntityEnableUpdate Members

        public void BindLabelMessage(string LabelMessage)
        {
            _labelMessage = LabelMessage;
        }

        public void BindLabelQuestion(string LabelQuestion)
        {
            _labelQuestion = LabelQuestion;
        }

        public void BindCancelButtonText(string CancelButtonText)
        {
            _cancelButtonText = CancelButtonText;
        }

        public void BindSubmitButtonText(string SubmitButtonText)
        {
            _submitButtonText = SubmitButtonText;
        }

        public event EventHandler OnSubmitButtonClick;

        public event EventHandler OnCancelButtonClick;

        #endregion
    }
}
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using SAHL.Common.Web.UI;


namespace SAHL.Web.Views.Capitec
{
    /// <summary>
    /// ContactClient View
    /// </summary>
    public partial class ContactClient : SAHLCommonBaseView, SAHL.Web.Views.Capitec.Interfaces.IContactClient
    {
        private string _comments;
        private const int MemoCharlimit = 7000;

        public DateTime? ContactDate
        {
            get
            {
                return dteContactDate.Date;
            }
            set
            {
                dteContactDate.Date = value;
            }
        }

        public string Comments
        {
            get
            {
                _comments = txtComments.Text;
                if (_comments.Length > MemoCharlimit)
                    _comments = _comments.Substring(0, MemoCharlimit);
                return _comments;
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



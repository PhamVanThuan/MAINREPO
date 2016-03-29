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
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using System.Collections.Generic;

namespace SAHL.Web.Views.Reports
{
    public partial class InterestRateReview : SAHLCommonBaseView, IInterestRateReview
    {

        #region Private Variables

        //private const string cReportPath = "/SAHL/Serv.LS.Interest Rate Review";
       // private IReportStatement _reportStatement;
        private string _requestString;
        private Dictionary<IReportParameter, object> _lstParameters;


        //        ReportController m_MyController = null;

        #endregion

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            IReportRepository rr = RepositoryFactory.GetRepository<IReportRepository>();
            if (txtAccountNumber.Text.Length > 0)
            {
                _lstParameters = new Dictionary<IReportParameter, object>();
                _requestString += "&AccountKey=" + txtAccountNumber.Text;


                IReportParameter rp = rr.CreateReportParameter();
                rp.DisplayName = "Account Key";
                rp.ParameterName = "AccountKey";
                _lstParameters.Add(rp, txtAccountNumber.Text);


                _requestString += "&NewLinkRate=" + Request.Form[ddlLinkRate.UniqueID];

                IReportParameter rp2 = rr.CreateReportParameter();
                rp2.DisplayName = "New Link Rate";
                rp2.ParameterName = "NewLinkRate";
                _lstParameters.Add(rp2, Request.Form[ddlLinkRate.UniqueID]);
            }
            OnSubmitButtonClicked(sender, e);
        }


        //private void bindCombos()
        //{
        //if (Request.Form[txtAccountNumber.UniqueID] != null && Request.Form[txtAccountNumber.UniqueID].Length > 0)
        //{

        //    int p_AccountKey = int.Parse(Request.Form[txtAccountNumber.UniqueID]);

        //    List<string[]> l = m_MyController.GetLinkRatesByAccountKey(p_AccountKey, base.GetClientMetrics());

        //    ddlLinkRate.Items.Clear();
        //    ddlLinkRate.VerifyPleaseSelect();

        //    foreach (string[] s in l)
        //    {
        //        ddlLinkRate.Items.Add(new ListItem(s[1], s[2]));
        //    }

        //    ddlLinkRate.Enabled = true;
        //}
        //else
        //{
        //    ddlLinkRate.Enabled = false;
        //}
        //}
     
        protected void txtAccountNumber_TextChanged(object sender, EventArgs e)
        {

        }
        protected void NextButton_Click(object sender, EventArgs e)
        {
            ICommonRepository cr = RepositoryFactory.GetRepository<ICommonRepository>();
            if (txtAccountNumber.Text.Length > 0)
            {
                Dictionary<int, string> lstlinkRates = cr.GetLinkRatesByAccountKey(int.Parse(txtAccountNumber.Text));
                ddlLinkRate.DataSource = lstlinkRates;
                ddlLinkRate.DataBind();
            }


        }

        protected void SAHLTextBox1_TextChanged(object sender, EventArgs e)
        {

        }



        #region IInterestRateReview Members

        public event EventHandler OnSubmitButtonClicked;

        //public IReportStatement ReportStatement
        //{
        //    set { _reportStatement = value; }
        //}

        public string RequestString
        {
            get { return _requestString; }
        }
        public IDictionary<IReportParameter, object> lstParameters
        {
            get { return _lstParameters; }
        }
       
        public event EventHandler OnCancelButtonClicked;

        #endregion
    }
}


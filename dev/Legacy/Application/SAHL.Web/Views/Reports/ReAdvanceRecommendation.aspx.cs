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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Reports.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Reports
{
    public partial class ReAdvanceRecommendation : SAHLCommonBaseView, IReAdvanceRecommendation
    {
        #region Private Variables

        private const string cReportPath = "/SAHL/Serv.LS.Readvance Recommendation";
        private IDictionary<IReportParameter, object> _lstParameters;

        //private IReportStatement _reportStatement;

        #endregion



        protected void CancelButton_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            //if (txtReadvance.Text.Length == 0)
            //    valReadvance.IsValid = false;
            //if (ddlLinkRate.SelectedIndex <= 0)
            //    valLinkRate.IsValid = false;

            IReportRepository rr = RepositoryFactory.GetRepository<IReportRepository>();

            if (txtAccountNumber.Text.Length > 0)
            {
                if (txtReadvance.Text.Length > 0)
                {
                    _lstParameters = new Dictionary<IReportParameter, object>();

                    IReportParameter rp = rr.CreateReportParameter();
                    rp.DisplayName = "Account Key";
                    rp.ParameterName = "AccountKey";
                    _lstParameters.Add(rp, txtAccountNumber.Text);

                    IReportParameter rp2 = rr.CreateReportParameter();
                    rp2.DisplayName = "New Link Rate";
                    rp2.ParameterName = "NewLinkRate";
                    _lstParameters.Add(rp2, Request.Form[ddlLinkRate.UniqueID]);

                    IReportParameter rp3 = rr.CreateReportParameter();
                    rp3.DisplayName = "Readvance";
                    rp3.ParameterName = "Readvance";
                    _lstParameters.Add(rp3, txtReadvance.Text);
                    OnSubmitButtonClicked(sender, e);
                }


            }
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

        //private void bindCombos()
        //{
        //    if (Request.Form[txtAccountNumber.UniqueID] != null && Request.Form[txtAccountNumber.UniqueID].Length > 0)
        //    {

        //        int p_AccountKey = int.Parse(Request.Form[txtAccountNumber.UniqueID]);

        //        List<string[]> l = m_MyController.GetLinkRatesByAccountKey(p_AccountKey, base.GetClientMetrics());

        //        ddlLinkRate.Items.Clear();
        //        ddlLinkRate.VerifyPleaseSelect();

        //        foreach (string[] s in l)
        //        {
        //            ddlLinkRate.Items.Add(new ListItem(s[1], s[2]));
        //        }

        //        ddlLinkRate.Enabled = true;
        //        txtReadvance.Enabled = true;
        //    }
        //    else
        //    {
        //        ddlLinkRate.Enabled = false;
        //        txtReadvance.Enabled = false;
        //    }
        //}


        #region IReAdvanceRecommendationReport Members

        public event EventHandler OnSubmitButtonClicked;

        //public IReportStatement ReportStatement
        //{
        //    set { _reportStatement = value; }
        //}



        public IDictionary<IReportParameter, object> lstParameters
        {
            get { return _lstParameters; }
        }

        public event EventHandler OnCancelButtonClicked;

        #endregion
    }

}

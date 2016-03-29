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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class SantamPolicyDetail : SAHLCommonBaseView, ISantamPolicy
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindPolicyDetails(ISANTAMPolicyTracking PolicyDetails)
        {
            pnlPolicyDisplay.Visible = true;
            pnlPolicyNone.Visible = false;

            lblLoanNumber.Text = PolicyDetails.AccountKey.ToString();
            lblQuoteReference.Text = PolicyDetails.QuoteNumber;
            lblPolicyStatus.Text = PolicyDetails.SANTAMPolicyStatus.Description;
            lblPolicyReference.Text = PolicyDetails.PolicyNumber;
            lblMonthlyPremium.Text = "R " + PolicyDetails.MonthlyPremium.ToString(SAHL.Common.Constants.NumberFormat);
            lblDebitOrderDate.Text = AddOrdinal(PolicyDetails.CollectionDay);

            lblOpenDate.Text = PolicyDetails.ActiveDate.ToString(SAHL.Common.Constants.DateFormat);
            lblCloseDate.Text = PolicyDetails.Canceldate.ToString(SAHL.Common.Constants.DateFormat);


        }

        public static string AddOrdinal(int num)
        {
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num.ToString() + "th";
            }
            switch (num % 10)
            {
                case 1:
                    return num.ToString() + "st";
                case 2:
                    return num.ToString() + "nd";
                case 3:
                    return num.ToString() + "rd";
                default:
                    return num.ToString() + "th";
            }
        }

        public void DisplayNoPolicy()
        {
            pnlPolicyDisplay.Visible = false;
            pnlPolicyNone.Visible = true;
        }
    }
}

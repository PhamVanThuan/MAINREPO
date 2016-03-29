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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common
{
    public partial class PostTransaction : SAHLCommonBaseView, IPostTransaction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPost_Click(object sender, EventArgs e)
        {
            OnPostButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
            //Navigator.Navigate("LoanTransactionSummary");
        }

        #region IPostTransaction Members

        public event EventHandler OnPostButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public event KeyChangedEventHandler onSelectedTransctionTypeChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransactionTypes"></param>
        public void BindTransactionTypes(IReadOnlyEventList<ITransactionType> TransactionTypes)
        {
            ddlTransactionType.DataTextField = "Description";
            ddlTransactionType.DataValueField = "Key";
            ddlTransactionType.DataSource = TransactionTypes;
            ddlTransactionType.DataBind();
        }

        public void BindFinancialServiceTypes(Dictionary<string,string> dictFinancialServices)
        {
            ddlFinancialService.PleaseSelectItem = dictFinancialServices.Count > 1 ? true : false;
            ddlFinancialService.DataTextField = "Value";
            ddlFinancialService.DataValueField = "Key";
            ddlFinancialService.DataSource = dictFinancialServices;
            ddlFinancialService.DataBind();
        }

        public void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onSelectedTransctionTypeChanged != null)
                onSelectedTransctionTypeChanged(sender, new KeyChangedEventArgs(ddlTransactionType.SelectedValue));
        }

        #region Properties
        public int TransactionType
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlTransactionType.SelectedValue) && ddlTransactionType.SelectedValue != "-select-")
                    return Convert.ToInt16(ddlTransactionType.SelectedValue);
                else
                    return -1;
            }
        }

        public int SelectedFinancialServiceKey
        {
            get
            {
                if (!string.IsNullOrEmpty(ddlFinancialService.SelectedValue) && ddlFinancialService.SelectedValue != "-select-")
                    return Convert.ToInt32(ddlFinancialService.SelectedValue);
                else
                    return -1;
            }
        }

        public bool FinancialServicesVisible
        {
            set { financialServiceRow.Visible = value; }
        }

        public DateTime EffectiveDate
        {
            get { return dteEffectiveDate.Date != null ? dteEffectiveDate.Date.Value : new DateTime() ; }
        }

        public double Amount
        {
            get { return (curAmount.Amount.HasValue ? curAmount.Amount.Value : 0); }
        }

        public string Reference
        {
            get { return txtReference.Text; }
        }

        #endregion

        #endregion
    }
}

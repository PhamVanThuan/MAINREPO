//* Program Amendments
// 
// Date         User            TRAC    Description
// ----------   ------------    ----    -----------
// 12/01/2007   Craig Fraser    3458    When cancelling a policy we have to call the stored procedure to
//                                      perform the cancellation and create the fin transactions if required.
//                                      Added a call to m_Controller.CancelLifePolicy.
// 29/01/2007   Craig Fraser    3860    If the cancellation date is more than 30days after the policy acceptance  date then 
//                                      disable the "Cancel from Inception (within 30 days)" radio  button
//                              3889    Removed hard-coding of OriginationSourceProductKey when finding the report to use on the
//                                      Correspondence screen
// 26/04/2007   Craig Fraser    4661    Allow Cancellation of Accepted policies.
//                                      Only allow "admin" users to perform the cancellation
//*
//*

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
using System.Text;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Life
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CancelPolicy : SAHLCommonBaseView, ICancelPolicy
    {
        private int _daysSinceAcceptedDate;
        private bool _policyHasCommenced;
        private bool _lifeIsConditionOfLoan;
        private IDictionary<string, int> _cancellationTypes;

        private bool _cancelFromInceptionEnabled; 
        private bool _cancelWithAuthorizationEnabled; 
        private bool _cancelWithProRateEnabled; 
        private bool _cancelWithNoRefundEnabled; 
        private int _cancellationTypeKey = -1;
        private string _selectedReason;
        private bool _adminUser;
        private string _cancellationDisabledMessage;
        private bool _cancellationLetterReceived;

        //•	Ensure that this cover can not be cancelled if this is the condition
        //If Life Cover is a condition of the loan, the policy may not be cancelled without authorization from SAHL Credit Dept.
        //    •	A client can cancel a policy at any stage before the loan registration has completed, at no charge.
        //o	Letter to be sent to SAHL Life Admin by the client. SAHL Life Admin to process cancellation. 
        //o	Letter sent to client confirming cancellation.
        //•	Cancellation within 30 days of acceptance. 
        //o	In this case all premiums will be refunded. 
        //o	To cancel the client must fax a cancellation letter to SAHL Life administration. The letter must be signed by the person who originally applied for the policy. Life Administration will update the life system, which will automatically process the refund if applicable. 
        //o	A letter will be sent to the client confirming that the policy has been cancelled.
        //•	A client can cancel a policy at any stage after 30 days from acceptance of the policy and receive a pro-rata refund.
        //o	A client may cancel his policy during the term of the policy provided it was not a condition of grant by SA Home Loans. 
        //o	The policy will be cancelled with effect from the end of the month in which the cancellation is requested and a prorate refund will be done and paid into the bond. 
        //o	To cancel the client must fax a cancellation letter to SAHL Life administration. The letter must be signed by the person who originally applied for the policy.  Life Administration will update the life system, which will automatically process the refund if applicable. 
        //o	A letter will be sent to the client confirming that the policy has been cancelled.
        //•	Management-authorised refund if requested.
        //o	An option must exist for a policy to be cancelled from inception and a full refund processed irrespective of how long the policy has been in force. In this case the requirements are as above, but the cancellation must be authorised by Management. If Management decline then a prorate refund will be processed.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            if (!this.ShouldRunPage)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!this.ShouldRunPage)
                return;

            lblDays.Text = _daysSinceAcceptedDate.ToString();
            lblCommenced.Text = _policyHasCommenced == true ? "Yes" : "No";
            lblCondition.Text = _lifeIsConditionOfLoan == true ? "Yes" : "No";

            if (!String.IsNullOrEmpty(_cancellationDisabledMessage))
            {
                lblError.Text = _cancellationDisabledMessage;
                lblError.Visible = true;

                btnSubmit.Visible = false;
                ddlReason.Enabled = false;
                chkLetter.Enabled = false;
            }

            rblType.Items[0].Enabled = _cancelFromInceptionEnabled;
            rblType.Items[1].Enabled = _cancelWithAuthorizationEnabled;
            rblType.Items[2].Enabled = _cancelWithProRateEnabled;
            rblType.Items[3].Enabled = _cancelWithNoRefundEnabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // set the cancellation type
            if (rblType.SelectedIndex > -1)
            {
                foreach (KeyValuePair<string, int> kv in _cancellationTypes)
                {
                    if (kv.Key.ToString() == rblType.SelectedItem.Text)
                    {
                        _cancellationTypeKey = kv.Value;
                        break;
                    }
                }
            }

            // set the reason type
            _selectedReason = ddlReason.SelectedItem.Text;

            _cancellationLetterReceived = chkLetter.Checked;

            OnSubmitButtonClicked(sender,e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        #region ICancelPolicy Members

        /// <summary>
        /// Implements ICancelPolicy.OnCancelButtonClicked
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Implements ICancelPolicy.OnSubmitButtonClicked
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// Implements ICancelPolicy.BindControls
        /// </summary>
        /// <param name="cancellationTypes"></param>
        /// <param name="reasons"></param>
        /// <param name="daysSinceAcceptedDate"></param>
        /// <param name="policyHasCommenced"></param>
        /// <param name="lifeIsConditionOfLoan"></param>
        public void BindControls(IDictionary<string, int> cancellationTypes, IDictionary<int, string> reasons, int daysSinceAcceptedDate, bool policyHasCommenced, bool lifeIsConditionOfLoan)
        {
            _cancellationTypes = cancellationTypes;
            _daysSinceAcceptedDate = daysSinceAcceptedDate;
            _policyHasCommenced = policyHasCommenced;
            _lifeIsConditionOfLoan = lifeIsConditionOfLoan;
            
            ddlReason.DataSource = reasons;
            ddlReason.DataBind();
            rblType.Items.Clear();
            rblType.DataSource = cancellationTypes;
            rblType.DataTextField = "Key";
            rblType.DataValueField = "Value";
            rblType.DataBind();
        }


        public string CancellationDisabledMessage
        {
            get { return _cancellationDisabledMessage; }
            set { _cancellationDisabledMessage = value; }
        }

        /// <summary>
        /// Implements ICancelPolicy.CancelFromInceptionEnabled
        /// </summary>
        public bool CancelFromInceptionEnabled
        {
            set { _cancelFromInceptionEnabled = value; }
        }

        /// <summary>
        /// Implements ICancelPolicy.CancelWithAuthorizationEnabled
        /// </summary>
        public bool CancelWithAuthorizationEnabled
        {
            set { _cancelWithAuthorizationEnabled = value; }
        }

        /// <summary>
        /// Implements ICancelPolicy.CancelWithProRateEnabled
        /// </summary>
        public bool CancelWithProRateEnabled
        {
            set { _cancelWithProRateEnabled = value; }
        }

        /// <summary>
        /// Implements ICancelPolicy.CancelWithNoRefundEnabled
        /// </summary>
        public bool CancelWithNoRefundEnabled
        {
            set { _cancelWithNoRefundEnabled = value; }
        }

        public bool CancellationLetterReceived
        {
            get { return _cancellationLetterReceived; }
        }
	
        /// <summary>
        /// 
        /// </summary>
        public int CancellationTypeKey
        {
            get { return _cancellationTypeKey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedReason
        {
            get { return _selectedReason; }
            set { _selectedReason = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool AdminUser
        {
            get { return _adminUser; }
            set { _adminUser = value; }
        }

        #endregion
    }
}
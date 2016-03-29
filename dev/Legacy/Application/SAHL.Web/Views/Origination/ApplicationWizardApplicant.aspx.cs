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
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Web.AJAX;

namespace SAHL.Web.Views.Origination
{
    public partial class ApplicationWizardApplicant : SAHLCommonBaseView, IApplicationWizardApplicant
    {
        private ILegalEntityRepository _leRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage)
                return;
            else
                base.RegisterWebService(ServiceConstants.LegalEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void acNatAddIDNumber_ItemSelected(object sender, KeyChangedEventArgs e)
        {
            GetLegalEntityAndDisplay(Convert.ToInt32(e.Key));           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leKey"></param>
        private void GetLegalEntityAndDisplay(int leKey)
        {
            //populate the legalentity
            ILegalEntityNaturalPerson le = (ILegalEntityNaturalPerson)LERepo.GetLegalEntityByKey(leKey);

            if (le != null)
            {
                if(le.FirstNames != null)
                    if (le.FirstNames.Trim().Length > 0)
                    {
                        lblFirstNames.Text = le.FirstNames;
                        lblFirstNames.Visible = true;
                        txtFirstNames.Attributes.Add("style", "display:none");
                    }
                if (le.Surname != null)
                {
                    if (le.Surname.Trim().Length > 0)
                    {
                        lblSurname.Text = le.Surname;
                        lblSurname.Visible = true;
                        txtSurname.Attributes.Add("style", "display:none");
                    }
                }
                txtIDNumber.Text = le.IDNumber;
                //lblContact.Text = le.HomePhoneCode + " " + le.HomePhoneNumber;
                //lblContact.Visible = true;
                phContact.Code = le.HomePhoneCode;
                phContact.Number = le.HomePhoneNumber;
                tbLegalEntityKey.Text = le.Key.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private ILegalEntityRepository LERepo
        {
            get
            {
                if (_leRepo == null)
                    _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCancel_Click(object sender, EventArgs e)
        {
            OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnNext_Click(object sender, EventArgs e)
        {
            OnNextButtonClicked(sender, e);
        }

        #region IApplicationWizardApplicant Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationSource"></param>
        public void PopulateMarketingSource(SAHL.Common.Collections.Interfaces.IEventList<IApplicationSource> applicationSource)
        {
            ddlMarketingSource.DataTextField = "Description";
            ddlMarketingSource.DataValueField = "Key";
            ddlMarketingSource.DataSource = applicationSource;
            ddlMarketingSource.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public int ExistingLegalEntityKey
        {
            get
            {
                if (tbLegalEntityKey.Text.Length > 0)
                    return Convert.ToInt32(tbLegalEntityKey.Text);

                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LEFirstNames
        {
            get { return txtFirstNames.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LESurname
        {
            get { return txtSurname.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LEIDNumber
        {
            get { return txtIDNumber.Text; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PhoneCode
        {
            get
            {
                return phContact.Code;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                return phContact.Number;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NumberOfApplicants
        {
            get
            {
                if (tbNumApplicants.Text.Length > 0)
                    return Convert.ToInt32(tbNumApplicants.Text);

                return 0;
            }
            set
            {
                tbNumApplicants.Text = value.ToString();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string MarketingSource
        {
            get
            {
                if (ddlMarketingSource.SelectedValue == "-select-")
                    return String.Empty;

                return ddlMarketingSource.SelectedValue;
            }
            set
            {
                for (int x = 0; x < ddlMarketingSource.Items.Count; x++)
                {
                    if (ddlMarketingSource.Items[x].Value == value)
                    {
                        ddlMarketingSource.SelectedIndex = x;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="application"></param>
        public void BindExistingLegalEntityAndApplication(ILegalEntity legalEntity, IApplication application)
        {
            if (IsPostBack)
                return;

            ILegalEntityNaturalPerson le = legalEntity as ILegalEntityNaturalPerson;
            txtFirstNames.Text = le.FirstNames;
            txtIDNumber.Text = le.IDNumber;
            txtSurname.Text = le.Surname;
            phContact.Code = le.HomePhoneCode;
            phContact.Number = le.HomePhoneNumber;
            if (application != null)
            {
                tbNumApplicants.Text = application.EstimateNumberApplicants.Value.ToString();
                if (application.ApplicationSource != null)
                {
                    for (int x = 0; x < ddlMarketingSource.Items.Count; x++)
                    {
                        if (ddlMarketingSource.Items[x].Value == application.ApplicationSource.Key.ToString())
                        {
                            ddlMarketingSource.SelectedIndex = x;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnBackButtonClicked;

        protected void OnCalculate_Click(object sender, EventArgs e)
        {
            OnBackButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowBackButton
        {
            set { btnBack.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowCancelButton
        {
            set { btnCancel.Visible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEstateAgentApplication
        {
            get { return chkEstateAgent.Checked; }
            set 
            { 
                //if this has been set, the X2 case has been created and this can not be switched off
                chkEstateAgent.Enabled = false;
                chkEstateAgent.Checked = value; 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsOldMutualDeveloperLoan
        {
            get { return chkDevelopmentLoan.Checked; }
            set
            {
                ////if this has been set, the X2 case has been created and this can not be switched off
                //chkDevelopmentLoan.Enabled = false;
                chkDevelopmentLoan.Checked = value;
            }
        }

        #endregion
    }
}

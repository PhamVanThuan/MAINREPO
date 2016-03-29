using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Origination.Interfaces;

namespace SAHL.Web.Views.Origination
{
    public partial class ApplicationAttributes : SAHLCommonBaseView, IApplicationAttributes
    {
        ILookupRepository _lookups;

        protected void Page_Load(object sender, EventArgs e)
        {
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        public bool ShowButtons
        {
            set
            {
                btnCancel.Visible = value;
                btnUpdate.Visible = value;
            }
        }

        public void showControlsForDisplay()
        {
            chkAttributes.Enabled = false;
            TxtTransferAttorney.Visible = false;
            ddlMarketingSource.Visible = false;
        }

        public void showControlsForUpdate()
        {
            lblTransferringAttorney.Visible = false;
            lblMarketingSource.Visible = false;
        }

        public void PopulateAttributes(IEventList<IApplicationAttributeType> applicationAttributes)
        {
            chkAttributes.DataTextField = "Description";
            chkAttributes.DataValueField = "Key";
            chkAttributes.DataSource = applicationAttributes;
            chkAttributes.DataBind();

            for (int i = 0; i < applicationAttributes.Count(); i++)
			{
                chkAttributes.Items[i].Enabled = applicationAttributes[i].UserEditable;
			}
        }

        public void PopulateMarketingSource(IEventList<IApplicationSource> applicationSource)
        {
            ddlMarketingSource.DataTextField = "Description";
            ddlMarketingSource.DataValueField = "Key";
            ddlMarketingSource.DataSource = applicationSource;
            ddlMarketingSource.DataBind();
        }

        public void BindApplication(IApplicationMortgageLoan application)
        {
            if (application.ApplicationAttributes != null)
            {
                for (int i = 0; i < application.ApplicationAttributes.Count; i++)
                {
                    if (application.ApplicationAttributes[i].ApplicationAttributeType.IsGeneric)
                    {
                        ListItem itemToCheck = chkAttributes.Items.FindByValue(application.ApplicationAttributes[i].ApplicationAttributeType.Key.ToString());
                        if (itemToCheck != null)
                        {
                            itemToCheck.Selected = true;
                        }
                    }
                }
            }

            if (application.ApplicationSource != null)
            {
                ddlMarketingSource.SelectedValue = application.ApplicationSource.Key.ToString();
                lblMarketingSource.Text = application.ApplicationSource.Description;
            }

            TxtTransferAttorney.Text = application.TransferringAttorney == null ? " " : application.TransferringAttorney;
            lblTransferringAttorney.Text = application.TransferringAttorney;
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (onCancelButtonClicked != null)
                onCancelButtonClicked(sender, e);
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (onUpdateButtonClicked != null)
                onUpdateButtonClicked(sender, e);
        }

        public IApplicationMortgageLoan GetUpdatedApplicationMortgageLoan(IApplicationMortgageLoan appMortgageLoan)
        {
            appMortgageLoan.TransferringAttorney = TxtTransferAttorney.Text;

            if (ddlMarketingSource.SelectedValue != "-select-")
                appMortgageLoan.ApplicationSource = _lookups.ApplicationSources.ObjectDictionary[ddlMarketingSource.SelectedValue];

            return appMortgageLoan;
        }

        public ListItemCollection GetAttributeOptions
        {
            get
            {
                return chkAttributes.Items;
            }
        }

        public event EventHandler onCancelButtonClicked;

        public event EventHandler onUpdateButtonClicked;
    }
}
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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common
{
    public partial class LegalAttorney : SAHLCommonBaseView, ILegalAttorney
    {
        // ILookupRepository lookups = RepositoryFactory.GetRepository<ILookupRepository>();

        /// <summary>
        /// Hide attorney panel when view is called in Registration User Mode
        /// </summary>
        public void HideAttorneyPanel()
        {
            pnlLegalAttorney.Visible = false;
            pnlPreferredAttorney.Visible = false;
            lblheader.Text = "Registration User";
        }
        /// <summary>
        /// Hide user panel when view is called in Registration Attorney Mode
        /// </summary>
        public void HideRegistrationUserPanel()
        {
            pnlLegalAmin.Visible = false;
            lblheader.Text = "Attorney";
        }
        /// <summary>
        /// Bind Registration users
        /// </summary>
        /// <param name="adUserLst"></param>
        public void BindRegistrationUsers(IEventList<IADUser> adUserLst)
        {
            ddlRegistrationAdmin.DataSource = adUserLst;
            ddlRegistrationAdmin.DataValueField = "Key";
            ddlRegistrationAdmin.DataTextField = "ADUserName";
            ddlRegistrationAdmin.DataBind();
        }
        /// <summary>
        /// Bind Conveyance Attorneys
        /// </summary>
        /// <param name="attorneys"></param>
        public void BindRegistrationAttorneys(IList<IAttorney> attorneys)
        {
            Dictionary<int, string>attorneyDict = new Dictionary<int, string>();
            if (attorneys != null)
            {
                for (int i = 0; i < attorneys.Count; i++)
                {
                    //TRAC 11245 change request
                    //The Attorney drop down must be populated with active registration attorneys 
                    //(where Attorneystatusnumber = 0 (0 is active on this table) 
                    //AND AttorneyRegistrationInd = 1 i.e.: listed as a Reg Attorney) 
					if (attorneys[i].GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active && attorneys[i].AttorneyRegistrationInd == true)
                        attorneyDict.Add(attorneys[i].Key, attorneys[i].LegalEntity.DisplayName);
                }
            }
            ddlRegistrationAttorney.Items.Clear();
            ddlRegistrationAttorney.DataSource = attorneyDict;
            ddlRegistrationAttorney.DataBind();
        }

        public void BindDeedsOffice(IEventList<IDeedsOffice> deedsOffice)
        {
            ddlDeedsOffice.DataValueField = "Key";
            ddlDeedsOffice.DataTextField = "Description";
            ddlDeedsOffice.DataSource = deedsOffice;
            ddlDeedsOffice.DataBind();
        }

		protected void ddlDeedsOffice_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (OnDeedsOfficeSelectedIndexChanged != null)
			{
				if (!String.IsNullOrEmpty(Request.Form[ddlDeedsOffice.UniqueID]) && Request.Form[ddlDeedsOffice.UniqueID] != "-select-")
					OnDeedsOfficeSelectedIndexChanged(this.Page, new KeyChangedEventArgs(Convert.ToInt32(ddlDeedsOffice.SelectedValue)));
			}
		}

        /// <summary>
        /// Update button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateButton_Click(object sender, EventArgs e)
        {
            if (OnUpdateButtonClicked != null)
                OnUpdateButtonClicked(sender, e);
        }
        /// <summary>
        /// Get Selected AD User
        /// </summary>
        public int GetADUserSelected
        {
            get
            { 
                return Convert.ToInt32(ddlRegistrationAdmin.SelectedValue); 
            }
        }
        /// <summary>
        /// Get Selected Attorney
        /// </summary>
        public int GetSetAttorneySelected
        {
            get
            {
				if (!String.IsNullOrEmpty(Request.Form[ddlRegistrationAttorney.UniqueID]) && Request.Form[ddlRegistrationAttorney.UniqueID] != "-select-")
                    return Convert.ToInt32(Request.Form[ddlRegistrationAttorney.UniqueID]);
                else
                    return -1;
            }

            set
            {
                ddlRegistrationAttorney.SelectedValue = value.ToString();
            }

        }
        /// <summary>
        /// Set Deeds Office of currently selected Attorney
        /// </summary>
        /// <param name="deedOfficeKey"></param>
        public void SetAttorneyDeedsOffice(int deedOfficeKey)
        {
            ddlDeedsOffice.SelectedValue = deedOfficeKey.ToString();
        }

        public void SetADUserSelected(int adUserKey)
        {
            ddlRegistrationAdmin.SelectedValue = adUserKey.ToString();
        }

        /// <summary>
        /// Event Handler for Update Button
        /// </summary>
        public event EventHandler OnUpdateButtonClicked;

        public event KeyChangedEventHandler OnDeedsOfficeSelectedIndexChanged;

    }
}

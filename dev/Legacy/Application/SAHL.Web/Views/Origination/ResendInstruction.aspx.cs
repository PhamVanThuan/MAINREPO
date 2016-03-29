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
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Origination
{
    public partial class ResendInstruction : SAHLCommonBaseView, IResendInstruction
    {
        ILookupRepository _lookups;
        IRegistrationRepository _regRepo;

        public void BindInstructionGrid(IList<IRegMail> regmail)
        {
            grdInstructions.AddGridBoundColumn("", "Deeds Office", Unit.Percentage(25), HorizontalAlign.Left, true);
            grdInstructions.AddGridBoundColumn("", "Attorney Instructed", Unit.Percentage(35), HorizontalAlign.Left, true);
            grdInstructions.AddGridBoundColumn("", "Instruction Date", Unit.Percentage(15), HorizontalAlign.Left, true);
            grdInstructions.AddGridBoundColumn("", "Instruction Status", Unit.Percentage(25), HorizontalAlign.Left, true);
            grdInstructions.DataSource = regmail;
            grdInstructions.DataBind();
        }

        public void SetLookups()
        {
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
        }

        public void SetUpdateableFields(IRegMail regmail)
        {
            ddlDeedsOffice.SelectedValue = _lookups.Attorneys.ObjectDictionary[regmail.AttorneyNumber.ToString()].DeedsOffice.Key.ToString();
            ddlRegistrationAttorney.SelectedValue = _lookups.Attorneys.ObjectDictionary[regmail.AttorneyNumber.ToString()].Key.ToString();
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

        protected void grdInstruction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            TableCellCollection cells = e.Row.Cells;
            IRegMail regmail = e.Row.DataItem as IRegMail;

            if (e.Row.DataItem != null)
           {
                _regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();
                IAttorney attorney = _regRepo.GetAttorneyByKey(Convert.ToInt32(regmail.AttorneyNumber));
                //
                cells[0].Text = attorney.DeedsOffice.Description;
                cells[1].Text = attorney.LegalEntity.DisplayName;
                cells[2].Text = Convert.ToDateTime(regmail.RegMailDateTime).ToString(SAHL.Common.Constants.DateFormat);
                cells[3].Text = _lookups.DetailTypes.ObjectDictionary[regmail.DetailTypeNumber.ToString()].Description;
            }
        }

        /// <summary>
        /// Get Selected Attorney
        /// </summary>
        public int GetAttorneySelected
        {
            get
            {
                if (Request.Form[ddlRegistrationAttorney.UniqueID] != null)
                    ddlRegistrationAttorney.SelectedValue = Request.Form[ddlRegistrationAttorney.UniqueID];

                return Convert.ToInt32(ddlRegistrationAttorney.SelectedValue);
            }
        }

        protected void ddlDeedsOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnDeedsOfficeSelectedIndexChanged != null)
                OnDeedsOfficeSelectedIndexChanged(this.Page, new KeyChangedEventArgs(Convert.ToInt32(ddlDeedsOffice.SelectedValue)));
        }

         /// <summary>
        /// Event Handler for Update Button
        /// </summary>
        public event EventHandler OnUpdateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSendInstructionClicked;
        
        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnDeedsOfficeSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendInstruction_Click(object sender, EventArgs e)
        {
            if (OnSendInstructionClicked != null)
                OnSendInstructionClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SetUpdateButtonEnabled
        {
            set { btnUpdateButton.Enabled = value; }
        }
    }
}

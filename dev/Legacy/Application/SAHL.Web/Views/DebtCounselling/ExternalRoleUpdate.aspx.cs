using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class ExternalRoleUpdate : SAHLCommonBaseView, IExternalRoleUpdate
    {

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;


        /// <summary>
        /// Handles the RowDataBound event of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDebtCounsel_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (NewLegalEntity != null)
                    e.Row.Cells[5].Text = NewLegalEntity.DisplayName;
                else
                    e.Row.Cells[5].Text = "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            Navigator.Navigate("Select");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage)
                return;

            if (NewLegalEntity != null)
                btnUpdate.Enabled = true;
        }

        #region IExternalRoleUpdate Members

        private ExternalRoleTypes _roleType;
        public ExternalRoleTypes RoleType
        {
            get
            {
                return _roleType;
            }
            set
            {
                _roleType = value;
            }
        }


        private ILegalEntity _newLE;
        public ILegalEntity NewLegalEntity
        {
            get
            {
                return _newLE;
            }
            set
            {
                _newLE = value;
            }
        }

        public void BindExternalRoleGrid(IList<BindableExternalRole> erList)
        {
            //    this.AddCheckBoxColumn("DoEnquiry", "Do Enquiry", true, Unit.Pixel(70), HorizontalAlign.Center, true);

            grdDebtCounsel.AddGridBoundColumn("DCKey", "DCKey", Unit.Empty, HorizontalAlign.Left, false);
            grdDebtCounsel.AddGridBoundColumn("AccountKey", "Account", Unit.Pixel(100), HorizontalAlign.Left, true);
            grdDebtCounsel.AddGridBoundColumn("DisplayName", "Legal Entity", Unit.Pixel(200), HorizontalAlign.Left, true);
			grdDebtCounsel.AddGridBoundColumn("CompanyName", "Company", Unit.Pixel(200), HorizontalAlign.Left, (RoleType == ExternalRoleTypes.DebtCounsellor));
			grdDebtCounsel.AddCheckBoxColumn("Update", "Update", true, Unit.Pixel(50), HorizontalAlign.Left, (NewLegalEntity != null));
			grdDebtCounsel.AddGridBoundColumn("DisplayName", "New Legal Entity", Unit.Pixel(200), HorizontalAlign.Left, (NewLegalEntity != null));

            grdDebtCounsel.DataSource = erList;
            grdDebtCounsel.DataBind();
        }

        public string GridHeader
        {
            get
            {
                return grdDebtCounsel.HeaderCaption;
            }
            set
            {
                grdDebtCounsel.HeaderCaption = value;
            }
        }
        
        public Dictionary<int, bool> GetCheckedItems
        {
            get
            {
                Dictionary<int, bool> dict = new Dictionary<int, bool>();
                foreach (GridViewRow row in grdDebtCounsel.Rows)
                {
                    CheckBox chkBox = (CheckBox)row.FindControl("Update");
                    dict.Add(Convert.ToInt32(row.Cells[0].Text), chkBox.Checked);
                }
                return dict;
            }
        }

        #endregion
    }
}
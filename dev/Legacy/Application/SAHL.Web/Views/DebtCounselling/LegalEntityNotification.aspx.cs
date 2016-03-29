using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;

namespace SAHL.Web.Views.DebtCounselling
{
    public partial class LegalEntityNotification : SAHLCommonBaseView, ILegalEntityNotification
    {
        public event EventHandler OnSubmitButtonClicked;
        public event EventHandler OnCancelButtonClicked;

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
                OnSubmitButtonClicked(sender, e);
        }

        public void BindGrid(IList<BindableLEReasonRole> erList)
        {
            grdLegalEntities.AddGridBoundColumn("Death", "isDeath", Unit.Empty, HorizontalAlign.Left, false);
            grdLegalEntities.AddGridBoundColumn("Sequestration", "isSeq", Unit.Empty, HorizontalAlign.Left, false);
            grdLegalEntities.AddGridBoundColumn("LEKey", "LEKey", Unit.Empty, HorizontalAlign.Left, false);
            grdLegalEntities.AddGridBoundColumn("DisplayName", "Name", Unit.Pixel(200), HorizontalAlign.Left, true);
            grdLegalEntities.AddGridBoundColumn("LEStatus", "Status", Unit.Pixel(100), HorizontalAlign.Left, true);
            grdLegalEntities.AddGridBoundColumn("Role", "Role", Unit.Pixel(200), HorizontalAlign.Left, true);
            grdLegalEntities.AddCheckBoxColumn("Death", "Death", !ReadOnlyAll, Unit.Pixel(50), HorizontalAlign.Left, UpdateDeath);
            grdLegalEntities.AddCheckBoxColumn("Sequestration", "Sequestration", !ReadOnlyAll, Unit.Pixel(50), HorizontalAlign.Left, UpdateSequestration);

            grdLegalEntities.DataSource = erList;
            grdLegalEntities.DataBind();
        }

        protected void grdLegalEntities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkBox = (CheckBox)e.Row.FindControl("Death");
                if (!string.IsNullOrEmpty(e.Row.Cells[0].Text) && String.Compare(e.Row.Cells[0].Text.Trim(), "true", true) == 0)
                {
                    if (chkBox != null)
                        chkBox.Checked = true;
                }
                if (chkBox != null && ReadOnlyAll)
                    chkBox.Enabled = false;


                chkBox = (CheckBox)e.Row.FindControl("Sequestration");
                if (!string.IsNullOrEmpty(e.Row.Cells[1].Text) && String.Compare(e.Row.Cells[1].Text.Trim(), "true", true) == 0)
                {
                    if (chkBox != null)
                        chkBox.Checked = true;
                }
                if (chkBox != null && ReadOnlyAll)
                    chkBox.Enabled = false;
            }
        }

        public string GridHeader
        {
            get
            {
                return grdLegalEntities.HeaderCaption;
            }
            set
            {
                grdLegalEntities.HeaderCaption = value;
            }
        }

        public Dictionary<int, bool> GetDeathItems
        {
            get
            {
                Dictionary<int, bool> dict = new Dictionary<int, bool>();
                foreach (GridViewRow row in grdLegalEntities.Rows)
                {
                    CheckBox chkBox = (CheckBox)row.FindControl("Death");
                    dict.Add(Convert.ToInt32(row.Cells[2].Text), chkBox.Checked);
                }
                return dict;
            }
        }

        public Dictionary<int, bool> GetSequestrationItems
        {
            get
            {
                Dictionary<int, bool> dict = new Dictionary<int, bool>();
                foreach (GridViewRow row in grdLegalEntities.Rows)
                {
                    CheckBox chkBox = (CheckBox)row.FindControl("Sequestration");
                    dict.Add(Convert.ToInt32(row.Cells[2].Text), chkBox.Checked);
                }
                return dict;
            }
        }

        private bool _readOnlyAll;
        public bool ReadOnlyAll
        {
            get
            {
                return _readOnlyAll;
            }
            set
            {
                _readOnlyAll = value;
                _updateDeath = value;
                _updateSequestration = value;
                btnCancel.Visible = !value;
                btnSubmit.Visible = !value;
            }
        }

        private bool _updateDeath;
        public bool UpdateDeath
        {
            get
            {
                return _updateDeath;
            }
            set
            {
                _updateDeath = value;
            }
        }

        private bool _updateSequestration;
        public bool UpdateSequestration
        {
            get
            {
                return _updateSequestration;
            }
            set
            {
                _updateSequestration = value;
            }
        }

    }
}
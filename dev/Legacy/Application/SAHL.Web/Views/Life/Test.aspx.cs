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
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Life
{
    public partial class Test : SAHLCommonBaseView ,ITest
    {
        private enum GridColumnPositions
        {
            LegalEntityKey = 0,
            Name = 1,
            IDNumber = 2,
            Role = 3,
            Status = 4
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region ITest Members

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnLegalEntityGridSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnLegalEntityGridNewSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstNaturalPersons"></param>
        public void BindLegalEntityGrid(IReadOnlyEventList<ILegalEntityNaturalPerson> lstNaturalPersons)
        {
            LegalEntityGrid.Columns.Clear();

            // Hidden Columns       
            LegalEntityGrid.AutoGenerateColumns = false;
            LegalEntityGrid.AddGridBoundColumn("Key", "", Unit.Percentage(0), HorizontalAlign.Left, false);
            // Visible Columns
            LegalEntityGrid.AddGridBoundColumn("", "Name", Unit.Percentage(40), HorizontalAlign.Left, true);
            LegalEntityGrid.AddGridBoundColumn("", "ID/Passport Number", Unit.Percentage(20), HorizontalAlign.Left, true);
            LegalEntityGrid.AddGridBoundColumn("", "Role", new Unit(20, UnitType.Percentage), HorizontalAlign.Left, true);
            LegalEntityGrid.AddGridBoundColumn("", "Status", new Unit(20, UnitType.Percentage), HorizontalAlign.Left, true);
            LegalEntityGrid.DataSource = lstNaturalPersons;
            LegalEntityGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legalEntities"></param>
        public void BindLegalEntityGridNew(IReadOnlyEventList<ILegalEntity> legalEntities)
        {
            // Setup the grid
            LegalEntityGridNew.HeaderCaption = "Applicants";

            LegalEntityGridNew.ColumnIDPassportVisible = true;
            LegalEntityGridNew.ColumnIDPassportHeadingType = SAHL.Web.Controls.LegalEntityGrid.GridColumnIDPassportHeadingType.IDNumber;
            LegalEntityGridNew.ColumnRoleVisible = true;
            LegalEntityGridNew.ColumnLegalEntityStatusVisible = true;

            // Bind the grid
            LegalEntityGridNew.BindLegalEntities(legalEntities);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnLegalEntityGridSelectedIndexChanged(sender, new KeyChangedEventArgs(LegalEntityGrid.Rows[LegalEntityGrid.SelectedIndex].Cells[0].Text));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGridNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnLegalEntityGridNewSelectedIndexChanged(sender, new KeyChangedEventArgs(LegalEntityGridNew.SelectedLegalEntity.Key));

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LegalEntityGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                ILegalEntityNaturalPerson le = e.Row.DataItem as ILegalEntityNaturalPerson;
                e.Row.Cells[(int)GridColumnPositions.Name].Text = le.GetLegalName(LegalNameFormat.Full);
                e.Row.Cells[(int)GridColumnPositions.IDNumber].Text = le.IDNumber == null ? (le.PassportNumber == null ? "" : le.PassportNumber) : le.IDNumber;
                e.Row.Cells[(int)GridColumnPositions.Role].Text = le.Roles[0].RoleType.Description;
                e.Row.Cells[(int)GridColumnPositions.Status].Text = le.LegalEntityStatus.Description;
            }
        }


    }
}

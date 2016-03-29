using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;

//using SAHL.Common.BusinessModel.Repositories.Schemas;


namespace SAHL.Web.Views.ReleaseAndVariations
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Conditions : SAHLCommonBaseView, Interfaces.IReleaseAndVariationsConditions
    {
        // The Event Handlers
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnAddClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnUpdateClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnCancelClicked;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler gridConditionsClicked;

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAddClicked != null)
                btnAddClicked(sender, e);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (btnUpdateClicked != null)
                btnUpdateClicked(sender, e);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            //releaseAndVariationsRepository.GridIndex = releaseAndVariationsRepository.SelectedIndex;
            if (btnCancelClicked != null)
                btnCancelClicked(sender, e);
        }

        protected void gridConditions_Click(object sender, EventArgs e)
        {
            if (gridConditionsClicked != null)
                gridConditionsClicked(sender, e);
        }

        /// <summary>
        /// Populate the Grid with the conditions dataset
        /// </summary>
        public void BindConditionGrid(DataTable DT)
        {
            gridConditions.Columns.Clear();
            gridConditions.DataSource = DT;
            gridConditions.AddGridBoundColumn("ConditionKey", "Favour Of", new Unit(0, UnitType.Percentage), HorizontalAlign.Left, false);
            gridConditions.AddGridBoundColumn("Condition", "Condition Description", new Unit(0, UnitType.Percentage), HorizontalAlign.Left, true);
            gridConditions.DataBind();
        }

        // ***********************  Interface Event Handler stuff *********************

        /// <summary>
        /// Property to show/hide the 'btnAdd' button
        /// </summary>
        public bool ShowbtnAdd
        {
            set { btnAdd.Visible = value; }
            get { return btnAdd.Visible; }
        }

        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        public bool ShowbtnUpdate
        {
            set { btnUpdate.Visible = value; }
        }

        /// <summary>
        /// Property to show/hide the 'btnUpdate' button
        /// </summary>
        public string Getcondition
        {
            get { return Page.Request.Form[txtNotes.UniqueID]; }
        }

        /// <summary>
        /// Cleat the Text in the Textbox;
        /// </summary>
        public void ClearText()
        {
            string newstring = txtNotes.Text.Remove(0);
            txtNotes.Text = newstring;
        }

        /// <summary>
        /// Property to show/hide the 'btnCancel' button
        /// </summary>
        public bool ShowbtnCancel
        {
            set { btnCancel.Visible = value; }
        }

        // Set Up the Grid
        /// <summary>
        /// Property to select the first row of the 'gridConditions' grid
        /// </summary>
        public bool SelectFirstRowgridConditions
        {
            set { gridConditions.SelectFirstRow = value; }
        }

        /// <summary>
        /// Set the index for gridConditions
        /// </summary>
        public int SetgridConditionsIndex
        {
            set { gridConditions.SelectedIndex = value; }
            get { return gridConditions.SelectedIndex; }
        }


        /// <summary>
        /// Set the index for gridConditions
        /// </summary>
        public string GetgridConditionsText
        {
            get { return gridConditions.SelectedRow.Cells[1].Text; }
        }

        /// <summary>
        ///  Set the Text of txtNotes
        /// </summary>
        public string  SettxtNotesText
        {
            set { txtNotes.Text = value; }
            get { return txtNotes.Text; }
        }

        /// <summary>
        /// Property to set The readonly attribure of the 'txtNotes'  object
        /// </summary>
        public bool SetReadOnlytxtNotes
        {
            set { txtNotes.ReadOnly = value; }
        }
    }
}
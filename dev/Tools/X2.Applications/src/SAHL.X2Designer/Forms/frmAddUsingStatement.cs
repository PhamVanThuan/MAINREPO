using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmAddUsingStatement : Form
    {
        List<string> _UsedUsingStatements;
        string _Mode = "";

        public frmAddUsingStatement(List<string> UsedUsingStatements, List<string> AvailUsingStatements, string Mode)
        {
            InitializeComponent();
            this._UsedUsingStatements = UsedUsingStatements;
            liUsing.DataSource = AvailUsingStatements;
            this._Mode = Mode;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_Mode == "ADD")
            {
                foreach (string item in liUsing.SelectedItems)
                {
                    string text = item.ToString();
                    if (!_UsedUsingStatements.Contains(text))
                        _UsedUsingStatements.Add(text);
                }
            }
            else if (_Mode == "EDIT")
            {
                foreach (string item in liUsing.SelectedItems)
                {
                    string text = item.ToString();
                    if (_UsedUsingStatements.Contains(text))
                        _UsedUsingStatements.Remove(text);
                }
            }
        }

        private void frmAddUsingStatement_Load(object sender, EventArgs e)
        {
            if (_Mode == "ADD")
            {
                this.Text = "Add Using Statement";
            }
            else if (_Mode == "EDIT")
            {
                this.Text = "Remove Using Statement";
            }
        }
    }
}
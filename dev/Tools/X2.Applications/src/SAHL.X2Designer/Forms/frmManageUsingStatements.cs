using System;
using System.Drawing;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageUsingStatements : Form
    {
        public frmManageUsingStatements()
        {
            InitializeComponent();
        }

        private void frmManageUsingStatements_Load(object sender, EventArgs e)
        {
            PopulateList();
        }

        private void PopulateList()
        {
            listViewUsingStatements.Items.Clear();
            listViewUsingStatements.Items.Add(new ListViewItem(new string[] { "SAHL.X2Designer.Languages", "false" }));
            int nUsedUsingStatements = MainForm.App.GetCurrentView().Document.UsedUsingStatements.Count;
            for (int x = 0; x < nUsedUsingStatements; x++)
            {
                listViewUsingStatements.Items.Add(new ListViewItem(new string[] { MainForm.App.GetCurrentView().Document.UsedUsingStatements[x], "true" }));
            }

            for (int x = 0; x < listViewUsingStatements.Items.Count; x++)
            {
                if (listViewUsingStatements.Items[x].SubItems[1].Text.ToLower() == "false")
                {
                    listViewUsingStatements.Items[x].ForeColor = Color.Gray;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUsingStatement fUsingStatement =
              new frmAddUsingStatement(MainForm.App.GetCurrentView().Document.UsedUsingStatements,
              MainForm.App.GetCurrentView().Document.UsingStatements, "ADD");
            fUsingStatement.ShowDialog();
            fUsingStatement.Close();
            PopulateList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewUsingStatements.SelectedIndices[0] != -1)
            {
                if (MessageBox.Show("Confirm Removal of Using Statement?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string Item = listViewUsingStatements.Items[listViewUsingStatements.SelectedIndices[0]].SubItems[0].Text;
                    MainForm.App.GetCurrentView().Document.RemoveUsedUsingStatement(Item);
                }
            }
            PopulateList();
        }
    }
}
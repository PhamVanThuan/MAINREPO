using System;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmSaveAll : Form
    {
        public frmSaveAll()
        {
            InitializeComponent();
        }

        private void frmSaveAll_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < MainForm.App.lstViewsToBeSaved.Count; x++)
            {
                listFilesToSave.Items.Add(MainForm.App.lstViewsToBeSaved[x].ProcessName.ToString());
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < MainForm.App.lstViewsToBeSaved.Count; x++)
            {
                foreach (Form f in MainForm.App.MdiChildren)
                {
                    ProcessForm p = f as ProcessForm;
                    if (MainForm.App.lstViewsToBeSaved[x].ProcessName == p.Text.Substring(0, p.Text.Length - 2))
                    {
                        p.Select();
                    }
                }
                Cursor = Cursors.WaitCursor;
                MainForm.App.setStatusBar("Saving " + (x + 1) + " of " + MainForm.App.lstViewsToBeSaved.Count.ToString() + " Documents");
                bool res = MainForm.App.Save(MainForm.App.lstViewsToBeSaved[x].view, MainForm.App.lstViewsToBeSaved[x].ProcessName);
                if (res == true)
                {
                    this.DialogResult = DialogResult.OK;
                    MainForm.App.lstViewsToBeSaved[x].view.setModified(false);
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    break;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MainForm.App.setStatusBar("Save Cancelled");
        }
    }
}
using System;
using System.Windows.Forms;

namespace SAHL.X2Designer.Forms
{
    public partial class frmAddGlobalReference : Form
    {
        public frmAddGlobalReference()
        {
            InitializeComponent();
        }

        private void frmReferences_Load(object sender, EventArgs e)
        {
            ColumnHeader colName = new ColumnHeader();
            ColumnHeader colVersion = new ColumnHeader();
            colName.Text = "Name";
            colName.Width = 400;
            colName.TextAlign = HorizontalAlignment.Left;
            colVersion.Text = "Version";
            colVersion.Width = listViewReferences.Width - 400;
            colVersion.TextAlign = HorizontalAlignment.Left;
            listViewReferences.Columns.Add(colName);
            listViewReferences.Columns.Add(colVersion);

            listViewReferences.View = System.Windows.Forms.View.Details;
            populateList();
        }

        private void populateList()
        {
            SAHL.X2Designer.Misc.GlobalAssemblyCache.IAssemblyName an;
            SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.CreateAssemblyCache();
            SAHL.X2Designer.Misc.GlobalAssemblyCache.IAssemblyEnum ae = SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.CreateGACEnum();
            while (SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.GetNextAssembly(ae, out an) == 0)
            {
                if (SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.GetName(an).Length > 0)
                {
                    ListViewItem mItem = new ListViewItem();
                    Version ver = SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.GetVersion(an);
                    string[] newItem = null;
                    if (ver != null)
                    {
                        newItem = new string[] { SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.GetName(an), ver.ToString() };
                    }
                    else
                    {
                        newItem = new string[] { SAHL.X2Designer.Misc.GlobalAssemblyCache.AssemblyCache.GetName(an), "" };
                    }
                    if (newItem[0].ToString().Length > 0)
                    {
                        ListViewItem li = new ListViewItem(newItem);
                        if (li.SubItems[0].Text.Length > 1)
                        {
                            listViewReferences.Items.Add(li);
                            listViewReferences.Tag = an;
                        }
                    }
                }
            }
        }

        private void btnAddGlobal_Click(object sender, EventArgs e)
        {
            if (listViewReferences.SelectedIndices[0] != -1)
            {
                if (MainForm.App.GetCurrentView() != null)
                {
                    bool foundRef = false;
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.References.Count; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.References[x].Name == listViewReferences.Items[listViewReferences.SelectedIndices[0]].SubItems[0].Text)
                        {
                            foundRef = true;
                            break;
                        }
                    }
                    if (foundRef == false)
                    {
                        string Name = listViewReferences.Items[listViewReferences.SelectedIndices[0]].SubItems[0].Text;
                        // TODO:            ReferenceItem ri = DomainLoader.GetCompiler().LoadGlobalAssembly(Name);
                        // TODO:           MainForm.App.GetCurrentView().Document.AddGlobalReference(ri);
                    }
                    else
                    {
                        MessageBox.Show("This reference has already been added!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
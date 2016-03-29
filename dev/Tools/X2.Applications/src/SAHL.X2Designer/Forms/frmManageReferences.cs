using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageReferences : Form
    {
        IComparer VersionCompare = new VersionComparer();

        public frmManageReferences()
        {
            InitializeComponent();
        }

        private void frmManageReferences_Load(object sender, EventArgs e)
        {
            ColumnHeader colName = new ColumnHeader();
            ColumnHeader colVersion = new ColumnHeader();
            ColumnHeader colGAC = new ColumnHeader();
            ColumnHeader colSubReference = new ColumnHeader();
            ColumnHeader colPath = new ColumnHeader();

            colName.Text = "Name";
            colName.Width = 280;
            colName.TextAlign = HorizontalAlignment.Left;
            colVersion.Text = "Version";
            colVersion.Width = 70;
            colVersion.TextAlign = HorizontalAlignment.Left;
            colGAC.Text = "Global";
            colGAC.Width = 70;
            colGAC.TextAlign = HorizontalAlignment.Left;
            colSubReference.Text = "Sub-Referenced";
            colSubReference.Width = 100;
            colSubReference.TextAlign = HorizontalAlignment.Left;
            colPath.Text = "Sub-Referenced";
            colPath.Width = listViewReferences.Width - colName.Width - colVersion.Width - colGAC.Width - colSubReference.Width;
            colPath.TextAlign = HorizontalAlignment.Left;
            listViewReferences.Columns.Add(colName);
            listViewReferences.Columns.Add(colVersion);
            listViewReferences.Columns.Add(colGAC);
            listViewReferences.Columns.Add(colSubReference);
            listViewReferences.Columns.Add(colPath);

            listViewReferences.View = System.Windows.Forms.View.Details;
            populateList("none");
        }

        private void populateList(string BinarySource)
        {
            listViewReferences.Items.Clear();
            if (MainForm.App.GetCurrentView() != null)
            {
                // Add the system references (stuff the designer loads by default)
                for (int i = 0; i < MainForm.App.GetCurrentView().Document.SysReferences.Count; i++)
                {
                    ReferenceItem item = MainForm.App.GetCurrentView().Document.SysReferences[i];
                    if (checkIfAlreadyInList(item))
                    {
                        continue;
                    }
                    ReferenceItem ri = MainForm.App.GetCurrentView().Document.SysReferences[i];
                    string[] newItem = new string[] {ri.Name ,ri.Version,"true",//ri.isGlobal.ToString(),
            "true",ri.FullPath.ToString()};

                    ListViewItem li = new ListViewItem(newItem);
                    li.Tag = MainForm.App.GetCurrentView().Document.SysReferences[i];
                    listViewReferences.Items.Add(li);
                }

                // add the global references
                for (int i = 0; i < MainForm.App.GetCurrentView().Document.GlobalReferences.Count; i++)
                {
                    ReferenceItem item = MainForm.App.GetCurrentView().Document.GlobalReferences[i];
                    if (checkIfAlreadyInList(item))
                    {
                        continue;
                    }
                    ReferenceItem ri = MainForm.App.GetCurrentView().Document.GlobalReferences[i];
                    string[] newItem = new string[] {ri.Name ,ri.Version,"true",//ri.isGlobal.ToString(),
            "false",ri.FullPath.ToString()};

                    ListViewItem li = new ListViewItem(newItem);
                    li.Tag = MainForm.App.GetCurrentView().Document.GlobalReferences[i];
                    listViewReferences.Items.Add(li);
                }

                // add the user references
                for (int x = 0; x < MainForm.App.GetCurrentView().Document.References.Count; x++)
                {
                    ReferenceItem item = MainForm.App.GetCurrentView().Document.References[x];
                    if (checkIfAlreadyInList(item))
                    {
                        continue;
                    }
                    ReferenceItem ri = MainForm.App.GetCurrentView().Document.References[x];
                    bool IsSubReferenced = MainForm.App.GetCurrentView().Document.CheckIfIsSubreferenced(ri); ;

                    string[] newItem = new string[] { ri.Name, ri.Version, ri.isGlobal.ToString(), IsSubReferenced.ToString(), ri.FullPath.ToString(), BinarySource };

                    ListViewItem li = new ListViewItem(newItem);
                    li.Tag = MainForm.App.GetCurrentView().Document.References[x];
                    listViewReferences.Items.Add(li);
                }
            }

            for (int x = 0; x < listViewReferences.Items.Count; x++)
            {
                if (listViewReferences.Items[x].SubItems[3].Text.ToLower() == "true")
                {
                    listViewReferences.Items[x].ForeColor = Color.Gray;
                }
            }
        }

        public bool checkIfAlreadyInList(ReferenceItem ri)
        {
            for (int i = 0; i < listViewReferences.Items.Count; i++)
            {
                if (listViewReferences.Items[i].Name == ri.Name)
                    return true;
            }
            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string FileToDelete = string.Empty;
            if (listViewReferences.SelectedIndices.Count > 0)
            {
                if (MessageBox.Show("Confirm removal of reference?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool foundRef = false;
                    int holdParentIndex = 0;
                    Cursor = Cursors.WaitCursor;
                    // check the user ref collection
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.References.Count; x++)
                    {
                        ReferenceItem i = listViewReferences.Items[listViewReferences.SelectedIndices[0]].Tag as ReferenceItem;
                        if (MainForm.App.GetCurrentView().Document.References[x].Name == i.Name)
                        {
                            foundRef = true;
                            holdParentIndex = x - 1;

                            MainForm.App.GetCurrentView().Document.References.RemoveAt(x);
                            listViewReferences.Items.RemoveAt(listViewReferences.SelectedIndices[0]);
                            if (MainForm.App.m_CodeView != null)
                            {
                                bool res = MainForm.App.dotNetProjectResolver.RemoveExternalReference(i.FullName);
                                if (res == false)
                                {
                                    MessageBox.Show("Could not remove referenced assembly from Syntax Editor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            FileToDelete = string.Format("{0}\\Build\\{1}", AppDomain.CurrentDomain.BaseDirectory, i.SavePath);
                            MessageBox.Show("Reference Removed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    }

                    // check the global collection
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.GlobalReferences.Count; x++)
                    {
                        ReferenceItem i = listViewReferences.Items[listViewReferences.SelectedIndices[0]].Tag as ReferenceItem;
                        if (MainForm.App.GetCurrentView().Document.GlobalReferences[x].Name == i.Name)
                        {
                            foundRef = true;
                            holdParentIndex = x - 1;

                            MainForm.App.GetCurrentView().Document.RemoveGlobalReference(i);
                            listViewReferences.Items.RemoveAt(listViewReferences.SelectedIndices[0]);
                            if (MainForm.App.m_CodeView != null)
                            {
                                bool res = MainForm.App.dotNetProjectResolver.RemoveExternalReference(i.FullName);
                                if (res == false)
                                {
                                    MessageBox.Show("Could not remove referenced assembly from Syntax Editor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            MessageBox.Show("Reference Removed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    }
                    // unload the remote domain we may want to load a newly compiled DLL with the same name
                    // into the appdomain.
                    //DomainLoader.UnloadCompileDomain();

                    // Delete file out of the build directory as well else things have a nasty habbit of getting their
                    // way back into the X2P file.
                    if (string.Empty != FileToDelete)
                        File.Delete(FileToDelete);
                    populateList("none");
                    if (MainForm.App.GetCurrentView() != null)
                    {
                        MainForm.App.GetCurrentView().setModified(true);
                    }
                    Cursor = Cursors.Default;
                }
            }
        }

        private void OpenDialog(string ExecutablePath, string BinarySource)
        {
            OpenFileDialog mDialog = new OpenFileDialog();
            mDialog.Title = "Add Reference";
            mDialog.Filter = "Component Files(*.dll,*.tlb,*.olb,*.ocx,*.exe,*.manifest) | *.dll;*.tlb;*.olb;*.ocx;*.exe;*.manifest";
            mDialog.InitialDirectory = ExecutablePath;
            mDialog.FilterIndex = 2;
            mDialog.RestoreDirectory = true;

            //mDialog.Multiselect = true;

            if (mDialog.ShowDialog() == DialogResult.OK)
            {
                MainForm.App.GetCurrentView().Document.LoadAssemblyFromFileIntoRemoteDomain(mDialog.FileName, BinarySource);
                if (MainForm.App.GetCurrentView() != null)
                {
                    MainForm.App.GetCurrentView().setModified(true);
                }
            }

            populateList(BinarySource);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddGlobalReference mFormRef = new frmAddGlobalReference();
            Cursor = Cursors.WaitCursor;
            mFormRef.ShowDialog();
            mFormRef.Dispose();
            Cursor = Cursors.Default;
            populateList("none");
        }

        private void btnAddFromFramework_Click(object sender, EventArgs e)
        {
            //OpenDialog(Helpers.GetBinaryFolderPath(Helpers.BinaryTypeInternal), Helpers.BinaryTypeInternal);
        }

        private void btnAddFromExternalBinaries_Click(object sender, EventArgs e)
        {
            //OpenDialog(Helpers.GetBinaryFolderPath(Helpers.BinaryTypeExternal), Helpers.BinaryTypeExternal);
        }

        private void btnAddFromDomainService_Click(object sender, EventArgs e)
        {
            //OpenDialog(Helpers.GetBinaryFolderPath(Helpers.BinaryTypeDomainClient), Helpers.BinaryTypeDomainClient);
        }

        private void listViewReferences_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewReferences.SelectedIndices.Count > 0)
            {
                if (listViewReferences.Items[listViewReferences.SelectedIndices[0]].SubItems[3].Text.ToLower() == "true")
                {
                    btnRemove.Enabled = false;
                }
                else
                {
                    btnRemove.Enabled = true;
                }
            }
        }
    }
}
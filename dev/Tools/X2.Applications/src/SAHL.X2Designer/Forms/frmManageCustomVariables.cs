using System;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageCustomVariables : Form
    {
        private ProcessDocument m_Doc;

        public frmManageCustomVariables(ProcessDocument mDoc)
        {
            InitializeComponent();
            m_Doc = mDoc;
        }

        private void frmManageCustomVariables_Load(object sender, EventArgs e)
        {
            ColumnHeader colName = new ColumnHeader();
            ColumnHeader colType = new ColumnHeader();
            ColumnHeader colLength = new ColumnHeader();

            colName.Text = "Variable";
            colName.Width = 180;
            colName.TextAlign = HorizontalAlignment.Left;
            colType.Text = "Type";
            colType.Width = 130;
            colType.TextAlign = HorizontalAlignment.Left;

            colLength.Text = "Length";
            colLength.Width = listViewVariables.Width - 315;
            colLength.TextAlign = HorizontalAlignment.Left;

            listViewVariables.Columns.Add(colName);
            listViewVariables.Columns.Add(colType);
            listViewVariables.Columns.Add(colLength);

            listViewVariables.View = System.Windows.Forms.View.Details;
            populateList();
        }

        private void populateList()
        {
            listViewVariables.Items.Clear();
            string[,] lst = new string[m_Doc.CurrentWorkFlow.CustomVariables.Count, 3];
            for (int x = 0; x < m_Doc.CurrentWorkFlow.CustomVariables.Count; x++)
            {
                CustomVariableTypeTypeConvertor m_CustomVariable = new CustomVariableTypeTypeConvertor();
                string typeStr = "";
                foreach (CustomVariableType y in m_CustomVariable.GetStandardValues())
                {
                    if (m_Doc.CurrentWorkFlow.CustomVariables[x].Type == y)
                    {
                        object mType = m_CustomVariable.ConvertTo(y, typeof(string));
                        typeStr = (string)mType;
                    }
                }

                lst[x, 0] = m_Doc.CurrentWorkFlow.CustomVariables[x].Name.ToString();
                lst[x, 1] = typeStr;
                lst[x, 2] = m_Doc.CurrentWorkFlow.CustomVariables[x].Length.ToString();

                string[] newItem = new string[] { lst[x, 0], lst[x, 1], lst[x, 2] };
                ListViewItem li = new ListViewItem(newItem);
                listViewVariables.Items.Add(li);
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmAddEditCustomVariable fAddVar = new frmAddEditCustomVariable();
            fAddVar.Text = "Add Custom Variable";
            fAddVar.ShowDialog();
            if (fAddVar.DialogResult == DialogResult.OK)
            {
                if (alreadyExists(fAddVar) == false)
                {
                    CustomVariableItem mVar = new CustomVariableItem();
                    mVar.Name = fAddVar.txtVariableName.Text.ToString();
                    mVar.Type = (CustomVariableType)new CustomVariableTypeTypeConvertor().ConvertFromString(fAddVar.cbxDataType.Text.ToString());
                    if (fAddVar.txtLength.Text.Length > 0)
                    {
                        mVar.Length = Convert.ToInt32(fAddVar.txtLength.Text);
                    }
                    if (mVar.Name.ToLower() != "instanceid")
                    {
                        m_Doc.CurrentWorkFlow.CustomVariables.Add(mVar);
                        populateList();

                        if (MainForm.App.GetCurrentView() != null)
                        {
                            MainForm.App.GetCurrentView().setModified(true);
                        }
                    }
                    else
                    {
                        MessageBox.Show(mVar.Name + " is an invalid variable name !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            fAddVar.Close();
            fAddVar.Dispose();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (listViewVariables.SelectedIndices.Count > 0 && listViewVariables.SelectedIndices != null)
            {
                for (int x = 0; x < m_Doc.CurrentWorkFlow.CustomVariables.Count; x++)
                {
                    if (m_Doc.CurrentWorkFlow.CustomVariables[x].Name.ToString() == listViewVariables.Items[listViewVariables.SelectedIndices[0]].SubItems[0].Text.ToString())
                    {
                        if (MessageBox.Show("Confirm removal of Custom Variable ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                            {
                                if (o.GetType() == typeof(ClapperBoard))
                                {
                                    ClapperBoard c = o as ClapperBoard;
                                    if (c.KeyVariable != null)
                                    {
                                        if (c.KeyVariable.Name.ToString() == listViewVariables.SelectedItems[0].Text)
                                        {
                                            c.KeyVariable = null;
                                        }
                                    }
                                }
                            }
                            m_Doc.CurrentWorkFlow.CustomVariables.RemoveAt(x);
                            if (MainForm.App.GetCurrentView() != null)
                            {
                                MainForm.App.GetCurrentView().setModified(true);
                            }
                        }
                        break;
                    }
                }
            }
            populateList();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool alreadyExists(frmAddEditCustomVariable fForm)
        {
            bool found = false;
            for (int x = 0; x < listViewVariables.Items.Count; x++)
            {
                if (listViewVariables.Items[x].Text == fForm.txtVariableName.Text)
                {
                    found = true;
                    MessageBox.Show("A Variable with this name already exists!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
                }
            }
            if (found == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void cmdModify_Click(object sender, EventArgs e)
        {
            if (listViewVariables.SelectedIndices.Count == 0)
            {
                return;
            }
            frmAddEditCustomVariable fAddVar = new frmAddEditCustomVariable();
            fAddVar.Text = "Edit Custom Variable";
            CustomVariableItem mCustomVar = new CustomVariableItem();
            string originalName = listViewVariables.Items[listViewVariables.SelectedIndices[0]].SubItems[0].Text;

            for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[x].Name == originalName)
                {
                    mCustomVar = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CustomVariables[x];
                    break;
                }
            }
            if (mCustomVar == null)
            {
                return;
            }
            fAddVar.txtVariableName.Text = listViewVariables.Items[listViewVariables.SelectedIndices[0]].SubItems[0].Text;
            fAddVar.cbxDataType.Text = listViewVariables.Items[listViewVariables.SelectedIndices[0]].SubItems[1].Text;
            if (listViewVariables.Items[listViewVariables.SelectedIndices[0]].SubItems[2].Text != "0")
            {
                fAddVar.txtLength.Text = listViewVariables.Items[listViewVariables.SelectedIndices[0]].SubItems[2].Text;
                fAddVar.txtLength.Enabled = true;
            }
            fAddVar.cbxDataType.Enabled = false;
            if (fAddVar.txtLength.Text == "0")
            {
                fAddVar.txtLength.Enabled = false;
            }
            fAddVar.ShowDialog();
            if (fAddVar.DialogResult == DialogResult.OK)
            {
                bool found = false;
                if (fAddVar.txtVariableName.Text != originalName)
                    found = alreadyExists(fAddVar);
                if (found == false)
                {
                    mCustomVar.Name = fAddVar.txtVariableName.Text;
                    if (fAddVar.txtLength.Text.Length > 0)
                    {
                        mCustomVar.Length = Convert.ToInt32(fAddVar.txtLength.Text);
                    }
                    else
                    {
                        mCustomVar.Length = 0;
                    }
                    populateList();
                    if (MainForm.App.GetCurrentView() != null)
                    {
                        MainForm.App.GetCurrentView().setModified(true);
                    }
                    if (MainForm.App.GetCurrentView() != null)
                    {
                        foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                        {
                            BaseItem b = o as BaseItem;
                            if (b != null)
                            {
                                if (b.AvailableCodeSections != null)
                                {
                                    for (int y = 0; y < b.AvailableCodeSections.Length; y++)
                                    {
                                        string codeData = b.GetCodeSectionData(b.AvailableCodeSections[y]);
                                        if (codeData.Contains(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName + "_Data." + originalName))
                                        {
                                            string newCode = "";
                                            newCode = codeData.Replace(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName + "_Data." + originalName, MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName + "_Data." + fAddVar.txtVariableName.Text);
                                            b.SetCodeSectionData(b.AvailableCodeSections[y], newCode);
                                            if (MainForm.App.m_CodeView != null)
                                            {
                                                MainForm.App.m_CodeView.DetachCode();
                                                MainForm.App.m_CodeView.ClearView();
                                                MainForm.App.m_CodeView.AttachCode();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            fAddVar.Close();
            fAddVar.Dispose();
        }
    }
}
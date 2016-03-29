using System;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageCustomForms : Form
    {
        ProcessDocument m_Doc;

        public frmManageCustomForms(ProcessDocument mDoc)
        {
            InitializeComponent();
            m_Doc = mDoc;
            populateList();
        }

        private void populateList()
        {
            string[,] lst = new string[m_Doc.CurrentWorkFlow.Forms.Count, 2];
            for (int x = 0; x < m_Doc.CurrentWorkFlow.Forms.Count; x++)
            {
                lst[x, 0] = m_Doc.CurrentWorkFlow.Forms[x].Name.ToString();
                lst[x, 1] = m_Doc.CurrentWorkFlow.Forms[x].Description.ToString();

                string[] newItem = new string[] { lst[x, 0], lst[x, 1] };
                ListViewItem li = new ListViewItem(newItem);
                li.Tag = m_Doc.CurrentWorkFlow.Forms[x];
                listViewForms.Items.Add(li);
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmAddEditForms fForm = new frmAddEditForms();
            fForm.Text = "Add Custom Form";
            fForm.ShowDialog();
            if (fForm.DialogResult == DialogResult.OK)
            {
                if (alreadyExists(fForm) == false)
                {
                    updateForms(fForm.txtName.Text, fForm.txtDescription.Text, "add", null);
                }
            }
            fForm.Close();
        }

        private bool alreadyExists(frmAddEditForms fForm)
        {
            bool found = false;
            for (int x = 0; x < listViewForms.Items.Count; x++)
            {
                if (listViewForms.Items[x].Text == fForm.txtName.Text)
                {
                    found = true;
                    MessageBox.Show("A Form with this name already exists!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void updateForms(string formName, string desc, string AddorEdit, string originalName)
        {
            if (AddorEdit == "add")
            {
                string[] newItem = new string[] { formName, desc };
                ListViewItem li = new ListViewItem(newItem);
                CustomFormItem mItem = new CustomFormItem();
                mItem.Name = formName;
                mItem.Description = desc;
                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Add(mItem);
                li.Tag = mItem;
                listViewForms.Items.Add(li);
            }
            if (AddorEdit == "edit")
            {
                CustomFormItem mFrmItem = listViewForms.SelectedItems[0].Tag as CustomFormItem;
                mFrmItem.Name = formName;
                mFrmItem.Description = desc;
                listViewForms.SelectedItems[0].SubItems[0].Text = formName;
                listViewForms.SelectedItems[0].SubItems[1].Text = desc;
                for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name == originalName)
                    {
                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name = formName;
                        MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Description = desc;
                    }
                }
            }
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }
        }

        private void frmManageCustomForms_Load(object sender, EventArgs e)
        {
            ColumnHeader colName = new ColumnHeader();
            ColumnHeader colDescription = new ColumnHeader();
            colName.Text = "Name";
            colName.Width = 100;
            colName.TextAlign = HorizontalAlignment.Left;
            colDescription.Text = "Description";
            colDescription.Width = listViewForms.Width - colName.Width - 5;
            colDescription.TextAlign = HorizontalAlignment.Left;

            listViewForms.Columns.Add(colName);
            listViewForms.Columns.Add(colDescription);

            listViewForms.View = System.Windows.Forms.View.Details;
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (listViewForms.SelectedItems.Count != 0)
            {
                frmAddEditForms mForm = new frmAddEditForms();
                string originalName = listViewForms.SelectedItems[0].SubItems[0].Text.ToString();
                mForm.txtName.Text = listViewForms.SelectedItems[0].SubItems[0].Text.ToString();
                mForm.txtDescription.Text = listViewForms.SelectedItems[0].SubItems[1].Text.ToString();
                mForm.Text = "Edit Custom Form";
                mForm.ShowDialog();
                if (mForm.DialogResult == DialogResult.OK)
                {
                    if (mForm.txtName.Text != originalName)
                    {
                        if (alreadyExists(mForm))
                        {
                            return;
                        }
                    }
                    updateForms(mForm.txtName.Text, mForm.txtDescription.Text, "edit", originalName);
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listViewForms.SelectedIndices.Count > 0)
            {
                if (MessageBox.Show("Confirm removal of form ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int x = 0; x < m_Doc.CurrentWorkFlow.Activities.Count; x++)
                    {
                        if (m_Doc.CurrentWorkFlow.Activities[x] is UserActivity)
                        {
                            UserActivity mUserActivity = m_Doc.CurrentWorkFlow.Activities[x] as UserActivity;
                            if (mUserActivity.CustomForm != null)
                            {
                                if (mUserActivity.CustomForm.Name == listViewForms.SelectedItems[0].Text)
                                {
                                    mUserActivity.CustomForm = null;
                                }
                            }
                        }
                    }
                    for (int x = 0; x < m_Doc.CurrentWorkFlow.States.Count; x++)
                    {
                        if (m_Doc.CurrentWorkFlow.States[x] is UserState)
                        {
                            UserState mState = m_Doc.CurrentWorkFlow.States[x] as UserState;
                            for (int y = 0; y < mState.CustomForms.Count; y++)
                            {
                                if (mState.CustomForms[y].Name == listViewForms.SelectedItems[0].Text)
                                {
                                    mState.CustomForms.RemoveAt(y);
                                }
                            }
                        }
                    }

                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow != null)
                    {
                        for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.Count; x++)
                        {
                            if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms[x].Name == listViewForms.SelectedItems[0].Text)
                            {
                                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Forms.RemoveAt(x);
                                break;
                            }
                        }
                    }

                    if (listViewForms.SelectedItems.Count > 0)
                    {
                        listViewForms.SelectedItems[0].Remove();
                    }

                    if (MainForm.App.GetCurrentView() != null)
                    {
                        MainForm.App.GetCurrentView().setModified(true);
                    }
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
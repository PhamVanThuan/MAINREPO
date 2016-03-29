using System;
using System.Windows.Forms;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageExternalActivities : Form
    {
        Documents.ProcessDocument m_Doc;

        public frmManageExternalActivities(ProcessDocument mDoc)
        {
            m_Doc = mDoc;
            InitializeComponent();
            populateList();
        }

        private void populateList()
        {
            string[,] lst = new string[m_Doc.CurrentWorkFlow.ExternalActivityCollection.Count, 2];
            for (int x = 0; x < m_Doc.CurrentWorkFlow.ExternalActivityCollection.Count; x++)
            {
                lst[x, 0] = m_Doc.CurrentWorkFlow.ExternalActivityCollection[x].ExternalActivity.ToString();
                lst[x, 1] = m_Doc.CurrentWorkFlow.ExternalActivityCollection[x].Description.ToString();
                string[] newItem = new string[] { lst[x, 0], lst[x, 1] };
                ListViewItem li = new ListViewItem(newItem);
                li.Tag = m_Doc.CurrentWorkFlow.ExternalActivityCollection[x];
                listViewExternalActivities.Items.Add(li);
            }
        }

        private void cmdDone_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmAddEditExternalActivities fExternalActivity = new frmAddEditExternalActivities();
            fExternalActivity.ShowDialog();
            if (fExternalActivity.DialogResult == DialogResult.OK)
            {
                if (alreadyExists(fExternalActivity) == false)
                {
                    updateExternalActivities(fExternalActivity.txtExternalActivity.Text, fExternalActivity.txtDescription.Text, "add");
                }
            }
            fExternalActivity.Close();
        }

        private void updateExternalActivities(string externalActivity, string desc, string AddorEdit)
        {
            if (AddorEdit == "add")
            {
                string[] newItem = new string[] { externalActivity, desc };
                ListViewItem li = new ListViewItem(newItem);
                ExternalActivityItem mItem = new ExternalActivityItem();
                mItem.ExternalActivity = externalActivity;
                mItem.Description = desc;
                MainForm.App.GetCurrentView().Document.CurrentWorkFlow.ExternalActivityCollection.Add(mItem);
                li.Tag = mItem;
                listViewExternalActivities.Items.Add(li);
            }
            if (AddorEdit == "edit")
            {
                ExternalActivityItem mExternalActivityItem = listViewExternalActivities.SelectedItems[0].Tag as ExternalActivityItem;
                mExternalActivityItem.ExternalActivity = externalActivity;
                mExternalActivityItem.Description = desc;
                listViewExternalActivities.SelectedItems[0].SubItems[0].Text = externalActivity;
                listViewExternalActivities.SelectedItems[0].SubItems[1].Text = desc;
            }
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }
        }

        private void frmManageExternalActivities_Load(object sender, EventArgs e)
        {
            ColumnHeader colFirstName = new ColumnHeader();
            ColumnHeader colLastName = new ColumnHeader();
            colFirstName.Text = "External Activity";
            colFirstName.Width = 100;
            colFirstName.TextAlign = HorizontalAlignment.Left;
            colLastName.Text = "Description";
            colLastName.Width = listViewExternalActivities.Width - colFirstName.Width - 5;
            colLastName.TextAlign = HorizontalAlignment.Left;

            listViewExternalActivities.Columns.Add(colFirstName);
            listViewExternalActivities.Columns.Add(colLastName);

            listViewExternalActivities.View = System.Windows.Forms.View.Details;
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (listViewExternalActivities.SelectedItems.Count != 0)
            {
                frmAddEditExternalActivities mExternalActivity = new frmAddEditExternalActivities();
                mExternalActivity.txtExternalActivity.Text = listViewExternalActivities.SelectedItems[0].SubItems[0].Text.ToString();
                mExternalActivity.txtDescription.Text = listViewExternalActivities.SelectedItems[0].SubItems[1].Text.ToString();
                mExternalActivity.Text = "Edit External Activity Source";
                mExternalActivity.ShowDialog();
                if (mExternalActivity.DialogResult == DialogResult.OK)
                {
                    if (alreadyExists(mExternalActivity) == false)
                    {
                        updateExternalActivities(mExternalActivity.txtExternalActivity.Text, mExternalActivity.txtDescription.Text, "edit");
                    }
                }
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (listViewExternalActivities.SelectedIndices.Count > 0)
            {
                if (MessageBox.Show("Confirm removal of External Activity Source?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int x = 0; x < m_Doc.CurrentWorkFlow.Activities.Count; x++)
                    {
                        if (m_Doc.CurrentWorkFlow.Activities[x].RaiseExternalActivity == null)
                        {
                            continue;
                        }
                        if (m_Doc.CurrentWorkFlow.Activities[x].RaiseExternalActivity.ExternalActivity == listViewExternalActivities.SelectedItems[0].Text)
                        {
                            m_Doc.CurrentWorkFlow.Activities[x].RaiseExternalActivity = null;
                        }
                        if (m_Doc.CurrentWorkFlow.Activities[x] is ExternalActivity)
                        {
                            ExternalActivity mItem = m_Doc.CurrentWorkFlow.Activities[x] as ExternalActivity;
                            mItem.InvokedBy = null;
                        }
                    }
                    if (listViewExternalActivities.SelectedItems.Count > 0)
                    {
                        ExternalActivityItem mItem = listViewExternalActivities.SelectedItems[0].Tag as ExternalActivityItem;
                        for (int x = 0; x < m_Doc.CurrentWorkFlow.ExternalActivityCollection.Count; x++)
                        {
                            if (m_Doc.CurrentWorkFlow.ExternalActivityCollection[x] == mItem)
                            {
                                m_Doc.CurrentWorkFlow.ExternalActivityCollection.RemoveAt(x);
                            }
                        }

                        listViewExternalActivities.SelectedItems[0].Remove();
                    }
                    if (MainForm.App.GetCurrentView() != null)
                    {
                        MainForm.App.GetCurrentView().setModified(true);
                    }
                }
            }
        }

        private bool alreadyExists(frmAddEditExternalActivities fForm)
        {
            bool found = false;
            for (int x = 0; x < listViewExternalActivities.Items.Count; x++)
            {
                if (listViewExternalActivities.Items[x].Text == fForm.txtExternalActivity.Text)
                {
                    found = true;
                    MessageBox.Show("A ExternalActivity with this name already exists!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
    }
}
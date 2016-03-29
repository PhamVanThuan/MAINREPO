using System;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageRoles : Form
    {
        private ProcessDocument m_Doc;

        public frmManageRoles(ProcessDocument mDoc)
        {
            InitializeComponent();
            m_Doc = mDoc;
            populateList();
        }

        private void populateList()
        {
            string[,] lst = new string[m_Doc.Roles.Count, 5];
            for (int x = 0; x < m_Doc.Roles.Count; x++)
            {
                bool found = false;
                for (int y = 0; y < listViewRoles.Items.Count; y++)
                {
                    if (listViewRoles.Items[y].SubItems[1].Text == m_Doc.Roles[x].Name.ToString())
                    {
                        if (m_Doc.Roles[x].WorkFlowItem != null)
                        {
                            if (listViewRoles.Items[y].SubItems[3].Text == m_Doc.Roles[x].WorkFlowItem.WorkFlowName.ToString())
                            {
                                found = true;
                                break;
                            }
                        }
                        else
                        {
                        }
                    }
                }
                if (!found)
                {
                    lst[x, 0] = m_Doc.Roles[x].RoleType.ToString();
                    lst[x, 1] = m_Doc.Roles[x].Name;
                    lst[x, 2] = m_Doc.Roles[x].Description;
                    if (m_Doc.Roles[x].WorkFlowItem != null)
                    {
                        lst[x, 3] = m_Doc.Roles[x].WorkFlowItem.WorkFlowName.ToString();
                    }
                    else
                    {
                        lst[x, 3] = "N/A";
                    }
                    lst[x, 4] = m_Doc.Roles[x].IsDynamic.ToString();

                    string[] newItem = new string[] { lst[x, 0], lst[x, 1], lst[x, 2], lst[x, 3], lst[x, 4] };
                    ListViewItem li = new ListViewItem(newItem);
                    li.Tag = m_Doc.Roles[x];
                    listViewRoles.Items.Add(li);
                }
            }
        }

        private void frmManageRoles_Load(object sender, EventArgs e)
        {
            ColumnHeader colType = new ColumnHeader();
            ColumnHeader colWorkFlow = new ColumnHeader();
            ColumnHeader colRole = new ColumnHeader();
            ColumnHeader colDescription = new ColumnHeader();
            ColumnHeader colDynamic = new ColumnHeader();
            colType.Text = "Type";
            colType.Width = 100;
            colType.TextAlign = HorizontalAlignment.Left;
            colRole.Text = "Role";
            colRole.Width = 100;
            colRole.TextAlign = HorizontalAlignment.Left;
            colDescription.Text = "Description";
            colDescription.Width = 150;
            colDescription.TextAlign = HorizontalAlignment.Left;
            colDynamic.Text = "Dynamic";
            colDynamic.Width = 100;
            colDynamic.TextAlign = HorizontalAlignment.Left;
            colWorkFlow.Text = "WorkFlow";
            colWorkFlow.Width = 100;
            colWorkFlow.TextAlign = HorizontalAlignment.Left;
            listViewRoles.Columns.Add(colType);
            listViewRoles.Columns.Add(colRole);
            listViewRoles.Columns.Add(colDescription);
            listViewRoles.Columns.Add(colWorkFlow);
            listViewRoles.Columns.Add(colDynamic);
            listViewRoles.View = System.Windows.Forms.View.Details;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmAddEditRoles fForm = new frmAddEditRoles();
            fForm.Text = "Add Role";
            fForm.ShowDialog();
            if (fForm.DialogResult == DialogResult.OK)
            {
                if (alreadyExists(fForm, null) == false)
                {
                    RoleType type = RoleType.Global;
                    WorkFlow mWorkFlow = null;

                    if (fForm.radioWorkFlow.Checked == true)
                    {
                        type = RoleType.WorkFlow;
                        for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                        {
                            if (MainForm.App.GetCurrentView().Document.WorkFlows[x].WorkFlowName == fForm.cbxSelectWorkFlow.Text)
                            {
                                mWorkFlow = MainForm.App.GetCurrentView().Document.WorkFlows[x];
                                break;
                            }
                        }
                    }
                    else if (fForm.radioGlobal.Checked == true)
                    {
                        type = RoleType.Global;
                    }

                    updateRoles(type, fForm.txtRole.Text, fForm.txtDescription.Text, mWorkFlow, fForm.checkDynamic.Checked, "add");
                }
            }
            fForm.Close();
        }

        private void updateRoles(RoleType type, string role, string desc, WorkFlow workFlow, bool dynamic, string AddorEdit)
        {
            if (AddorEdit == "add")
            {
                string[] newItem;
                if (workFlow != null)
                {
                    newItem = new string[] { type.ToString(), role, desc, workFlow.WorkFlowName, dynamic.ToString() };
                }
                else
                {
                    newItem = new string[] { type.ToString(), role, desc, "N/A", dynamic.ToString() };
                }
                ListViewItem li = new ListViewItem(newItem);
                RolesCollectionItem mItem = new RolesCollectionItem();
                mItem.RoleType = type;
                mItem.Name = role;
                mItem.Description = desc;
                mItem.WorkFlowItem = workFlow;
                mItem.IsDynamic = dynamic;
                MainForm.App.GetCurrentView().Document.Roles.Add(mItem);
                //foreach (BaseActivity a in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.Activities)
                //{
                //    UserActivity u = a as UserActivity;
                //    if (u != null)
                //    {
                //        RoleInstance i = new RoleInstance();
                //        i.RoleItem = mItem;
                //        i.IsChecked = false;
                //        u.Access.Add(i);
                //    }
                //}
                li.Tag = mItem;
                listViewRoles.Items.Add(li);
            }
            if (AddorEdit == "edit")
            {
                RolesCollectionItem mRoleItem = listViewRoles.SelectedItems[0].Tag as RolesCollectionItem;
                mRoleItem.Name = role;
                mRoleItem.Description = desc;
                mRoleItem.IsDynamic = dynamic;
                mRoleItem.WorkFlowItem = workFlow;
                mRoleItem.RoleType = type;
                listViewRoles.SelectedItems[0].SubItems[0].Text = type.ToString();
                listViewRoles.SelectedItems[0].SubItems[1].Text = role;
                listViewRoles.SelectedItems[0].SubItems[2].Text = desc;
                if (workFlow != null)
                {
                    listViewRoles.SelectedItems[0].SubItems[3].Text = workFlow.WorkFlowName;
                }
                else
                {
                    listViewRoles.SelectedItems[0].SubItems[3].Text = "N/A";
                }
                listViewRoles.SelectedItems[0].SubItems[4].Text = dynamic.ToString();
            }
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (listViewRoles.SelectedItems.Count != 0)
            {
                if (RoleItemCollection.FixedRoles.Contains(listViewRoles.Items[listViewRoles.SelectedIndices[0]].SubItems[1].Text))
                {
                    MessageBox.Show("Fixed roles are not modifiable!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                frmAddEditRoles mForm = new frmAddEditRoles();
                mForm.Text = "Edit Role";
                if (listViewRoles.SelectedItems[0].SubItems[0].Text == "Global")
                {
                    mForm.radioGlobal.Checked = true;
                }
                else if (listViewRoles.SelectedItems[0].SubItems[0].Text == "WorkFlow")
                {
                    mForm.radioWorkFlow.Checked = true;
                    mForm.PopulateWorkFlowCbx();
                    mForm.cbxSelectWorkFlow.Text = listViewRoles.SelectedItems[0].SubItems[3].Text;
                }
                string originalName = listViewRoles.SelectedItems[0].SubItems[1].Text.ToString();
                mForm.holdRoleItem = listViewRoles.SelectedItems[0].Tag as RolesCollectionItem;
                mForm.txtRole.Text = listViewRoles.SelectedItems[0].SubItems[1].Text.ToString();
                mForm.txtDescription.Text = listViewRoles.SelectedItems[0].SubItems[2].Text.ToString();
                mForm.checkDynamic.Checked = Convert.ToBoolean(listViewRoles.SelectedItems[0].SubItems[4].Text.ToString());
                mForm.ShowDialog();
                if (mForm.DialogResult == DialogResult.OK)
                {
                    if (alreadyExists(mForm, originalName) == false)
                    {
                        RoleType type = RoleType.Global;
                        WorkFlow mWorkFlow = null;

                        if (mForm.radioWorkFlow.Checked == true)
                        {
                            type = RoleType.WorkFlow;
                            for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                            {
                                if (MainForm.App.GetCurrentView().Document.WorkFlows[x].WorkFlowName == mForm.cbxSelectWorkFlow.Text)
                                {
                                    mWorkFlow = MainForm.App.GetCurrentView().Document.WorkFlows[x];
                                    break;
                                }
                            }
                        }
                        else if (mForm.radioGlobal.Checked == true)
                        {
                            type = RoleType.Global;
                        }

                        updateRoles(type, mForm.txtRole.Text, mForm.txtDescription.Text, mWorkFlow, mForm.checkDynamic.Checked, "edit");
                    }
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listViewRoles.SelectedIndices.Count > 0)
            {
                if (RoleItemCollection.FixedRoles.Contains(listViewRoles.Items[listViewRoles.SelectedIndices[0]].SubItems[1].Text))
                {
                    MessageBox.Show("Fixed roles are not removable!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show("Confirm removal of role ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RolesCollectionItem RCI = listViewRoles.SelectedItems[0].Tag as RolesCollectionItem;
                    for (int y = 0; y < m_Doc.WorkFlows.Length; y++)
                    {
                        foreach (GoObject o in m_Doc.WorkFlows[y])
                        {
                            if (o.GetType() is ClapperBoard)
                            {
                                ClapperBoard mClapperBoard = o as ClapperBoard;
                                mClapperBoard.LimitAccessTo = null;
                                continue;
                            }
                        }

                        for (int x = 0; x < m_Doc.WorkFlows[y].Activities.Count; x++)
                        {
                            UserActivity mUserActivity = m_Doc.WorkFlows[y].Activities[x] as UserActivity;
                            if (mUserActivity != null)
                            {
                                for (int z = 0; z < mUserActivity.Access.Count; z++)
                                {
                                    if (RCI != null)
                                    {
                                        RoleInstance RI = mUserActivity.Access.GetRoleInstanceByRoleItem(RCI);
                                        if (mUserActivity.Access[z].RoleItem.Name == listViewRoles.SelectedItems[0].Text)
                                        {
                                            mUserActivity.Access.RemoveAt(z);
                                        }
                                    }
                                }
                            }
                        }

                        for (int x = 0; x < m_Doc.WorkFlows[y].States.Count; x++)
                        {
                            BaseStateWithLists mState = m_Doc.WorkFlows[y].States[x] as BaseStateWithLists;
                            if (mState != null)
                            {
                                for (int z = 0; z < mState.WorkList.Count; z++)
                                {
                                    if (RCI != null)
                                    {
                                        RoleInstance RI = mState.WorkList.GetRoleInstanceByRoleItem(RCI);
                                        if (RI != null)
                                            mState.WorkList.Remove(RI);
                                    }
                                }

                                for (int z = 0; z < mState.TrackList.Count; z++)
                                {
                                    if (mState.TrackList[y].RoleItem.Name == listViewRoles.SelectedItems[0].Text)
                                    {
                                        mState.TrackList.RemoveAt(y);
                                    }
                                }
                            }
                        }
                    }
                    if (listViewRoles.SelectedItems.Count > 0)
                    {
                        listViewRoles.SelectedItems[0].Remove();
                    }

                    m_Doc.Roles.Remove(RCI);

                    if (MainForm.App.GetCurrentView() != null)
                    {
                        MainForm.App.GetCurrentView().setModified(true);
                    }
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            BaseStateWithLists bs = MainForm.App.GetCurrentView().Selection.Primary as BaseStateWithLists;
            if (bs != null)
            {
                bs.WorkList.RefreshRoles();
                bs.TrackList.RefreshRoles();
            }
            UserActivity ba = MainForm.App.GetCurrentView().Selection.Primary as UserActivity;
            if (ba != null)
            {
                ba.Access.RefreshRoles();
            }

            Close();
        }

        private bool alreadyExists(frmAddEditRoles fForm, string originalName)
        {
            bool found = false;
            for (int x = 0; x < listViewRoles.Items.Count; x++)
            {
                if (listViewRoles.Items[x].SubItems[1].Text == fForm.txtRole.Text && fForm.txtRole.Text != originalName)
                {
                    if (listViewRoles.Items[x].SubItems[3].Text == fForm.cbxSelectWorkFlow.Text)
                    {
                        found = true;
                        MessageBox.Show("A Role with this name already exists!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
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
using System;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmManageAccess : Form
    {
        private ProcessDocument m_ProcDoc;
        private Items.RoleInstanceCollection m_AvailCol;
        private BaseItem m_Item;

        public frmManageAccess(Items.RoleInstanceCollection AvailCol, ProcessDocument pDoc, BaseItem myItem)
        {
            InitializeComponent();
            m_ProcDoc = pDoc;
            m_AvailCol = AvailCol;
            m_Item = myItem;

            foreach (RolesCollectionItem r in MainForm.App.GetCurrentView().Document.Roles)
            {
                string NameToAdd = r.Name;
                if (r.RoleType == RoleType.Global)
                {
                    switch (myItem.WorkflowItemBaseType)
                    {
                        case WorkflowItemBaseType.State:

                            if (myItem is BaseState)
                            {
                                if (r.Name == "WorkList" || r.Name == "TrackList")
                                {
                                    continue;
                                }
                            }
                            break;
                        case WorkflowItemBaseType.Activity:
                            BaseActivity BA = myItem as BaseActivity;
                            BaseState BS = GetStateForActivity(BA);
                            if (BS == null)
                            {
                                if (r.Name == "WorkList" || r.Name == "TrackList" || r.Name == "Originator" || r.IsDynamic)
                                {
                                    continue;
                                }
                            }
                            break;
                    }

                    checkedListAvailableGroups.Items.Add(NameToAdd, false);
                }
                else
                {
                    if (r.WorkFlowItem.WorkFlowName == MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName)
                    {
                        switch (r.WorkflowItemBaseType)
                        {
                            case WorkflowItemBaseType.State:

                                if (myItem is BaseState)
                                {
                                    if (r.Name == "WorkList" || r.Name == "TrackList")
                                    {
                                        continue;
                                    }
                                }
                                break;
                            case WorkflowItemBaseType.Activity:
                                BaseActivity BA = myItem as BaseActivity;
                                BaseState BS = GetStateForActivity(BA);
                                if (BS == null)
                                {
                                    if (r.Name == "WorkList" || r.Name == "TrackList" || r.Name == "Originator" || r.IsDynamic)
                                    {
                                        continue;
                                    }
                                }
                                break;
                        }
                        checkedListAvailableGroups.Items.Add(NameToAdd, false);
                    }
                }
            }

            foreach (Items.RoleInstance mGroup in AvailCol)
            {
                if (mGroup.RoleItem != null)
                {
                    //if (mGroup.RoleItem.RoleType == RoleType.WorkFlow)
                    //{
                    //    if (mGroup.RoleItem.WorkFlowItem != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    //    {
                    //        continue;
                    //    }
                    //}
                    //string NameToAdd;
                    for (int x = 0; x < checkedListAvailableGroups.Items.Count; x++)
                    {
                        if (mGroup.RoleItem.Name == checkedListAvailableGroups.GetItemText(checkedListAvailableGroups.Items[x]))
                        {
                            checkedListAvailableGroups.SetItemChecked(x, mGroup.IsChecked);
                            break;
                        }
                    }
                }
            }
        }

        private void cmdDone_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < checkedListAvailableGroups.Items.Count; x++)
            {
                bool found = false;
                for (int y = 0; y < m_AvailCol.Count; y++)
                {
                    if (checkedListAvailableGroups.Items[x].ToString() == m_AvailCol[y].RoleItem.Name.ToString())
                    {
                        m_AvailCol[y].IsChecked = checkedListAvailableGroups.GetItemChecked(x);
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    for (int z = 0; z < MainForm.App.GetCurrentView().Document.Roles.Count; z++)
                    {
                        if (MainForm.App.GetCurrentView().Document.Roles[z].Name == checkedListAvailableGroups.GetItemText(x))
                        {
                            RoleInstance ri = new RoleInstance();
                            ri.RoleItem = MainForm.App.GetCurrentView().Document.Roles[z];
                            ri.IsChecked = checkedListAvailableGroups.GetItemChecked(x);
                            m_AvailCol.Add(ri);
                            break;
                        }
                    }
                }
            }
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().FireOnPropertyChangedEvent(PropertyType.Access);
                MainForm.App.GetCurrentView().setModified(true);
            }

            Close();
        }

        private void checkedListAvailableGroups_ItemCheck(object sender, ItemCheckEventArgs e)
        {
        }

        private void checkedListAvailableGroups_SelectedValueChanged(object sender, EventArgs e)
        {
            if (checkedListAvailableGroups.SelectedIndex != -1)
            {
                int indexToUse = -1;
                for (int x = 0; x < m_AvailCol.Count; x++)
                {
                    if (m_AvailCol[x].RoleItem.Name == checkedListAvailableGroups.Items[checkedListAvailableGroups.SelectedIndex].ToString())
                    {
                        indexToUse = x;
                        break;
                    }
                }
                if (m_AvailCol[indexToUse].RoleItem.IsDynamic == true)
                {
                    BaseActivity mActivity = m_Item as BaseActivity;
                    if (mActivity != null)
                    {
                        foreach (CustomLink l in mActivity.Links)
                        {
                            if (l.FromNode.GetType() == typeof(ClapperBoard) || l.ToNode.GetType() == typeof(ClapperBoard))
                            {
                                MessageBox.Show("This is a dynamic role and cannot be selected for a start activity!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                checkedListAvailableGroups.SetItemCheckState(checkedListAvailableGroups.SelectedIndex, CheckState.Unchecked);
                            }
                        }
                    }
                }
            }
        }

        private static BaseState GetStateForActivity(BaseActivity Activity)
        {
            IGoLink Link = null;

            foreach (IGoLink Lnk in Activity.Links)
            {
                if (Lnk.FromNode != Activity)
                {
                    Link = Lnk;
                    break;
                }
            }
            if (Link != null)
            {
                if (Link.FromNode is BaseState)
                {
                    return ((BaseState)Link.FromNode);
                }
            }
            return null;
        }

        private void frmManageAccess_Load(object sender, EventArgs e)
        {
        }
    }
}
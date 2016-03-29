using System;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Items;
using WeifenLuo.WinFormsUI;

namespace SAHL.X2Designer.Views
{
    public partial class SecurityView : DockContent
    {
        private ISecurity_HasAccessList m_AccessItem;
        private ISecurity_HasWorkTrackLists m_WorkTrackItem;
        private ISecurity_HasLimitAccessTo m_LimitAccessToItem;

        public SecurityView()
        {
            InitializeComponent();
            MainForm.App.OnProcessViewActivated += new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
            MainForm.App.OnProcessViewDeactivated += new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
            MainForm.App.OnGeneralCustomFormClosed += new MainForm.GeneralCustomFormClosedHandler(App_OnGeneralCustomFormClosed);

            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected += new ProcessView.OnWorkFlowItemSelectedHandler(SecurityView_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected += new ProcessView.OnWorkFlowItemUnSelectedHandler(SecurityView_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnPropertyChanged += new ProcessView.OnPropertyChangedHandler(SecurityView_OnPropertyChanged);
            }
        }

        private void SecurityView_OnPropertyChanged(PropertyType propType)
        {
            if (propType == PropertyType.Access)
            {
                if (MainForm.App.GetCurrentView().Selection != null)
                {
                    GoObject mSelection = MainForm.App.GetCurrentView().Selection.Primary;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mSelection);
                }
            }
        }

        private void SecurityView_OnWorkFlowItemUnSelected(SAHL.X2Designer.Items.IWorkflowItem UnSelectedItem)
        {
            ClearView();
        }

        public void ClearView()
        {
            for (int x = tabControlMain.TabPages.Count - 1; x > -1; x--)
            {
                for (int y = 0; y < tabControlMain.TabPages[x].Controls.Count; y++)
                {
                    switch (x)
                    {
                        case 0:
                            {
                                CheckedListBox checkedBox1 = tabControlMain.TabPages[x].Controls[0] as CheckedListBox;
                                if (checkedBox1 != null)
                                {
                                    checkedBox1.ItemCheck -= new ItemCheckEventHandler(checkedBox1_ItemCheck);
                                }
                                else
                                {
                                    ComboBox comboBox1 = tabControlMain.TabPages[x].Controls[0] as ComboBox;
                                    if (comboBox1 != null)
                                    {
                                        comboBox1.SelectedValueChanged -= new EventHandler(comboBox1_SelectedValueChanged);
                                    }
                                }
                                break;
                            }
                        case 1:
                            {
                                CheckedListBox checkedBox2 = tabControlMain.TabPages[x].Controls[0] as CheckedListBox;
                                checkedBox2.ItemCheck -= new ItemCheckEventHandler(checkedBox2_ItemCheck);
                                break;
                            }
                    }
                    tabControlMain.TabPages[x].Controls.RemoveAt(y);
                }
                tabControlMain.Controls.Remove(tabControlMain.TabPages[x]);
            }
        }

        private void SecurityView_OnWorkFlowItemSelected(SAHL.X2Designer.Items.IWorkflowItem SelectedItem)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection.Count > 1)
                {
                    ClearView();
                    return;
                }
            }
            m_AccessItem = SelectedItem as ISecurity_HasAccessList;
            m_WorkTrackItem = SelectedItem as ISecurity_HasWorkTrackLists;
            m_LimitAccessToItem = SelectedItem as ISecurity_HasLimitAccessTo;

            if (m_AccessItem != null)
            {
                CheckedListBox checkedBox1 = new CheckedListBox();
                checkedBox1.Dock = DockStyle.Fill;
                checkedBox1.Sorted = true;
                checkedBox1.CheckOnClick = true;
                tabControlMain.TabPages.Add(new TabPage("Access"));
                tabControlMain.TabPages[0].Controls.Add(checkedBox1);
                PopulateActivityAccess(m_AccessItem);
                checkedBox1.ItemCheck += new ItemCheckEventHandler(checkedBox1_ItemCheck);
            }
            else if (m_WorkTrackItem != null)
            {
                if (m_WorkTrackItem is CommonState == false)
                {
                    CheckedListBox checkedBox1 = new CheckedListBox();
                    checkedBox1.Dock = DockStyle.Fill;
                    checkedBox1.Sorted = true;
                    checkedBox1.CheckOnClick = true;
                    checkedBox1.ItemCheck += new ItemCheckEventHandler(checkedBox1_ItemCheck);
                    tabControlMain.TabPages.Add(new TabPage("Work List"));
                    tabControlMain.TabPages[0].Controls.Add(checkedBox1);

                    CheckedListBox checkedBox2 = new CheckedListBox();
                    checkedBox2.Dock = DockStyle.Fill;
                    checkedBox2.Sorted = true;
                    checkedBox2.CheckOnClick = true;
                    tabControlMain.TabPages.Add(new TabPage("Track List"));
                    tabControlMain.TabPages[1].Controls.Add(checkedBox2);
                    PopulateStateWorkList(m_WorkTrackItem);
                    PopulateStateTrackList(m_WorkTrackItem);
                    checkedBox2.ItemCheck += new ItemCheckEventHandler(checkedBox2_ItemCheck);
                }
                else
                {
                    CheckedListBox checkedBox1 = new CheckedListBox();
                    checkedBox1.Dock = DockStyle.Fill;
                    checkedBox1.Sorted = true;
                    checkedBox1.CheckOnClick = true;
                    tabControlMain.TabPages.Add(new TabPage("Applies To"));
                    tabControlMain.TabPages[0].Controls.Add(checkedBox1);
                    PopulateAppliesToList(m_WorkTrackItem);

                    checkedBox1.ItemCheck += new ItemCheckEventHandler(checkedBox1_ItemCheck);
                }
            }
            else if (m_LimitAccessToItem != null)
            {
                ComboBox comboBox1 = new ComboBox();
                comboBox1.Dock = DockStyle.Fill;
                comboBox1.Sorted = true;
                comboBox1.DropDownStyle = ComboBoxStyle.Simple;
                tabControlMain.TabPages.Add(new TabPage("Limit Access To"));
                tabControlMain.TabPages[0].Controls.Add(comboBox1);
                PopulateLimitAccessToList(m_LimitAccessToItem);
                comboBox1.SelectedValueChanged += new EventHandler(comboBox1_SelectedValueChanged);
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox mBox = tabControlMain.TabPages[0].Controls[0] as ComboBox;
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.Roles[x].Name == mBox.SelectedItem.ToString())
                {
                    ClapperBoard mClapperBoard = m_LimitAccessToItem as ClapperBoard;
                    mClapperBoard.LimitAccessTo = MainForm.App.GetCurrentView().Document.Roles[x];
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mClapperBoard);
                    break;
                }
            }
        }

        private void checkedBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }
            CheckedListBox mBox = tabControlMain.TabPages[0].Controls[0] as CheckedListBox;
            if (m_AccessItem != null)
            {
                for (int y = 0; y < m_AccessItem.Access.Count; y++)
                {
                    if (mBox.SelectedItem != null)
                    {
                        if (mBox.SelectedItem.ToString() == m_AccessItem.Access[y].RoleItem.Name)
                        {
                            if (e.NewValue == CheckState.Checked)
                            {
                                m_AccessItem.Access[y].IsChecked = true;
                            }
                            else
                            {
                                m_AccessItem.Access[y].IsChecked = false;
                            }
                        }
                    }
                }
            }
            else if (m_WorkTrackItem != null)
            {
                if (m_WorkTrackItem is CommonState == false)
                {
                    for (int y = 0; y < m_WorkTrackItem.WorkList.Count; y++)
                    {
                        if (mBox.SelectedItem != null)
                        {
                            if (mBox.SelectedItem.ToString() == m_WorkTrackItem.WorkList[y].RoleItem.Name)
                            {
                                if (e.NewValue == CheckState.Checked)
                                {
                                    m_WorkTrackItem.WorkList[y].IsChecked = true;
                                }
                                else
                                {
                                    m_WorkTrackItem.WorkList[y].IsChecked = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    CommonState mState = m_WorkTrackItem as CommonState;

                    if (mBox.SelectedItem != null)
                    {
                        if (e.NewValue == CheckState.Unchecked)
                        {
                            for (int y = 0; y < mState.AppliesTo.Count; y++)
                            {
                                if (mState.AppliesTo[y].Name == mBox.SelectedItem.ToString())
                                {
                                    mState.AppliesTo.RemoveAt(y);
                                    break;
                                }
                            }
                        }
                        else if (e.NewValue == CheckState.Checked)
                        {
                            foreach (BaseState bState in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States)
                            {
                                if (bState.Name == mBox.SelectedItem.ToString())
                                {
                                    mState.AppliesTo.Add(bState);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void checkedBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().setModified(true);
            }
            CheckedListBox mBox = tabControlMain.TabPages[1].Controls[0] as CheckedListBox;
            for (int x = 0; x < mBox.Items.Count; x++)
            {
                if (m_WorkTrackItem != null)
                {
                    for (int y = 0; y < m_WorkTrackItem.TrackList.Count; y++)
                    {
                        if (mBox.SelectedItem != null)
                        {
                            if (mBox.SelectedItem.ToString() == m_WorkTrackItem.TrackList[y].RoleItem.Name)
                            {
                                if (e.NewValue == CheckState.Checked)
                                {
                                    m_WorkTrackItem.TrackList[y].IsChecked = true;
                                }
                                else
                                {
                                    m_WorkTrackItem.TrackList[y].IsChecked = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PopulateStateWorkList(ISecurity_HasWorkTrackLists mState)
        {
            CheckedListBox mBox = tabControlMain.TabPages[0].Controls[0] as CheckedListBox;

            for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.Roles[x].Name == "WorkList"
                    || MainForm.App.GetCurrentView().Document.Roles[x].Name == "TrackList")
                {
                    continue;
                }
                if (MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.WorkFlow)
                {
                    if (MainForm.App.GetCurrentView().Document.Roles[x].WorkFlowItem != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        continue;
                    }
                }
                mBox.Items.Add(MainForm.App.GetCurrentView().Document.Roles[x].Name, false);
            }

            for (int x = 0; x < mBox.Items.Count; x++)
            {
                for (int y = 0; y < mState.WorkList.Count; y++)
                {
                    if (mBox.Items[x].ToString() == mState.WorkList[y].RoleItem.Name)
                    {
                        mBox.SetItemChecked(x, mState.WorkList[y].IsChecked);
                    }
                }
            }
        }

        private void PopulateStateTrackList(ISecurity_HasWorkTrackLists mState)
        {
            CheckedListBox mBox = tabControlMain.TabPages[1].Controls[0] as CheckedListBox;

            for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.Roles[x].Name == "WorkList"
                    || MainForm.App.GetCurrentView().Document.Roles[x].Name == "TrackList")
                {
                    continue;
                }
                if (MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.WorkFlow)
                {
                    if (MainForm.App.GetCurrentView().Document.Roles[x].WorkFlowItem != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        continue;
                    }
                }
                mBox.Items.Add(MainForm.App.GetCurrentView().Document.Roles[x].Name, false);
            }

            for (int x = 0; x < mBox.Items.Count; x++)
            {
                for (int y = 0; y < mState.TrackList.Count; y++)
                {
                    if (mBox.Items[x].ToString() == mState.TrackList[y].RoleItem.Name)
                    {
                        mBox.SetItemChecked(x, mState.TrackList[y].IsChecked);
                    }
                }
            }
        }

        private void PopulateAppliesToList(ISecurity_HasWorkTrackLists State)
        {
            CommonState mCommonstate = State as CommonState;

            CheckedListBox mBox = tabControlMain.TabPages[0].Controls[0] as CheckedListBox;
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x] is CommonState == false
                    && MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x] is ArchiveState == false)
                {
                    mBox.Items.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x].Name, false);
                }
            }

            for (int x = 0; x < mBox.Items.Count; x++)
            {
                for (int y = 0; y < mCommonstate.AppliesTo.Count; y++)
                {
                    if (mBox.Items[x].ToString() == mCommonstate.AppliesTo[y].Name)
                    {
                        mBox.SetItemChecked(x, true);
                    }
                }
            }
        }

        private void PopulateActivityAccess(ISecurity_HasAccessList mActivity)
        {
            CheckedListBox mBox = tabControlMain.TabPages[0].Controls[0] as CheckedListBox;
            BaseState BS = GetStateForActivity(mActivity);
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
            {
                if (BS == null)
                {
                    if (MainForm.App.GetCurrentView().Document.Roles[x].Name == "WorkList"
                        || MainForm.App.GetCurrentView().Document.Roles[x].Name == "TrackList"
                        || MainForm.App.GetCurrentView().Document.Roles[x].Name == "Originator"
                        || MainForm.App.GetCurrentView().Document.Roles[x].IsDynamic)
                    {
                        continue;
                    }
                }
                if (MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.WorkFlow)
                {
                    if (MainForm.App.GetCurrentView().Document.Roles[x].WorkFlowItem != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        continue;
                    }
                }
                mBox.Items.Add(MainForm.App.GetCurrentView().Document.Roles[x].Name, false);
            }

            for (int x = 0; x < mBox.Items.Count; x++)
            {
                for (int y = 0; y < mActivity.Access.Count; y++)
                {
                    if (mBox.Items[x].ToString() == mActivity.Access[y].RoleItem.Name)
                    {
                        mBox.SetItemChecked(x, mActivity.Access[y].IsChecked);
                    }
                }
            }
        }

        private void PopulateLimitAccessToList(ISecurity_HasLimitAccessTo m_LimitAccessToItem)
        {
            ComboBox mBox = tabControlMain.TabPages[0].Controls[0] as ComboBox;
            for (int x = 0; x < MainForm.App.GetCurrentView().Document.Roles.Count; x++)
            {
                if (MainForm.App.GetCurrentView().Document.Roles[x].Name == "WorkList"
                    || MainForm.App.GetCurrentView().Document.Roles[x].Name == "TrackList"
                    || MainForm.App.GetCurrentView().Document.Roles[x].IsDynamic
                    || MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.WorkFlow)
                {
                    continue;
                }

                if (MainForm.App.GetCurrentView().Document.Roles[x].RoleType == RoleType.WorkFlow)
                {
                    if (MainForm.App.GetCurrentView().Document.Roles[x].WorkFlowItem != MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        continue;
                    }
                }
                mBox.Items.Add(MainForm.App.GetCurrentView().Document.Roles[x].Name);
            }
            if (m_LimitAccessToItem.LimitAccessTo != null)
            {
                for (int x = 0; x < mBox.Items.Count; x++)
                {
                    if (mBox.Items[x].ToString() == m_LimitAccessToItem.LimitAccessTo.Name)
                    {
                        mBox.SelectedItem = mBox.Items[x];
                        break;
                    }
                }
            }
        }

        private void App_OnGeneralCustomFormClosed(SAHL.X2Designer.Items.GeneralCustomFormType formType)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection != null)
                {
                    GoObject mSelection = MainForm.App.GetCurrentView().Selection.Primary;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mSelection);
                }
            }
        }

        private void App_OnProcessViewDeactivated(ProcessView v)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected -= new ProcessView.OnWorkFlowItemSelectedHandler(SecurityView_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected -= new ProcessView.OnWorkFlowItemUnSelectedHandler(SecurityView_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnPropertyChanged -= new ProcessView.OnPropertyChangedHandler(SecurityView_OnPropertyChanged);
            }
            ClearView();
        }

        private void App_OnProcessViewActivated(ProcessView v)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected += new ProcessView.OnWorkFlowItemSelectedHandler(SecurityView_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected += new ProcessView.OnWorkFlowItemUnSelectedHandler(SecurityView_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnPropertyChanged += new ProcessView.OnPropertyChangedHandler(SecurityView_OnPropertyChanged);
            }
        }

        private void SecurityView_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.App.mnuItemSecurityView.Checked = false;
        }

        private void SecurityView_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.App.OnProcessViewActivated -= new MainForm.ProcessViewActivatedHandler(App_OnProcessViewActivated);
            MainForm.App.OnProcessViewDeactivated -= new MainForm.ProcessViewDeactivatedHandler(App_OnProcessViewDeactivated);
            MainForm.App.OnGeneralCustomFormClosed -= new MainForm.GeneralCustomFormClosedHandler(App_OnGeneralCustomFormClosed);

            if (MainForm.App.GetCurrentView() != null)
            {
                MainForm.App.GetCurrentView().OnWorkFlowItemSelected -= new ProcessView.OnWorkFlowItemSelectedHandler(SecurityView_OnWorkFlowItemSelected);
                MainForm.App.GetCurrentView().OnWorkFlowItemUnSelected -= new ProcessView.OnWorkFlowItemUnSelectedHandler(SecurityView_OnWorkFlowItemUnSelected);
                MainForm.App.GetCurrentView().OnPropertyChanged -= new ProcessView.OnPropertyChangedHandler(SecurityView_OnPropertyChanged);
            }
        }

        private static BaseState GetStateForActivity(ISecurity_HasAccessList Activity)
        {
            BaseItem mItem = Activity as BaseItem;
            IGoLink Link = null;

            foreach (IGoLink Lnk in mItem.Links)
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

        private void SecurityView_Load(object sender, EventArgs e)
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                if (MainForm.App.GetCurrentView().Selection != null)
                {
                    GoObject mObj = MainForm.App.GetCurrentView().Selection.Primary;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mObj);
                }
            }
        }
    }
}
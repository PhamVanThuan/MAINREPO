using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Views;

namespace SAHL.X2Designer.Forms
{
    public partial class frmFindReplace : Form
    {
        public List<ResultItem> listResults = new List<ResultItem>();
        public List<WorkFlow> listWorkFlows = new List<WorkFlow>();

        public frmFindReplace()
        {
            InitializeComponent();
        }

        private void cmdFind_Click(object sender, EventArgs e)
        {
            FindText();
        }

        public void FindText()
        {
            if (txtFindWhat.Text.Length > 0)
            {
                listResults.Clear();
                if (MainForm.App.m_CodeView != null)
                {
                    MainForm.App.m_CodeView.syntaxEditor.SelectedView.Selection.SelectRange(0, 0);
                }
                listViewResults.Items.Clear();
                listWorkFlows.Clear();

                if (radioAllWorkflows.Checked)
                {
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.WorkFlows.Length; x++)
                    {
                        listWorkFlows.Add(MainForm.App.GetCurrentView().Document.WorkFlows[x]);
                    }
                }
                else
                {
                    listWorkFlows.Add(MainForm.App.GetCurrentView().Document.CurrentWorkFlow);
                }

                switch (comboLookIn.Text)
                {
                    case "All":
                        {
                            SearchActivities();
                            SearchStates();
                            SearchComments();
                            SearchCode();
                            if (checkMatchCase.Checked)
                            {
                                SearchMatchCaseProperties();
                            }
                            else
                            {
                                SearchProperties();
                            }
                            break;
                        }
                    case "Activity Names":
                        {
                            SearchActivities();
                            break;
                        }
                    case "State Names":
                        {
                            SearchStates();
                            break;
                        }
                    case "Comments":
                        {
                            SearchComments();
                            break;
                        }
                    case "Code":
                        {
                            SearchCode();
                            break;
                        }
                    case "Properties":
                        {
                            SearchProperties();
                            break;
                        }
                }
                //                MessageBox.Show(listViewResults.Items.Count.ToString() + " Occurrences Found!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SearchActivities()
        {
            for (int x = 0; x < listWorkFlows.Count; x++)
            {
                foreach (GoObject o in listWorkFlows[x])
                {
                    if (o is BaseActivity)
                    {
                        BaseActivity b = o as BaseActivity;
                        int pos = -1;
                        if (checkMatchCase.Checked)
                        {
                            pos = b.Name.IndexOf(txtFindWhat.Text);
                        }
                        else
                        {
                            pos = b.Name.ToLower().IndexOf(txtFindWhat.Text.ToLower());
                        }
                        if (pos != -1)
                        {
                            ListViewItem lItem = new ListViewItem(new string[] { b.Name, "Activity", listWorkFlows[x].WorkFlowName });
                            lItem.Tag = o;
                            listViewResults.Items.Add(lItem);
                            ResultItem mItem = new ResultItem();
                            mItem.Name = b.Name;
                            mItem.resultType = b.GetType().ToString();
                            mItem.GoObj = b as GoObject;
                            listResults.Add(mItem);
                        }
                    }
                }
            }
        }

        private void SearchStates()
        {
            for (int x = 0; x < listWorkFlows.Count; x++)
            {
                foreach (GoObject o in listWorkFlows[x])
                {
                    if (o is BaseState)
                    {
                        BaseState b = o as BaseState;
                        int pos = -1;
                        if (checkMatchCase.Checked)
                        {
                            pos = b.Name.IndexOf(txtFindWhat.Text);
                        }
                        else
                        {
                            pos = b.Name.ToLower().IndexOf(txtFindWhat.Text.ToLower());
                        }
                        if (pos != -1)
                        {
                            ListViewItem lItem = new ListViewItem(new string[] { b.Name, "State", listWorkFlows[x].WorkFlowName });
                            lItem.Tag = o;
                            listViewResults.Items.Add(lItem);
                            ResultItem mItem = new ResultItem();
                            mItem.Name = b.Name;
                            mItem.resultType = b.GetType().ToString();
                            mItem.GoObj = b as GoObject;
                            listResults.Add(mItem);
                        }
                    }
                }
            }
        }

        private void SearchComments()
        {
            for (int x = 0; x < listWorkFlows.Count; x++)
            {
                foreach (GoObject o in listWorkFlows[x])
                {
                    if (o.GetType() == typeof(Comment))
                    {
                        Comment b = o as Comment;
                        int pos = -1;
                        if (checkMatchCase.Checked)
                        {
                            pos = b.Text.IndexOf(txtFindWhat.Text);
                        }
                        else
                        {
                            pos = b.Text.ToLower().IndexOf(txtFindWhat.Text.ToLower());
                        }
                        if (pos != -1)
                        {
                            ListViewItem lItem = new ListViewItem(new string[] { "Comment" + b.CommentID.ToString(), "Comment", listWorkFlows[x].WorkFlowName });
                            lItem.Tag = o;
                            listViewResults.Items.Add(lItem);
                            ResultItem mItem = new ResultItem();
                            mItem.Name = b.Name;
                            mItem.resultType = b.GetType().ToString();
                            mItem.GoObj = b as GoObject;
                            listResults.Add(mItem);
                        }
                    }
                }
            }
        }

        private void SearchCode()
        {
            if (MainForm.App.m_CodeView != null && MainForm.App.m_CodeView.Visible == true)
            {
                ActiproSoftware.SyntaxEditor.FindReplaceOptions mOptions = new ActiproSoftware.SyntaxEditor.FindReplaceOptions();
                mOptions.FindText = txtFindWhat.Text;
                mOptions.MatchCase = checkMatchCase.Checked;
                mOptions.SearchHiddenText = true;
                mOptions.MatchWholeWord = false;
                ActiproSoftware.SyntaxEditor.FindReplaceResultSet mResultSet = MainForm.App.m_CodeView.syntaxEditor.Document.FindReplace.FindAll(mOptions);
                for (int x = 0; x < mResultSet.Count; x++)
                {
                    ResultItem mItem = new ResultItem();
                    mItem.Name = "Code Text";
                    mItem.resultType = "Code";
                    mItem.CodeSection = MainForm.App.m_CodeView.toolStripComboBoxCodeSection.Text;
                    mItem.GoObj = MainForm.App.GetCurrentView().Selection.Primary;
                    mItem.CodePosition = mResultSet[x].StartOffset;
                    listResults.Add(mItem);

                    ListViewItem lItem = new ListViewItem(new string[] { MainForm.App.GetCurrentView().Selection.Primary.ToString(), MainForm.App.m_CodeView.toolStripComboBoxCodeSection.Text + " (Code) - Pos: " + mResultSet[x].StartOffset.ToString(), MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName });
                    lItem.Tag = mItem;
                    listViewResults.Items.Add(lItem);
                }
                for (int z = 0; z < listWorkFlows.Count; z++)
                {
                    foreach (GoObject o in listWorkFlows[z])
                    {
                        BaseItem b = o as BaseItem;
                        if (b != null)
                        {
                            if (b.AvailableCodeSections != null)
                            {
                                for (int y = 0; y < b.AvailableCodeSections.Length; y++)
                                {
                                    string codeData = b.GetCodeSectionData(b.AvailableCodeSections[y]);
                                    if (checkMatchCase.Checked)
                                    {
                                        if (codeData.Contains(txtFindWhat.Text) == false)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (codeData.ToLower().Contains(txtFindWhat.Text.ToLower()) == false)
                                        {
                                            continue;
                                        }
                                    }
                                    if (o as GoObject != MainForm.App.GetCurrentView().Selection.Primary || b.AvailableCodeSections[y].ToString() != MainForm.App.m_CodeView.toolStripComboBoxCodeSection.Text)
                                    {
                                        ResultItem mItem = new ResultItem();
                                        mItem.GoObj = o;
                                        mItem.resultType = "Code";
                                        mItem.Name = b.Name;
                                        mItem.CodeSection = b.AvailableCodeSections[y];
                                        listResults.Add(mItem);

                                        ListViewItem lItem = new ListViewItem(new string[] { b.Name, "Code - " + b.AvailableCodeSections[y], listWorkFlows[z].WorkFlowName });
                                        lItem.Tag = mItem;
                                        listViewResults.Items.Add(lItem);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int x = 0; x < listWorkFlows.Count; x++)
                {
                    foreach (GoObject o in listWorkFlows[x])
                    {
                        BaseItem b = o as BaseItem;
                        if (b != null)
                        {
                            if (b.AvailableCodeSections != null)
                            {
                                for (int y = 0; y < b.AvailableCodeSections.Length; y++)
                                {
                                    string codeData = b.GetCodeSectionData(b.AvailableCodeSections[y]);
                                    if (checkMatchCase.Checked)
                                    {
                                        if (codeData.Contains(txtFindWhat.Text) == false)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (codeData.ToLower().Contains(txtFindWhat.Text.ToLower()) == false)
                                        {
                                            continue;
                                        }
                                    }
                                    ResultItem mItem = new ResultItem();
                                    mItem.GoObj = o;
                                    mItem.resultType = "Code";
                                    mItem.Name = b.Name;
                                    mItem.CodeSection = b.AvailableCodeSections[y];
                                    listResults.Add(mItem);

                                    ListViewItem lItem = new ListViewItem(new string[] { b.Name, "Code - " + b.AvailableCodeSections[y], listWorkFlows[x].WorkFlowName });
                                    lItem.Tag = mItem;
                                    listViewResults.Items.Add(lItem);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SearchProperties()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                for (int y = 0; y < listWorkFlows.Count; y++)
                {
                    foreach (GoObject o in listWorkFlows[y])
                    {
                        if (o is BaseActivity)
                        {
                            BaseActivity mActivity = o as BaseActivity;
                            if (mActivity.SplitWorkFlow.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                            {
                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "SplitWorkFlow (Property)", listWorkFlows[y].WorkFlowName });
                                lItem.Tag = o;
                                listViewResults.Items.Add(lItem);

                                ResultItem mResult = new ResultItem();
                                mResult.GoObj = mActivity;
                                mResult.Property = "SplitWorkFlow";
                                mResult.resultType = "Property";
                                listResults.Add(mResult);
                            }

                            if (mActivity.RaiseExternalActivity != null)
                            {
                                if (mActivity.RaiseExternalActivity.ExternalActivity.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                                {
                                    ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "RaiseExternalActivity (Property)", listWorkFlows[y].WorkFlowName });
                                    lItem.Tag = o;
                                    listViewResults.Items.Add(lItem);

                                    ResultItem mResult = new ResultItem();
                                    mResult.GoObj = mActivity;
                                    mResult.Property = "RaiseExternalActivity";
                                    mResult.resultType = "Property";
                                    listResults.Add(mResult);
                                }
                            }

                            if (mActivity.Description.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                            {
                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "Description (Property)", listWorkFlows[y].WorkFlowName });
                                lItem.Tag = o;
                                listViewResults.Items.Add(lItem);

                                ResultItem mResult = new ResultItem();
                                mResult.GoObj = mActivity;
                                mResult.Property = "Description";
                                mResult.resultType = "Property";
                                listResults.Add(mResult);
                            }

                            if (mActivity.Message.ToLower().Contains(txtFindWhat.Text.ToLower()))
                            {
                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "Message (Property)", listWorkFlows[y].WorkFlowName });
                                lItem.Tag = o;
                                listViewResults.Items.Add(lItem);

                                ResultItem mResult = new ResultItem();
                                mResult.GoObj = mActivity;
                                mResult.Property = "Message";
                                mResult.resultType = "Property";
                                listResults.Add(mResult);
                            }
                        }
                        BaseItem mItem = o as BaseItem;
                        if (mItem != null)
                        {
                            switch (mItem.WorkflowItemType)
                            {
                                case WorkflowItemType.UserState:
                                    {
                                        UserState mState = o as UserState;
                                        if (mState != null)
                                        {
                                            for (int x = 0; x < mState.CustomForms.Count; x++)
                                            {
                                                if (mState.CustomForms[x].Name.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "CustomForms (Property)", listWorkFlows[x].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mState;
                                                    mResult.Property = "CustomForms";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }

                                            for (int x = 0; x < mState.TrackList.Count; x++)
                                            {
                                                if (mState.TrackList[x].IsChecked)
                                                {
                                                    if (mState.TrackList[x].RoleItem.Name.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                                    {
                                                        ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "TrackList (Property)", listWorkFlows[x].WorkFlowName });
                                                        lItem.Tag = o;
                                                        listViewResults.Items.Add(lItem);
                                                        ResultItem mResult = new ResultItem();
                                                        mResult.GoObj = mState;
                                                        mResult.Property = "TrackList";
                                                        mResult.resultType = "Property";
                                                        listResults.Add(mResult);
                                                    }
                                                }
                                            }

                                            for (int x = 0; x < mState.WorkList.Count; x++)
                                            {
                                                if (mState.WorkList[x].IsChecked)
                                                {
                                                    if (mState.WorkList[x].RoleItem.Name.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                                    {
                                                        ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "WorkList (Property)", listWorkFlows[x].WorkFlowName });
                                                        lItem.Tag = o;
                                                        listViewResults.Items.Add(lItem);

                                                        ResultItem mResult = new ResultItem();
                                                        mResult.GoObj = mState;
                                                        mResult.Property = "WorkList";
                                                        mResult.resultType = "Property";
                                                        listResults.Add(mResult);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }

                                case WorkflowItemType.SystemState:
                                    {
                                        SystemState mState = o as SystemState;

                                        if (mState.UseAutoForward.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                                        {
                                            ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "UseAutoForward (Property)", listWorkFlows[y].WorkFlowName });
                                            lItem.Tag = o;
                                            listViewResults.Items.Add(lItem);

                                            ResultItem mResult = new ResultItem();
                                            mResult.GoObj = mState;
                                            mResult.Property = "UseAutoForward";
                                            mResult.resultType = "Property";
                                            listResults.Add(mResult);
                                        }

                                        for (int x = 0; x < mState.TrackList.Count; x++)
                                        {
                                            if (mState.TrackList[x].IsChecked)
                                            {
                                                if (mState.TrackList[x].RoleItem.Name.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "TrackList (Property)", listWorkFlows[y].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mState;
                                                    mResult.Property = "TrackList";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }
                                        }

                                        for (int x = 0; x < mState.WorkList.Count; x++)
                                        {
                                            if (mState.WorkList[x].IsChecked)
                                            {
                                                if (mState.WorkList[x].RoleItem.Name.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "WorkList (Property)", listWorkFlows[y].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mState;
                                                    mResult.Property = "WorkList";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }
                                        }
                                        break;
                                    }

                                case WorkflowItemType.CommonState:
                                    {
                                        CommonState mState = o as CommonState;

                                        for (int x = 0; x < mState.AppliesTo.Count; x++)
                                        {
                                            if (mState.AppliesTo[x].Text.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "AppliesTo (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mState;
                                                mResult.Property = "AppliesTo";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }
                                        break;
                                    }
                                case WorkflowItemType.ArchiveState:
                                    {
                                        ArchiveState mState = o as ArchiveState;
                                        break;
                                    }

                                case WorkflowItemType.UserActivity:
                                    {
                                        UserActivity mActivity = o as UserActivity;
                                        if (mActivity.UseLinkedActivity.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                                        {
                                            ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "UseLinkedActivity (Property)", listWorkFlows[y].WorkFlowName });
                                            lItem.Tag = o;
                                            listViewResults.Items.Add(lItem);

                                            ResultItem mResult = new ResultItem();
                                            mResult.GoObj = mActivity;
                                            mResult.Property = "UseLinkedActivity";
                                            mResult.resultType = "Property";
                                            listResults.Add(mResult);
                                        }

                                        if (mActivity.CustomForm != null)
                                        {
                                            if (mActivity.CustomForm.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "CustomForm (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mActivity;
                                                mResult.Property = "CustomForm";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }

                                        for (int x = 0; x < mActivity.Access.Count; x++)
                                        {
                                            if (mActivity.Access[x].IsChecked)
                                            {
                                                if (mActivity.Access[x].RoleItem.Name.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "Access (Property)", listWorkFlows[y].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mActivity;
                                                    mResult.Property = "Access";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }
                                        }

                                        break;
                                    }
                                case WorkflowItemType.TimedActivity:
                                    {
                                        break;
                                    }

                                case WorkflowItemType.ExternalActivity:
                                    {
                                        ExternalActivity mActivity = o as ExternalActivity;

                                        if (mActivity.InvokeOnInstanceTarget != null)
                                        {
                                            if (mActivity.InvokeOnInstanceTarget.ToLower().Contains(txtFindWhat.Text.ToLower()))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "ExternalActivityRaiseFolder (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mActivity;
                                                mResult.Property = "ExternalActivityRaiseFolder";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }

                                        if (mActivity.InvokedBy != null)
                                        {
                                            if (mActivity.InvokedBy.ExternalActivity.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "InvokedBy (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mActivity;
                                                mResult.Property = "InvokedBy";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }
                                        break;
                                    }

                                case WorkflowItemType.ConditionalActivity:
                                    {
                                        break;
                                    }

                                case WorkflowItemType.Comment:
                                    {
                                        Comment mComment = o as Comment;

                                        if (mComment.CommentText.ToString().ToLower().Contains(txtFindWhat.Text.ToLower()))
                                        {
                                            ListViewItem lItem = new ListViewItem(new string[] { mComment.Name, "CommentText (Property)", listWorkFlows[y].WorkFlowName });
                                            lItem.Tag = o;
                                            listViewResults.Items.Add(lItem);

                                            ResultItem mResult = new ResultItem();
                                            mResult.GoObj = mComment;
                                            mResult.Property = "CommentText";
                                            mResult.resultType = "Property";
                                            listResults.Add(mResult);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        private void SearchMatchCaseProperties()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                for (int y = 0; y < listWorkFlows.Count; y++)
                {
                    foreach (GoObject o in listWorkFlows[y])
                    {
                        if (o is BaseActivity)
                        {
                            BaseActivity mActivity = o as BaseActivity;
                            if (mActivity.SplitWorkFlow.ToString().Contains(txtFindWhat.Text))
                            {
                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "SplitWorkFlow (Property)", listWorkFlows[y].WorkFlowName });
                                lItem.Tag = o;
                                listViewResults.Items.Add(lItem);

                                ResultItem mResult = new ResultItem();
                                mResult.GoObj = mActivity;
                                mResult.Property = "SplitWorkFlow";
                                mResult.resultType = "Property";
                                listResults.Add(mResult);
                            }

                            if (mActivity.RaiseExternalActivity != null)
                            {
                                if (mActivity.RaiseExternalActivity.ExternalActivity.ToString().Contains(txtFindWhat.Text))
                                {
                                    ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "RaiseExternalActivity (Property)", listWorkFlows[y].WorkFlowName });
                                    lItem.Tag = o;
                                    listViewResults.Items.Add(lItem);

                                    ResultItem mResult = new ResultItem();
                                    mResult.GoObj = mActivity;
                                    mResult.Property = "RaiseExternalActivity";
                                    mResult.resultType = "Property";
                                    listResults.Add(mResult);
                                }
                            }

                            if (mActivity.Description.ToString().Contains(txtFindWhat.Text))
                            {
                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "Description (Property)", listWorkFlows[y].WorkFlowName });
                                lItem.Tag = o;
                                listViewResults.Items.Add(lItem);

                                ResultItem mResult = new ResultItem();
                                mResult.GoObj = mActivity;
                                mResult.Property = "Description";
                                mResult.resultType = "Property";
                                listResults.Add(mResult);
                            }

                            if (mActivity.Message.Contains(txtFindWhat.Text))
                            {
                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "Message (Property)", listWorkFlows[y].WorkFlowName });
                                lItem.Tag = o;
                                listViewResults.Items.Add(lItem);

                                ResultItem mResult = new ResultItem();
                                mResult.GoObj = mActivity;
                                mResult.Property = "Message";
                                mResult.resultType = "Property";
                                listResults.Add(mResult);
                            }
                        }
                        BaseItem mItem = o as BaseItem;
                        if (mItem != null)
                        {
                            switch (mItem.WorkflowItemType)
                            {
                                case WorkflowItemType.UserState:
                                    {
                                        UserState mState = o as UserState;
                                        if (mState != null)
                                        {
                                            for (int x = 0; x < mState.CustomForms.Count; x++)
                                            {
                                                if (mState.CustomForms[x].Name.Contains(txtFindWhat.Text))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "CustomForms (Property)", listWorkFlows[x].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mState;
                                                    mResult.Property = "CustomForms";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }

                                            for (int x = 0; x < mState.TrackList.Count; x++)
                                            {
                                                if (mState.TrackList[x].IsChecked)
                                                {
                                                    if (mState.TrackList[x].RoleItem.Name.Contains(txtFindWhat.Text))
                                                    {
                                                        ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "TrackList (Property)", listWorkFlows[x].WorkFlowName });
                                                        lItem.Tag = o;
                                                        listViewResults.Items.Add(lItem);
                                                        ResultItem mResult = new ResultItem();
                                                        mResult.GoObj = mState;
                                                        mResult.Property = "TrackList";
                                                        mResult.resultType = "Property";
                                                        listResults.Add(mResult);
                                                    }
                                                }
                                            }

                                            for (int x = 0; x < mState.WorkList.Count; x++)
                                            {
                                                if (mState.WorkList[x].IsChecked)
                                                {
                                                    if (mState.WorkList[x].RoleItem.Name.Contains(txtFindWhat.Text))
                                                    {
                                                        ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "WorkList (Property)", listWorkFlows[x].WorkFlowName });
                                                        lItem.Tag = o;
                                                        listViewResults.Items.Add(lItem);

                                                        ResultItem mResult = new ResultItem();
                                                        mResult.GoObj = mState;
                                                        mResult.Property = "WorkList";
                                                        mResult.resultType = "Property";
                                                        listResults.Add(mResult);
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                    }

                                case WorkflowItemType.SystemState:
                                    {
                                        SystemState mState = o as SystemState;

                                        if (mState.UseAutoForward.ToString().Contains(txtFindWhat.Text))
                                        {
                                            ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "UseAutoForward (Property)", listWorkFlows[y].WorkFlowName });
                                            lItem.Tag = o;
                                            listViewResults.Items.Add(lItem);

                                            ResultItem mResult = new ResultItem();
                                            mResult.GoObj = mState;
                                            mResult.Property = "UseAutoForward";
                                            mResult.resultType = "Property";
                                            listResults.Add(mResult);
                                        }

                                        for (int x = 0; x < mState.TrackList.Count; x++)
                                        {
                                            if (mState.TrackList[x].IsChecked)
                                            {
                                                if (mState.TrackList[x].RoleItem.Name.Contains(txtFindWhat.Text))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "TrackList (Property)", listWorkFlows[y].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mState;
                                                    mResult.Property = "TrackList";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }
                                        }

                                        for (int x = 0; x < mState.WorkList.Count; x++)
                                        {
                                            if (mState.WorkList[x].IsChecked)
                                            {
                                                if (mState.WorkList[x].RoleItem.Name.Contains(txtFindWhat.Text))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "WorkList (Property)", listWorkFlows[y].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mState;
                                                    mResult.Property = "WorkList";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }
                                        }
                                        break;
                                    }

                                case WorkflowItemType.CommonState:
                                    {
                                        CommonState mState = o as CommonState;

                                        for (int x = 0; x < mState.AppliesTo.Count; x++)
                                        {
                                            if (mState.AppliesTo[x].Text.Contains(txtFindWhat.Text))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mState.Name, "AppliesTo (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mState;
                                                mResult.Property = "AppliesTo";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }
                                        break;
                                    }
                                case WorkflowItemType.ArchiveState:
                                    {
                                        ArchiveState mState = o as ArchiveState;
                                        break;
                                    }

                                case WorkflowItemType.UserActivity:
                                    {
                                        UserActivity mActivity = o as UserActivity;
                                        if (mActivity.UseLinkedActivity.ToString().Contains(txtFindWhat.Text))
                                        {
                                            ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "UseLinkedActivity (Property)", listWorkFlows[y].WorkFlowName });
                                            lItem.Tag = o;
                                            listViewResults.Items.Add(lItem);

                                            ResultItem mResult = new ResultItem();
                                            mResult.GoObj = mActivity;
                                            mResult.Property = "UseLinkedActivity";
                                            mResult.resultType = "Property";
                                            listResults.Add(mResult);
                                        }

                                        if (mActivity.CustomForm != null)
                                        {
                                            if (mActivity.CustomForm.ToString().Contains(txtFindWhat.Text))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "CustomForm (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mActivity;
                                                mResult.Property = "CustomForm";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }

                                        for (int x = 0; x < mActivity.Access.Count; x++)
                                        {
                                            if (mActivity.Access[x].IsChecked)
                                            {
                                                if (mActivity.Access[x].RoleItem.Name.Contains(txtFindWhat.Text))
                                                {
                                                    ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "Access (Property)", listWorkFlows[y].WorkFlowName });
                                                    lItem.Tag = o;
                                                    listViewResults.Items.Add(lItem);

                                                    ResultItem mResult = new ResultItem();
                                                    mResult.GoObj = mActivity;
                                                    mResult.Property = "Access";
                                                    mResult.resultType = "Property";
                                                    listResults.Add(mResult);
                                                }
                                            }
                                        }

                                        break;
                                    }
                                case WorkflowItemType.TimedActivity:
                                    {
                                        break;
                                    }

                                case WorkflowItemType.ExternalActivity:
                                    {
                                        ExternalActivity mActivity = o as ExternalActivity;

                                        if (mActivity.InvokeOnInstanceTarget != null)
                                        {
                                            if (mActivity.InvokeOnInstanceTarget.Contains(txtFindWhat.Text))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "ExternalActivityRaiseFolder (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mActivity;
                                                mResult.Property = "ExternalActivityRaiseFolder";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }

                                        if (mActivity.InvokedBy != null)
                                        {
                                            if (mActivity.InvokedBy.ExternalActivity.ToString().Contains(txtFindWhat.Text))
                                            {
                                                ListViewItem lItem = new ListViewItem(new string[] { mActivity.Name, "InvokedBy (Property)", listWorkFlows[y].WorkFlowName });
                                                lItem.Tag = o;
                                                listViewResults.Items.Add(lItem);

                                                ResultItem mResult = new ResultItem();
                                                mResult.GoObj = mActivity;
                                                mResult.Property = "InvokedBy";
                                                mResult.resultType = "Property";
                                                listResults.Add(mResult);
                                            }
                                        }
                                        break;
                                    }

                                case WorkflowItemType.ConditionalActivity:
                                    {
                                        break;
                                    }
                                case WorkflowItemType.Comment:
                                    {
                                        Comment mComment = o as Comment;

                                        if (mComment.CommentText.ToString().Contains(txtFindWhat.Text))
                                        {
                                            ListViewItem lItem = new ListViewItem(new string[] { mComment.Name, "CommentText (Property)", listWorkFlows[y].WorkFlowName });
                                            lItem.Tag = o;
                                            listViewResults.Items.Add(lItem);

                                            ResultItem mResult = new ResultItem();
                                            mResult.GoObj = mComment;
                                            mResult.Property = "CommentText";
                                            mResult.resultType = "Property";
                                            listResults.Add(mResult);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        private void ActivateFind()
        {
            if (listViewResults.SelectedIndices.Count > 0)
            {
                if (listViewResults.SelectedIndices[0] != -1)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (listViewResults.Items[listViewResults.SelectedIndices[0]].Tag != null)
                    {
                        GoObject g = listViewResults.Items[listViewResults.SelectedIndices[0]].Tag as GoObject;
                        if (g != null)
                        {
                            if (MainForm.App.GetCurrentView() != null)
                            {
                                GoObject gObj = listViewResults.Items[listViewResults.SelectedIndices[0]].Tag as GoObject;
                                MainForm.App.GetCurrentView().Document.SelectWorkFlow(gObj.Parent as WorkFlow);
                                MainForm.App.GetCurrentView().Selection.Clear();
                                MainForm.App.GetCurrentView().Selection.Add(listViewResults.Items[listViewResults.SelectedIndices[0]].Tag as GoObject);
                            }
                        }

                        else
                        {
                            ResultItem mItem = listViewResults.Items[listViewResults.SelectedIndices[0]].Tag as ResultItem;
                            if (mItem != null)
                            {
                                ActiproSoftware.SyntaxEditor.FindReplaceOptions mOptions = new FindReplaceOptions();
                                mOptions.FindText = txtFindWhat.Text;
                                mOptions.MatchCase = checkMatchCase.Checked;
                                mOptions.MatchWholeWord = false;
                                CodeView mCodeView;
                                bool mustSearchAgain = false;
                                if (MainForm.App.m_CodeView == null)
                                {
                                    mCodeView = new CodeView();
                                    mCodeView.Show(MainForm.App.dockPanel, WeifenLuo.WinFormsUI.DockState.DockBottom);
                                    MainForm.App.m_CodeView = mCodeView;
                                    MainForm.App.mnuItemCodeView.Checked = true;
                                    mustSearchAgain = true;
                                }
                                else
                                {
                                    MainForm.App.m_CodeView.Visible = true;
                                }

                                if (MainForm.App.GetCurrentView().Selection.Primary != mItem.GoObj)
                                {
                                    MainForm.App.GetCurrentView().Selection.Clear();
                                    MainForm.App.GetCurrentView().Selection.Add(mItem.GoObj);
                                    MainForm.App.m_CodeView.toolStripComboBoxCodeSection.Text = mItem.CodeSection;
                                }

                                MainForm.App.m_CodeView.Activate();
                                MainForm.App.m_CodeView.toolStripComboBoxCodeSection.Text = mItem.CodeSection;

                                MainForm.App.m_CodeView.syntaxEditor.SelectedView.Selection.SelectRange(mItem.CodePosition, txtFindWhat.Text.Length);
                                if (mustSearchAgain == true)
                                {
                                    FindText();
                                }
                            }
                        }

                        //else
                        //{
                        //    MainForm.App.GetCurrentView().Selection.Clear();
                        //    MainForm.App.GetCurrentView().Selection.Add(
                        //}
                    }
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void ActivateReplace()
        {
            if (listViewResults.SelectedIndices[0] != -1)
            {
                ActiproSoftware.SyntaxEditor.FindReplaceOptions mOptions = new FindReplaceOptions();
                mOptions.FindText = txtFindWhat.Text;
                mOptions.MatchCase = checkMatchCase.Checked;
                mOptions.MatchWholeWord = false;
                MainForm.App.m_CodeView.syntaxEditor.SelectedView.Selection.SelectRange(listResults[listViewResults.SelectedIndices[0]].CodePosition, txtReplaceFindWhat.Text.Length);
            }
        }

        private void cmdReplace_Click(object sender, EventArgs e)
        {
            if (MainForm.App.m_CodeView != null && txtReplaceFindWhat.Text.Length > 0)
            {
                ActiproSoftware.SyntaxEditor.FindReplaceOptions mOptions = new ActiproSoftware.SyntaxEditor.FindReplaceOptions();
                ActiproSoftware.SyntaxEditor.FindReplaceResultSet mResultSet = new FindReplaceResultSet();
                if (txtReplaceFindWhat.Text.Length > 0)
                {
                    listViewResults.Items.Clear();
                    listResults.Clear();
                    MainForm.App.m_CodeView.syntaxEditor.SelectedView.Selection.SelectRange(0, 0);
                    mOptions.FindText = txtReplaceFindWhat.Text;
                    mOptions.MatchCase = checkMatchCase.Checked;
                    mOptions.SearchHiddenText = true;
                    mOptions.MatchWholeWord = checkReplaceMatchWholeWord.Checked;
                    mResultSet = MainForm.App.m_CodeView.syntaxEditor.Document.FindReplace.FindAll(mOptions);
                    for (int x = 0; x < mResultSet.Count; x++)
                    {
                        ResultItem mItem = new ResultItem();
                        mItem.Name = "Code Text";
                        mItem.resultType = "Code";
                        mItem.CodePosition = mResultSet[x].StartOffset;
                        listResults.Add(mItem);
                        listViewResults.Items.Add(new ListViewItem(new string[] { "Code View", "Code", MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName }));
                    }
                }

                if (checkConfirmReplacement.Checked == false)
                {
                    MainForm.App.m_CodeView.syntaxEditor.SelectedView.Selection.SelectRange(0, 0);
                    mOptions.FindText = txtReplaceFindWhat.Text;
                    mOptions.ReplaceText = txtReplaceReplaceWithWhat.Text;
                    mOptions.MatchCase = checkMatchCase.Checked;
                    mOptions.SearchHiddenText = true;
                    mOptions.MatchWholeWord = checkReplaceMatchWholeWord.Checked;
                    mResultSet = MainForm.App.m_CodeView.syntaxEditor.Document.FindReplace.ReplaceAll(mOptions);
                    MessageBox.Show("Replaced " + listResults.Count.ToString() + " Occurrences", "Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information); listViewResults.Items.Clear();
                }
                else
                {
                    mOptions.FindText = txtReplaceFindWhat.Text;
                    mOptions.ReplaceText = txtReplaceReplaceWithWhat.Text;
                    mOptions.MatchCase = checkMatchCase.Checked;
                    mOptions.SearchHiddenText = true;
                    mOptions.MatchWholeWord = checkReplaceMatchWholeWord.Checked;
                    int ReplaceCount = 0;
                    for (int x = 0; x < listResults.Count; x++)
                    {
                        MainForm.App.m_CodeView.syntaxEditor.SelectedView.Selection.SelectRange(listResults[x].CodePosition, txtReplaceFindWhat.Text.Length);
                        FindReplaceResultSet mFindResultSet = MainForm.App.m_CodeView.syntaxEditor.Document.FindReplace.Find(mOptions, listResults[x].CodePosition);
                        FindReplaceResult mFindResult = mFindResultSet[0];

                        DialogResult mRes = MessageBox.Show("Confirm Replace?", "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (mRes == DialogResult.Yes)
                        {
                            ReplaceCount++;
                            MainForm.App.m_CodeView.syntaxEditor.Document.FindReplace.Replace(mOptions, mFindResult);
                            MainForm.App.setStatusBar(ReplaceCount.ToString() + " Replacements");
                        }
                        else if (mRes == DialogResult.Cancel)
                        {
                            break;
                        }
                        else if (mRes == DialogResult.No)
                        {
                            MainForm.App.setStatusBar("Skipped Replacement");
                        }
                    }
                    listViewResults.Items.Clear();
                    MainForm.App.setStatusBar("Ready");
                    MessageBox.Show("Replaced " + ReplaceCount.ToString() + " Occurrences", "Complete!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Please open the appropriate code view/specify text to be replaced first!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tabControlMain_TabIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboLookIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewResults.Items.Count > 0)
            {
                listViewResults.Items.Clear();
            }
        }

        private void txtFindWhat_TextChanged(object sender, EventArgs e)
        {
            if (listViewResults.Items.Count > 0)
            {
                listViewResults.Items.Clear();
            }
        }

        private void txtReplaceFindWhat_TextChanged(object sender, EventArgs e)
        {
            if (listViewResults.Items.Count > 0)
            {
                listViewResults.Items.Clear();
            }
        }

        private void checkReplaceMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            if (listViewResults.Items.Count > 0)
            {
                listViewResults.Items.Clear();
            }
        }

        private void checkReplaceMatchWholeWord_CheckedChanged(object sender, EventArgs e)
        {
            if (listViewResults.Items.Count > 0)
            {
                listViewResults.Items.Clear();
            }
        }

        private void checkMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            if (listViewResults.Items.Count > 0)
            {
                listViewResults.Items.Clear();
            }
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewResults.Items.Count > 0)
            {
                listViewResults.Items.Clear();
            }
        }

        private void listViewResults_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
        }

        private void listViewResults_ItemActivate(object sender, EventArgs e)
        {
            switch (tabControlMain.SelectedTab.Text)
            {
                case "Find":
                    {
                        ActivateFind();
                        break;
                    }
                case "Replace":
                    {
                        ActivateReplace();
                        break;
                    }
            }
        }

        private void frmFindReplace_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void radioAllWorkflows_CheckedChanged(object sender, EventArgs e)
        {
            if (radioAllWorkflows.Checked == true)
            {
                FindText();
            }
        }

        private void radioCurrentWorkFlowOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCurrentWorkFlowOnly.Checked == true)
            {
                FindText();
            }
        }

        private void frmFindReplace_Enter(object sender, EventArgs e)
        {
        }

        private void frmFindReplace_Activated(object sender, EventArgs e)
        {
            if (MainForm.App.m_CodeView != null)
            {
                MainForm.App.m_CodeView.SaveCode();
            }
        }
    }

    public class ResultItem
    {
        private string m_Name;
        private string m_Type;
        private GoObject m_Object;
        private int m_CodePosition = -1;
        private string m_Property;
        private string m_CodeSection;

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string resultType
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        public GoObject GoObj
        {
            get
            {
                return m_Object;
            }
            set
            {
                m_Object = value;
            }
        }

        public int CodePosition
        {
            get
            {
                return m_CodePosition;
            }
            set
            {
                m_CodePosition = value;
            }
        }

        public string Property
        {
            get
            {
                return m_Property;
            }
            set
            {
                m_Property = value;
            }
        }

        public string CodeSection
        {
            get
            {
                return m_CodeSection;
            }
            set
            {
                m_CodeSection = value;
            }
        }
    }
}
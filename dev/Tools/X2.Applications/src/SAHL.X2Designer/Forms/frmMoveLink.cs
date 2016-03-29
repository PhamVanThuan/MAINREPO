using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Items;

namespace SAHL.X2Designer.Forms
{
    public partial class frmMoveLink : Form
    {
        private BaseItem originalFromState = null;
        private BaseItem originalToState = null;
        private List<BaseActivity> lstActivites = new List<BaseActivity>();
        private int lstIndex = 0;
        private List<BaseActivity> lstOriginalFromStateActivities = new List<BaseActivity>();
        private List<BaseActivity> lstOriginalToStateActivities = new List<BaseActivity>();
        private BaseActivity selNode;

        public frmMoveLink(List<BaseActivity> lstSentActivities)
        {
            lstActivites = lstSentActivities;
            InitializeComponent();
        }

        private void frmMoveLink_Load(object sender, EventArgs e)
        {
            populateListBoxes();
        }

        private void populateListBoxes()
        {
            listFromNode.Items.Clear();
            listToNode.Items.Clear();
            selNode = lstActivites[lstIndex];
            this.Text = selNode.Text;
            if (selNode is CallWorkFlowActivity)
            {
                lblToState.Enabled = false;
                listToNode.Enabled = false;
            }

            if (selNode is ReturnWorkflowActivity)
            {
                lblFromState.Enabled = false;
                listFromNode.Enabled = false;
            }

            if (selNode != null && selNode is BaseActivity)
            {
                BaseActivity mNode22 = selNode as BaseActivity;
                if (mNode22 != null)
                {
                    switch (mNode22.WorkflowItemType)
                    {
                        case WorkflowItemType.UserActivity:
                            {
                                listFromNode.Items.Add("ClapperBoard");
                                break;
                            }
                        case WorkflowItemType.ExternalActivity:
                            {
                                listFromNode.Items.Add("ClapperBoard");
                                break;
                            }
                        case WorkflowItemType.TimedActivity:
                            {
                                break;
                            }
                        case WorkflowItemType.ConditionalActivity:
                            {
                                break;
                            }
                    }
                }
                foreach (BaseState bs in MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States)
                {
                    if ((bs is ArchiveState == false) && (selNode is ReturnWorkflowActivity == false))
                    {
                        listFromNode.Items.Add(bs.Name);
                    }
                    foreach (CustomLink l in selNode.Links)
                    {
                        bool found = false;
                        BaseItem bi = l.FromNode as BaseItem;
                        for (int y = 0; y < listFromNode.Items.Count; y++)
                        {
                            if (bi.Name.ToString() == listFromNode.Items[y].ToString())
                            {
                                listFromNode.SelectedIndex = y;
                                originalFromState = bi as BaseItem;
                                found = true;
                                break;
                            }
                            if (found == true)
                            {
                                break;
                            }
                        }
                    }

                    if (selNode is CallWorkFlowActivity == false)
                    {
                        listToNode.Items.Add(bs.Name);
                    }
                    foreach (CustomLink l in selNode.Links)
                    {
                        BaseItem bi2 = l.ToNode as BaseItem;
                        if (listToNode.Items.Count > 0)
                        {
                            if (bi2.Name.ToString() == listToNode.Items[listToNode.Items.Count - 1].ToString())
                            {
                                originalToState = bi2;

                                listToNode.SelectedIndex = listToNode.Items.Count - 1;
                                break;
                            }
                        }
                    }
                }

                listFromNode.Sorted = true;
                listToNode.Sorted = true;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            BaseActivity selNode = lstActivites[lstIndex];

            if (selNode is CallWorkFlowActivity
                || selNode is ReturnWorkflowActivity
                || originalFromState.Name != listFromNode.SelectedItem.ToString()
                || originalToState.Name != listToNode.SelectedItem.ToString())
            {
                if (lstIndex < lstActivites.Count)
                {
                    CreateLinks();
                    lstIndex++;
                    if (lstIndex < lstActivites.Count)
                    {
                        populateListBoxes();
                        return;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void CreateLinks()
        {
            MainForm.App.GetCurrentView().Document.StartTransaction();

            BaseActivity selNode = lstActivites[lstIndex];

            BaseItem mFromNode = null;
            BaseItem mToNode = null;
            bool okToCreateLink = true;
            foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
            {
                if (o is BaseState && listFromNode.SelectedItem != null)
                {
                    BaseItem mBaseItem = o as BaseItem;
                    if (mBaseItem.Name == listFromNode.SelectedItem.ToString())
                    {
                        mFromNode = mBaseItem;
                    }
                    if (listToNode.Items.Count > 0)
                    {
                        if (mBaseItem.Name == listToNode.SelectedItem.ToString())
                        {
                            mToNode = mBaseItem;
                        }
                    }
                }

                //new code
                if (o is ReturnWorkflowActivity && o == selNode)
                {
                    okToCreateLink = false;
                    BaseState bs = null;
                    for (int x = 0; x < MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States.Count; x++)
                    {
                        if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x].Name == listToNode.SelectedItem.ToString())
                        {
                            bs = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.States[x];
                        }
                    }
                    ReturnWorkflowActivity rwa = o as ReturnWorkflowActivity;
                    foreach (CustomLink l in rwa.Links)
                    {
                        GoPort p = null;
                        foreach (GoPort port in bs.Ports)
                        {
                            p = port;
                        }
                        if (p == null)
                        {
                            p = bs.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                            p.Size = new SizeF(55, 55);
                            p.Center = bs.Location;
                        }
                        l.FromPort = p;
                    }
                }

                if (listFromNode.SelectedItem != null)
                {
                    if (listFromNode.SelectedItem.ToString() == "ClapperBoard" && o is ClapperBoard)
                    {
                        mFromNode = o as BaseItem;
                    }
                }
            }

            if (mToNode != null)
            {
                if (mToNode.GetType() == typeof(CommonState) && mFromNode.GetType() != typeof(CommonState))
                {
                    MessageBox.Show("An activity cannot be linked from another state to a common state!", "Error");
                    okToCreateLink = false;
                }
            }
            if (okToCreateLink)
            {
                if (originalFromState != null || originalToState != null)
                {
                    foreach (GoObject o in selNode)
                    {
                        if (o.GetType() == typeof(MultiPortNodePort))
                        {
                            MultiPortNodePort p = o as MultiPortNodePort;

                            foreach (CustomLink l in p.Links)
                            {
                                if (l.ToNode as BaseItem == selNode as BaseItem)
                                {
                                    int portCount = 0;
                                    if (mFromNode == null) continue;

                                    foreach (GoPort prt in mFromNode.Ports)
                                    {
                                        portCount++;
                                    }
                                    if (portCount == 0)
                                    {
                                        MultiPortNodePort mpnp1 = (MultiPortNodePort)mFromNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                        mpnp1.Size = new SizeF(55, 55);
                                        mpnp1.Center = mFromNode.Location;
                                        l.FromPort = mpnp1;
                                    }
                                    else
                                    {
                                        MultiPortNodePort mPort = null; ;
                                        foreach (MultiPortNodePort prt in mFromNode.Ports)
                                        {
                                            mPort = prt;
                                            foreach (CustomLink link in mPort.Links)
                                            {
                                                if (link.ToNode is BaseState == false)
                                                {
                                                    lstOriginalFromStateActivities.Add(link.ToNode as BaseActivity);
                                                }
                                            }
                                            break;
                                        }
                                        l.FromPort = mPort;
                                    }
                                }
                                if (l.FromNode as BaseItem == selNode as BaseItem)
                                {
                                    int portCount = 0;
                                    if (mToNode != null)
                                    {
                                        foreach (MultiPortNodePort prt in mToNode.Ports)
                                        {
                                            portCount++;
                                        }
                                        if (portCount == 0)
                                        {
                                            MultiPortNodePort mpnp2 = (MultiPortNodePort)mToNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                                            mpnp2.Size = new SizeF(55, 55);
                                            mpnp2.Center = mToNode.Location;

                                            l.ToPort = mpnp2;
                                        }
                                        else
                                        {
                                            MultiPortNodePort mPort = null; ;
                                            foreach (MultiPortNodePort prt in mToNode.Ports)
                                            {
                                                mPort = prt;
                                                //foreach (CustomLink link in mPort.Links)
                                                //{
                                                //    if (link.FromNode is BaseState == false)
                                                //    {
                                                //        lstOriginalFromStateActivities.Add(link.FromNode as BaseActivity);
                                                //    }
                                                //}
                                                break;
                                            }
                                            l.ToPort = mPort;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    int portCount = 0;
                    MultiPortNodePort selPort = new MultiPortNodePort();
                    foreach (MultiPortNodePort p in selNode.Ports)
                    {
                        portCount++;
                        selPort = p;
                    }
                    if (portCount == 0)
                    {
                        MultiPortNodePort mpnp1 = (MultiPortNodePort)selNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                        mpnp1.Size = new SizeF(55, 55);
                        mpnp1.Center = selNode.Location;
                        selPort = mpnp1;
                    }
                    CustomLink l1 = new CustomLink();
                    CustomLink l2 = new CustomLink();
                    portCount = 0;
                    foreach (GoPort prt in mFromNode.Ports)
                    {
                        portCount++;
                    }
                    if (portCount == 0)
                    {
                        MultiPortNodePort mpnp1 = (MultiPortNodePort)mFromNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                        mpnp1.Size = new SizeF(55, 55);
                        mpnp1.Center = mFromNode.Location;
                        l1.FromPort = mpnp1;
                    }
                    else
                    {
                        MultiPortNodePort mPort = null; ;
                        foreach (MultiPortNodePort prt in mFromNode.Ports)
                        {
                            mPort = prt;
                            break;
                        }
                        l1.FromPort = mPort;
                    }
                    l1.ToPort = selPort;
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.InsertBefore(null, l1);
                    portCount = 0;
                    if (mToNode != null)
                    {
                        foreach (MultiPortNodePort prt in mToNode.Ports)
                        {
                            portCount++;
                        }
                        if (portCount == 0)
                        {
                            MultiPortNodePort mpnp2 = (MultiPortNodePort)mToNode.AddPort(1, new SizeF(55, 55), GoObject.NoHandle);
                            mpnp2.Size = new SizeF(55, 55);
                            mpnp2.Center = mToNode.Location;
                            l2.ToPort = mpnp2;
                        }
                        else
                        {
                            MultiPortNodePort mPort = null; ;
                            foreach (MultiPortNodePort prt in mToNode.Ports)
                            {
                                mPort = prt;
                                break;
                            }
                            l2.ToPort = mPort;
                        }
                    }
                    l2.FromPort = selPort;
                    l2.ToArrow = true;
                    MainForm.App.GetCurrentView().Document.CurrentWorkFlow.InsertBefore(null, l2);
                }

                BaseState fromNode = null;
                BaseActivity mNode = selNode as BaseActivity;
                if (originalFromState != null || originalToState != null)
                {
                    if (null != originalFromState)
                    {
                        foreach (CustomLink l in originalFromState.Links)
                        {
                            if (l.FromNode != mNode)
                            {
                                fromNode = l.FromNode as BaseState;
                                break;
                            }
                        }
                    }
                    if (fromNode != null)
                    {
                        foreach (CustomLink l in fromNode.Links)
                        {
                            if (l.ToNode != fromNode)
                            {
                                BaseActivity toActivity = l.ToNode as BaseActivity;
                                if (toActivity != null)
                                {
                                    if (toActivity.Priority > mNode.Priority)
                                    {
                                        toActivity.Priority--;
                                    }
                                }
                            }
                        }
                    }
                    if (null != originalFromState)
                    {
                        foreach (GoPort port in originalFromState.Ports)
                        {
                            if (port.LinksCount == 0)
                            {
                                port.Remove();
                            }
                        }
                    }

                    if (originalToState != null)
                    {
                        foreach (GoPort port in originalToState.Ports)
                        {
                            if (port.LinksCount == 0)
                            {
                                port.Remove();
                            }
                        }
                    }

                    int priorityToUse = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.getNextActivityPriority(mNode, mFromNode);
                    mNode.Priority = priorityToUse - 1;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mNode);
                    foreach (CustomLink l in mNode.Links)
                    {
                        l.CalculateStroke();
                    }
                }
                else
                {
                    int priorityToUse = MainForm.App.GetCurrentView().Document.CurrentWorkFlow.getNextActivityPriority(mNode, mFromNode);
                    mNode.Priority = priorityToUse - 1;
                    MainForm.App.GetCurrentView().Selection.Clear();
                    MainForm.App.GetCurrentView().Selection.Add(mNode);
                    foreach (CustomLink l in mNode.Links)
                    {
                        l.CalculateStroke();
                    }
                }
            }

            //Make sure the moved item is part of the correct list

            BaseItem movedItem = MainForm.App.GetCurrentView().Selection.Primary as BaseItem;
            for (int x = 0; x < lstOriginalFromStateActivities.Count; x++)
            {
                if (lstOriginalFromStateActivities[x] == movedItem)
                {
                    lstOriginalFromStateActivities.RemoveAt(x);
                    lstOriginalToStateActivities.Add(movedItem as BaseActivity);
                    break;
                }
            }

            //Sort the lists according to priority and then reset the priorities for each activity

            //int r = lstOriginalFromStateActivities.Count;
            //do
            //{
            //    int last_Exchange = 0;
            //    for (int y = 0; y < lstOriginalFromStateActivities.Count - 1; y++)
            //    {
            //        if (lstOriginalFromStateActivities[y].Priority > lstOriginalFromStateActivities[y + 1].Priority)
            //        {
            //            BaseActivity holdActivity = lstOriginalFromStateActivities[y];
            //            lstOriginalFromStateActivities[y] = lstOriginalFromStateActivities[y + 1];
            //            lstOriginalFromStateActivities[y + 1] = holdActivity;
            //            last_Exchange = y;
            //        }
            //        r = last_Exchange;
            //    }

            //}
            //while (r > 0 && lstOriginalFromStateActivities.Count > 1);

            //for (int z = 0; z < lstOriginalFromStateActivities.Count; z++)
            //{
            //    lstOriginalFromStateActivities[z].Priority = z + 1;
            //}

            //// populate the lstOriginalToStateActivities list
            //if (selNode is CallWorkFlowActivity == false && selNode is ReturnWorkflowActivity == false)
            //{
            //    foreach (GoPort p in originalToState.Ports)
            //    {
            //        foreach (GoLink l in p.Links)
            //        {
            //            if (l.FromPort.Node is BaseActivity)
            //            {
            //                lstOriginalToStateActivities.Add(l.FromPort.Node as BaseActivity);
            //            }
            //        }
            //    }
            //}
            //r = lstOriginalToStateActivities.Count;
            //do
            //{
            //    int last_Exchange = 0;
            //    for (int y = 0; y < lstOriginalToStateActivities.Count - 1; y++)
            //    {
            //        if (lstOriginalToStateActivities[y].Priority > lstOriginalToStateActivities[y + 1].Priority)
            //        {
            //            BaseActivity holdActivity = lstOriginalToStateActivities[y];
            //            lstOriginalToStateActivities[y] = lstOriginalToStateActivities[y + 1];
            //            lstOriginalToStateActivities[y + 1] = holdActivity;
            //            last_Exchange = y;
            //        }
            //        r = last_Exchange;
            //    }

            //}
            //while (r > 0 && lstOriginalToStateActivities.Count > 1);

            //for (int z = 0; z < lstOriginalToStateActivities.Count; z++)
            //{
            //    lstOriginalToStateActivities[z].Priority = z + 1;
            //}

            MainForm.App.GetCurrentView().Document.FinishTransaction("Complete move activity");
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            if (originalFromState == null || originalToState == null)
            {
                if (selNode.WorkflowItemType != WorkflowItemType.ReturnWorkFlowActivity && selNode.WorkflowItemType != WorkflowItemType.CallWorkFlowActivity)
                {
                    DialogResult res = MessageBox.Show("Cancelling this form will cause the pasted activities to be deleted ! \n Are you sure you want to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        for (int x = 0; x < lstActivites.Count; x++)
                        {
                            lstActivites[x].Remove();
                            this.DialogResult = DialogResult.Cancel;
                            Close();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    Close();
                }
            }
        }
    }
}
using System;
using System.Windows.Forms;
using Northwoods.Go;

namespace SAHL.X2Designer.Items
{
    public class ReturnWorkflowActivity : BaseActivity
    {
        public ReturnWorkflowActivity(String ID)
        {
            m_Id = ID;
            m_ItemBaseType = WorkflowItemBaseType.Activity;
            m_ItemType = WorkflowItemType.ReturnWorkFlowActivity;
            m_IconName = "new_workFlow_32x32x32b";
        }

        public override bool HasStageTransitions()
        {
            return false;
        }

        public override void Copy(BaseItem newItem)
        {
            ReturnWorkflowActivity mActivity = newItem as ReturnWorkflowActivity;
        }

        public override object Properties
        {
            get
            {
                return new ReturnWorkflowActivityProperties(this);
            }
        }
    }

    [Serializable]
    public class ReturnWorkflowActivityProperties : BaseProperties
    {
        ReturnWorkflowActivity mOwner;

        public ReturnWorkflowActivityProperties(ReturnWorkflowActivity Owner)
            : base(Owner)
        {
            mOwner = Owner;
        }

        public string Message
        {
            get
            {
                return (mOwner).Message;
            }
            set
            {
                (mOwner).Message = value;
            }
        }

        public string Name
        {
            get
            {
                return (mOwner).Name;
            }
            set
            {
                bool found = false;
                if (MainForm.App.GetCurrentView() != null)
                {
                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        BaseActivity mActivity = o as BaseActivity;
                        if (mActivity != null)
                        {
                            if (mActivity.Name == value)
                            {
                                found = true;
                            }
                        }
                    }
                }
                if (found == false)
                {
                    (mOwner).Name = value;
                }
                else
                {
                    MessageBox.Show("An Activity with this name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }
    }
}
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Documents;

namespace SAHL.X2Designer.Items
{
    [Serializable]
    public class CallWorkFlowActivity : BaseActivity
    {
        private WorkFlow m_WorkFlowToCall;
        private BaseActivity m_ActivityToCall;
        private BaseActivity _ReturnActivity;

        public CallWorkFlowActivity(String ID)
        {
            m_Id = ID;
            m_ItemBaseType = WorkflowItemBaseType.Activity;
            m_ItemType = WorkflowItemType.CallWorkFlowActivity;
            m_IconName = "workflow_activity";
        }

        #region Properties

        public WorkFlow WorkFlowToCall
        {
            get
            {
                return m_WorkFlowToCall;
            }
            set
            {
                m_WorkFlowToCall = value;
            }
        }

        public BaseActivity ActivityToCall
        {
            get
            {
                return m_ActivityToCall;
            }
            set
            {
                m_ActivityToCall = value;
            }
        }

        public BaseActivity ReturnActivity
        {
            get
            {
                return _ReturnActivity;
            }
            set
            {
                _ReturnActivity = value;
            }
        }

        #endregion Properties

        #region Overrides

        public override object Properties
        {
            get
            {
                return new WorkFlowActivityProperties(this, m_WorkFlowToCall);
            }
        }

        public override void Copy(BaseItem newItem)
        {
            CallWorkFlowActivity mActivity = newItem as CallWorkFlowActivity;
            if (mActivity != null)
            {
                if (this.WorkFlowToCall != null)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.WorkFlowName != this.WorkFlowToCall.WorkFlowName)
                    {
                        mActivity.WorkFlowToCall = this.WorkFlowToCall;
                        mActivity.ActivityToCall = this.ActivityToCall;
                    }
                }
            }
        }

        #endregion Overrides

        public override bool HasStageTransitions()
        {
            return false;
        }
    }

    [Serializable]
    public class WorkFlowActivityProperties : BaseProperties
    {
        CallWorkFlowActivity mOwner;
        public WorkFlow selectedWorkFlow;
        public BaseActivity m_ActivityToCall;
        public BaseActivity _ReturnActivity;

        public WorkFlowActivityProperties(CallWorkFlowActivity Owner, WorkFlow WorkFlowToCall)
            : base(Owner)
        {
            mOwner = Owner;
            selectedWorkFlow = WorkFlowToCall;
            m_ActivityToCall = Owner.ActivityToCall;
            _ReturnActivity = Owner.ReturnActivity;
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

        [Description("Indicates the workflow to which the action must be linked."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.WorkFlowToCallDropDownEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.WorkFlowToCallConvertor))]
        public WorkFlow WorkFlowToCall
        {
            get
            {
                return ((CallWorkFlowActivity)m_Owner).WorkFlowToCall;
            }
            set
            {
                if (value != null)
                {
                    ((CallWorkFlowActivity)m_Owner).WorkFlowToCall = value;
                    selectedWorkFlow = value;
                }
                else
                {
                    if (selectedWorkFlow != null)
                    {
                        ((CallWorkFlowActivity)m_Owner).WorkFlowToCall = selectedWorkFlow;
                    }
                }
            }
        }

        [Description("Indicates which action must be called from the selected workflow."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.ActivityToCallDropDownEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.ActivityToCallConvertor))]
        public BaseActivity ActivityToCall
        {
            get
            {
                UiEditors.ActivityToCallDropDownEditor.workFlowToSearch = selectedWorkFlow;
                return ((CallWorkFlowActivity)m_Owner).ActivityToCall;
            }
            set
            {
                if (value != null)
                {
                    ((CallWorkFlowActivity)m_Owner).ActivityToCall = value;
                }
                else
                {
                    if (m_ActivityToCall != null)
                    {
                        ((CallWorkFlowActivity)m_Owner).ActivityToCall = m_ActivityToCall;
                    }
                }
            }
        }

        [Description("Indicates which activity must be called on return to this workflow."), Category(CommonProps.ActivityProps)]
        [EditorAttribute(typeof(UiEditors.ActivityToReturnDropDownEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.ExternalActivityConvertor))]
        public BaseActivity ReturnActivity
        {
            get
            {
                UiEditors.ActivityToReturnDropDownEditor.workFlow = MainForm.App.GetCurrentView().Document.CurrentWorkFlow;
                //selectedWorkFlow;
                return ((CallWorkFlowActivity)m_Owner).ReturnActivity;
            }
            set
            {
                if (value != null)
                {
                    ((CallWorkFlowActivity)m_Owner).ReturnActivity = value;
                }
                else
                {
                    if (ReturnActivity != null)
                    {
                        ((CallWorkFlowActivity)m_Owner).ReturnActivity = ReturnActivity;
                    }
                }
            }
        }

        public bool SplitWorkflow
        {
            get
            {
                return mOwner.SplitWorkFlow;
            }
            set
            {
                mOwner.SplitWorkFlow = value;
            }
        }
    }
}
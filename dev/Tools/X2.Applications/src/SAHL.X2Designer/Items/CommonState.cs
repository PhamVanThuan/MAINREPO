using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using Northwoods.Go;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.UiEditors;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// 1) WorkList
    /// </summary>
    internal class CommonState : BaseStateWithLists
    {
        protected AppliedStatesCollection m_AppliesTo;

        public CommonState()
        {
            m_ItemBaseType = WorkflowItemBaseType.State;
            m_ItemType = WorkflowItemType.CommonState;
            m_IconName = "common_state_32x32x32b";
            m_AppliesTo = new AppliedStatesCollection();
        }

        public AppliedStatesCollection AppliesTo
        {
            get
            {
                return m_AppliesTo;
            }
        }

        #region Overrides

        public override object Properties
        {
            get
            {
                return new CommonStateProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        public override void Copy(BaseItem newItem)
        {
            CommonState mState = newItem as CommonState;
            if (mState != null)
            {
                foreach (BaseState s in this.AppliesTo)
                {
                    if (MainForm.App.GetCurrentView().Document.CurrentWorkFlow.CheckIfItemExists(s))
                    {
                        mState.AppliesTo.Add(s);
                    }
                }
            }
        }

        #endregion Overrides
    }

    [Serializable]
    public class CommonStateProperties : BaseProperties
    {
        CheckedListCollection m_AppliesTo;

        public CommonStateProperties(BaseItem Owner, Documents.ProcessDocument pDoc)
            : base(Owner)
        {
            m_AppliesTo = new CheckedListCollection();

            foreach (GoObject obj in pDoc.CurrentWorkFlow)
            {
                if (obj as BaseState != null)
                {
                    BaseState bs = obj as BaseState;
                    if ((bs is CommonState) == false
                     && (bs is ArchiveState) == false)
                    {
                        CheckedListItem i = new CheckedListItem();
                        i.Name = bs.Label.Text;
                        m_AppliesTo.Add(i);
                    }
                }
            }
        }

        [Description("Edits the states this common state is applied to."), Category(CommonProps.StateProps)]
        [Editor(typeof(ManageAppliedToStatesEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CheckedListConvertor))]
        public AppliedStatesCollection AppliesTo
        {
            get
            {
                return ((CommonState)m_Owner).AppliesTo;
            }
        }

        [Description("The name of the common state."), Category(CommonProps.GeneralProps)]
        public string StateName
        {
            get
            {
                return ((CommonState)m_Owner).Name;
            }
            set
            {
                ((CommonState)m_Owner).Name = value;
            }
        }
    }

    [Serializable]
    public class AppliedStatesCollection : List<BaseState>
    {
    }
}
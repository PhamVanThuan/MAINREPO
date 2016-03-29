using System;
using System.Text;
using System.Text.RegularExpressions;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Documents;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// Required Properties
    /// 1) Auto Forward To
    /// </summary>
    ///
    [Serializable]
    public class SystemState : BaseStateWithLists
    {
        private bool m_UseAutoForward = false;
        private string m_AutoForwardTo = "";

        public SystemState()
        {
            m_ItemBaseType = WorkflowItemBaseType.State;
            m_ItemType = WorkflowItemType.SystemState;
            m_IconName = "system_state_32x32x32b";
        }

        #region Properties

        public bool UseAutoForward
        {
            get
            {
                return m_UseAutoForward;
            }
            set
            {
                m_UseAutoForward = value;
            }
        }

        #endregion Properties

        #region Overrides

        public override object Properties
        {
            get
            {
                return new SystemStateProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                //       UpdateCodeSectionData(base.Name, value);

                base.Name = value;
            }
        }

        protected override string[] GetAvailableCodeSectionsInternal()
        {
            if (m_UseAutoForward)
            {
                base.m_AvailableCodeSections = new string[] { "OnEnter", "OnExit", "AutoForwardTo" };
            }
            else
            {
                base.m_AvailableCodeSections = new string[] { "OnEnter", "OnExit" };
            }

            return m_AvailableCodeSections;
        }

        public override string GetCodeSectionData(string CodeSectionName)
        {
            if (CodeSectionName == "AutoForwardTo")
            {
                if (m_AutoForwardTo == "" && MainForm.App.isCompiling == false)
                {
                    m_AutoForwardTo = "";

                    // generate the header
                    StringBuilder SB = new StringBuilder();
                    SB.Append(X2Generator.GenerateStateAutoForwardHeader(this, MainForm.App.GetCurrentView().Document));
                    SB.AppendLine("\treturn string.Empty;");
                    SB.AppendLine("}");
                    m_AutoForwardTo = SB.ToString();
                }
                return m_AutoForwardTo;
            }
            else
                return base.GetCodeSectionData(CodeSectionName);
        }

        public override void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
            if (CodeSectionName == "AutoForwardTo")
            {
                m_AutoForwardTo = SectionData;
            }
            else
                base.SetCodeSectionData(CodeSectionName, SectionData);
        }

        public override void UpdateCodeSectionData(string OldValue, string NewValue)
        {
            Regex MCN = new Regex("\\b" + OldValue + "\\b");
            m_AutoForwardTo = MCN.Replace(m_AutoForwardTo, NewValue);
            X2Generator.ReplaceStateAutoForwardHeader(OldValue, NewValue, ref m_AutoForwardTo);
            base.UpdateCodeSectionData(OldValue, NewValue);
        }

        public override void Copy(BaseItem newItem)
        {
            SystemState mState = newItem as SystemState;
            bool useAutoForward = this.UseAutoForward;
            mState.UseAutoForward = useAutoForward;
        }

        #endregion Overrides
    }

    [Serializable]
    public class SystemStateProperties : NamedStateProperties
    {
        //       private SystemState m_Owner;
        public SystemStateProperties(SystemState Owner, ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            m_Owner = Owner;
        }

        public bool UseAutoForward
        {
            get
            {
                return ((SystemState)m_Owner).UseAutoForward;
            }
            set
            {
                ((SystemState)m_Owner).UseAutoForward = value;
            }
        }
    }
}
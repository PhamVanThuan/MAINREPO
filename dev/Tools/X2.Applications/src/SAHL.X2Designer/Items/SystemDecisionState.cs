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
    public class SystemDecisionState : BaseStateWithLists
    {
        private string m_AutoForwardTo = "";

        public SystemDecisionState()
        {
            m_ItemBaseType = WorkflowItemBaseType.State;
            m_ItemType = WorkflowItemType.SystemDecisionState;
            m_IconName = "system_decision_32x32x32b";
        }

        #region Properties

        #endregion Properties

        #region Overrides

        public override object Properties
        {
            get
            {
                return new SystemDecisionStateProperties(this, MainForm.App.GetCurrentView().Document);
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
                //   UpdateCodeSectionData(base.Name, value);

                base.Name = value;
            }
        }

        protected override string[] GetAvailableCodeSectionsInternal()
        {
            base.m_AvailableCodeSections = new string[] { "OnEnter", "OnExit" };

            return m_AvailableCodeSections;
        }

        public override string GetCodeSectionData(string CodeSectionName)
        {
            if (CodeSectionName == "AutoForwardTo")
            {
                if (m_AutoForwardTo == "" && MainForm.App.isCompiling == false)
                {
                    m_AutoForwardTo = "";
                }
                // generate the header
                StringBuilder SB = new StringBuilder();
                SB.Append(X2Generator.GenerateStateAutoForwardHeader(this, MainForm.App.GetCurrentView().Document));
                SB.AppendLine("\treturn string.Empty;");
                SB.AppendLine("}");
                m_AutoForwardTo = SB.ToString();

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

        #endregion Overrides
    }

    [Serializable]
    public class SystemDecisionStateProperties : NamedStateProperties
    {
        //       private SystemState m_Owner;
        public SystemDecisionStateProperties(SystemDecisionState Owner, ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            m_Owner = Owner;
        }
    }
}
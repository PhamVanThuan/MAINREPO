using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.UiEditors;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// this is the starting point for any process,
    /// it only has an outgoing port, nothing can attach to it inbound
    /// </summary>

    [Serializable]
    public class ClapperBoard : BaseItem, ISecurity_HasLimitAccessTo
    {
        protected string m_Subject = "";
        protected RolesCollectionItem m_LimitAccessTo = null;
        protected List<string> m_StateOrder;
        protected CustomVariableItem m_KeyVariable;
        protected static string m_Name = "ClapperBoard";

        public ClapperBoard()
        {
            m_IconName = "starting_point_32x32x32b";
            m_AvailableCodeSections = new string[0];
        }

        #region Properties

        public RolesCollectionItem LimitAccessTo
        {
            get
            {
                return m_LimitAccessTo;
            }
            set
            {
                m_LimitAccessTo = value;
            }
        }

        public List<string> StateOrder
        {
            get
            {
                return m_StateOrder;
            }
        }

        public string Subject
        {
            get
            {
                return m_Subject;
            }
            set
            {
                m_Subject = value;
            }
        }

        public CustomVariableItem KeyVariable
        {
            get
            {
                return m_KeyVariable;
            }
            set
            {
                m_KeyVariable = value;
            }
        }

        public override object Properties
        {
            get
            {
                return new ClapperBoardProperties(this);
            }
        }

        public override string Name
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

        #endregion Properties
    }

    [Serializable]
    public class ClapperBoardProperties : BaseProperties
    {
        protected RolesCollectionItem m_LimitAccessTo;
        protected string returnedAccessTo;
        private CustomVariableItem m_KeyVariable = null;
        //        private string[] StrUsers;

        List<String> m_StateOrder;

        public ClapperBoardProperties(BaseItem Owner)
            : base(Owner)
        {
            m_StateOrder = ((ClapperBoard)Owner).StateOrder;
            m_LimitAccessTo = ((ClapperBoard)Owner).LimitAccessTo;
            m_KeyVariable = ((ClapperBoard)Owner).KeyVariable;
        }

        [Description("The name of the workflow."), Category(CommonProps.GeneralProps)]
        public string WorkFlowName
        {
            get
            {
                return ((ProcessDocument)m_Owner.Document).CurrentWorkFlow.WorkFlowName;
            }
            set
            {
                if (value.Length < 1)
                {
                    MessageBox.Show("The name cannot be zero length!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                bool found = false;
                if (MainForm.App.GetCurrentView() != null)
                {
                    foreach (GoObject o in MainForm.App.GetCurrentView().Document)
                    {
                        WorkFlow mWorkFlow = o as WorkFlow;
                        if (mWorkFlow != null)
                        {
                            if (mWorkFlow.WorkFlowName == value)
                            {
                                found = true;
                            }
                        }
                    }
                }
                if (found == false)
                {
                    ((ProcessDocument)m_Owner.Document).CurrentWorkFlow.WorkFlowName = value;
                }
                else
                {
                    MessageBox.Show("A WorkFlow with this name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        [Description("Subject for the workflow."), Category(CommonProps.GeneralProps)]
        public string Subject
        {
            get
            {
                return ((ClapperBoard)m_Owner).Subject;
            }
            set
            {
                ((ClapperBoard)m_Owner).Subject = value;
            }
        }

        [Description("Orders the states in the workflow."), Category(CommonProps.WorkFlowProps)]
        [Editor(typeof(StateOrderPropertiesEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CheckedListConvertor))]
        public List<String> StateOrder
        {
            get
            {
                return m_StateOrder;
            }
        }

        [Description("Limits access to the workflow."), Category(CommonProps.WorkFlowProps)]
        [EditorAttribute(typeof(UiEditors.LimitAccessToEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(LimitAccessToConvertor))]
        public RolesCollectionItem LimitAccessTo
        {
            get
            {
                return ((ClapperBoard)m_Owner).LimitAccessTo;
            }
            set
            {
                if (value != null)
                {
                    ((ClapperBoard)m_Owner).LimitAccessTo = value;
                }
                else
                {
                    if (m_LimitAccessTo != null)
                    {
                        ((ClapperBoard)m_Owner).LimitAccessTo = m_LimitAccessTo;
                    }
                }
            }
        }

        [Description("Key Variable for the workflow."), Category(CommonProps.WorkFlowProps)]
        [EditorAttribute(typeof(UiEditors.KeyVariableDropDownListEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(UiEditors.KeyVariableConvertor))]
        public CustomVariableItem KeyVariable
        {
            get
            {
                return ((ClapperBoard)m_Owner).KeyVariable;
            }
            set
            {
                if (value != null)
                {
                    ((ClapperBoard)m_Owner).KeyVariable = value;
                }
                else
                {
                    if (m_KeyVariable != null)
                    {
                        ((ClapperBoard)m_Owner).KeyVariable = m_KeyVariable;
                    }
                }
            }
        }
    }
}
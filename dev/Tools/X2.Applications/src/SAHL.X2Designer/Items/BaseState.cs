using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Northwoods.Go;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Forms;
using SAHL.X2Designer.Misc;
using SAHL.X2Designer.UiEditors;

namespace SAHL.X2Designer.Items
{
    /// <summary>
    /// Required Properties
    /// 1) State Name
    /// 2) OnEnter State
    /// 3) OnExit State
    /// </summary>
    ///
    [Serializable]
    public class BaseState : BaseItem, IPopupMenu
    {
        protected string m_OnEnter = "";
        protected string m_OnExit = "";
        int _sequence = int.MaxValue;
        internal static string OnEnter = "OnEnter";
        internal static string OnExit = "OnExit";
        private Guid x2ID = Guid.Empty;

        public BaseState()
        {
            m_AvailableCodeSections = new string[] { OnEnter, OnExit };
            this.AutoRescales = false;
        }

        #region Properties

        #endregion Properties

        #region Overrides

        public override string GetCodeSectionData(string CodeSectionName)
        {
            StringBuilder SB = new StringBuilder();
            if (CodeSectionName == "OnEnter")
            {
                if (m_OnEnter == "")
                {
                    // generate the header
                    SB.Append(X2Generator.GenerateStateEnterHeader(this, MainForm.App.GetCurrentView().Document));
                    SB.AppendLine("\treturn true;");
                    SB.AppendLine("}");
                    m_OnEnter = SB.ToString();
                }
                return m_OnEnter;
            }
            else
                if (CodeSectionName == "OnExit")
                {
                    if (m_OnExit == "")
                    {
                        // generate the header
                        SB.Append(X2Generator.GenerateStateExitHeader(this, MainForm.App.GetCurrentView().Document));
                        SB.AppendLine("\treturn true;");
                        SB.AppendLine("}");
                        m_OnExit = SB.ToString();
                    }
                    return m_OnExit;
                }
            return "";
        }

        public override string RefreshCodeSectionData(string CodeSectionName)
        {
            StringBuilder SB = new StringBuilder();
            if (CodeSectionName == "OnEnter")
            {
                if (m_OnEnter != "")
                {
                    m_OnEnter = "";
                }
                // generate the header
                SB.Append(X2Generator.GenerateStateEnterHeader(this, MainForm.App.GetCurrentView().Document));
                SB.AppendLine("\treturn true;");
                SB.AppendLine("}");
                m_OnEnter = SB.ToString();

                return m_OnEnter;
            }
            else
                if (CodeSectionName == "OnExit")
                {
                    if (m_OnExit != "")
                    {
                        m_OnExit = "";
                    }
                    // generate the header
                    SB.Append(X2Generator.GenerateStateExitHeader(this, MainForm.App.GetCurrentView().Document));
                    SB.AppendLine("\treturn true;");
                    SB.AppendLine("}");
                    m_OnExit = SB.ToString();

                    return m_OnExit;
                }
            return "";
        }

        public override void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
            if (CodeSectionName == "OnEnter")
            {
                m_OnEnter = SectionData;
            }
            else
                if (CodeSectionName == "OnExit")
                {
                    m_OnExit = SectionData;
                }
        }

        public override void UpdateCodeSectionData(string OldValue, string NewValue)
        {
            if (NewValue.Length > 0 && OldValue.Length > 0)
            {
                Regex MCN = new Regex("\\b" + OldValue + "\\b");

                m_OnEnter = MCN.Replace(m_OnEnter, NewValue);
                X2Generator.ReplaceStateEnterHeader(OldValue, NewValue, ref m_OnEnter);
                m_OnExit = MCN.Replace(m_OnExit, NewValue);
                X2Generator.ReplaceStateExitHeader(OldValue, NewValue, ref m_OnExit);
            }
        }

        public override object Properties
        {
            get
            {
                return new BaseStateProperties(this, MainForm.App.GetCurrentView().Document);
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
                //     UpdateCodeSectionData(base.Name, value);

                base.Name = value;
            }
        }

        public Guid X2ID
        {
            get
            {
                if (this.x2ID == null || this.x2ID == Guid.Empty)
                {
                    this.x2ID = Guid.NewGuid();
                }
                return this.x2ID;
            }
            set
            {
                this.x2ID = value;
            }
        }

        #endregion Overrides

        # region PopUp Menu

        public void populateMenu(GoContextMenu e)
        {
            e.MenuItems.Add(0, new MenuItem("-"));
            e.MenuItems.Add(0, new MenuItem("Paste", new EventHandler(this.cmdPaste)));
            e.MenuItems.Add(0, new MenuItem("Copy", new EventHandler(this.cmdCopy)));
            e.MenuItems.Add(0, new MenuItem("Cut", new EventHandler(this.cmdCut)));
        }

        public void cmdCut(Object sender, EventArgs e)
        {
            MainForm.App.GetCurrentView().Cut_Command(this, e);
            MainForm.App.GetCurrentView().clipBoardHasContents = true;
        }

        public void cmdCopy(Object sender, EventArgs e)
        {
            MainForm.App.GetCurrentView().Copy_Command(this, e);
            MainForm.App.GetCurrentView().clipBoardHasContents = true;
        }

        public void cmdPaste(Object sender, EventArgs e)
        {
            MainForm.App.GetCurrentView().Paste_Command(this, e);
            MainForm.App.GetCurrentView().clipBoardHasContents = false; ;
        }

        public void OnMenuClosed(GoContextMenu e)
        {
            e.MenuItems[0].Click -= new EventHandler(this.cmdCut);
            e.MenuItems[1].Click -= new EventHandler(this.cmdCopy);
            e.MenuItems[2].Click -= new EventHandler(this.cmdPaste);

            e.MenuItems.RemoveAt(0);
            e.MenuItems.RemoveAt(0);
            e.MenuItems.RemoveAt(0);
        }

        # endregion
    }

    [Serializable]
    public class BaseStateProperties : BaseProperties
    {
        public BaseStateProperties(BaseItem Owner, Documents.ProcessDocument pDoc)
            : base(Owner)
        {
        }
    }

    [Serializable]
    public class NamedStateProperties : BaseStateProperties
    {
        public NamedStateProperties(BaseState Owner, Documents.ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
        }

        [Description("The name of the state"), Category(CommonProps.GeneralProps)]
        public string StateName
        {
            get
            {
                return ((BaseState)m_Owner).Name;
            }
            set
            {
                bool found = false;
                if (MainForm.App.GetCurrentView() != null)
                {
                    if (value.Length < 1 || value.Length > 30)
                    {
                        MessageBox.Show("The length of the name must be between 1 and 30 characters!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    if (value.Contains("(") ||
                        value.Contains(")") ||
                        value.Contains("\\") ||
                        value.Contains("/") ||
                        value.Contains("&") ||
                        value.Contains("?") ||
                        value.Contains("%") ||
                        value.Contains("<") ||
                        value.Contains(">") ||
                        value.Contains("+") ||
                        value.Contains("!") ||
                        value.Contains("$") ||
                        value.Contains("@") ||
                        value.Contains("#") ||
                        value.Contains("*") ||
                        value.Contains("{") ||
                        value.Contains("}") ||
                        value.Contains(".") ||
                        value.Contains(",") ||
                        value.Contains("'") ||
                        value.Contains("/") ||
                        value.Contains("|") ||
                        value.Contains(":"))
                    {
                        MessageBox.Show("Not updated - there are illegal characters in the name!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    foreach (GoObject o in MainForm.App.GetCurrentView().Document.CurrentWorkFlow)
                    {
                        BaseState mState = o as BaseState;
                        if (mState != null)
                        {
                            if (mState.Name == value)
                            {
                                found = true;
                            }
                        }
                    }
                }
                if (found == false)
                {
                    ((BaseState)m_Owner).Name = value;
                }
                else
                {
                    MessageBox.Show("A State with this name already exists!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        [Description("ID to uniquely identify state"), Category(CommonProps.StateProps)]
        public Guid X2ID
        {
            get
            {
                return ((BaseState)m_Owner).X2ID;
            }
            set
            {
                ((BaseState)m_Owner).X2ID = value;
            }
        }
    }

    [Serializable]
    public class BaseStateWithLists : BaseState, ISecurity_HasWorkTrackLists
    {
        protected RoleInstanceCollection m_WorkList;
        protected RoleInstanceCollection m_TrackList;

        public BaseStateWithLists()
        {
            m_WorkList = new RoleInstanceCollection();
            m_TrackList = new RoleInstanceCollection();
            this.AutoRescales = false;
        }

        #region Properties

        public RoleInstanceCollection TrackList
        {
            get
            {
                return m_TrackList;
            }
        }

        public RoleInstanceCollection WorkList
        {
            get
            {
                return m_WorkList;
            }
        }

        #endregion Properties

        #region Overrides

        public override object Properties
        {
            get
            {
                return new BaseStateWithListsProperties(this, MainForm.App.GetCurrentView().Document);
            }
        }

        #endregion Overrides
    }

    [Serializable]
    public class BaseStateWithListsProperties : NamedStateProperties
    {
        public BaseStateWithListsProperties(BaseState Owner, Documents.ProcessDocument pDoc)
            : base(Owner, pDoc)
        {
            this.WorkList.RefreshRoles();
            this.TrackList.RefreshRoles();
        }

        [Description("Edits the state watch list."), Category(CommonProps.StateProps)]
        [Editor(typeof(ManageRolesEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CheckedListConvertor))]
        public RoleInstanceCollection TrackList
        {
            get
            {
                return ((BaseStateWithLists)m_Owner).TrackList;
            }
        }

        [Description("Edits the state todo list."), Category(CommonProps.StateProps)]
        [Editor(typeof(ManageRolesEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(CheckedListConvertor))]
        public RoleInstanceCollection WorkList
        {
            get
            {
                return ((BaseStateWithLists)m_Owner).WorkList;
            }
        }
    }
}
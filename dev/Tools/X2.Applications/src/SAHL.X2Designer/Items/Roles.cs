using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SAHL.X2Designer.CodeGen;
using SAHL.X2Designer.Documents;

namespace SAHL.X2Designer.Items
{
    [Serializable]
    public class RoleInstanceCollection : List<RoleInstance>
    {
        public RoleInstanceCollection()
        {
            RefreshRoles();
        }

        public void RefreshRoles()
        {
            if (MainForm.App.GetCurrentView() != null)
            {
                RolesCollectionItem[] roleArray = MainForm.App.GetCurrentView().Document.Roles.ToArray();
                for (int x = 0; x < roleArray.Length; x++)
                {
                    if (GetRoleInstanceByRoleItem(roleArray[x]) == null)
                    {
                        RoleInstance mRoleInstance = new RoleInstance();
                        mRoleInstance.RoleItem = roleArray[x];
                        base.Add(mRoleInstance);
                    }
                }
            }
        }

        public RoleInstance GetRoleInstanceByRoleItem(RolesCollectionItem RoleItem)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].RoleItem.Name == RoleItem.Name)
                    return base[i];
            }
            return null;
        }

        public RoleInstance GetByName(string RoleName)
        {
            for (int i = 0; i < base.Count; i++)
            {
                if (base[i].RoleItem.Name == RoleName)
                    return base[i];
            }
            return null;
        }
    }

    [Serializable]
    public class RoleInstance
    {
        bool m_IsChecked = false;
        RolesCollectionItem m_RoleItem;

        public RolesCollectionItem RoleItem
        {
            get
            {
                return m_RoleItem;
            }
            set
            {
                m_RoleItem = value;
            }
        }

        public bool IsChecked
        {
            get
            {
                return m_IsChecked;
            }
            set
            {
                m_IsChecked = value;
            }
        }

        public override string ToString()
        {
            return m_RoleItem.Name;
        }
    }

    [Serializable]
    public class RoleItemCollection : List<RolesCollectionItem>
    {
        public static ArrayList FixedRoles = new ArrayList(new string[] { "Originator", "Everyone", "WorkList", "TrackList" });

        public RoleItemCollection()
        {
            RolesCollectionItem RCI = null;
            for (int k = 0; k < FixedRoles.Count; k++)
            {
                RCI = new RolesCollectionItem();
                RCI.Name = FixedRoles[k].ToString();
                RCI.Description = RCI.Name + " Fixed Role";
                this.Add(RCI);
            }
        }
    }

    public class RolesComparer : IComparer<RolesCollectionItem>
    {
        public int Compare(RolesCollectionItem x, RolesCollectionItem y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }

    [Serializable]
    public class RolesCollectionItem : IWorkflowItem
    {
        string m_Name;
        string m_Description;
        bool m_IsDynamic;
        string m_OnGetRole = "";
        RoleType m_RoleType; // Global or WorkFlow
        WorkFlow m_WorkFlow;

        public string Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }

        public bool IsDynamic
        {
            get
            {
                return m_IsDynamic;
            }
            set
            {
                m_IsDynamic = value;
            }
        }

        public WorkFlow WorkFlowItem
        {
            get
            {
                return m_WorkFlow;
            }
            set
            {
                m_WorkFlow = value;
            }
        }

        public RoleType RoleType
        {
            get
            {
                return m_RoleType;
            }
            set
            {
                m_RoleType = value;
            }
        }

        #region IWorkflowItem Members

        public object Properties
        {
            get
            {
                if (!RoleItemCollection.FixedRoles.Contains(this.Name))
                {
                    return new GlobalRoleItemProperties(this, MainForm.App.GetCurrentView().Document);
                }
                else
                {
                    return null;
                }
            }
        }

        public string[] AvailableCodeSections
        {
            get
            {
                if (m_IsDynamic)
                    return new string[] { "OnGetRole" };
                else
                    return new string[] { };
            }
        }

        public string GetCodeSectionData(string CodeSectionName)
        {
            if (CodeSectionName == "OnGetRole")
            {
                if (m_OnGetRole == "")
                {
                    // generate the header
                    StringBuilder SB = new StringBuilder();
                    SB.Append(X2Generator.GenerateGetRoleHeader(this, MainForm.App.GetCurrentView().Document));
                    SB.AppendLine("\treturn string.Empty;");
                    SB.AppendLine("}");
                    m_OnGetRole = SB.ToString();
                }
                return m_OnGetRole;
            }
            return "";
        }

        public void SetCodeSectionData(string CodeSectionName, string SectionData)
        {
            if (CodeSectionName == "OnGetRole")
            {
                m_OnGetRole = SectionData;
            }
        }

        public void UpdateCodeSectionData(string OldValue, string NewValue)
        {
            Regex MCN = new Regex("\\b" + OldValue + "\\b");

            m_OnGetRole = MCN.Replace(m_OnGetRole, NewValue);
            X2Generator.ReplaceGetRoleHeader(OldValue, NewValue, ref m_OnGetRole);
        }

        public string RefreshCodeSectionData(string CodeSectionName)
        {
            if (CodeSectionName == "OnGetRole")
            {
                if (m_OnGetRole != "")
                {
                    m_OnGetRole = "";
                }
                // generate the header
                StringBuilder SB = new StringBuilder();
                SB.Append(X2Generator.GenerateGetRoleHeader(this, MainForm.App.GetCurrentView().Document));
                SB.AppendLine("\treturn string.Empty;");
                SB.AppendLine("}");
                m_OnGetRole = SB.ToString();
                return m_OnGetRole;
            }
            return "";
        }

        public void Copy(BaseItem newItem)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (m_IsDynamic)
                    UpdateCodeSectionData(m_Name, value);
                m_Name = value;
            }
        }

        public WorkflowItemBaseType WorkflowItemBaseType
        {
            get { return WorkflowItemBaseType.Role; }
        }

        public WorkflowItemType WorkflowItemType
        {
            get
            {
                if (m_IsDynamic)
                {
                    return WorkflowItemType.RoleDynamic;
                }
                else
                {
                    return WorkflowItemType.RoleStatic;
                }
            }
        }

        public List<string> ValidationErrors
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Validate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion IWorkflowItem Members

        #region IComparable Members

        public int CompareTo(object x)
        {
            String mName = x.ToString();
            return string.Compare(this.Name, mName);
        }

        #endregion IComparable Members

        public override string ToString()
        {
            if (WorkFlowItem != null)
                return string.Format("{0}-{1}", WorkFlowItem.WorkFlowName, Name);
            return Name;
        }
    }

    [Serializable]
    public class GlobalRoleItemProperties
    {
        RolesCollectionItem m_Owner;

        public GlobalRoleItemProperties(RolesCollectionItem Owner, ProcessDocument pDoc)
        {
            m_Owner = Owner;
        }

        [Description(""), Category(CommonProps.GeneralProps)]
        public String Name
        {
            get
            {
                return ((RolesCollectionItem)m_Owner).Name;
            }
            set
            {
                ((RolesCollectionItem)m_Owner).Name = value;
            }
        }

        public String Description
        {
            get
            {
                return ((RolesCollectionItem)m_Owner).Description;
            }
            set
            {
                ((RolesCollectionItem)m_Owner).Description = value;
            }
        }

        public bool IsDynamic
        {
            get
            {
                return ((RolesCollectionItem)m_Owner).IsDynamic;
            }
            set
            {
                TreeNode mNode = null;
                if (MainForm.App.m_BrowserView != null)
                {
                    mNode = MainForm.App.m_BrowserView.treeViewProc.SelectedNode;
                }

                ((RolesCollectionItem)m_Owner).IsDynamic = value;
                if (MainForm.App.m_BrowserView != null)
                {
                    MainForm.App.m_BrowserView.treeViewProc.SelectedNode = null;
                    MainForm.App.m_BrowserView.treeViewProc.SelectedNode = mNode;
                }
            }
        }
    }
}
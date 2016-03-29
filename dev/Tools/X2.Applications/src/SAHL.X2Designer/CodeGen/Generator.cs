using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Northwoods.Go;
using SAHL.X2Designer.Documents;
using SAHL.X2Designer.Items;
using SAHL.X2Designer.Misc;

namespace SAHL.X2Designer.CodeGen
{
    [Serializable]
    public class X2Generator
    {
        public static List<CodePosition> CodeSections = new List<CodePosition>();

        public static string CurrentCode;

        public static void ReplaceDataClassDeclarationHeader(string OldWorkFlowName, string NewWorkFlowName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + GetWorkFlowClassDataName(OldWorkFlowName) + " " + GetWorkFlowClassDataInstanceName(OldWorkFlowName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, GetWorkFlowClassDataName(NewWorkFlowName) + " " + GetWorkFlowClassDataInstanceName(NewWorkFlowName));
        }

        public static void ReplaceDataClassInstanceHeader(string OldWorkFlowName, string NewWorkFlowName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + GetWorkFlowClassDataInstanceName(OldWorkFlowName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, GetWorkFlowClassDataInstanceName(NewWorkFlowName));
        }

        #region Activity and State headers and replacements

        public static string GenerateActivityStartHeader(BaseActivity Activity, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = Activity.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public bool OnStartActivity_" + X2Generator.BuildItemName(Activity.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        public static string GenerateActivityMessageHeader(BaseActivity Activity, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = Activity.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public string GetActivityMessage_" + X2Generator.BuildItemName(Activity.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        public static void ReplaceActivityStartHeader(string OldActivityName, string NewActivityName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public bool OnStartActivity_" + X2Generator.BuildItemName(OldActivityName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public bool OnStartActivity_" + X2Generator.BuildItemName(NewActivityName));
        }

        public static void ReplaceActivityMessageHeader(string OldActivityName, string NewActivityName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public string GetActivityMessage_" + X2Generator.BuildItemName(OldActivityName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public string GetActivityMessage_" + X2Generator.BuildItemName(NewActivityName));
        }

        public static void ReplaceStageTransitionMessage(string oldMessage)
        {
        }

        public static string GenerateActivityCompleteHeader(BaseActivity Activity, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = Activity.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public bool OnCompleteActivity_" + X2Generator.BuildItemName(Activity.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages, ref string alertMessage)\r\n{");
            return SB.ToString();
        }

        public static void ReplaceActivityCompleteHeader(string OldActivityName, string NewActivityName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public bool OnCompleteActivity_" + X2Generator.BuildItemName(OldActivityName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public bool OnCompleteActivity_" + X2Generator.BuildItemName(NewActivityName));
        }

        public static void ReplaceStageTransitionHeader(string OldActivityName, string NewActivityName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public string GetStageTransition_" + X2Generator.BuildItemName(OldActivityName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public string GetStageTransition_" + X2Generator.BuildItemName(NewActivityName));
        }

        public static string GenerateActivityTimerHeader(BaseActivity Activity, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = Activity.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public DateTime GetActivityTime_" + X2Generator.BuildItemName(Activity.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        internal static string GenerateOnStageTransitionHeader(BaseActivity Activity, ProcessDocument pd)
        {
            WorkFlow mWorkFlow = Activity.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public string GetStageTransition_" + X2Generator.BuildItemName(Activity.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");

            return sb.ToString();
        }

        public static void ReplaceActivityTimerHeader(string OldActivityName, string NewActivityName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public DateTime GetActivityTime_" + X2Generator.BuildItemName(OldActivityName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public DateTime GetActivityTime_" + X2Generator.BuildItemName(NewActivityName));
        }

        public static string GenerateGetRoleHeader(RolesCollectionItem Role, ProcessDocument Document)
        {
            string WorkFlowName = "";
            if (Role.RoleType == RoleType.WorkFlow)
            {
                WorkFlowName = FixWorkFlowName(Role.WorkFlowItem.WorkFlowName);
            }
            StringBuilder SB = new StringBuilder();

            // decide if its a process or workflow role
            if (Role.WorkFlowItem == null)
            {
                SB.AppendLine("public string OnGetRole_" + X2Generator.BuildItemName(Role.Name) + "()\r\n{");
                return SB.ToString();
            }
            else
            {
                SB.AppendLine("public string OnGetRole_" + WorkFlowName + "_" + X2Generator.BuildItemName(Role.Name) + "(InstanceDataModel instance, " + GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages, string RoleName)\r\n{");
                return SB.ToString();
            }
        }

        public static void ReplaceGetRoleHeader(string OldRoleName, string NewRoleName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public string OnGetRole_" + X2Generator.BuildItemName(OldRoleName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public string OnGetRole_" + X2Generator.BuildItemName(NewRoleName));
        }

        public static string GenerateStateEnterHeader(BaseState State, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = State.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public bool OnEnter_" + X2Generator.BuildItemName(State.Name) + "(InstanceDataModel instance, " +
                  GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        public static void ReplaceStateEnterHeader(string OldStateName, string NewStateName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public bool OnEnter_" + X2Generator.BuildItemName(OldStateName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public bool OnEnter_" + X2Generator.BuildItemName(NewStateName));
        }

        public static string GenerateReturnHeader(BaseState State, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = State.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public bool OnReturn_" + X2Generator.BuildItemName(State.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        public static void ReplaceReturnHeader(string OldStateName, string NewStateName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public bool OnReturn_" + X2Generator.BuildItemName(OldStateName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public bool OnReturn_" + X2Generator.BuildItemName(NewStateName));
        }

        public static string GenerateStateExitHeader(BaseState State, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = State.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public bool OnExit_" + X2Generator.BuildItemName(State.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        public static void ReplaceStateExitHeader(string OldStateName, string NewStateName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public bool OnExit_" + X2Generator.BuildItemName(OldStateName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public bool OnExit_" + X2Generator.BuildItemName(NewStateName));
        }

        public static string GenerateStateAutoForwardHeader(BaseState State, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = State.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public string GetForwardStateName_" + X2Generator.BuildItemName(State.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        public static string GenerateStateAutoForwardHeader(BaseActivity Activity, ProcessDocument Document)
        {
            WorkFlow mWorkFlow = Activity.Parent as WorkFlow;
            string WorkFlowName = mWorkFlow.WorkFlowName;
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("public string GetForwardStateName_" + X2Generator.BuildItemName(Activity.Name) + "(InstanceDataModel instance, " +
                 GetWorkFlowInterfaceDataName(WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(WorkFlowName) + ", SAHL.Core.X2.IX2Params param, ISystemMessageCollection messages)\r\n{");
            return SB.ToString();
        }

        public static void ReplaceStateAutoForwardHeader(string OldStateName, string NewStateName, ref string ToBeReplaced)
        {
            Regex Reg = new Regex("\\b" + "public string GetForwardStateName_" + X2Generator.BuildItemName(OldStateName));
            ToBeReplaced = Reg.Replace(ToBeReplaced, "public string GetForwardStateName_" + X2Generator.BuildItemName(NewStateName));
        }

        public static void RenameWorkFlowInCodeHeaders(WorkFlow workFlow, string oldWorkFlowName)
        {
            string OldFixWorkFlowName = GetWorkFlowInterfaceDataName(oldWorkFlowName) + " " + GetWorkFlowClassDataInstanceName(oldWorkFlowName);
            string NewFixWorkFlowName = GetWorkFlowInterfaceDataName(workFlow.WorkFlowName) + " " + GetWorkFlowClassDataInstanceName(workFlow.WorkFlowName);
            foreach (GoObject o in workFlow)
            {
                BaseItem i = o as BaseItem;
                if (i != null)
                {
                    for (int x = 0; x < i.AvailableCodeSections.Length; x++)
                    {
                        string codeSection = i.AvailableCodeSections[x];
                        string code = i.GetCodeSectionData(codeSection);
                        Regex Reg = new Regex("\\b" + OldFixWorkFlowName + "\\b");
                        code = Reg.Replace(code, NewFixWorkFlowName);
                        i.SetCodeSectionData(codeSection, code);
                    }
                }
            }

            // any workflow dynamic roles
            foreach (RolesCollectionItem RCI in workFlow.Process.Roles)
            {
                if (RCI.WorkFlowItem == workFlow)
                {
                    string[] CodeSecs = RCI.AvailableCodeSections;
                    for (int k = 0; k < CodeSecs.Length; k++)
                    {
                        string NewCode = RCI.GetCodeSectionData(CodeSecs[k]);
                        X2Generator.ReplaceDataClassDeclarationHeader(oldWorkFlowName, workFlow.WorkFlowName, ref NewCode);
                        X2Generator.ReplaceDataClassInstanceHeader(oldWorkFlowName, workFlow.WorkFlowName, ref NewCode);
                        RCI.SetCodeSectionData(CodeSecs[k], NewCode);
                    }
                }
            }
        }

        #endregion Activity and State headers and replacements

        public static string GetWorkFlowClassName(string WorkFlowName)
        {
            return "X2" + FixWorkFlowName(WorkFlowName);
        }

        public static string GetWorkFlowClassDataName(string WorkFlowName)
        {
            string str = "X2" + FixWorkFlowName(WorkFlowName) + "_Data";
            return str;
        }

        public static string GetWorkFlowInterfaceDataName(string WorkFlowName)
        {
            string str = "IX2" + FixWorkFlowName(WorkFlowName) + "_Data";
            return str;
        }

        public static string GetWorkFlowClassDataInstanceName(string WorkFlowName)
        {
            return FixWorkFlowName(WorkFlowName) + "_Data";
        }

        public static string BuildItemName(string OriginalName)
        {
            OriginalName = OriginalName.Replace("'", "");
            OriginalName = OriginalName.Replace("-", "_");
            OriginalName = OriginalName.Replace("&", "_");
            OriginalName = OriginalName.Replace("?", "_");
            OriginalName = OriginalName.Replace("%", "_");
            OriginalName = OriginalName.Replace("<", "_");
            OriginalName = OriginalName.Replace(">", "_");
            if (OriginalName.Contains("/"))
                OriginalName = OriginalName.Replace("/", "_");
            OriginalName = OriginalName.Replace("+", "_");
            OriginalName = OriginalName.Replace(".", "_");
            OriginalName = OriginalName.Replace(",", "_");
            return OriginalName.Replace(" ", "_");
        }

        public static string FixWorkFlowName(string WorkFlowName)
        {
            char[] InvalidChars = Path.GetInvalidPathChars();
            for (int i = 0; i < InvalidChars.Length; i++)
                WorkFlowName = WorkFlowName.Replace(InvalidChars[i].ToString(), "_");
            WorkFlowName = WorkFlowName.Replace(" ", "_");
            WorkFlowName = WorkFlowName.Replace("?", "_");
            WorkFlowName = WorkFlowName.Replace("\\", "_");
            WorkFlowName = WorkFlowName.Replace("/", "_");
            WorkFlowName = WorkFlowName.Replace("&", "_");
            WorkFlowName = WorkFlowName.Replace("?", "_");
            WorkFlowName = WorkFlowName.Replace("%", "_");
            WorkFlowName = WorkFlowName.Replace("<", "_");
            WorkFlowName = WorkFlowName.Replace(">", "_");
            WorkFlowName = WorkFlowName.Replace("+", "_");
            if (WorkFlowName.Contains("/"))
                WorkFlowName = WorkFlowName.Replace("/", "_");
            return WorkFlowName;
        }

        public static void GenerateWorkFlowData(WorkFlow WorkFlow, StringBuilder SB, int tabs)
        {
            string WorkFlowDataClassName = FixWorkFlowName(WorkFlow.WorkFlowName) + "_Data";
            SB.AppendLine(AddTabs(tabs) + "public class " + GetWorkFlowClassDataName(WorkFlow.WorkFlowName) + " : MarshalByRefObject, " + GetWorkFlowInterfaceDataName(WorkFlow.WorkFlowName));
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "private bool m_HasChanges = false;");
            SB.AppendLine("");
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                string Type = GetTypeFromCustomVariable(WorkFlow.CustomVariables[k].Type);
                string Name = BuildItemName(WorkFlow.CustomVariables[k].Name);
                if (WorkFlow.CustomVariables[k].Type == CustomVariableType.ctdate)
                    SB.AppendLine(AddTabs(tabs) + "private " + Type + " m_" + Name + " = (DateTime)System.Data.SqlTypes.SqlDateTime.Now;");
                else
                    SB.AppendLine(AddTabs(tabs) + "private " + Type + " m_" + Name + ";");
                SB.AppendLine(AddTabs(tabs) + "public " + Type + " " + Name);
                SB.AppendLine(AddTabs(tabs) + "{");
                tabs++;
                SB.AppendLine(AddTabs(tabs) + "get");
                SB.AppendLine(AddTabs(tabs) + "{");
                tabs++;
                SB.AppendLine(AddTabs(tabs) + "return m_" + Name + ";");
                tabs--;
                SB.AppendLine(AddTabs(tabs) + "}");
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    SB.AppendLine(AddTabs(tabs) + "set");
                    SB.AppendLine(AddTabs(tabs) + "{");
                    tabs++;
                    SB.AppendLine(AddTabs(tabs) + "m_HasChanges = true;");
                    SB.AppendLine(AddTabs(tabs) + "m_" + Name + " = value;");
                    tabs--;
                    SB.AppendLine(AddTabs(tabs) + "}");
                }
                tabs--;
                SB.AppendLine(AddTabs(tabs) + "}");
                SB.AppendLine("");
            }

            // implement IX2ContextualDataProvider
            SB.AppendLine(AddTabs(tabs) + "#region IX2ContextualDataProvider Members");
            SB.AppendLine("");
            SB.AppendLine(AddTabs(tabs) + "public void LoadData(Int64 InstanceID)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "SqlDataAdapter SDA = null;");
            SB.AppendLine(AddTabs(tabs) + "DataTable WorkFlowData = new DataTable();");
            SB.AppendLine(AddTabs(tabs) + "try");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "WorkerHelper.FillFromQuery(WorkFlowData, \"select * from [X2DATA]." + FixWorkFlowName(WorkFlow.WorkFlowName) + " (nolock) where InstanceID = \" + InstanceID, Tran.Context, null);");
            SB.AppendLine(AddTabs(tabs) + "if (WorkFlowData.Rows.Count > 0)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                SB.AppendLine(AddTabs(tabs) + "if(WorkFlowData.Rows[0][\"" + WorkFlow.CustomVariables[k].Name + "\"] != DBNull.Value)");
                tabs++;
                SB.AppendLine(AddTabs(tabs) + "m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + " = Convert.To" + GetTypeFromCustomVariable(WorkFlow.CustomVariables[k].Type) + "(WorkFlowData.Rows[0][\"" + WorkFlow.CustomVariables[k].Name + "\"]);");
                tabs--;
            }
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine(AddTabs(tabs) + "catch");
            SB.AppendLine(AddTabs(tabs) + "{");

            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine(AddTabs(tabs) + "finally");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "if(SDA != null)");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "SDA.Dispose();");
            tabs--;
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");
            SB.AppendLine(AddTabs(tabs) + "public void SetMapVariables(System.Collections.Generic.Dictionary<string, string> Fields)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "if (Fields != null)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "string[] Keys = new string[Fields.Count];");
            SB.AppendLine(AddTabs(tabs) + "Fields.Keys.CopyTo(Keys, 0);");
            SB.AppendLine(AddTabs(tabs) + "for (int i = 0; i < Fields.Count; i++)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;

            if (WorkFlow.CustomVariables.Count > 0)
            {
                SB.AppendLine(AddTabs(tabs) + "switch (Keys[i].ToLower())");
                SB.AppendLine(AddTabs(tabs) + "{");
                tabs++;
                for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
                {
                    if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                    {
                        SB.AppendLine(AddTabs(tabs) + "case \"" + WorkFlow.CustomVariables[k].Name.ToLower() + "\":");
                        tabs++;
                        SB.AppendLine(AddTabs(tabs) + BuildItemName(WorkFlow.CustomVariables[k].Name) + " = Convert.To" + GetTypeFromCustomVariable(WorkFlow.CustomVariables[k].Type) + "(Fields[Keys[i]]);");
                        SB.AppendLine(AddTabs(tabs) + "break;");
                        tabs--;
                    }
                }
                tabs--;
                SB.AppendLine(AddTabs(tabs) + "}");
            }
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");
            SB.AppendLine(AddTabs(tabs) + "public void SaveData(Int64 InstanceID)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "if(m_HasChanges == true)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            string CmdTxt = "update [X2DATA].[" + FixWorkFlowName(WorkFlow.WorkFlowName) + "] with (rowlock) set ";
            int P = 0;
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    CmdTxt += "[" + WorkFlow.CustomVariables[k].Name + "]" + " = @P" + P.ToString() + ", ";
                    P++;
                }
            }
            if (CmdTxt.EndsWith(", "))
                CmdTxt = CmdTxt.Remove(CmdTxt.Length - 2, 2);
            CmdTxt += " where InstanceID = '\" + InstanceID + \"'";

            SB.AppendLine(AddTabs(tabs) + "// Create a collection");
            SB.AppendLine(AddTabs(tabs) + "ParameterCollection Parameters = new ParameterCollection();");
            SB.AppendLine(AddTabs(tabs) + "// Add the required parameters");

            // now add the parameters
            P = 0;
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    if (IsReferenceType(WorkFlow.CustomVariables[k].Type))
                    {
                        SB.AppendLine(AddTabs(tabs) + "if(m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + " != null)");
                        tabs++;
                        SB.AppendLine(AddTabs(tabs) + "WorkerHelper.AddParameter(Parameters, \"@P" + P.ToString() + "\", " + WorkFlowSqlDataType(WorkFlow.CustomVariables[k].Type) + ", ParameterDirection.Input , m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + ");");
                        tabs--;
                        SB.AppendLine(AddTabs(tabs) + "else");
                        tabs++;
                        SB.AppendLine(AddTabs(tabs) + "WorkerHelper.AddParameter(Parameters, \"@P" + P.ToString() + "\", " + WorkFlowSqlDataType(WorkFlow.CustomVariables[k].Type) + ", ParameterDirection.Input , DBNull.Value);");
                        tabs--;
                    }
                    else
                    {
                        SB.AppendLine(AddTabs(tabs) + "WorkerHelper.AddParameter(Parameters, \"@P" + P.ToString() + "\", " + WorkFlowSqlDataType(WorkFlow.CustomVariables[k].Type) + ", ParameterDirection.Input , m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + ");");
                    }
                    P++;
                }
            }
            if (WorkFlow.CustomVariables.Count > 0)
                SB.AppendLine(AddTabs(tabs) + "WorkerHelper.ExecuteNonQuery(Tran.Context, \"" + CmdTxt + "\", Parameters);");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");

            SB.AppendLine(AddTabs(tabs) + "public void InsertData(Int64 InstanceID, Dictionary<string, string> Fields)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "//// Set Data Fields");
            SB.AppendLine(AddTabs(tabs) + "SetMapVariables(Fields);");
            CmdTxt = "insert into [X2DATA].[" + FixWorkFlowName(WorkFlow.WorkFlowName) + "] values( \" + InstanceID + \", ";
            P = 0;
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    CmdTxt += "@P" + P.ToString() + ", ";
                    P++;
                }
            }
            if (CmdTxt.EndsWith(", "))
                CmdTxt = CmdTxt.Remove(CmdTxt.Length - 2, 2);
            CmdTxt += ")";

            SB.AppendLine(AddTabs(tabs) + "// Create a collection");
            SB.AppendLine(AddTabs(tabs) + "ParameterCollection Parameters = new ParameterCollection();");
            SB.AppendLine(AddTabs(tabs) + "// Add the required parameters");

            // now add the parameters
            P = 0;
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    if (IsReferenceType(WorkFlow.CustomVariables[k].Type))
                    {
                        SB.AppendLine(AddTabs(tabs) + "if(m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + " != null)");
                        tabs++;
                        SB.AppendLine(AddTabs(tabs) + "WorkerHelper.AddParameter(Parameters, \"@P" + P.ToString() + "\", " + WorkFlowSqlDataType(WorkFlow.CustomVariables[k].Type) + ", ParameterDirection.Input , m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + ");");
                        tabs--;
                        SB.AppendLine(AddTabs(tabs) + "else");
                        tabs++;
                        SB.AppendLine(AddTabs(tabs) + "WorkerHelper.AddParameter(Parameters, \"@P" + P.ToString() + "\", " + WorkFlowSqlDataType(WorkFlow.CustomVariables[k].Type) + ", ParameterDirection.Input , DBNull.Value);");
                        tabs--;
                    }
                    else
                    {
                        SB.AppendLine(AddTabs(tabs) + "WorkerHelper.AddParameter(Parameters, \"@P" + P.ToString() + "\", " + WorkFlowSqlDataType(WorkFlow.CustomVariables[k].Type) + ", ParameterDirection.Input , m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + ");");
                    }
                    P++;
                }
            }
            if (WorkFlow.CustomVariables.Count > 0)
                SB.AppendLine(AddTabs(tabs) + "WorkerHelper.ExecuteNonQuery(Tran.Context, \"" + CmdTxt + "\", Parameters);");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");

            SB.AppendLine(AddTabs(tabs) + "public bool Contains(string FieldName)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;

            SB.AppendLine(AddTabs(tabs) + "switch (FieldName.ToLower())");
            SB.AppendLine(AddTabs(tabs) + "{");

            tabs++;
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    SB.AppendLine(AddTabs(tabs) + "case \"" + BuildItemName(WorkFlow.CustomVariables[k].Name.ToLower()) + "\":");
                }
                tabs++;
                SB.AppendLine(AddTabs(tabs) + "return true;");
                tabs--;
            }

            SB.AppendLine(AddTabs(tabs) + "default:");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "return false;");
            tabs--;
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");

            SB.AppendLine(AddTabs(tabs) + "public string GetField(string FieldName)");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "switch (FieldName.ToLower())");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    SB.AppendLine(AddTabs(tabs) + "case \"" + BuildItemName(WorkFlow.CustomVariables[k].Name.ToLower()) + "\":");
                    tabs++;
                    SB.AppendLine(AddTabs(tabs) + "return m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + ".ToString();");
                    tabs--;
                }
            }
            SB.AppendLine(AddTabs(tabs) + "default:");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "return \"\";");
            tabs--;
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");

            //  Dictionary<string, string> GetData();
            SB.AppendLine(AddTabs(tabs) + "public Dictionary<string, string> GetData()");
            SB.AppendLine(AddTabs(tabs) + "{");
            tabs++;
            SB.AppendLine(AddTabs(tabs) + "Dictionary<string, string> Data = new Dictionary<string, string>();");
            for (int k = 0; k < WorkFlow.CustomVariables.Count; k++)
            {
                if (WorkFlow.CustomVariables[k].Name != "InstanceID")
                {
                    if (IsReferenceType(WorkFlow.CustomVariables[k].Type))
                    {
                        SB.AppendLine(AddTabs(tabs) + "if(m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + " != null)");
                    }
                    SB.AppendLine(AddTabs(tabs) + "Data.Add( \"" + BuildItemName(WorkFlow.CustomVariables[k].Name.ToLower()) + "\", m_" + BuildItemName(WorkFlow.CustomVariables[k].Name) + ".ToString());");
                }
            }
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "return Data;");
            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");

            SB.AppendLine(AddTabs(tabs) + "#endregion");

            tabs--;
            SB.AppendLine(AddTabs(tabs) + "}");
            SB.AppendLine("");
        }

        public static string WorkFlowSqlDataType(SAHL.X2Designer.Misc.CustomVariableType customVariableType)
        {
            switch (customVariableType)
            {
                case SAHL.X2Designer.Misc.CustomVariableType.ctbool:
                    return "SqlDbType.Bit";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdate:
                    return "SqlDbType.DateTime";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdouble:
                    return "SqlDbType.Float";
                case SAHL.X2Designer.Misc.CustomVariableType.ctfloat:
                    return "SqlDbType.Real";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdecimal:
                    return "SqlDbType.Decimal";
                case SAHL.X2Designer.Misc.CustomVariableType.ctinteger:
                    return "SqlDbType.Int";
                case SAHL.X2Designer.Misc.CustomVariableType.ctlong:
                    return "SqlDbType.BigInt";
                case SAHL.X2Designer.Misc.CustomVariableType.ctstring:
                    return "SqlDbType.VarChar";
                case SAHL.X2Designer.Misc.CustomVariableType.ctbigstring:
                    return "SqlDbType.Text";
                default:
                    return "";
            }
        }

        public static string GetTypeFromCustomVariable(SAHL.X2Designer.Misc.CustomVariableType customVariableType)
        {
            switch (customVariableType)
            {
                case SAHL.X2Designer.Misc.CustomVariableType.ctbool:
                    return "Boolean";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdate:
                    return "DateTime";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdecimal:
                    return "Decimal";
                case SAHL.X2Designer.Misc.CustomVariableType.ctinteger:
                    return "Int32";
                case SAHL.X2Designer.Misc.CustomVariableType.ctstring:
                case SAHL.X2Designer.Misc.CustomVariableType.ctbigstring:
                    return "String";
                case SAHL.X2Designer.Misc.CustomVariableType.ctdouble:
                    return "Double";
                case SAHL.X2Designer.Misc.CustomVariableType.ctfloat:
                    return "Float";
                case SAHL.X2Designer.Misc.CustomVariableType.ctlong:
                    return "Int64";
                default:
                    return "";
            }
        }

        public static bool IsReferenceType(SAHL.X2Designer.Misc.CustomVariableType customVariableType)
        {
            switch (customVariableType)
            {
                case SAHL.X2Designer.Misc.CustomVariableType.ctbool:
                    return false;
                case SAHL.X2Designer.Misc.CustomVariableType.ctdate:
                    return false;
                case SAHL.X2Designer.Misc.CustomVariableType.ctdecimal:
                    return false;
                case SAHL.X2Designer.Misc.CustomVariableType.ctinteger:
                    return false;
                case SAHL.X2Designer.Misc.CustomVariableType.ctstring:
                case SAHL.X2Designer.Misc.CustomVariableType.ctbigstring:
                    return true;
                case SAHL.X2Designer.Misc.CustomVariableType.ctdouble:
                    return false;
                case SAHL.X2Designer.Misc.CustomVariableType.ctfloat:
                    return false;
                case SAHL.X2Designer.Misc.CustomVariableType.ctlong:
                    return false;
                default:
                    return false;
            }
        }

        private static string AddTabs(int tabs)
        {
            string tabstr = "";
            for (int i = 0; i < tabs; i++)
                tabstr += "\t";
            return tabstr;
        }

        private static string GetStateNameForActivity(BaseActivity Activity)
        {
            IGoLink Link = null;

            foreach (IGoLink Lnk in Activity.Links)
            {
                if (Lnk.FromNode != Activity)
                {
                    Link = Lnk;
                    break;
                }
            }
            if (Link != null)
            {
                if (Link.FromNode is BaseState)
                {
                    if (((BaseState)Link.FromNode).GetType() != typeof(ClapperBoard))
                        return ((BaseState)Link.FromNode).Name;
                }
            }
            return "";
        }

        private static CommonState IsActivityCommon(BaseActivity Activity)
        {
            foreach (IGoLink Lnk in Activity.Links)
            {
                if (Lnk.FromNode.GetType() == typeof(CommonState))
                {
                    return Lnk.FromNode as CommonState;
                }
            }
            return null;
        }

        private static string Tabify(string ToTabify, int tabs)
        {
            StringReader Sr = new StringReader(ToTabify);
            string Result = "";
            string Line = Sr.ReadLine();
            while (Line != null)
            {
                Result += ("\r\n" + AddTabs(tabs) + Line);
                Line = Sr.ReadLine();
            }

            return Result;
        }
    }

    [Serializable]
    public class CodePosition
    {
        public int SectionStart;
        public int SectionEnd;
        public int SectionOffset;
        public BaseItem Node;
        public string CodeSection;

        public CodePosition(int Start, int End, BaseItem Item, string SectionName)
        {
            SectionStart = Start;
            SectionEnd = End;
            Node = Item;
            CodeSection = SectionName;
        }
    }
}
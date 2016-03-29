using System;
using System.IO;
using System.Text;
using SAHL.Tools.Workflow.Common.Database.WorkflowElements;

namespace SAHL.Tools.MapConverter
{
    class ScriptController
    {
        #region variables
        private StringBuilder builder = null;
        #endregion

        #region constructor
        internal ScriptController()
        {
            builder = new StringBuilder();
            builder.AppendLine("USE X2");
            builder.AppendLine("GO");
            builder.AppendLine("");
            builder.AppendLine("ALTER TABLE X2.State");
            builder.AppendLine("ADD X2ID UNIQUEIDENTIFIER");
            builder.AppendLine("GO");

            builder.AppendLine("Declare @LatestProcesses table(ID int)");
            builder.AppendLine("");
            builder.AppendLine("INSERT INTO @LatestProcesses (ID) SELECT MAX(ID) as ID FROM X2.Process GROUP BY NAME ORDER BY NAME");
            builder.AppendLine("");
        }
        #endregion

        #region methods
        internal void InsertHeader(string filename)
        {
            builder.AppendLine("----------------------------------------------------------------------------------------------------");
            builder.AppendLine("--" + filename);
            builder.AppendLine("----------------------------------------------------------------------------------------------------");
        }

        internal void AddStateToScript(State state)
        {
            builder.AppendLine("");
            builder.AppendLine(string.Format(@"
            UPDATE [X2].[X2].[State]
            SET [X2ID] = '{0}' 
            FROM [X2].[X2].[State] (NOLOCK) 
            INNER JOIN [X2].[X2].[Workflow](NOLOCK) ON [X2].[X2].[State].WorkflowID = [X2].[X2].[Workflow].ID
            INNER JOIN [X2].[X2].[Process](NOLOCK) ON [X2].[X2].[Workflow].ProcessID = [X2].[X2].[Process].ID
            WHERE [X2].[X2].[Process].Name = '{1}' AND [X2].[X2].[Workflow].Name = '{2}' AND [X2].[X2].[State].Name = '{3}' AND [X2].[X2].[Process].ID IN (SELECT * FROM @LatestProcesses)
            ", state.X2ID, state.WorkFlow.Process.Name, state.WorkFlow.Name, state.Name));
            builder.AppendLine("");
        }

        internal void SaveScriptsToFile()
        {
            builder.AppendLine("");
            builder.AppendLine(@"UPDATE nst
            SET nst.X2ID = st.X2ID
            FROM [x2].[x2].[State] st
            INNER JOIN [X2].[X2].[Workflow] wo (NOLOCK) ON st.WorkflowID = wo.ID
            INNER JOIN [X2].[X2].[Process] pr (NOLOCK) ON wo.ProcessID = pr.ID
            INNER JOIN [X2].[X2].[Process] npr (NOLOCK) ON npr.Name = pr.Name
            INNER JOIN [X2].[X2].[Workflow] nwo (NOLOCK) ON nwo.ProcessID = npr.ID and nwo.Name = wo.Name
            INNER JOIN [X2].[X2].[State] nst (NOLOCK) ON nst.WorkflowID = nwo.ID and nst.Name = st.Name
            WHERE st.X2ID IS NOT NULL
            ");

            builder.AppendLine("");

            builder.AppendLine(@"UPDATE nst
            SET nst.X2ID = st.X2ID
            FROM [x2].[x2].[Activity] st
            LEFT JOIN [x2].[State] fromState (NOLOCK) ON st.StateID = fromState.ID
            LEFT JOIN [x2].[State] toState (NOLOCK) ON st.NextStateID = toState.ID
            INNER JOIN [X2].[X2].[Workflow] wo (NOLOCK) ON st.WorkflowID = wo.ID
            INNER JOIN [X2].[X2].[Process] pr (NOLOCK) ON wo.ProcessID = pr.ID
            INNER JOIN [X2].[X2].[Process] npr (NOLOCK) ON npr.Name = pr.Name
            INNER JOIN [X2].[X2].[Workflow] nwo (NOLOCK) ON nwo.ProcessID = npr.ID and nwo.Name = wo.Name
            LEFT JOIN [X2].[X2].[Activity] nst (NOLOCK) ON nst.WorkflowID = nwo.ID and nst.Name = st.Name
            LEFT JOIN [x2].[State] nfromState (NOLOCK) ON nst.StateID = nfromState.ID
            JOIN [x2].[State] ntoState (NOLOCK) ON nst.NextStateID = ntoState.ID
            WHERE st.X2ID IS NOT NULL and (nfromState.Name = fromState.Name or ntoState.Name = toState.Name)
            ");
            builder.AppendLine("");

            FileStream stream = File.Create(string.Format(@"Update_Script_{0}.sql",DateTime.Today.ToString("yyyy_MM_dd")));
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(builder.ToString());
            }
        }
        

        internal void AddActivityToScript(Activity activity)
        {
            builder.AppendLine(string.Format(@"
            UPDATE [X2].[X2].[Activity]
            SET [X2ID] = '{0}' 
            FROM [X2].[X2].[Activity] (NOLOCK) 
            INNER JOIN [X2].[X2].[Workflow](NOLOCK) ON [X2].[X2].[Activity].WorkflowID = [X2].[X2].[Workflow].ID
            INNER JOIN [X2].[X2].[Process](NOLOCK) ON [X2].[X2].[Workflow].ProcessID = [X2].[X2].[Process].ID
            "+(GetName(activity.FromState) != ""?@" LEFT OUTER JOIN [X2].[X2].[State] fromState (NOLOCK) ON [X2].[X2].[Activity].StateID = fromState.ID":"")
            + (GetName(activity.ToState) != "" ?@" LEFT OUTER JOIN [X2].[X2].[State] toState (NOLOCK) ON [X2].[X2].[Activity].NextStateID = toState.ID" : "")
            + @" WHERE [X2].[X2].[Process].Name = '{1}' AND [X2].[X2].[Workflow].Name = '{2}' AND [X2].[X2].[Activity].Name = '{3}' "
            + (GetName(activity.FromState) != "" ? " AND fromState.Name = '{4}' " : "")
            + (GetName(activity.FromState) != "" ? " AND toState.Name  = '{5}' " : "")
            + "AND [X2].[X2].[Process].ID IN (SELECT * FROM @LatestProcesses)"
            , activity.X2ID, activity.WorkFlow.Process.Name, activity.WorkFlow.Name, activity.Name, GetName(activity.FromState), GetName(activity.ToState)));
        }
        #endregion

        private string GetName(State state)
        {
            if (state != null)
            {
                return state.Name+"";
            }
            return "";
        }
    }
}

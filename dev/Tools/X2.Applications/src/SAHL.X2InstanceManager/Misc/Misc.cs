using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SAHL.X2InstanceManager.Misc
{
    public class Misc
    {
        public static string FixName(string WorkFlowName)
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
    }
}

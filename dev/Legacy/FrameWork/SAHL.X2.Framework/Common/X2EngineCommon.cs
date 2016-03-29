using System.IO;

namespace SAHL.X2.Framework.Common
{
    public class X2EngineCommon
    {
        public static string CheckName(string OriginalName)
        {
            OriginalName = OriginalName.Replace("'", "");
            OriginalName = OriginalName.Replace("-", "_");
            return OriginalName.Replace(" ", "_");
        }

        public static string FixWorkFlowName(string WorkFlowName)
        {
            char[] InvalidChars = Path.GetInvalidPathChars();
            for (int i = 0; i < InvalidChars.Length; i++)
                WorkFlowName = WorkFlowName.Replace(InvalidChars[i].ToString(), "_");
            WorkFlowName = WorkFlowName.Replace("?", "_");
            WorkFlowName = WorkFlowName.Replace("\\", "_");
            WorkFlowName = WorkFlowName.Replace("/", "_");
            WorkFlowName = WorkFlowName.Replace(" ", "_");
            return WorkFlowName;
        }
    }
}
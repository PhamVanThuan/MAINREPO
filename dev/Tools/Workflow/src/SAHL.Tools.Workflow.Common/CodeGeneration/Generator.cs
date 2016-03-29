using System.IO;
using SAHL.Tools.Workflow.Common.Templates;
using SAHL.Tools.Workflow.Common.WorkflowElements;
using System;

namespace SAHL.Tools.Workflow.Common.CodeGeneration
{
    public class Generator
    {
        public void Generate(Process modelData)
        {
            string result = string.Empty;

            if (Convert.ToBoolean(modelData.Legacy))
            {
                X2ProcessLegacy x2Process = new X2ProcessLegacy(modelData);
                result =  x2Process.TransformText();
            }
            else
            {
                X2Process x2Process = new X2Process(modelData);
                result = x2Process.TransformText();
            }

            using (FileStream fs = new FileStream(@"c:\code2.cs", FileMode.Create, FileAccess.ReadWrite))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(result);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        public string GenerateToString(Process modelData)
        {
            if (Convert.ToBoolean(modelData.Legacy))
            {
                X2ProcessLegacy x2Process = new X2ProcessLegacy(modelData);
                return x2Process.TransformText();
            }
            else
            {
                X2Process x2Process = new X2Process(modelData);
                return x2Process.TransformText();
            }
        }
    }
}
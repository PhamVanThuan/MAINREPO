using System.Linq;
using SAHL.Core.X2.Messages;

namespace SAHL.X2Engine2.Builders
{
    public class ProcessBuilder
    {
        public X2Process Build(string processName, params string[] workflowNames)
        {
            return new X2Process(processName, workflowNames.Select(x =>
            {
                return new X2Workflow(processName, x);
            }));
        }
    }
}
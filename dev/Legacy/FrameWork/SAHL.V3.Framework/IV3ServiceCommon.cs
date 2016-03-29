using SAHL.Common.Service.Interfaces;
using SAHL.Core.SystemMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.V3.Framework
{
    public interface IV3ServiceCommon
    {
        X2ServiceResponse CreateWorkflowCase(ISystemMessageCollection systemMessageCollection, string ProcessName, string ProcessVersion, string WorkFlowName, string ActivityName, Dictionary<string, string> InputFields, bool IgnoreWarnings);

        void HandleSystemMessages(ISystemMessageCollection systemMessageCollection);
    }
}

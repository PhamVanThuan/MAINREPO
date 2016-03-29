using SAHL.X2.Common.DataAccess;

namespace SAHL.X2.Framework.Common
{
    public interface IX2Process
    {
        IX2WorkFlow GetWorkFlow(string WorkFlowName);

        string GetDynamicRole(string RoleName, IActiveDataTransaction Tran);
    }
}
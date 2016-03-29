using SAHL.Core.Data;
using SAHL.X2Engine2.Tests.X2.Models;

namespace SAHL.X2Engine2.Tests.X2.SqlStatements
{
    public class X2ApplicationCaptureCasesStatement : ISqlStatement<X2ApplicationCaptureCase>
    {
        public X2ApplicationCaptureCasesStatement(string hostName, int workerId)
        {
            this.HostName = hostName;
            this.WorkerId = workerId;
        }

        public string HostName { get; set; }

        public int WorkerId { get; set; }

        public string GetStatement()
        {
            return @"select offerKey as 'ApplicationKey' from [2am].[test].[ApplicationCapture] where isUsed = 0 and HostName= @HostName and WorkerId = @WorkerId";
        }
    }
}
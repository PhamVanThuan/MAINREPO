using SAHL.Core.Data;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class RemoveOpenNewBusinessApplicationStatement : ISqlStatement<int>
    {
        public int ApplicationNumber { get; protected set; }

        public RemoveOpenNewBusinessApplicationStatement(int ApplicationNumber)
        {
            this.ApplicationNumber = ApplicationNumber;
        }

        public string GetStatement()
        {
            return @"delete from FeTest.dbo.OpenNewBusinessApplications where offerKey = @ApplicationNumber";
        }
    }
}
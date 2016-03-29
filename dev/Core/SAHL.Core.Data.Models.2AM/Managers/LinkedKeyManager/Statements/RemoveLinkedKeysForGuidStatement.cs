using System;
using System.Linq;

namespace SAHL.Core.Data.Models._2AM.Managers.LinkedKeyManager.Statements
{
    public class RemoveLinkedKeysForGuidStatement : ISqlStatement<LinkedKeysDataModel>
    {
        public Guid GuidKey { get; protected set; }

        public RemoveLinkedKeysForGuidStatement(Guid guidKey)
        {
            this.GuidKey = guidKey;
        }

        public string GetStatement()
        {
            var query = @"DELETE FROM [2AM].[dbo].[LinkedKeys] WHERE [GuidKey] = @GuidKey";
            return query;
        }
    }
}
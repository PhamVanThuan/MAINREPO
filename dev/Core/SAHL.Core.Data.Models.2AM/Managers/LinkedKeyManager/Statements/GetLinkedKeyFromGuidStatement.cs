using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;

namespace SAHL.Core.Data.Models._2AM.Managers.LinkedKeyManager.Statements
{
    public class GetLinkedKeyFromGuidStatement : ISqlStatement<LinkedKeysDataModel>
    {
        public Guid GuidKey { get; protected set; }

        public GetLinkedKeyFromGuidStatement(Guid guidKey)
        {
            this.GuidKey = guidKey;
        }

        public string GetStatement()
        {
            var query = @"SELECT
                               [LinkedKey]
                             , [GuidKey]
                        FROM
                            [2AM].[dbo].[LinkedKeys]
                        WHERE
                            [GuidKey] = @GuidKey";

            return query;
        }
    }
}
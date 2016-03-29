using System;
using SAHL.Core.Data;
using SAHL.Core.Attributes;

namespace SAHL.Services.Capitec.Managers.Application.Statements
{
    [NolockConventionExclude]
    public class GetApplicationExistsQuery : ISqlStatement<int>
    {
        public Guid ApplicationID { get; protected set; }

        public GetApplicationExistsQuery(Guid applicationID)
        {
            this.ApplicationID = applicationID;
        }

        public string GetStatement()
        {
            return @"SELECT CASE WHEN EXISTS (
                        SELECT ID FROM [Capitec].[dbo].[Application] (NOLOCK)
                        WHERE ID = @ApplicationID
                    ) THEN 1 ELSE 0 END";
        }
    }
}

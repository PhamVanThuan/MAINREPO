using SAHL.Core.Data;
using SAHL.Services.Capitec.Managers.CapitecApplication.Models;
using System;

namespace SAHL.Services.Capitec.Managers.CapitecApplication.Statements
{
    public class GetCapitecUserBranchForApplicationQuery : ISqlStatement<CapitecUserBranchMappingModel>
    {
        public Guid ApplicationId { get; private set; }

        public GetCapitecUserBranchForApplicationQuery(Guid applicationId)
        {
            this.ApplicationId = applicationId;
        }

        public string GetStatement()
        {
            return @"SELECT   
                        app.Id,   
                        app.ApplicationNumber,               
                        app.UserId,
                        [UserName],
                        [BranchName]
                    FROM [dbo].[Application] app
                        join [security].[User] u on u.Id = app.UserId
                        join [security].[Branch] b on b.Id = app.BranchId
                    WHERE 
                        u.IsActive = 1                    
                    AND b.IsActive = 1
                    AND app.Id = @ApplicationId";
        }
    }
}
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Managers.Application.Statements
{
    public class SetApplicationStatusStatement : ISqlStatement<ApplicationDataModel>
    {
        public Guid ApplicationID { get; protected set; }
        public Guid ApplicationStatusID { get; protected set; }
        public DateTime StatusChangeDate { get;protected set; }

        public SetApplicationStatusStatement(Guid applicationID, Guid applicationStatusID, DateTime statusChangeDate)
        {
            this.ApplicationID = applicationID;
            this.ApplicationStatusID = applicationStatusID;
            this.StatusChangeDate = statusChangeDate;
        }
        public string GetStatement()
        {
            return string.Format(@"UPDATE [Capitec].[dbo].[Application]
                                  SET ApplicationStatusEnumID = @ApplicationStatusID, LastStatusChangeDate = @StatusChangeDate
                                  WHERE Id = @ApplicationID");
        }
    }
}

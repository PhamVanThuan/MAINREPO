using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using System;

namespace SAHL.Services.Capitec.Managers.Application.Statements
{
    public class SetApplicationCaptureEndTime : ISqlStatement<ApplicationDataModel>
    {
        public DateTime CaptureEndTime { get; protected set; }

        public Guid ApplicationID { get; protected set; }

        public SetApplicationCaptureEndTime(Guid applicationID, DateTime captureEndTime)
        {
            this.CaptureEndTime = captureEndTime;
            this.ApplicationID = applicationID;
        }

        public string GetStatement()
        {
            return string.Format(@"UPDATE [Capitec].[dbo].[Application]
                                SET CaptureEndTime = @CaptureEndTime
                                WHERE Id = @ApplicationID");
        }
    }
}
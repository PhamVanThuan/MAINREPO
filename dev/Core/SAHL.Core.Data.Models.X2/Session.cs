using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class SessionDataModel :  IDataModel
    {
        public SessionDataModel(string sessionID, string aDUserName, DateTime sessionStartTime, DateTime lastActivityTime)
        {
            this.SessionID = sessionID;
            this.ADUserName = aDUserName;
            this.SessionStartTime = sessionStartTime;
            this.LastActivityTime = lastActivityTime;
		
        }		

        public string SessionID { get; set; }

        public string ADUserName { get; set; }

        public DateTime SessionStartTime { get; set; }

        public DateTime LastActivityTime { get; set; }
    }
}
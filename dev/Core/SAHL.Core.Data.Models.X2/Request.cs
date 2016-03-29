using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class RequestDataModel :  IDataModel
    {
        public RequestDataModel(Guid requestID, string contents, int requestStatusID, DateTime requestDate, DateTime requestUpdatedDate, int requestTimeoutRetries)
        {
            this.RequestID = requestID;
            this.Contents = contents;
            this.RequestStatusID = requestStatusID;
            this.RequestDate = requestDate;
            this.RequestUpdatedDate = requestUpdatedDate;
            this.RequestTimeoutRetries = requestTimeoutRetries;
		
        }		

        public Guid RequestID { get; set; }

        public string Contents { get; set; }

        public int RequestStatusID { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime RequestUpdatedDate { get; set; }

        public int RequestTimeoutRetries { get; set; }
    }
}
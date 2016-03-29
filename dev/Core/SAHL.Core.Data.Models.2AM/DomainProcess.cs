using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DomainProcessDataModel :  IDataModel
    {
        public DomainProcessDataModel(Guid domainProcessId, string domainProcessType, string processState, string startResultData, int domainProcessStatusKey, string statusReason, string dataModel, DateTime? dateCreated, DateTime? dateModified)
        {
            this.DomainProcessId = domainProcessId;
            this.DomainProcessType = domainProcessType;
            this.ProcessState = processState;
            this.StartResultData = startResultData;
            this.DomainProcessStatusKey = domainProcessStatusKey;
            this.StatusReason = statusReason;
            this.DataModel = dataModel;
            this.DateCreated = dateCreated;
            this.DateModified = dateModified;
		
        }		

        public Guid DomainProcessId { get; set; }

        public string DomainProcessType { get; set; }

        public string ProcessState { get; set; }

        public string StartResultData { get; set; }

        public int DomainProcessStatusKey { get; set; }

        public string StatusReason { get; set; }

        public string DataModel { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
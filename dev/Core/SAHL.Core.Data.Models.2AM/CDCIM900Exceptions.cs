using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CDCIM900ExceptionsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CDCIM900ExceptionsDataModel(DateTime fileDate, int recordNumber, int? branchNumber, string exception, DateTime actionDate)
        {
            this.FileDate = fileDate;
            this.RecordNumber = recordNumber;
            this.BranchNumber = branchNumber;
            this.Exception = exception;
            this.ActionDate = actionDate;
		
        }
		[JsonConstructor]
        public CDCIM900ExceptionsDataModel(int cDCIM900ExceptionsKey, DateTime fileDate, int recordNumber, int? branchNumber, string exception, DateTime actionDate)
        {
            this.CDCIM900ExceptionsKey = cDCIM900ExceptionsKey;
            this.FileDate = fileDate;
            this.RecordNumber = recordNumber;
            this.BranchNumber = branchNumber;
            this.Exception = exception;
            this.ActionDate = actionDate;
		
        }		

        public int CDCIM900ExceptionsKey { get; set; }

        public DateTime FileDate { get; set; }

        public int RecordNumber { get; set; }

        public int? BranchNumber { get; set; }

        public string Exception { get; set; }

        public DateTime ActionDate { get; set; }

        public void SetKey(int key)
        {
            this.CDCIM900ExceptionsKey =  key;
        }
    }
}
using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class Tmp_Life_BulkLeadUploadDataModel :  IDataModel
    {
        public Tmp_Life_BulkLeadUploadDataModel(int loanNumber, string consultant, string result, DateTime? date)
        {
            this.LoanNumber = loanNumber;
            this.Consultant = consultant;
            this.Result = result;
            this.Date = date;
		
        }		

        public int LoanNumber { get; set; }

        public string Consultant { get; set; }

        public string Result { get; set; }

        public DateTime? Date { get; set; }
    }
}
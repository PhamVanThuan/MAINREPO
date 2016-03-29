using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class Tmp_Life_LeadCreateDataModel :  IDataModel
    {
        public Tmp_Life_LeadCreateDataModel(int loanNumber, DateTime? date)
        {
            this.LoanNumber = loanNumber;
            this.Date = date;
		
        }		

        public int LoanNumber { get; set; }

        public DateTime? Date { get; set; }
    }
}
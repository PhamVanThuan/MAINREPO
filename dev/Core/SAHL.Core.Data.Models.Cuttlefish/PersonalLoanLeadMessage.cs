using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Cuttlefish
{
    [Serializable]
    public partial class PersonalLoanLeadMessageDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public PersonalLoanLeadMessageDataModel(string idNumber, int? batchID, int? failureCount, string source, string userName, DateTime? messageDate, string machineName, string application)
        {
            this.IdNumber = idNumber;
            this.BatchID = batchID;
            this.FailureCount = failureCount;
            this.Source = source;
            this.UserName = userName;
            this.MessageDate = messageDate;
            this.MachineName = machineName;
            this.Application = application;
		
        }

        public PersonalLoanLeadMessageDataModel(int id, string idNumber, int? batchID, int? failureCount, string source, string userName, DateTime? messageDate, string machineName, string application)
        {
            this.Id = id;
            this.IdNumber = idNumber;
            this.BatchID = batchID;
            this.FailureCount = failureCount;
            this.Source = source;
            this.UserName = userName;
            this.MessageDate = messageDate;
            this.MachineName = machineName;
            this.Application = application;
		
        }		

        public int Id { get; set; }

        public string IdNumber { get; set; }

        public int? BatchID { get; set; }

        public int? FailureCount { get; set; }

        public string Source { get; set; }

        public string UserName { get; set; }

        public DateTime? MessageDate { get; set; }

        public string MachineName { get; set; }

        public string Application { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}
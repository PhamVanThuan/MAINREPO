using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CorrespondenceDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CorrespondenceDataModel(int genericKey, int reportStatementKey, int correspondenceMediumKey, string destinationValue, DateTime? dueDate, DateTime? completedDate, string userID, DateTime? changeDate, string outputFile, int genericKeyTypeKey, int? legalEntityKey)
        {
            this.GenericKey = genericKey;
            this.ReportStatementKey = reportStatementKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
            this.DestinationValue = destinationValue;
            this.DueDate = dueDate;
            this.CompletedDate = completedDate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.OutputFile = outputFile;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.LegalEntityKey = legalEntityKey;
		
        }
		[JsonConstructor]
        public CorrespondenceDataModel(int correspondenceKey, int genericKey, int reportStatementKey, int correspondenceMediumKey, string destinationValue, DateTime? dueDate, DateTime? completedDate, string userID, DateTime? changeDate, string outputFile, int genericKeyTypeKey, int? legalEntityKey)
        {
            this.CorrespondenceKey = correspondenceKey;
            this.GenericKey = genericKey;
            this.ReportStatementKey = reportStatementKey;
            this.CorrespondenceMediumKey = correspondenceMediumKey;
            this.DestinationValue = destinationValue;
            this.DueDate = dueDate;
            this.CompletedDate = completedDate;
            this.UserID = userID;
            this.ChangeDate = changeDate;
            this.OutputFile = outputFile;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.LegalEntityKey = legalEntityKey;
		
        }		

        public int CorrespondenceKey { get; set; }

        public int GenericKey { get; set; }

        public int ReportStatementKey { get; set; }

        public int CorrespondenceMediumKey { get; set; }

        public string DestinationValue { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string UserID { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string OutputFile { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int? LegalEntityKey { get; set; }

        public void SetKey(int key)
        {
            this.CorrespondenceKey =  key;
        }
    }
}
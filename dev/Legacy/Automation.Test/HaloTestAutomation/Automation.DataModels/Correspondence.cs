using System;

namespace Automation.DataModels
{
    public class Correspondence
    {
        public int GenericKey { get; set; }

        public int ReportStatementKey { get; set; }

        public int CorrespondenceMediumKey { get; set; }

        public DateTime DueDate { get; set; }

        public int LegalEntityKey { get; set; }

        public string UserID { get; set; }

        public string OutputFile { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public string CorrespondenceMediumDescription { get; set; }

        public string ReportDescription { get; set; }

        public DateTime ChangeDate { get; set; }

        public string IDMGuid { get; set; }

        public string DestinationValue { get; set; }
    }
}
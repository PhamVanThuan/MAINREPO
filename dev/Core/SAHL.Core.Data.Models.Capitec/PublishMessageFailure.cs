using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class PublishMessageFailureDataModel :  IDataModel
    {
        public PublishMessageFailureDataModel(string message, DateTime date)
        {
            this.Message = message;
            this.Date = date;
		
        }

        public PublishMessageFailureDataModel(Guid id, string message, DateTime date)
        {
            this.Id = id;
            this.Message = message;
            this.Date = date;
		
        }		

        public Guid Id { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }
    }
}
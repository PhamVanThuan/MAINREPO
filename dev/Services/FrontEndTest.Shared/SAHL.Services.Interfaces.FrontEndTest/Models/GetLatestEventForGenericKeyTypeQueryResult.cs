using System;

namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetLatestEventForGenericKeyTypeQueryResult
    {
        public GetLatestEventForGenericKeyTypeQueryResult(int EventKey, int GenericKey, int GenericKeyTypeKey, string Data, DateTime EventInsertDate)
        {
            this.EventKey = EventKey;
            this.GenericKey = GenericKey;
            this.GenericKeyTypeKey = GenericKeyTypeKey;
            this.Data = Data;
            this.EventInsertDate = EventInsertDate;
        }

        public int EventKey { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public string Data { get; set; }

        public DateTime EventInsertDate { get; set; }
    }
}
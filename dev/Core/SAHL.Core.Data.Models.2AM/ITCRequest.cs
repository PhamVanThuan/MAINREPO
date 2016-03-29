using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ITCRequestDataModel :  IDataModel
    {
        public ITCRequestDataModel(DateTime iTCDate, string iTCData)
        {
            this.ITCDate = iTCDate;
            this.ITCData = iTCData;
		
        }
		[JsonConstructor]
        public ITCRequestDataModel(Guid id, DateTime iTCDate, string iTCData)
        {
            this.Id = id;
            this.ITCDate = iTCDate;
            this.ITCData = iTCData;
		
        }		

        public Guid Id { get; set; }

        public DateTime ITCDate { get; set; }

        public string ITCData { get; set; }
    }
}
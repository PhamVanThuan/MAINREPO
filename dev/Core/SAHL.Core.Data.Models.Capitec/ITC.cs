using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ITCDataModel :  IDataModel
    {
        public ITCDataModel(DateTime iTCDate, string iTCData)
        {
            this.ITCDate = iTCDate;
            this.ITCData = iTCData;
		
        }

        public ITCDataModel(Guid id, DateTime iTCDate, string iTCData)
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
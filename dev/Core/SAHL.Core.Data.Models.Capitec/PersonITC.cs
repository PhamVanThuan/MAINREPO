using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class PersonITCDataModel :  IDataModel
    {
        public PersonITCDataModel(Guid currentITCId, DateTime iTCDate)
        {
            this.CurrentITCId = currentITCId;
            this.ITCDate = iTCDate;
		
        }

        public PersonITCDataModel(Guid id, Guid currentITCId, DateTime iTCDate)
        {
            this.Id = id;
            this.CurrentITCId = currentITCId;
            this.ITCDate = iTCDate;
		
        }		

        public Guid Id { get; set; }

        public Guid CurrentITCId { get; set; }

        public DateTime ITCDate { get; set; }
    }
}
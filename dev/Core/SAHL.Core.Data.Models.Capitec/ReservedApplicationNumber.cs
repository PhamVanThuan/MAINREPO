using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ReservedApplicationNumberDataModel :  IDataModel
    {
        public ReservedApplicationNumberDataModel(int applicationNumber, bool isUsed)
        {
            this.ApplicationNumber = applicationNumber;
            this.IsUsed = isUsed;
		
        }		

        public int ApplicationNumber { get; set; }

        public bool IsUsed { get; set; }
    }
}
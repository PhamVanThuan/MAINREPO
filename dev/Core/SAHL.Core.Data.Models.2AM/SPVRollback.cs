using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class SPVRollbackDataModel :  IDataModel
    {
        public SPVRollbackDataModel(int? loannumber)
        {
            this.Loannumber = loannumber;
		
        }		

        public int? Loannumber { get; set; }
    }
}
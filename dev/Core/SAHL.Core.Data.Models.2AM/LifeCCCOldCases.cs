using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LifeCCCOldCasesDataModel :  IDataModel
    {
        public LifeCCCOldCasesDataModel(int? loannumber)
        {
            this.loannumber = loannumber;
		
        }		

        public int? loannumber { get; set; }
    }
}
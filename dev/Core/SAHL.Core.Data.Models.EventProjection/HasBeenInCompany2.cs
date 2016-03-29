using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class HasBeenInCompany2DataModel :  IDataModel
    {
        public HasBeenInCompany2DataModel(int accountKey)
        {
            this.AccountKey = accountKey;
		
        }		

        public int AccountKey { get; set; }
    }
}
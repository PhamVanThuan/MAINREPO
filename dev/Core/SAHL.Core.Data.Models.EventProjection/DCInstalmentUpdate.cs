using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class DCInstalmentUpdateDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DCInstalmentUpdateDataModel(int accountKey)
        {
            this.AccountKey = accountKey;
		
        }
		[JsonConstructor]
        public DCInstalmentUpdateDataModel(int pK, int accountKey)
        {
            this.PK = pK;
            this.AccountKey = accountKey;
		
        }		

        public int PK { get; set; }

        public int AccountKey { get; set; }

        public void SetKey(int key)
        {
            this.PK =  key;
        }
    }
}
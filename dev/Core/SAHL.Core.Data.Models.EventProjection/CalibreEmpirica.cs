using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class CalibreEmpiricaDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CalibreEmpiricaDataModel(int accountKey, int empirica)
        {
            this.AccountKey = accountKey;
            this.Empirica = empirica;
		
        }
		[JsonConstructor]
        public CalibreEmpiricaDataModel(int pK, int accountKey, int empirica)
        {
            this.PK = pK;
            this.AccountKey = accountKey;
            this.Empirica = empirica;
		
        }		

        public int PK { get; set; }

        public int AccountKey { get; set; }

        public int Empirica { get; set; }

        public void SetKey(int key)
        {
            this.PK =  key;
        }
    }
}
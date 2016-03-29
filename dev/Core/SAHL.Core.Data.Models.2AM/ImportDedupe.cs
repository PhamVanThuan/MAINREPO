using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportDedupeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportDedupeDataModel(int fileKey, string iDNumber, int importStatusKey, string errorMsg)
        {
            this.FileKey = fileKey;
            this.IDNumber = iDNumber;
            this.ImportStatusKey = importStatusKey;
            this.ErrorMsg = errorMsg;
		
        }
		[JsonConstructor]
        public ImportDedupeDataModel(int dedupeKey, int fileKey, string iDNumber, int importStatusKey, string errorMsg)
        {
            this.DedupeKey = dedupeKey;
            this.FileKey = fileKey;
            this.IDNumber = iDNumber;
            this.ImportStatusKey = importStatusKey;
            this.ErrorMsg = errorMsg;
		
        }		

        public int DedupeKey { get; set; }

        public int FileKey { get; set; }

        public string IDNumber { get; set; }

        public int ImportStatusKey { get; set; }

        public string ErrorMsg { get; set; }

        public void SetKey(int key)
        {
            this.DedupeKey =  key;
        }
    }
}
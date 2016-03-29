using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DocumentVersionDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public DocumentVersionDataModel(int documentTypeKey, string version, DateTime effectiveDate, bool activeIndicator)
        {
            this.DocumentTypeKey = documentTypeKey;
            this.Version = version;
            this.EffectiveDate = effectiveDate;
            this.ActiveIndicator = activeIndicator;
		
        }
		[JsonConstructor]
        public DocumentVersionDataModel(int documentVersionKey, int documentTypeKey, string version, DateTime effectiveDate, bool activeIndicator)
        {
            this.DocumentVersionKey = documentVersionKey;
            this.DocumentTypeKey = documentTypeKey;
            this.Version = version;
            this.EffectiveDate = effectiveDate;
            this.ActiveIndicator = activeIndicator;
		
        }		

        public int DocumentVersionKey { get; set; }

        public int DocumentTypeKey { get; set; }

        public string Version { get; set; }

        public DateTime EffectiveDate { get; set; }

        public bool ActiveIndicator { get; set; }

        public void SetKey(int key)
        {
            this.DocumentVersionKey =  key;
        }
    }
}
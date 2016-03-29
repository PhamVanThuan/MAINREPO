using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CaseStatusDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CaseStatusDataModel(string description, bool? background, int? productKey)
        {
            this.Description = description;
            this.Background = background;
            this.ProductKey = productKey;
		
        }
		[JsonConstructor]
        public CaseStatusDataModel(int caseStatusKey, string description, bool? background, int? productKey)
        {
            this.CaseStatusKey = caseStatusKey;
            this.Description = description;
            this.Background = background;
            this.ProductKey = productKey;
		
        }		

        public int CaseStatusKey { get; set; }

        public string Description { get; set; }

        public bool? Background { get; set; }

        public int? ProductKey { get; set; }

        public void SetKey(int key)
        {
            this.CaseStatusKey =  key;
        }
    }
}
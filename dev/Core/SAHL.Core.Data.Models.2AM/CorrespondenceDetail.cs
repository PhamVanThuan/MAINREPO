using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CorrespondenceDetailDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public CorrespondenceDetailDataModel(int correspondenceKey, string correspondenceText)
        {
            this.CorrespondenceKey = correspondenceKey;
            this.CorrespondenceText = correspondenceText;
		
        }
		[JsonConstructor]
        public CorrespondenceDetailDataModel(int correspondenceDetailKey, int correspondenceKey, string correspondenceText)
        {
            this.CorrespondenceDetailKey = correspondenceDetailKey;
            this.CorrespondenceKey = correspondenceKey;
            this.CorrespondenceText = correspondenceText;
		
        }		

        public int CorrespondenceDetailKey { get; set; }

        public int CorrespondenceKey { get; set; }

        public string CorrespondenceText { get; set; }

        public void SetKey(int key)
        {
            this.CorrespondenceDetailKey =  key;
        }
    }
}
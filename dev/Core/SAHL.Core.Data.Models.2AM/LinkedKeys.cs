using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class LinkedKeysDataModel :  IDataModel
    {
        public LinkedKeysDataModel(int linkedKey, Guid guidKey)
        {
            this.LinkedKey = linkedKey;
            this.GuidKey = guidKey;
		
        }		

        public int LinkedKey { get; set; }

        public Guid GuidKey { get; set; }
    }
}
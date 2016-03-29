using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FLStageTranDataModel :  IDataModel
    {
        public FLStageTranDataModel(int? genericKey, int? aDUserKey, string eStageName, DateTime? eCreationTime)
        {
            this.GenericKey = genericKey;
            this.ADUserKey = aDUserKey;
            this.eStageName = eStageName;
            this.eCreationTime = eCreationTime;
		
        }		

        public int? GenericKey { get; set; }

        public int? ADUserKey { get; set; }

        public string eStageName { get; set; }

        public DateTime? eCreationTime { get; set; }
    }
}
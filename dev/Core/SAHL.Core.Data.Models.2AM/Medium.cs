using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class MediumDataModel :  IDataModel
    {
        public MediumDataModel(int mediumKey, string description)
        {
            this.MediumKey = mediumKey;
            this.Description = description;
		
        }		

        public int MediumKey { get; set; }

        public string Description { get; set; }
    }
}
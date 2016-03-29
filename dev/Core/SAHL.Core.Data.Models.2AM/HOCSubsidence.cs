using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCSubsidenceDataModel :  IDataModel
    {
        public HOCSubsidenceDataModel(int hOCSubsidenceKey, string description)
        {
            this.HOCSubsidenceKey = hOCSubsidenceKey;
            this.Description = description;
		
        }		

        public int HOCSubsidenceKey { get; set; }

        public string Description { get; set; }
    }
}
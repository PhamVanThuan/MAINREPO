using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCRoofDataModel :  IDataModel
    {
        public HOCRoofDataModel(int hOCRoofKey, string description)
        {
            this.HOCRoofKey = hOCRoofKey;
            this.Description = description;
		
        }		

        public int HOCRoofKey { get; set; }

        public string Description { get; set; }
    }
}
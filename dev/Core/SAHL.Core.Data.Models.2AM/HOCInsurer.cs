using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HOCInsurerDataModel :  IDataModel
    {
        public HOCInsurerDataModel(int hOCInsurerKey, string description, short? hOCInsurerStatus)
        {
            this.HOCInsurerKey = hOCInsurerKey;
            this.Description = description;
            this.HOCInsurerStatus = hOCInsurerStatus;
		
        }		

        public int HOCInsurerKey { get; set; }

        public string Description { get; set; }

        public short? HOCInsurerStatus { get; set; }
    }
}
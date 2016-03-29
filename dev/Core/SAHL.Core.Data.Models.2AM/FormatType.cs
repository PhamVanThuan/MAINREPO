using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class FormatTypeDataModel :  IDataModel
    {
        public FormatTypeDataModel(int formatTypeKey, string description, string format)
        {
            this.FormatTypeKey = formatTypeKey;
            this.Description = description;
            this.Format = format;
		
        }		

        public int FormatTypeKey { get; set; }

        public string Description { get; set; }

        public string Format { get; set; }
    }
}
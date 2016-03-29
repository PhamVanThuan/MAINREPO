using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CallingContextTypeDataModel :  IDataModel
    {
        public CallingContextTypeDataModel(int callingContextTypeKey, string description)
        {
            this.CallingContextTypeKey = callingContextTypeKey;
            this.Description = description;
		
        }		

        public int CallingContextTypeKey { get; set; }

        public string Description { get; set; }
    }
}
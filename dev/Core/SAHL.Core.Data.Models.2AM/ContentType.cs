using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ContentTypeDataModel :  IDataModel
    {
        public ContentTypeDataModel(int contentTypeKey, string description)
        {
            this.ContentTypeKey = contentTypeKey;
            this.Description = description;
		
        }		

        public int ContentTypeKey { get; set; }

        public string Description { get; set; }
    }
}
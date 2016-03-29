using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class JsonDocumentTypeDataModel :  IDataModel
    {
        public JsonDocumentTypeDataModel(string name)
        {
            this.Name = name;
		
        }

        public JsonDocumentTypeDataModel(Guid id, string name)
        {
            this.Id = id;
            this.Name = name;
		
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class JsonDocumentDataModel :  IDataModel
    {
        public JsonDocumentDataModel(string name, string description, int version, string documentFormatVersion, Guid documentType, string data)
        {
            this.Name = name;
            this.Description = description;
            this.Version = version;
            this.DocumentFormatVersion = documentFormatVersion;
            this.DocumentType = documentType;
            this.Data = data;
        
        }

        public JsonDocumentDataModel(Guid id, string name, string description, int version, string documentFormatVersion, Guid documentType, string data)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Version = version;
            this.DocumentFormatVersion = documentFormatVersion;
            this.DocumentType = documentType;
            this.Data = data;
        
        }		

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Version { get; set; }

        public string DocumentFormatVersion { get; set; }

        public Guid DocumentType { get; set; }

        public string Data { get; set; }
    }
}
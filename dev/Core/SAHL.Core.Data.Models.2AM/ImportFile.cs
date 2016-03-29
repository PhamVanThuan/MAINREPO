using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ImportFileDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ImportFileDataModel(string fileName, string fileType, DateTime dateImported, string status, string userID, string xML_Data)
        {
            this.FileName = fileName;
            this.FileType = fileType;
            this.DateImported = dateImported;
            this.Status = status;
            this.UserID = userID;
            this.XML_Data = xML_Data;
		
        }
		[JsonConstructor]
        public ImportFileDataModel(int fileKey, string fileName, string fileType, DateTime dateImported, string status, string userID, string xML_Data)
        {
            this.FileKey = fileKey;
            this.FileName = fileName;
            this.FileType = fileType;
            this.DateImported = dateImported;
            this.Status = status;
            this.UserID = userID;
            this.XML_Data = xML_Data;
		
        }		

        public int FileKey { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public DateTime DateImported { get; set; }

        public string Status { get; set; }

        public string UserID { get; set; }

        public string XML_Data { get; set; }

        public void SetKey(int key)
        {
            this.FileKey =  key;
        }
    }
}
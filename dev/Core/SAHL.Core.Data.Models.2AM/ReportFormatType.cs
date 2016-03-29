using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ReportFormatTypeDataModel :  IDataModel
    {
        public ReportFormatTypeDataModel(int reportFormatTypeKey, string description, string reportServicesFormatType, string fileExtension, string contentType, int displayOrder)
        {
            this.ReportFormatTypeKey = reportFormatTypeKey;
            this.Description = description;
            this.ReportServicesFormatType = reportServicesFormatType;
            this.FileExtension = fileExtension;
            this.ContentType = contentType;
            this.DisplayOrder = displayOrder;
		
        }		

        public int ReportFormatTypeKey { get; set; }

        public string Description { get; set; }

        public string ReportServicesFormatType { get; set; }

        public string FileExtension { get; set; }

        public string ContentType { get; set; }

        public int DisplayOrder { get; set; }
    }
}
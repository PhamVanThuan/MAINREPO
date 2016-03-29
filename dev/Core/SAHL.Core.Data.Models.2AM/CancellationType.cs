using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CancellationTypeDataModel :  IDataModel
    {
        public CancellationTypeDataModel(int cancellationTypeKey, string description, string cancellationWebCode)
        {
            this.CancellationTypeKey = cancellationTypeKey;
            this.Description = description;
            this.CancellationWebCode = cancellationWebCode;
		
        }		

        public int CancellationTypeKey { get; set; }

        public string Description { get; set; }

        public string CancellationWebCode { get; set; }
    }
}
using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CancellationReasonDataModel :  IDataModel
    {
        public CancellationReasonDataModel(int cancellationReasonKey, string description)
        {
            this.CancellationReasonKey = cancellationReasonKey;
            this.Description = description;
		
        }		

        public int CancellationReasonKey { get; set; }

        public string Description { get; set; }
    }
}
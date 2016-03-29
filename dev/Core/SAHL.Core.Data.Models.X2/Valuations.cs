using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ValuationsDataModel :  IDataModel
    {
        public ValuationsDataModel(long instanceID, int? applicationKey, int? propertyKey, bool? withdrawn, string requestingAdUser, int? adcheckPropertyID, int? adcheckValuationID, int? valuationKey, int? adCheckValuationIDStatus, int? entryPath, bool? onManagerWorkList, int? nLoops, int? valuationDataProviderDataServiceKey, string lightstonePropertyID, bool? isReview, int genericKey)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.PropertyKey = propertyKey;
            this.Withdrawn = withdrawn;
            this.RequestingAdUser = requestingAdUser;
            this.AdcheckPropertyID = adcheckPropertyID;
            this.AdcheckValuationID = adcheckValuationID;
            this.ValuationKey = valuationKey;
            this.AdCheckValuationIDStatus = adCheckValuationIDStatus;
            this.EntryPath = entryPath;
            this.OnManagerWorkList = onManagerWorkList;
            this.nLoops = nLoops;
            this.ValuationDataProviderDataServiceKey = valuationDataProviderDataServiceKey;
            this.LightstonePropertyID = lightstonePropertyID;
            this.IsReview = isReview;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public int? PropertyKey { get; set; }

        public bool? Withdrawn { get; set; }

        public string RequestingAdUser { get; set; }

        public int? AdcheckPropertyID { get; set; }

        public int? AdcheckValuationID { get; set; }

        public int? ValuationKey { get; set; }

        public int? AdCheckValuationIDStatus { get; set; }

        public int? EntryPath { get; set; }

        public bool? OnManagerWorkList { get; set; }

        public int? nLoops { get; set; }

        public int? ValuationDataProviderDataServiceKey { get; set; }

        public string LightstonePropertyID { get; set; }

        public bool? IsReview { get; set; }

        public int GenericKey { get; set; }
    }
}
using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class CreditDataModel :  IDataModel
    {
        public CreditDataModel(long instanceID, int? applicationKey, bool? isResub, string actionSource, string previousState, bool? reviewRequired, bool? stopRecursing, int? entryPath, bool? exceptionsDeclineWithOffer, bool? policyOverride, bool? is2ndPass, int genericKey)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.IsResub = isResub;
            this.ActionSource = actionSource;
            this.PreviousState = previousState;
            this.ReviewRequired = reviewRequired;
            this.StopRecursing = stopRecursing;
            this.EntryPath = entryPath;
            this.ExceptionsDeclineWithOffer = exceptionsDeclineWithOffer;
            this.PolicyOverride = policyOverride;
            this.Is2ndPass = is2ndPass;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public bool? IsResub { get; set; }

        public string ActionSource { get; set; }

        public string PreviousState { get; set; }

        public bool? ReviewRequired { get; set; }

        public bool? StopRecursing { get; set; }

        public int? EntryPath { get; set; }

        public bool? ExceptionsDeclineWithOffer { get; set; }

        public bool? PolicyOverride { get; set; }

        public bool? Is2ndPass { get; set; }

        public int GenericKey { get; set; }
    }
}
using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Application_ManagementDataModel :  IDataModel
    {
        public Application_ManagementDataModel(long instanceID, int? applicationKey, string previousState, int? genericKey, string caseOwnerName, bool? isFL, string eWorkFolderID, bool? isResub, int? offerTypeKey, long? appCapIID, bool? requireValuation, bool alphaHousingSurveyEmailSent)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.PreviousState = previousState;
            this.GenericKey = genericKey;
            this.CaseOwnerName = caseOwnerName;
            this.IsFL = isFL;
            this.EWorkFolderID = eWorkFolderID;
            this.IsResub = isResub;
            this.OfferTypeKey = offerTypeKey;
            this.AppCapIID = appCapIID;
            this.RequireValuation = requireValuation;
            this.AlphaHousingSurveyEmailSent = alphaHousingSurveyEmailSent;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public string PreviousState { get; set; }

        public int? GenericKey { get; set; }

        public string CaseOwnerName { get; set; }

        public bool? IsFL { get; set; }

        public string EWorkFolderID { get; set; }

        public bool? IsResub { get; set; }

        public int? OfferTypeKey { get; set; }

        public long? AppCapIID { get; set; }

        public bool? RequireValuation { get; set; }

        public bool AlphaHousingSurveyEmailSent { get; set; }
    }
}
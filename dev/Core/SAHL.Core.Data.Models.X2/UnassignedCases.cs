using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class UnassignedCasesDataModel :  IDataModel
    {
        public UnassignedCasesDataModel(long? instanceID, string stateName, string subject, int? offerKey, string creatorADUserName, int? migrated, bool? isFL, int? oSKey, string description, int? parentKey, string parentDescription)
        {
            this.InstanceID = instanceID;
            this.StateName = stateName;
            this.Subject = subject;
            this.OfferKey = offerKey;
            this.CreatorADUserName = creatorADUserName;
            this.Migrated = migrated;
            this.IsFL = isFL;
            this.OSKey = oSKey;
            this.Description = description;
            this.ParentKey = parentKey;
            this.ParentDescription = parentDescription;
		
        }		

        public long? InstanceID { get; set; }

        public string StateName { get; set; }

        public string Subject { get; set; }

        public int? OfferKey { get; set; }

        public string CreatorADUserName { get; set; }

        public int? Migrated { get; set; }

        public bool? IsFL { get; set; }

        public int? OSKey { get; set; }

        public string Description { get; set; }

        public int? ParentKey { get; set; }

        public string ParentDescription { get; set; }
    }
}
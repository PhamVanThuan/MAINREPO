using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class StuckCasesDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public StuckCasesDataModel(string mapName, string stateName, long? instanceID, string subject, int? offerKey, int? stateID, string creatorADUserName, int? oSKey, string description, int? parentKey, string pDescription)
        {
            this.MapName = mapName;
            this.StateName = stateName;
            this.InstanceID = instanceID;
            this.Subject = subject;
            this.OfferKey = offerKey;
            this.StateID = stateID;
            this.CreatorADUserName = creatorADUserName;
            this.OSKey = oSKey;
            this.Description = description;
            this.ParentKey = parentKey;
            this.PDescription = pDescription;
		
        }
		[JsonConstructor]
        public StuckCasesDataModel(int id, string mapName, string stateName, long? instanceID, string subject, int? offerKey, int? stateID, string creatorADUserName, int? oSKey, string description, int? parentKey, string pDescription)
        {
            this.id = id;
            this.MapName = mapName;
            this.StateName = stateName;
            this.InstanceID = instanceID;
            this.Subject = subject;
            this.OfferKey = offerKey;
            this.StateID = stateID;
            this.CreatorADUserName = creatorADUserName;
            this.OSKey = oSKey;
            this.Description = description;
            this.ParentKey = parentKey;
            this.PDescription = pDescription;
		
        }		

        public int id { get; set; }

        public string MapName { get; set; }

        public string StateName { get; set; }

        public long? InstanceID { get; set; }

        public string Subject { get; set; }

        public int? OfferKey { get; set; }

        public int? StateID { get; set; }

        public string CreatorADUserName { get; set; }

        public int? OSKey { get; set; }

        public string Description { get; set; }

        public int? ParentKey { get; set; }

        public string PDescription { get; set; }

        public void SetKey(int key)
        {
            this.id =  key;
        }
    }
}
using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Application_CaptureDataModel :  IDataModel
    {
        public Application_CaptureDataModel(long instanceID, int? applicationKey, string last_State, string last_NTU_State, int? leadType, int? genericKey, string caseOwnerName, string adminUserName, bool? isEA, bool? isEstateAgentApplication, string netLeadXML)
        {
            this.InstanceID = instanceID;
            this.ApplicationKey = applicationKey;
            this.Last_State = last_State;
            this.Last_NTU_State = last_NTU_State;
            this.LeadType = leadType;
            this.GenericKey = genericKey;
            this.CaseOwnerName = caseOwnerName;
            this.AdminUserName = adminUserName;
            this.IsEA = isEA;
            this.isEstateAgentApplication = isEstateAgentApplication;
            this.NetLeadXML = netLeadXML;
		
        }		

        public long InstanceID { get; set; }

        public int? ApplicationKey { get; set; }

        public string Last_State { get; set; }

        public string Last_NTU_State { get; set; }

        public int? LeadType { get; set; }

        public int? GenericKey { get; set; }

        public string CaseOwnerName { get; set; }

        public string AdminUserName { get; set; }

        public bool? IsEA { get; set; }

        public bool? isEstateAgentApplication { get; set; }

        public string NetLeadXML { get; set; }
    }
}
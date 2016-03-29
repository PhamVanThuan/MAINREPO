using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class Help_DeskDataModel :  IDataModel
    {
        public Help_DeskDataModel(long instanceID, int? legalEntityKey, string currentConsultant, int? helpDeskQueryKey, int genericKey)
        {
            this.InstanceID = instanceID;
            this.LegalEntityKey = legalEntityKey;
            this.CurrentConsultant = currentConsultant;
            this.HelpDeskQueryKey = helpDeskQueryKey;
            this.GenericKey = genericKey;
		
        }		

        public long InstanceID { get; set; }

        public int? LegalEntityKey { get; set; }

        public string CurrentConsultant { get; set; }

        public int? HelpDeskQueryKey { get; set; }

        public int GenericKey { get; set; }
    }
}
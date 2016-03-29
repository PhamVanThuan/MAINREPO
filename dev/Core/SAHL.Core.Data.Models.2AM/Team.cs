using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class TeamDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public TeamDataModel(string description, DateTime? changeDate, string changeUser)
        {
            this.Description = description;
            this.ChangeDate = changeDate;
            this.ChangeUser = changeUser;
		
        }
		[JsonConstructor]
        public TeamDataModel(int teamKey, string description, DateTime? changeDate, string changeUser)
        {
            this.TeamKey = teamKey;
            this.Description = description;
            this.ChangeDate = changeDate;
            this.ChangeUser = changeUser;
		
        }		

        public int TeamKey { get; set; }

        public string Description { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public void SetKey(int key)
        {
            this.TeamKey =  key;
        }
    }
}
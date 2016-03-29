using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HeaderIconTypeDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HeaderIconTypeDataModel(string description, string icon, string statementName)
        {
            this.Description = description;
            this.Icon = icon;
            this.StatementName = statementName;
		
        }
		[JsonConstructor]
        public HeaderIconTypeDataModel(int headerIconTypeKey, string description, string icon, string statementName)
        {
            this.HeaderIconTypeKey = headerIconTypeKey;
            this.Description = description;
            this.Icon = icon;
            this.StatementName = statementName;
		
        }		

        public int HeaderIconTypeKey { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public string StatementName { get; set; }

        public void SetKey(int key)
        {
            this.HeaderIconTypeKey =  key;
        }
    }
}
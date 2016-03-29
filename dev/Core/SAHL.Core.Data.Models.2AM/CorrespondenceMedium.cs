using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CorrespondenceMediumDataModel :  IDataModel
    {
        public CorrespondenceMediumDataModel(int correspondenceMediumKey, string description)
        {
            this.CorrespondenceMediumKey = correspondenceMediumKey;
            this.Description = description;
		
        }		

        public int CorrespondenceMediumKey { get; set; }

        public string Description { get; set; }
    }
}
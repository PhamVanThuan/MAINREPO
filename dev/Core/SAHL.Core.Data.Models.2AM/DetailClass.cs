using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DetailClassDataModel :  IDataModel
    {
        public DetailClassDataModel(int detailClassKey, string description)
        {
            this.DetailClassKey = detailClassKey;
            this.Description = description;
		
        }		

        public int DetailClassKey { get; set; }

        public string Description { get; set; }
    }
}
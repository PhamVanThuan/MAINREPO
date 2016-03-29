using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CourtDataModel :  IDataModel
    {
        public CourtDataModel(int courtKey, int courtTypeKey, int provinceKey, string name, int generalStatusKey)
        {
            this.CourtKey = courtKey;
            this.CourtTypeKey = courtTypeKey;
            this.ProvinceKey = provinceKey;
            this.Name = name;
            this.GeneralStatusKey = generalStatusKey;
		
        }		

        public int CourtKey { get; set; }

        public int CourtTypeKey { get; set; }

        public int ProvinceKey { get; set; }

        public string Name { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}
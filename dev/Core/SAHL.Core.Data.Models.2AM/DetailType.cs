using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class DetailTypeDataModel :  IDataModel
    {
        public DetailTypeDataModel(int detailTypeKey, string description, int detailClassKey, int generalStatusKey, bool allowUpdateDelete, bool allowUpdate, bool allowScreen)
        {
            this.DetailTypeKey = detailTypeKey;
            this.Description = description;
            this.DetailClassKey = detailClassKey;
            this.GeneralStatusKey = generalStatusKey;
            this.AllowUpdateDelete = allowUpdateDelete;
            this.AllowUpdate = allowUpdate;
            this.AllowScreen = allowScreen;
		
        }		

        public int DetailTypeKey { get; set; }

        public string Description { get; set; }

        public int DetailClassKey { get; set; }

        public int GeneralStatusKey { get; set; }

        public bool AllowUpdateDelete { get; set; }

        public bool AllowUpdate { get; set; }

        public bool AllowScreen { get; set; }
    }
}
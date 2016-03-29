using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class ProductDataModel :  IDataModel
    {
        public ProductDataModel(int productKey, string description, string originateYN)
        {
            this.ProductKey = productKey;
            this.Description = description;
            this.OriginateYN = originateYN;
		
        }		

        public int ProductKey { get; set; }

        public string Description { get; set; }

        public string OriginateYN { get; set; }
    }
}
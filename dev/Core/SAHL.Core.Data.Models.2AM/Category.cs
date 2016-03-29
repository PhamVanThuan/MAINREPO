using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CategoryDataModel :  IDataModel
    {
        public CategoryDataModel(int categoryKey, double value, string description)
        {
            this.CategoryKey = categoryKey;
            this.Value = value;
            this.Description = description;
		
        }		

        public int CategoryKey { get; set; }

        public double Value { get; set; }

        public string Description { get; set; }
    }
}
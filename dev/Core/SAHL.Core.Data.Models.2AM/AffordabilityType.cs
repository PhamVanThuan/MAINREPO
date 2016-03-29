using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class AffordabilityTypeDataModel :  IDataModel
    {
        public AffordabilityTypeDataModel(int affordabilityTypeKey, string description, bool isExpense, int? affordabilityTypeGroupKey, bool descriptionRequired, int sequence)
        {
            this.AffordabilityTypeKey = affordabilityTypeKey;
            this.Description = description;
            this.IsExpense = isExpense;
            this.AffordabilityTypeGroupKey = affordabilityTypeGroupKey;
            this.DescriptionRequired = descriptionRequired;
            this.Sequence = sequence;
		
        }		

        public int AffordabilityTypeKey { get; set; }

        public string Description { get; set; }

        public bool IsExpense { get; set; }

        public int? AffordabilityTypeGroupKey { get; set; }

        public bool DescriptionRequired { get; set; }

        public int Sequence { get; set; }
    }
}
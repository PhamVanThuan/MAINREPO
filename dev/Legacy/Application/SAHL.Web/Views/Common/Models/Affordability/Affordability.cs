using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Models.Affordability
{
    [Serializable]
    public class AffordabilityModel
    {
        public AffordabilityModel(int key, string description, bool descriptionRequired, double amount, AffordabilityTypeGroups affordabilityTypeGroups, string name, int sequence)
        {
            this.Key = key;
            this.Description = description;
            this.Amount = amount;
            this.AffordabilityTypeGroups = affordabilityTypeGroups;
            this.DescriptionRequired = descriptionRequired;
            this.Name = name;
            this.Sequence = sequence;
        }
        
        public double Amount { get; set; }

        public string  Name { get; set; }

        public string Description { get; set; }

        public bool DescriptionRequired { get; set; }

        public int Key { get; set; }

        public AffordabilityTypeGroups AffordabilityTypeGroups { get; set; }

        public int Sequence { get; set; }
    }
}
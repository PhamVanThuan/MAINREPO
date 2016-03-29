using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicantAffordabilityModel : IDataModel
    {
        public ApplicantAffordabilityModel(AffordabilityType affordabilityType, string description, decimal amount)
        {
            this.AffordabilityType = affordabilityType;
            this.Amount = amount;
            this.Description = description;
        }

        [DataMember]
        public AffordabilityType AffordabilityType { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}

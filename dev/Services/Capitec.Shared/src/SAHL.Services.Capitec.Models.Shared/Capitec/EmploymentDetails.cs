using System;
using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [KnownType(typeof(SalariedWithHousingAllowanceDetails))]
    [KnownType(typeof(SelfEmployedDetails))]
    [KnownType(typeof(SalariedWithCommissionDetails))]
    [KnownType(typeof(SalariedDetails))]
    [DataContract]
    public abstract class EmploymentDetails
    {
        [DataMember]
        public decimal BasicMonthlyIncome { get; protected set; }

        public abstract decimal Total();
    }
}
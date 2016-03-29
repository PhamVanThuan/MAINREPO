using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class SalariedWithHousingAllowanceDetails : EmploymentDetails
    {
        public SalariedWithHousingAllowanceDetails(decimal grossMonthlyIncome, decimal housingAllowance)
        {
            this.BasicMonthlyIncome = grossMonthlyIncome;
            this.HousingAllowance = housingAllowance;
        }

        [DataMember]
        public decimal HousingAllowance { get; protected set; }

        public override decimal Total()
        {
            return BasicMonthlyIncome + HousingAllowance;
        }
    }
}
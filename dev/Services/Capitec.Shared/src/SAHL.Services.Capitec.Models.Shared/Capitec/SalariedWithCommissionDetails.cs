using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class SalariedWithCommissionDetails : EmploymentDetails
    {
        public SalariedWithCommissionDetails(decimal grossMonthlyIncome, decimal threeMonthAverageCommission)
        {
            this.BasicMonthlyIncome = grossMonthlyIncome;
            this.ThreeMonthAverageCommission = threeMonthAverageCommission;
        }

        [DataMember]
        public decimal ThreeMonthAverageCommission { get; protected set; }

        public override decimal Total()
        {
            return BasicMonthlyIncome + ThreeMonthAverageCommission;
        }
    }
}
using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class SelfEmployedDetails : EmploymentDetails
    {
        public SelfEmployedDetails(decimal grossMonthlyIncome)
        {
            this.BasicMonthlyIncome = grossMonthlyIncome;
        }

        public override decimal Total()
        {
            return BasicMonthlyIncome;
        }
    }
}
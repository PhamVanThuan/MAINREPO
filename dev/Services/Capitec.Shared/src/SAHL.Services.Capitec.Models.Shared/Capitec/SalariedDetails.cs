using System;
using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class SalariedDetails : EmploymentDetails
    {
        public SalariedDetails(decimal grossMonthlyIncome)
        {
            this.BasicMonthlyIncome = grossMonthlyIncome;
        }

        public override decimal Total()
        {
            return this.BasicMonthlyIncome;
        }
    }
}
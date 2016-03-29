using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class SwitchLoanDetails
    {
        [DataMember]
        public decimal EstimatedMarketValueOfTheHome { get; protected set; }

        [DataMember]
        public decimal CashRequired { get; protected set; }

        [DataMember]
        public decimal CurrentBalance { get; protected set; }

        [DataMember]
        public int EmploymentTypeKey { get; protected set; }

        [DataMember]
        public decimal HouseholdIncome { get; protected set; }

        [DataMember]
        public decimal InterimInterest { get; protected set; }

        [DataMember]
        public bool CapitaliseFees { get; protected set; }

        [DataMember]
        public int Term { get; protected set; }

        public SwitchLoanDetails(int employmentTypeKey, decimal householdIncome, decimal estimatedMarketValueOfTheHome, decimal cashRequired, decimal currentBalance, decimal interimInterest, bool capitaliseFees, int term)
        {
            this.EmploymentTypeKey = employmentTypeKey;
            this.HouseholdIncome = householdIncome;
            this.EstimatedMarketValueOfTheHome = estimatedMarketValueOfTheHome;
            this.CashRequired = cashRequired;
            this.CurrentBalance = currentBalance;
            this.InterimInterest = interimInterest;

            this.CapitaliseFees = capitaliseFees;
            this.Term = term;
        }
    }
}
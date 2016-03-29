using System.Runtime.Serialization;
namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class NewPurchaseLoanDetails
    {
        [DataMember]
        public decimal PurchasePrice { get; protected set; }

        [DataMember]
        public decimal Deposit { get; protected set; }

        [DataMember]
        public int EmploymentTypeKey { get; protected set; }

        [DataMember]
        public decimal HouseholdIncome { get; protected set; }

        [DataMember]
        public decimal Fees { get; protected set; }

        [DataMember]
        public bool CapitaliseFees { get; protected set; }

        [DataMember]
        public int Term { get; protected set; }

        public NewPurchaseLoanDetails(int employmentTypeKey, decimal householdIncome, decimal purchasePrice, decimal deposit, bool capitaliseFees, int term)
        {
            this.EmploymentTypeKey = employmentTypeKey;
            this.HouseholdIncome = householdIncome;
            this.PurchasePrice = purchasePrice;
            this.Deposit = deposit;

            this.CapitaliseFees = capitaliseFees;
            this.Term = term;
        }
    }
}
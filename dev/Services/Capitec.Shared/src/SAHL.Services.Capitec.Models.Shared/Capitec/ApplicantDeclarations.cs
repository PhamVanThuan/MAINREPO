using System;
using System.Runtime.Serialization;

namespace SAHL.Services.Capitec.Models.Shared
{
    [DataContract]
    public class ApplicantDeclarations
    {
        public ApplicantDeclarations(bool incomeContributor, bool allowCreditBureauCheck, bool hasCapitecTransactionAccount, bool marriedInCommunityOfProperty)
        {
            this.IncomeContributor = incomeContributor;
            this.AllowCreditBureauCheck = allowCreditBureauCheck;
            this.HasCapitecTransactionAccount = hasCapitecTransactionAccount;
            this.MarriedInCommunityOfProperty = marriedInCommunityOfProperty;
            this.DeclarationsDate = DateTime.Now;
        }

        [DataMember]
        public DateTime DeclarationsDate { get; set; }

        [DataMember]
        public bool IncomeContributor { get; protected set; }

        [DataMember]
        public bool AllowCreditBureauCheck { get; protected set; }

        [DataMember]
        public bool HasCapitecTransactionAccount { get; protected set; }

        [DataMember]
        public bool MarriedInCommunityOfProperty { get; protected set; }
    }
}
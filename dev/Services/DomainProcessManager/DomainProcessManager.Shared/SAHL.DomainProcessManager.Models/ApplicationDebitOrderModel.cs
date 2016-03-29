using System;
using System.Runtime.Serialization;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class ApplicationDebitOrderModel : IDataModel
    {
        public ApplicationDebitOrderModel(FinancialServicePaymentType paymentType, int debitOrderDay, BankAccountModel bankAccount)
        {
            this.PaymentType   = paymentType;
            this.DebitOrderDay = debitOrderDay;
            this.BankAccount   = bankAccount;
        }

        [DataMember]
        public FinancialServicePaymentType PaymentType { get; set; }

        [DataMember]
        public int DebitOrderDay { get; set; }

        [DataMember]
        public BankAccountModel BankAccount { get; set; }
    }
}

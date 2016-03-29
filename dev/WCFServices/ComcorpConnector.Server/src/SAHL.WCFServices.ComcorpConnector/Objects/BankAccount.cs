using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class BankAccount
    {
        [DataMember]
        public string AccountBranch { get; set; }

        [DataMember]
        public string AccountInstitution { get; set; }

        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string AccountType { get; set; }

        [DataMember]
        public bool IsBusinessAccount { get; set; }

        [DataMember]
        public bool isMainAccount { get; set; }

        [DataMember]
        public string STDAccountBranchCode { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace SAHL.WCFServices.ComcorpConnector.Objects
{
    [DataContract]
    public class Application
    {
        [DataMember]
        public long ApplicationCode { get; set; }

        [DataMember]
        [Required]
        public string SAHLVendorCode { get; set; }

        [DataMember]
        public bool CapitaliseFees { get; set; }

        [DataMember]
        public decimal CashOutAmt { get; set; }

        [DataMember]
        public string CashOutReason { get; set; }

        [DataMember]
        [Required]
        public string ConsultantFirstName { get; set; }

        [DataMember]
        [Required]
        public string ConsultantSurname { get; set; }

        [DataMember]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Deposit must be R 0.00 or greater")]
        public decimal DepositCash { get; set; }

        [DataMember]
        public string DepositOther { get; set; }

        [DataMember]
        public bool DepositSource { get; set; }

        [DataMember]
        public decimal EstCostFees { get; set; }

        [DataMember]
        public decimal HigherBond { get; set; }

        [DataMember]
        public string InstalmentMethod { get; set; }

        [DataMember]
        public decimal LoanAmountRequired { get; set; }

        [DataMember]
        public string NamePropertyRegistered { get; set; }

        [DataMember]
        public bool OffPlanIndicator { get; set; }

        [DataMember]
        public decimal OutstandingLoan { get; set; }

        [DataMember]
        public string ProductSelection { get; set; }

        [DataMember]
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Purchase price must be greater than R 1.00")]
        public decimal PropertyPurchasePrice { get; set; }

        [DataMember]
        public bool QuickCashIndicator { get; set; }

        [DataMember]
        public string SAHLBondAccountNo { get; set; }

        [DataMember]
        [Required]
        [RegularExpression("New Purchase Loan|Switch Loan|Refinance Loan")]
        public string SahlLoanPurpose { get; set; }

        [DataMember]
        [Required]
        public string SAHLOccupancyType { get; set; }

        [DataMember]
        [Required]
        public string SAHLPropertyType { get; set; }

        [DataMember]
        [Required]
        public string SAHLTitleType { get; set; }

        [DataMember]
        public string SahlTransferAtt { get; set; }

        [DataMember]
        public string SectionalTitleUnitNo { get; set; }

        [DataMember]
        [Required]
        public decimal TermOfLoan { get; set; }

        [DataMember]
        public decimal PropertyMarketValue { get; set; }

        [DataMember]
        public decimal TotalEstLoan { get; set; }

        [DataMember]
        public string DeveloperName { get; set; }

        [DataMember]
        public string DevelopmentName { get; set; }

        [DataMember]
        public bool IsForADevelopment { get; set; }

        [DataMember]
        [Required]
        public MainApplicant MainApplicant { get; set; }

        [DataMember]
        public List<CoApplicant> CoApplicants { get; set; }

        [DataMember]
        [Required]
        public Property Property { get; set; }
    }
}
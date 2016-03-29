using System;
namespace Automation.DataModels
{
    public class OriginationTestCase : IDataModel
    {
        public string TestIdentifier { get; set; }

        public int OfferKey { get; set; }

        public string Username { get; set; }

        public string LoanType { get; set; }

        public string ExistingLoan { get; set; }

        public string CashOut { get; set; }

        public string ReAssignedConsultant { get; set; }

        public string TestGroup { get; set; }

        public string Password { get; set; }

        public string Product { get; set; }

        public string MarketValue { get; set; }

        public string CashDeposit { get; set; }

        public string EmploymentType { get; set; }

        public string Term { get; set; }
        
        public string CapitaliseFees { get; set; }

        public string InterestOnly { get; set; }

        public string HouseHoldIncome { get; set; }

        public string LegalEntityType { get; set; }

        public string LegalEntityRole { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public string CompanyName { get; set; }

        public string EstateAgency { get; set; }

        public bool AtQAFlag { get; set; }

        public bool AtManageApplicationFlag { get; set; }

        public string ApplicationManagementTestID { get; set; }

        public bool AtCreditFlag { get; set; }

        public string CreditTestID { get; set; }

        public bool AtRegistrationPipelineFlag { get; set; }

        public string RegistrationPipelineTestID { get; set; }

        public bool ProcessViaWFAutomation { get; set; }

        public string ApplicationManagementTestGroup { get; set; }

        public Int64 InstanceID { get; set; }

        public override string ToString()
        {
            return TestIdentifier.ToString();
        }
    }
}
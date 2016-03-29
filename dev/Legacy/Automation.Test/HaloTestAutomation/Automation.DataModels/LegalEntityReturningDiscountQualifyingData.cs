
namespace Automation.DataModels
{
    public class LegalEntityReturningDiscountQualifyingData
    {
        public int LegalEntityKey { get; set; }
        public string LegalEntityLegalName { get; set; }
        public string IDNumber { get; set; }
        public int MainApplicantOpenAccountsCount { get; set; }
        public int MainApplicantClosedAccountsCount { get; set; }
        public int SuretorOpenAccountsCount { get; set; }
        public int SuretorClosedAccountsCount { get; set; }
        public int ReturningClientDiscountOpenAccountsCount { get; set; }
        public int ReturningClientDiscountClosedAccountsCount { get; set; }
        public bool Unavailable { get; set; }
    }
}

namespace Automation.DataModels
{
    public class LeadTestCase : IDataModel
    {
        public string Role { get; set; }

        public string IDNumber { get; set; }

        public string Salutation { get; set; }

        public string Initials { get; set; }

        public string FirstNames { get; set; }

        public string Surname { get; set; }

        public string PreferredName { get; set; }

        public string Gender { get; set; }

        public string MaritalStatus { get; set; }

        public string PopulationGroup { get; set; }

        public string Education { get; set; }

        public string CitizenshipType { get; set; }

        public string PassportNumber { get; set; }

        public string TaxNumber { get; set; }

        public string HomeLanguage { get; set; }

        public string DocumentLanguage { get; set; }

        public string Status { get; set; }

        public string TestIdentifier { get; set; }

        public int OfferKey { get; set; }

        public string OrigConsultant { get; set; }

        public string ReAssignedConsultant { get; set; }

        public string TestGroup { get; set; }

        public override string ToString()
        {
            return TestIdentifier.ToString();
        }
    }
}
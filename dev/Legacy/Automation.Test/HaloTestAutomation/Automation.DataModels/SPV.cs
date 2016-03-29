namespace Automation.DataModels
{
    public class SPV
    {
        public int SPVKey { get; set; }

        public string Description { get; set; }

        public string ReportDescription { get; set; }

        public int ResetConfigurationKey { get; set; }

        public string CreditProviderNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public int ParentSPVKey { get; set; }

        public string ParentDescription { get; set; }

        public string ParentReportDescription { get; set; }
    }
}
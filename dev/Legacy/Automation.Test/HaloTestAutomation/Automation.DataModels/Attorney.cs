using Common.Enums;

namespace Automation.DataModels
{
    public sealed class Attorney
    {
        public int AttorneyKey { get; set; }

        public Automation.DataModels.LegalEntity LegalEntity { get; set; }

        public Automation.DataModels.Address Address { get; set; }

        public string ContactName { get; set; }

        public GeneralStatusEnum Status { get; set; }

        public bool IsWorkflowEnable { get; set; }

        public bool IsRegistrationAttorney { get; set; }

        public bool IsLitigationAttorney { get; set; }

        public float Mandate { get; set; }

        public string DeedsOffice { get; set; }
    }
}
using System;

namespace Automation.DataModels
{
    public sealed class ExternalLifePolicy
    {
        public int? ExtrernalLifePolicyKey { get; set; }

        public int InsurerKey { get; set; }

        public string PolicyNumber { get; set; }

        public DateTime CommencementDate { get; set; }

        public int LifePolicyStatusKey { get; set; }

        public DateTime? CloseDate { get; set; }

        public double SumInsured { get; set; }

        public bool PolicyCeded { get; set; }

        public Insurer Insurer { get; set; }

        public LifePolicyStatus LifePolicyStatus { get; set; }

        public int? LegalEntityKey { get; set; }
    }

    public sealed class Insurer
    {
        public int InsurerKey { get; set; }

        public string Descripton { get; set; }
    }

    public sealed class LifePolicyStatus
    {
        public int PolicyStatusKey { get; set; }

        public string Description { get; set; }
    }
}
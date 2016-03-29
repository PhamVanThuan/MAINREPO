using Common.Enums;

namespace Automation.DataModels
{
    public class LegalEntityEmployment
    {
        public float MonthlyIncome { get; set; }

        public float Income01 { get; set; }

        public float Income02 { get; set; }

        public EmploymentStatusEnum EmploymentStatus { get; set; }

        public RemunerationTypeEnum RemunerationType { get; set; }

        public EmploymentTypeEnum EmploymentType { get; set; }
    }
}
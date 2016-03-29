using SAHL.Core.SystemMessages;
namespace SAHL.Services.Interfaces.Capitec.Models
{
    public class CalculateApplicationScenarioQueryResult
    {
        public decimal InterestRate { get; set; }

        public decimal LTV { get; set; }

        public decimal PTI { get; set; }

        public decimal Instalment { get; set; }

        public decimal LoanAmount { get; set; }

        public int TermInMonths { get; set; }

        public string LTVAsPercent { get; set; }

        public string PTIAsPercent { get; set; }

        public string InterestRateAsPercent { get; set; }

        public bool EligibleApplication { get; set; }

        public ISystemMessageCollection DecisionTreeMessages { get; set; }
    }
}
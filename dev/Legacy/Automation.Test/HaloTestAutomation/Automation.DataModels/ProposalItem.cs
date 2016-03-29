using System;

namespace Automation.DataModels
{
    public sealed class ProposalItem : IComparable<ProposalItem>
    {
        public int ProposalItemKey { get; set; }

        public int ProposalKey { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string MarketRate { get; set; }

        public double InterestRate { get; set; }

        public double Amount { get; set; }

        public double InstalmentPercent { get; set; }

        public double AdditionalAmount { get; set; }

        public double AnnualEscalation { get; set; }

        public int StartPeriod { get; set; }

        public int EndPeriod { get; set; }

        public DateTime CreateDate { get; set; }

        public bool MonthlyServiceFee { get; set; }

        public int CompareTo(ProposalItem other)
        {
            if (this.ProposalItemKey != other.ProposalItemKey)
                return 0;
            if (this.StartDate.Date != other.StartDate.Date)
                return 0;
            if (this.EndDate.Date != other.EndDate.Date)
                return 0;
            if (this.MarketRate != other.MarketRate)
                return 0;
            if (this.InterestRate != other.InterestRate)
                return 0;
            if (this.Amount != other.Amount)
                return 0;
            if (this.AdditionalAmount != other.AdditionalAmount)
                return 0;
            return 1;
        }

        public string HOC { get; set; }

        public string Life { get; set; }

        public string Note { get; set; }
    }
}
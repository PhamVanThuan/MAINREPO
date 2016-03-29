using System;

namespace Automation.DataModels
{
    public class RecoveriesProposal
    {
        public int RecoveriesProposalKey { get; set; }

        public int AccountKey { get; set; }

        public double ShortfallAmount { get; set; }

        public double RepaymentAmount { get; set; }

        public DateTime StartDate { get; set; }

        public Boolean AcknowledgementOfDebt { get; set; }

        public int ADUserKey { get; set; }

        public DateTime CreateDate { get; set; }

        public int GeneralStatusKey { get; set; }
    }
}
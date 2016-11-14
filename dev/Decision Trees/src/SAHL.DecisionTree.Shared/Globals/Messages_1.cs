using SAHL.Core.SystemMessages;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DecisionTree.Shared.Globals
{
    public class Messages_1
    {
        private ISystemMessageCollection messages;
        public ISystemMessageCollection SystemMessages
        {
            get { return messages; }
        } 
        
        public Messages_1(ISystemMessageCollection messagesCollection)
        {
            this.messages = messagesCollection;
        }

        public void AddError(string errorMessage)
        {
            if(!SystemMessages.ErrorMessages().Any(e => e.Message.Equals(errorMessage)))
            {
                this.messages.AddMessage(new SystemMessage(errorMessage, SystemMessageSeverityEnum.Error));
            }
        }

        public void AddWarning(string warningMessage)
        {
            if(!SystemMessages.WarningMessages().Any(w => w.Message.Equals(warningMessage)))
            {
                this.messages.AddMessage(new SystemMessage(warningMessage, SystemMessageSeverityEnum.Warning));
            }
        }

        public void AddInfo(string infoMessage)
        {
            if(!SystemMessages.InfoMessages().Any(i => i.Message.Equals(infoMessage)))
            {
                this.messages.AddMessage(new SystemMessage(infoMessage, SystemMessageSeverityEnum.Info));
            }
        }

        public void AddDebugInfo(string debugMessage)
        {
            if(!SystemMessages.DebugMessages().Any(d => d.Message.Equals(debugMessage)))
            {
                this.messages.AddMessage(new SystemMessage(debugMessage, SystemMessageSeverityEnum.Debug));
            }
        }

		public class Capitec
		{
			public class Credit
			{
				public string ApplicantMinimumEmpirica { get { return "\"The Empirica score is below SAHL policy minimum\""; } }
				public string ApplicantMaximumJudgementsinLast3Years { get { return "\"There is record of multiple recent unpaid judgements in the last 3 years\""; } }
				public string MaximumAggregateJudgementValuewith3JudgementsinLast3Years { get { return "\"There is record of unpaid judgements with a material aggregated rand value\""; } }
				public string MaximumAggregatedJudgementValueUnsettledForBetween13And36Months { get { return "\"There is record of an outstanding aggregated unpaid judgement of material value\""; } }
				public string MaximumNumberOfUnsettledDefaultsWithinPast2Years { get { return "\"There is record of numerous unsettled defaults within the past 2 years\""; } }
				public string NoticeOfSequestration { get { return "\"There is a record of Sequestration\""; } }
				public string NoticeOfAdministrationOrder { get { return "\"There is a record of an Administration Order\""; } }
				public string NoticeOfDebtCounselling { get { return "\"There is a record of Debt Counselling\""; } }
				public string NoticeOfDebtReview { get { return "\"There is a record of Debt Review\""; } }
				public string NoticeOfConsumerIsDeceased { get { return "\"There is record that the consumer is deceased\""; } }
				public string NoticeOfCreditCardRevoked { get { return "\"There is record of a revoked credit card\""; } }
				public string NoticeOfAbsconded { get { return "\"There is record that the applicant has absconded\""; } }
				public string NoticeOfPaidOutOnDeceasedClaim { get { return "\"There is record that a deceased claim has been paid out\""; } }
				public string NoCreditBureauMatchFound { get { return "\"No credit bureau match found.\""; } }
				public string LoantoValueAboveCreditMaximum { get { return "\"Insufficient property value for loan amount requested.\""; } }
			}

			public Credit credit = new Credit();

			public string Insufficientinformation { get { return "\"The correct information is not available to continue.\""; } }
		}

		public Capitec capitec = new Capitec();
    }
}
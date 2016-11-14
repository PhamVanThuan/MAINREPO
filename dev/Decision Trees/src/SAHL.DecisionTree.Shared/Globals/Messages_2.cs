using SAHL.Core.SystemMessages;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.DecisionTree.Shared.Globals
{
    public class Messages_2
    {
        private ISystemMessageCollection messages;
        public ISystemMessageCollection SystemMessages
        {
            get { return messages; }
        } 
        
        public Messages_2(ISystemMessageCollection messagesCollection)
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
				public class Salaried
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for salaried applicants.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for salaried applicants.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
				}
				public class SalariedwithDeduction
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for salaried with deduction applicants.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for salaried with deduction applicants.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for salaried with deduction applicants.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
				}
				public class SelfEmployed
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for self-employed applicants.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for self-employed applicants.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for self-employed applicants.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
				}
				public class Alpha
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for the product.\""; } }
					public string HouseholdIncomeAboveMaximum { get { return "\"Total income is above the maximum for the product.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for the product.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for the product.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
					public string PropertyValueBelowMinimum { get { return "\"The property value is below the minimum for the product.\""; } }
				}

				public Salaried salaried = new Salaried();
				public SalariedwithDeduction salariedwithDeduction = new SalariedwithDeduction();
				public SelfEmployed selfEmployed = new SelfEmployed();
				public Alpha alpha = new Alpha();

				public string ApplicantMinimumEmpirica { get { return "\"Your credit score is below the SA Home Loans credit policy limit in terms of the loan details supplied.\""; } }
				public string ApplicantMaximumJudgementsinLast3Years { get { return "\"There is record of multiple recent unpaid judgements in the last 3 years.\""; } }
				public string MaximumAggregateJudgementValuewith3JudgementsinLast3Years { get { return "\"There is record of unpaid judgements with a material aggregated rand value.\""; } }
				public string MaximumAggregatedJudgementValueUnsettledForBetween13And36Months { get { return "\"There is record of an outstanding aggregated unpaid judgement of material value.\""; } }
				public string MaximumNumberOfUnsettledDefaultsWithinPast2Years { get { return "\"There is record of numerous unsettled defaults within the past 2 years.\""; } }
				public string NoticeOfSequestration { get { return "\"There is a record of Sequestration.\""; } }
				public string NoticeOfAdministrationOrder { get { return "\"There is a record of an Administration Order.\""; } }
				public string NoticeOfDebtCounselling { get { return "\"There is a record of Debt Counselling.\""; } }
				public string NoticeOfDebtReview { get { return "\"There is a record of Debt Review.\""; } }
				public string NoticeOfConsumerIsDeceased { get { return "\"There is record that the consumer is deceased.\""; } }
				public string NoticeOfCreditCardRevoked { get { return "\"There is record of a revoked credit card.\""; } }
				public string NoticeOfAbsconded { get { return "\"There is record that the applicant has absconded.\""; } }
				public string NoticeOfPaidOutOnDeceasedClaim { get { return "\"There is record that a deceased claim has been paid out.\""; } }
				public string NoCreditBureauMatchFound { get { return "\"No credit bureau match found.\""; } }
				public string LoantoValueAboveCreditMaximum { get { return "\"Insufficient property value for loan amount requested.\""; } }
				public string NewPurchaseInvestmentPropertyLoanToValueAboveMaximum { get { return "\"Your requested loan amount as a percentage of property value (LTV) for an investment property would be #{Variables::outputs.LoantoValueasPercent} which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit, by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string NewPurchaseLTVaboveMaximum { get { return "\"Your loan amount as a percentage of purchase price (LTV) would be #{Variables::outputs.LoantoValueasPercent}, which is greater than or equal to the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string SwitchLTVaboveMaximum { get { return "\"Your loan amount as a percentage of estimated property value (LTV) would be #{Variables::outputs.LoantoValueasPercent}, which is greater than or equal to the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string PTIaboveMaximum { get { return "\"Your repayment as a percentage of household income (PTI) would be #{Variables::outputs.PaymenttoIncomeasPercent} and is greater than or equal to the maximum of #{maximumpti}%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R#{maximumloanamount} or alternatively additional income so that total income is at least R#{requiredhouseholdincome}.\""; } }
				public string HouseholdIncomeTypeIsUnemployed { get { return "\"Your household income type may not be Unemployed.\""; } }
				public string SwitchInvestmentPropertyLoanToValueAboveMaximum { get { return "\"Your requested loan amount as a percentage of property value (LTV) for an investment property would be #{Variables::outputs.LoantoValueasPercent} which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string ApplicantAgeLimit { get { return "\"All applicants must be between the ages of 18 and 65 years.\""; } }
			}

			public Credit credit = new Credit();

			public string Insufficientinformation { get { return "\"The correct information is not available to continue.\""; } }
			public string AffordabilityMinimumHouseholdIncome { get { return "\"The minimum Household Income is :R #{Variables::capitec.AffordabilityMinimumHouseholdIncome}\""; } }
			public string AffordabilityMaximumHouseholdIncome { get { return "\"The maximum Household Income is :R #{Variables::capitec.AffordabilityMaximumHouseholdIncome}\""; } }
		}
		public class SAHomeLoans
		{
			public class Credit
			{
				public class Salaried
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for salaried applicants.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for salaried applicants.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for salaried applicants.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
				}
				public class SalariedwithDeduction
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for salaried with deduction applicants.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for salaried with deduction applicants.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for salaried with deduction applicants.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
				}
				public class SelfEmployed
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for self-employed applicants.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for self-employed applicants.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for self-employed applicants.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
				}
				public class Alpha
				{
					public string HouseholdIncomeBelowMinimum { get { return "\"Total income is below the required minimum for the product.\""; } }
					public string HouseholdIncomeAboveMaximum { get { return "\"Total income is above the maximum for the product.\""; } }
					public string LoanAmountBelowMinimum { get { return "\"Loan amount requested is below the product minimum.\""; } }
					public string LoanAmountAboveMaximum { get { return "\"Loan amount requested is above the product maximum.\""; } }
					public string LTVAboveMaximum { get { return "\"The application LTV is above the maximum allowed for the product.\""; } }
					public string CreditScoreBelowMinimum { get { return "\"The best Credit Bureau score applicable for this application is below the minimum requirement for the product.\""; } }
					public string PTIAboveMaximum { get { return "\"Affordability: Loan repayment to income ratio (PTI) is above the maximum limit. Reducing the loan amount may make the loan more affordable.\""; } }
					public string PropertyValueBelowMinimum { get { return "\"The property value is below the minimum for the product.\""; } }
				}

				public Salaried salaried = new Salaried();
				public SalariedwithDeduction salariedwithDeduction = new SalariedwithDeduction();
				public SelfEmployed selfEmployed = new SelfEmployed();
				public Alpha alpha = new Alpha();

				public string ApplicantMinimumEmpirica { get { return "\"The Credit Bureau score is below required minimum.\""; } }
				public string ApplicantMaximumJudgementsinLast3Years { get { return "\"There is record of multiple recent unpaid judgements in the last 3 years.\""; } }
				public string MaximumAggregateJudgementValuewith3JudgementsinLast3Years { get { return "\"There is record of unpaid judgements with a material aggregated rand value.\""; } }
				public string MaximumAggregatedJudgementValueUnsettledForBetween13And36Months { get { return "\"There is record of an outstanding aggregated unpaid judgement of material value.\""; } }
				public string MaximumNumberOfUnsettledDefaultsWithinPast2Years { get { return "\"There is record of numerous unsettled defaults within the past 2 years.\""; } }
				public string NoticeOfSequestration { get { return "\"There is a record of Sequestration.\""; } }
				public string NoticeOfAdministrationOrder { get { return "\"There is a record of an Administration Order.\""; } }
				public string NoticeOfDebtCounselling { get { return "\"There is a record of Debt Counselling.\""; } }
				public string NoticeOfDebtReview { get { return "\"There is a record of Debt Review.\""; } }
				public string NoticeOfConsumerIsDeceased { get { return "\"There is record that the consumer is deceased.\""; } }
				public string NoticeOfCreditCardRevoked { get { return "\"There is record of a revoked credit card.\""; } }
				public string NoticeOfAbsconded { get { return "\"There is record that the applicant has absconded.\""; } }
				public string NoticeOfPaidOutOnDeceasedClaim { get { return "\"There is record that a deceased claim has been paid out.\""; } }
				public string NoCreditBureauMatchFound { get { return "\"No credit bureau match found.\""; } }
				public string LoantoValueAboveCreditMaximum { get { return "\"Insufficient property value for loan amount requested.\""; } }
				public string NewPurchaseInvestmentPropertyLoanToValueAboveMaximum { get { return "\"Your requested loan amount as a percentage of property value (LTV) for an investment property would be #{Variables::outputs.LoantoValueasPercent} which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit, by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string NewPurchaseLTVaboveMaximum { get { return "\"Your loan amount as a percentage of purchase price (LTV) would be #{Variables::outputs.LoantoValueasPercent}, which is greater than or equal to the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by increasing the deposit by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string SwitchLTVaboveMaximum { get { return "\"Your loan amount as a percentage of estimated property value (LTV) would be #{Variables::outputs.LoantoValueasPercent}, which is greater than or equal to the maximum of #{maximumloantovalue}%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string PTIaboveMaximum { get { return "\"Your repayment as a percentage of household income (PTI) would be #{Variables::outputs.PaymenttoIncomeasPercent} and is greater than or equal to the maximum of #{maximumpti}%, which is the maximum per responsible affordability guidelines. In order to reduce your PTI, the loan amount would need to reduce to no more than R#{maximumloanamount} or alternatively additional income so that total income is at least R#{requiredhouseholdincome}.\""; } }
				public string HouseholdIncomeTypeIsUnemployed { get { return "\"Your household income type may not be Unemployed.\""; } }
				public string SwitchInvestmentPropertyLoanToValueAboveMaximum { get { return "\"Your requested loan amount as a percentage of property value (LTV) for an investment property would be #{Variables::outputs.LoantoValueasPercent} which equals or exceeds the maximum of 80%. In order to reduce your LTV, the loan amount would need to be lowered by decreasing the cash required by R#{requiredamounttolowerloanamountby} or more.\""; } }
				public string ApplicantAgeLimit { get { return "\"All applicants must be between the ages of 18 and 65 years.\""; } }
			}

			public Credit credit = new Credit();

			public string Insufficientinformation { get { return "\"The correct information is not available to continue.\""; } }
		}

		public Capitec capitec = new Capitec();
		public SAHomeLoans sAHomeLoans = new SAHomeLoans();
    }
}
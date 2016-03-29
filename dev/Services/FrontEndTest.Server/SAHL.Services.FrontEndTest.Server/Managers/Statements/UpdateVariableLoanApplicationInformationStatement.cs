using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Linq;

namespace SAHL.Services.FrontEndTest.Managers.Statements
{
    public class UpdateVariableLoanApplicationInformationStatement : ISqlStatement<OfferInformationVariableLoanDataModel>
    {
        public int ApplicationInformationKey { get; protected set; }

        public double? LoanAmountNoFees { get; protected set; }

        public double? LoanAgreementAmount { get; protected set; }

        public UpdateVariableLoanApplicationInformationStatement(int applicationInformationKey, double? loanAmountNoFees, double? loanAgreementAmount)
        {
            this.ApplicationInformationKey = applicationInformationKey;
            this.LoanAmountNoFees = loanAmountNoFees;
            this.LoanAgreementAmount = loanAgreementAmount;
        }

        public string GetStatement()
        {
            return @"update [2am].dbo.OfferInformationVariableLoan
                set LoanAmountNoFees = @LoanAmountNoFees, LoanAgreementAmount = @LoanAgreementAmount
                where OfferInformationKey = @ApplicationInformationKey";
        }
    }
}
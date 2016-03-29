using SAHL.Core.Data;
using SAHL.Services.Interfaces.FinancialDomain.Models;

namespace SAHL.Services.FinancialDomain.Managers.Statements
{
    public class GetFeeApplicationAttributesStatement : ISqlStatement<FeeApplicationAttributesModel>
    {
        public int ApplicationNumber { get; protected set; }

        public GetFeeApplicationAttributesStatement(int ApplicationNumber)
        {
            this.ApplicationNumber = ApplicationNumber;
        }

        public string GetStatement()
        {
            return @"
                select cast(p.[Capitalize Fees] as bit) as CapitaliseFees, cast([Staff Home Loan] as bit) as StaffHomeLoan, cast([QuickPayLoan] as bit) as QuickPayLoan,
                cast([Discounted Initiation Fee - Returning Client] as bit) as DiscountedInitiationFeeReturningClient, cast([Government Employee Pension Fund] as bit) as GEPF,
				cast([Capitalise Initiation Fee] as bit) as CapitaliseInitiationFee
                    from
                (
                    select oat.[Description],
                isnull(oa.OfferKey, 0) as [Exists]
                from [2am].dbo.offerattributetype oat
                left join [2am].dbo.offerattribute oa on oa.OfferAttributeTypeKey = oat.OfferAttributeTypeKey
                    and oa.OfferKey = @ApplicationNumber
                ) as attribute
                pivot
                (
                    max([Exists])
                    FOR [Description] in ([Capitalize Fees], [Staff Home Loan], [QuickPayLoan], [Discounted Initiation Fee - Returning Client], [Government Employee Pension Fund], [Capitalise Initiation Fee])
                ) as p";
        }
    }
}
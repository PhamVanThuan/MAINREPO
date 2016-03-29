using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class GetApplicationMailingAddressStatement : ISqlStatement<OfferMailingAddressDataModel>
    {
        public int ApplicationNumber { get; protected set; }

        public GetApplicationMailingAddressStatement(int applicationNumber)
        {
            this.ApplicationNumber = applicationNumber;
        }

        public string GetStatement()
        {
            var sql = @"select
                            [OfferMailingAddressKey],
                            [OfferKey],
                            [AddressKey],
                            [OnlineStatement],
                            [OnlineStatementFormatKey],
                            [LanguageKey],
                            [LegalEntityKey],
                            [CorrespondenceMediumKey]
                        from
                            [2AM].[dbo].[OfferMailingAddress]
                        where
                            [OfferKey] = @ApplicationNumber";

            return sql;
        }
    }
}
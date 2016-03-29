using SAHL.Core.Data;

namespace SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData.Statements
{
    public class GetSAHLReferenceStatement : ISqlStatement<string>
    {
        public string GetStatement()
        {
            return @" DECLARE @referenceNumber VARCHAR(100)
                      EXEC  [2AM].[dbo].[GetNextInvoiceReference] @referenceNumber OUT
                      SELECT @referenceNumber
                    ";
        }
    }
}
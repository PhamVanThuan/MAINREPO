using SAHL.Core.Data;

namespace SAHL.Services.ApplicationDomain.Managers.Application.Statements
{
    public class DoesOpenApplicationExistForPropertyAndClientStatement : ISqlStatement<int>
    {
        public int PropertyKey { get; protected set; }
        public string ClientIDNumber { get; protected set; }

        public DoesOpenApplicationExistForPropertyAndClientStatement(int propertyKey, string clientIDNumber)
        {
            PropertyKey = propertyKey;
            ClientIDNumber = clientIDNumber;
        }

        public string GetStatement()
        {
            var sql = @"SELECT COUNT(o.OfferKey) as Total
                        FROM [2AM].[dbo].[Offer] o
                        JOIN [2AM].[dbo].[OfferMortgageLoan] oml ON oml.OfferKey = o.OfferKey
                        JOIN [2AM].[dbo].[Property] p ON p.PropertyKey = oml.PropertyKey
                        JOIN [2AM].[dbo].[Address] a ON a.AddressKey = p.AddressKey
                        JOIN [2AM].[dbo].[OfferRole] ofr ON ofr.OfferKey = o.OfferKey
                            and ofr.GeneralStatusKey = 1
                        JOIN [2AM].[dbo].[LegalEntity] le ON le.LegalEntityKey = ofr.LegalEntityKey
                        LEFT JOIN [2AM].dbo.[StageTransitionComposite] stc on ofr.offerKey = stc.GenericKey
                            and stc.StageDefinitionStageDefinitionGroupKey in (110,111)
                        where ofr.OfferRoleTypeKey in (8,10,11,12)
                        AND o.OfferStatusKey in (1,4,5)
                        AND stc.GenericKey IS NULL
                        AND le.IDNumber = @ClientIDNumber
                        AND p.PropertyKey = @PropertyKey";
            return sql;
        }
    }
}
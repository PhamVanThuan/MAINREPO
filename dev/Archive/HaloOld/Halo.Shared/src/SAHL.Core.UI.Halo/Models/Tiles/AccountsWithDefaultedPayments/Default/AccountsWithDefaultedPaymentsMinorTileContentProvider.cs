using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;

namespace SAHL.Core.UI.Halo.Tiles.AccountsWithDefaultedPayments.Default
{
    public class AccountsWithDefaultedPaymentsMinorTileContentProvider : AbstractSqlTileContentProvider<AccountsWithDefaultedPaymentsMinorTileModel>
    {
        public AccountsWithDefaultedPaymentsMinorTileContentProvider()
            : base()
        {
        }

        public override string GetStatement(BusinessKey businessKey)
        {
            return string.Format(@"DECLARE @BusinessKeys VARCHAR(MAX)
                                    SELECT @BusinessKeys = COALESCE(@BusinessKeys+', ','') + CONVERT(VARCHAR, r.AccountKey)
                                      FROM 
                                      [2AM].dbo.Role r
                                      LEFT JOIN 
                                      [X2].[X2].[Instance] i ON r.AccountKey = i.Name
                                      WHERE 
	                                    WorkFlowID = 927
                                      AND 
	                                    StateID = 21178
                                      AND 
	                                    r.LegalEntityKey = {0}
 
                                     SELECT 
	                                    COUNT(1) AS BusinessKeysCount,
                                            'Defaulted Accounts' AS Title,	                                    
	                                    @BusinessKeys AS BusinessKeys,	                                  
                                        2 AS BusinessKeysType
                                       FROM 
                                      [2AM].dbo.Role r
                                      LEFT JOIN 
                                      [X2].[X2].[Instance] i ON r.AccountKey = i.Name
                                      WHERE 
	                                    WorkFlowID = 927
                                      AND 
	                                    StateID = 21178
                                      AND 
	                                    r.LegalEntityKey = {0}", businessKey.Key);
        }
    }
}
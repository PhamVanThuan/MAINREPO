using SAHL.Core.BusinessModel;
using SAHL.Core.UI.Providers.Tiles;
using System;

namespace SAHL.Core.UI.Halo.Tiles.Affordability.Default
{
    public class LegalEntityAffordabilitySummaryMinorTileContentProvider : AbstractSqlTileContentProvider<LegalEntityAffordabilitySummaryMinorTileModel>
    {
        public override string GetStatement(BusinessKey businessKey)
        {
            return String.Format(@"select TotalIncome, TotalExpenses, (TotalIncome - TotalExpenses) as DisposableIncomeAmount 
                    from 
                    (
	                    select SUM(lea.Amount) as  TotalIncome
	                    from [2am].dbo.LegalEntityAffordability lea
	                    join [2am].dbo.AffordabilityType aft on lea.AffordabilityTypeKey = aft.AffordabilityTypeKey
	                    where aft.AffordabilityTypeGroupKey = 1
	                    and lea.LegalEntityKey = {0}
                    ) income
                    cross apply 
                    (
	                    select SUM(lea.Amount) as  TotalExpenses
	                    from [2am].dbo.LegalEntityAffordability lea
	                    join [2am].dbo.AffordabilityType aft on lea.AffordabilityTypeKey = aft.AffordabilityTypeKey
	                    where aft.AffordabilityTypeGroupKey in (2, 3)
	                    and lea.LegalEntityKey = {0}
                    ) expenses", businessKey.Key.ToString());
        }
    }
}
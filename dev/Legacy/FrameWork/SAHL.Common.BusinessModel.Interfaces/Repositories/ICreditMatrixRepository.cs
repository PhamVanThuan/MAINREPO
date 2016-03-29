using System.Data;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface ICreditMatrixRepository
    {
        ICreditMatrix GetCreditMatrixByKey(int key);

        ICategory GetCategoryByKey(int key);

        IRateConfiguration GetRateConfigurationByMarginKeyAndMarketRateKey(int MarginKey, int MarketRateKey);

        IMarginProduct GetMarginProductByRateConfigAndOSP(int RateConfigurationKey, int OriginationSourceKey, int ProductKey);

        DataSet GetCreditCriteriaByOSP(int originationSourceKey, int productKey);

		ICreditMatrix GetCreditMatrix(Globals.OriginationSources originationSource);
	}
}
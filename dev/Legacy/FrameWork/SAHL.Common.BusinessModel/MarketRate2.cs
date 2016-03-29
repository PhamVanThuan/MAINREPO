using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
namespace SAHL.Common.BusinessModel
{
    public partial class MarketRate : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MarketRate_DAO>, IMarketRate
    {
        public static IMarketRate GetForCreditMatrixCalc(int OriginationSourceKey, int ProductKey, int FinancialServiceTypeKey)
        {
            //Select MR.* from [2AM].[dbo].[OriginationSourceProduct] OSP (NOLOCK)  
            //join [2AM].[dbo].[OriginationSourceProductConfiguration] OSPC (NOLOCK) on OSPC.OriginationSourceProductKey = OSP.OriginationSourceProductKey  
            //join [2AM].[dbo].[MarketRate] MR (NOLOCK) on OSPC.MarketRateKey = MR.MarketRateKey 
            //where OSP.OriginationSourceKey = @OriginationSourceKey  
            //and OSP.ProductKey = @ProductKey  
            //and OSPC.FinancialServiceTypeKey = @FinancialServiceTypeKey

            string HQL = "select distinct mr from OriginationSourceProductConfiguration_DAO as ospc join ospc.MarketRate mr "
                + "where ospc.OriginationSourceProduct.OriginationSource.Key = ? AND ospc.OriginationSourceProduct.Product.Key = ? AND ospc.FinancialServiceType.Key = ?";

            SimpleQuery<MarketRate_DAO> q = new SimpleQuery<MarketRate_DAO>(HQL, OriginationSourceKey, ProductKey, FinancialServiceTypeKey);

            MarketRate_DAO[] res = q.Execute();

            if (res == null || res.Length == 0)
                return null;

            return new MarketRate(res[0]);
        }

        protected void OnMarketRateHistories_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnMarketRateHistories_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnMarketRateHistories_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnMarketRateHistories_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnRateConfigurations_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
    }
}



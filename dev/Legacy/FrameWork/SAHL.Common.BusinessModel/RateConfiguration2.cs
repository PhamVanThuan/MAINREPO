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
    public partial class RateConfiguration : BusinessModelBase<SAHL.Common.BusinessModel.DAO.RateConfiguration_DAO>, IRateConfiguration
	{
        public static IRateConfiguration GetByMarginKeyAndMarketRateKey(int MarginKey, int MarketRateKey)
        {
            string HQL = "from RateConfiguration_DAO rc where rc.Margin.Key = ? AND rc.MarketRate.Key = ?";
            SimpleQuery<RateConfiguration_DAO> q = new SimpleQuery<RateConfiguration_DAO>(HQL, MarginKey, MarketRateKey);
            q.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
            RateConfiguration_DAO[] res = q.Execute();
            

            if (res.Length > 0)
                return new RateConfiguration(res[0]);

            return null;
        }

        protected void OnMortgageLoans_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnMortgageLoans_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
	}
}



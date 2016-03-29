using System.Data;
using System.Collections.Generic;
using SAHL.Common.DataAccess;
using SAHL.Common.Collections;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using System;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    [FactoryType(typeof(IMarketRateRepository))]
    public class MarketRateRepository : AbstractRepositoryBase, IMarketRateRepository
    {
        /// <summary>
        /// Gets the Market Rates (order by Description by default).
        /// </summary>
        public IReadOnlyEventList<IMarketRate> GetMarketRates()
        {
            string HQL = "FROM MarketRate_DAO ORDER BY Description";
            SimpleQuery<MarketRate_DAO> q = new SimpleQuery<MarketRate_DAO>(HQL);
            MarketRate_DAO[] res = q.Execute();
            if (res.Length == 0)
                return new ReadOnlyEventList<IMarketRate>();

            IEventList<IMarketRate> list = new DAOEventList<MarketRate_DAO, IMarketRate, MarketRate>(res);
            return new ReadOnlyEventList<IMarketRate>(list);
        }

        /// <summary>
        /// Gets the Market Rate by key
        /// </summary>
        public IMarketRate GetMarketRateByKey(int Key)
        {
            return base.GetByKey<IMarketRate, MarketRate_DAO>(Key);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool UpdateMarketRate(IMarketRate marketRate, double oldval, string userID)
        {

            base.Save<IMarketRate, MarketRate_DAO>(marketRate);

            if (marketRate.Key == (int)SAHL.Common.Globals.MarketRates.PrimeLendingRate)
            {
                //update control no.3 'Banks Mortgage Rate' set controlnumeric = value * 100 
                IControl ctrl = base.GetByKey<IControl, Control_DAO>(3);
                ctrl.ControlNumeric = marketRate.Value * 100;
                base.Save<IControl, Control_DAO>(ctrl);
            }

            if (marketRate.Value != oldval)
            {
                SAHLPrincipal principal = SAHLPrincipal.GetCurrent();
                IMarketRateHistory mrh = base.CreateEmpty<IMarketRateHistory, MarketRateHistory_DAO>();

                mrh.ChangeDate = DateTime.Now;
                mrh.ChangedBy = principal.Identity.Name;
                mrh.ChangedByApp = "HALO";
                mrh.ChangedByHost = Environment.MachineName;
                mrh.MarketRate = marketRate;
                mrh.RateAfter = marketRate.Value;
                mrh.RateBefore = oldval;

                base.Save<IMarketRateHistory, MarketRateHistory_DAO>(mrh);
            }

            return true;
        }
    }
}

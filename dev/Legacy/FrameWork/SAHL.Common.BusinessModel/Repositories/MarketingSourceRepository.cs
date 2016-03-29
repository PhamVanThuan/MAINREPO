using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using Castle.ActiveRecord.Queries;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IMarketingSourceRepository))]
    public class MarketingSourceRepository : AbstractRepositoryBase, IMarketingSourceRepository
    {
        /// <summary>
        /// Gets all Marketing Sources (order by Description by default).
        /// </summary>
        public IReadOnlyEventList<IApplicationSource> GetMarketingSources()
        {
            string HQL = "FROM ApplicationSource_DAO  ORDER BY Description";
            SimpleQuery<ApplicationSource_DAO> q = new SimpleQuery<ApplicationSource_DAO>(HQL);
            ApplicationSource_DAO[] res = q.Execute();
            if (res.Length == 0)
                return new ReadOnlyEventList<IApplicationSource>();

            IEventList<IApplicationSource> list = new DAOEventList<ApplicationSource_DAO, IApplicationSource, ApplicationSource>(res);
            return new ReadOnlyEventList<IApplicationSource>(list);
        }

        public IApplicationSource GetMarketingSourceByKey(int SourceKey)
        {
            string HQL = "from ApplicationSource_DAO asrc where asrc.Key = ?";

            SimpleQuery<ApplicationSource_DAO> query
                 = new SimpleQuery<ApplicationSource_DAO>(HQL, SourceKey);

            query.SetQueryRange(1);
            ApplicationSource_DAO[] asrcDAO = query.Execute();

            if (asrcDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<IApplicationSource>(asrcDAO[0]);
            }
            return null;
        }

        public IApplicationSource GetEmptyApplicationSource()
        {
            return base.CreateEmpty<IApplicationSource, ApplicationSource_DAO>();

        }

        public void SaveApplicationSource(IApplicationSource ApplicationSource)
        {
            base.Save<IApplicationSource, ApplicationSource_DAO>(ApplicationSource);
           

        }

        public bool ApplicationSourceExists(string strDescription)
        {
            string HQL = "from ApplicationSource_DAO asrc where asrc.Description = ?";

            SimpleQuery<ApplicationSource_DAO> query
                 = new SimpleQuery<ApplicationSource_DAO>(HQL, strDescription);

            query.SetQueryRange(1);
            ApplicationSource_DAO[] asrcDAO = query.Execute();

            if (asrcDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return true;
            }
            return false;
        }

    }
}

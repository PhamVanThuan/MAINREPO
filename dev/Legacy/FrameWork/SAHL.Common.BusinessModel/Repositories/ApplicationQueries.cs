using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.SearchCriteria;
using SAHL.Common.BusinessModel.DAO;
using NHibernate;
using SAHL.Common.BusinessModel.Interfaces;
using NHibernate.SqlCommand;
using Castle.ActiveRecord.Queries;
using NHibernate.Transform;
using NHibernate.Criterion;
namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationCallbackQuery : ActiveRecordBaseQuery
    {
        int _applicationKey;
        bool _latestRecordOnly;
        bool _openCallbacksOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApplicationKey"></param>
        /// <param name="LatestRecordOnly"></param>
        /// <param name="OpenCallbacksOnly"></param>
        public ApplicationCallbackQuery(int ApplicationKey, bool LatestRecordOnly, bool OpenCallbacksOnly)
            : base(typeof(Callback_DAO))
        {
            _applicationKey = ApplicationKey;
            _latestRecordOnly = LatestRecordOnly;
            _openCallbacksOnly = OpenCallbacksOnly;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override NHibernate.IQuery CreateQuery(NHibernate.ISession session)
        {
            IQuery q = session.CreateQuery("from Callback_DAO cb");
            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        protected override object InternalExecute(ISession session)
        {
            ICriteria Criteria = session.CreateCriteria(typeof(Callback_DAO));

            Criteria.Add(Expression.Eq("GenericKey", _applicationKey)); // where generickey = the applicationkey

            if (_openCallbacksOnly)
                Criteria.Add(Expression.IsNull("CompletedDate")); // where completed date is null

            if (_latestRecordOnly)
                Criteria.SetMaxResults(1); // select top 1 

            Criteria.AddOrder(new Order("CallbackDate", false)); // order by callbackdate desc

            return Criteria.List<Callback_DAO>();           
        }
    }
}

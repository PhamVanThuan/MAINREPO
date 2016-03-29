using Castle.ActiveRecord;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.X2.BusinessModel.DAO.Database
{
    public abstract class DB_X2<T> : ActiveRecordBase<T>, IProxyableDAOObject
    {
        public virtual object ActualDAOObject
        {
            get { return this; }
        }

        public virtual U As<U>() where U : class
        {
            return this as U;
        }
    }
}
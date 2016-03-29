using System;

namespace SAHL.Core.Services
{
    public class QueryParameterManager : IQueryParameterManager
    {
        private IIocContainer iocContainer;

        public QueryParameterManager(IIocContainer iocContainer)
        {
            this.iocContainer = iocContainer;
        }

        public T GetParameter<T>() where T : IQueryParameter
        {
            return this.iocContainer.GetInstance<T>();
        }

        public void SetParameter<T>(Action<T> parameterToSet) where T : IQueryParameter
        {
            var param = this.GetParameter<T>();
            parameterToSet(param);
        }
    }
}
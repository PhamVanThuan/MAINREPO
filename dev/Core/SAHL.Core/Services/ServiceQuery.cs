using System;

namespace SAHL.Core.Services
{
    public abstract class ServiceQuery<T> : ServiceCommand, IServiceQuery<IServiceQueryResult<T>>
    {
        public ServiceQuery()
            : base()
        {
        }

        public ServiceQuery(Guid id)
            : base(id)
        {
        }

        public IServiceQueryResult<T> Result { get; set; }
    }
}
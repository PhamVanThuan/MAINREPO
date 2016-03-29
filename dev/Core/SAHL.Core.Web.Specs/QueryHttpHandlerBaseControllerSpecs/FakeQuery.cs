using SAHL.Core.Services;
using System;

namespace SAHL.Core.Web.Specs.QueryHttpHandlerBaseControllerSpecs
{
    public class FakeQuery : IServiceQuery
    {
        public Guid Id
        {
            get;
            protected set;
        }

        public object Result
        {
            get
            {
                return 1;
            }
        }

        public FakeQuery(Guid id)
        {
            this.Id = id;
        }
    }
}
using DomainService2.IOC;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Test;

namespace SAHL.DomainService.Test
{
    public abstract class DomainServiceTestBase : TestBase
    {
        protected IDomainMessageCollection messages;
        protected DomainServiceLoader loader;

        public DomainServiceTestBase()
        {
            messages = new DomainMessageCollection();
            loader = new DomainServiceLoader();
        }
    }
}
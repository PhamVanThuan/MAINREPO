using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DomainProcessManager.Specs
{
    public class FakeEvent1 : Event
    {
        public FakeEvent1(DateTime date)
            : base(date)
        {
        }

        public FakeEvent1(Guid id, DateTime date)
            : base(id, date)
        {
        }
    }
}

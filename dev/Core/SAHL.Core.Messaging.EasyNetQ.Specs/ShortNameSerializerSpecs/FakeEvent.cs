using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.EasyNetQ.Specs.ShortNameSerializerSpecs
{
    public class FakeEvent : IEvent
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string ClassName { get; set; }
        public Guid Id { get;set; }
    }

    public class ComplexWrappedEvent<T> : IEvent where T : class
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string ClassName { get; set; }
        public Guid Id { get;set; }
    }

    public class FakeEventWithALongNameAboveTwoHundredAndFiftyFiveCharactersLongToBreakSerializerWhereExpectedSoThisShouldBeFunAndTediousToWriteAndEvenThoughThisClassIsAbsurdTakeIntoAccountThatThereAreNamespacesThatComeIntoPlay : IEvent
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string ClassName { get; set; }
        public Guid Id { get; set; }
    }
}

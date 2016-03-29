using EasyNetQ;
using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.EasyNetQ.Specs.ShortNameSerializerSpecs
{
    [Subject(typeof(ShortNameSerializer))]
    public class when_name_is_too_long : WithFakes
    {
        static Type type;
        static ITypeNameSerializer serializer;
        static Exception exception;
        
        Establish context = () =>
        {
            type = typeof(FakeEventWithALongNameAboveTwoHundredAndFiftyFiveCharactersLongToBreakSerializerWhereExpectedSoThisShouldBeFunAndTediousToWriteAndEvenThoughThisClassIsAbsurdTakeIntoAccountThatThereAreNamespacesThatComeIntoPlay);
            serializer = new ShortNameSerializer();
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => serializer.Serialize(type));
        };

        It should_throw_exception = () =>
        {
            exception.ShouldNotBeNull();
        };
    }
}

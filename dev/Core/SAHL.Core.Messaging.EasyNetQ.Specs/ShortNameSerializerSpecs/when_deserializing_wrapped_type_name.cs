using EasyNetQ;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Core.Messaging.EasyNetQ.Specs.ShortNameSerializerSpecs
{
    [Subject(typeof(ShortNameSerializer))]
    public class when_deserializing_wrapped_type_name : WithFakes
    {
        static Type type;
        static string serializedString;
        static ITypeNameSerializer serializer;
        static Type result;

        Establish context = () =>
        {
            type = typeof(WrappedEvent<FakeEvent>);
            serializer = new ShortNameSerializer();
            serializedString = serializer.Serialize(type);
        };

        Because of = () =>
        {
            result = serializer.DeSerialize(serializedString);
        };

        It should_return_type_expected = () =>
        {
            result.ShouldEqual(type);
        };
    }
}

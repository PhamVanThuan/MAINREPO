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
    public class when_serializing_a_type : WithFakes
    {
        static Type type;
        static string result;
        static ITypeNameSerializer serializer;

        Establish context = () =>
        {
            type = typeof(WrappedEvent<FakeEvent>);
            serializer = new ShortNameSerializer();
            
        };

        Because of = () =>
        {
            result = serializer.Serialize(type);
        };

        It should_not_serialize_the_version_of_the_type = () =>
        {
            result.ShouldNotContain("Version=");
        };

        It should_not_serialize_the_culture = () =>
        {
            result.ShouldNotContain("Culture=neutral");
        };

        It should_not_serialize_the_public_key_token = () =>
        {
            result.ShouldNotContain("PublicKeyToken=null");
        };
    }
}

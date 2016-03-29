using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events.Specs.EventSerialiserSpecs.Fakes;
using SAHL.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Core.Events.Specs.EventSerialiserSpecs
{
    public class when_serialising_an_event_with_a_null_value : WithFakes
    {
        private static IEventSerialiser eventSerialiser;
        private static string result;
        private static FakeEvent eventToSerialise;
        private static int numberOfProperties;

        private Establish context = () =>
        {
            eventSerialiser = new EventSerialiser();
            numberOfProperties = typeof(FakeEvent).GetProperties().Count();
            eventToSerialise = new FakeEvent(CombGuid.Instance.Generate(), DateTime.Now, null, 1, FakeEnum.abc, 1, 2000, new List<string>() { "hello", "world" }, null, null,null);
        };

        private Because of = () =>
        {
            result = eventSerialiser.Serialise(eventToSerialise);
        };

        private It should_serialise_the_event_to_xml_with_a_event_root_element = () =>
        {
            XDocument xDoc = XDocument.Parse(result);
            xDoc.Root.Name.ShouldEqual("Event");
        };

        private It should_serialise_the_event_to_xml_with_nodes_corresponding_to_public_properties = () =>
        {
            XDocument xDoc = XDocument.Parse(result);
            xDoc.Root.Nodes().Count().ShouldEqual(numberOfProperties);
        };
    }
}
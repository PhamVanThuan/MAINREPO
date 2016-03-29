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
    public class when_serialising_an_event : WithFakes
    {
        private static IEventSerialiser eventSerialiser;
        private static string result;
        private static FakeEvent eventToSerialise;
        private static int numberOfProperties;
        private static DateTime dateProperty;

        private Establish context = () =>
        {
            eventSerialiser = new EventSerialiser();
            numberOfProperties = typeof(FakeEvent).GetProperties().Count();
            dateProperty = DateTime.Now;
            eventToSerialise = new FakeEvent(CombGuid.Instance.Generate(), dateProperty, "john", 1, FakeEnum.abc, 1, 2000,
                new List<string>() { "hello", "world" },
                new string[] { "hello", "world" },
                new List<Fake>() { new Fake(1, "hello"),
                new Fake(2, "world") },
                new Fake(1, "hello world"));
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

        It should_serialise_the_list_property_to_xml = () =>
        {
            XDocument xml = XDocument.Parse(result);
            var element = xml.Root.Elements().Where(x => x.Name == "ListProperty").Single();
            element.Elements().Count().ShouldEqual(2);
            element.Elements().ShouldContain(x => x.Value == "hello" || x.Value == "world");
        };

        It should_serialise_the_fake_list_property_to_xml = () =>
        {
            XDocument xml = XDocument.Parse(result);
            var element = xml.Root.Elements().Where(x => x.Name == "FakeList").Single();
            element.Elements().Count().ShouldEqual(2);
            element.Elements().Elements().ShouldContain(x => x.Value == "hello" || x.Value == "world" || x.Value == "1" || x.Value == "2");
        };

        It should_serialise_the_array_property_to_xml = () =>
        {
            XDocument xml = XDocument.Parse(result);
            var element = xml.Root.Elements().Where(x => x.Name == "ArrayProperty").Single();
            element.Elements().Count().ShouldEqual(2);
            element.Elements().ShouldContain(x => x.Value == "hello" || x.Value == "world");
        };

        It should_serial_date_property_to_xml = () =>
        {
            XDocument xml = XDocument.Parse(result);
            var element = xml.Root.Elements().Where(x => x.Name == "Date").Single();
            element.Value.ShouldEqual(dateProperty.ToString(EventSerialiser.W3CDateTimeFormatString));
        };

        It should_serial_fake_property_to_xml = () =>
        {
            XDocument xml = XDocument.Parse(result);
            var element = xml.Root.Elements().Where(x => x.Name == "FakeProperty").Single();
            element.Elements().Count().ShouldEqual(2);
            element.Elements().ShouldContain(x => x.Value == "1" || x.Value == "hello world");
        };
    }
}
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events.Specs.EventSerialiserSpecs.Fakes;
using SAHL.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Core.Events.Specs.EventSerialiserSpecs
{
    public class when_deserialising_an_event : WithFakes
    {
        private static IEventSerialiser eventSerialiser;
        private static string serializedEvent;
        private static FakeEvent eventToSerialise;
        private static FakeEvent result;

        private static DateTime eventDate;

        private Establish context = () =>
        {
            eventSerialiser = new EventSerialiser();
            eventDate = DateTime.Now;
            eventToSerialise = new FakeEvent(CombGuid.Instance.Generate(), eventDate, "john", 1, FakeEnum.def, 1, 2000, new List<string> { "hello", "world" }
                , new [] { "hello", "world" }, new List<Fake> { new Fake(1, "hello"), new Fake(2, "world") }, new Fake(2, "hello world"));
            serializedEvent = eventSerialiser.Serialise(eventToSerialise);
        };

        private Because of = () =>
        {
            result = eventSerialiser.Deserialise<FakeEvent>(serializedEvent);
        };

        private It should_deserialise_xml_to_an_event = () =>
        {
            result.ShouldNotBeNull();
        };

        private It should_deserialise_the_enum_property_from_xml = () =>
        {
            result.FakeEnum.ShouldEqual(eventToSerialise.FakeEnum);
        };

        private It should_deserialise_the_list_property_from_xml = () =>
        {
            result.ListProperty.ShouldEqual(eventToSerialise.ListProperty);
            result.ListProperty.Count().ShouldEqual(2);
            result.ListProperty.ShouldContain(x => x == "hello" || x == "world");
        };

        private It should_deserialise_the_fake_list_property_from_xml = () =>
        {
            result.FakeList.Count().ShouldEqual(2);
            result.FakeList.ShouldContain(x => x.StringProperty == "hello" || x.StringProperty == "world" || x.IntProperty == 1 || x.IntProperty == 2);
        };

        private It should_deserialise_fake_property_from_xml = () =>
        {
            result.FakeProperty.ShouldNotBeNull();
            result.FakeProperty.IntProperty.ShouldEqual(2);
            result.FakeProperty.StringProperty.ShouldEqual("hello world");
        };

        private It should_deserialise_the_array_property_from_xml = () =>
        {
            result.ArrayProperty.Count().ShouldEqual(2);
            result.ArrayProperty.ShouldContain(x => x == "hello" || x == "world");
        };

        private It should_deserialise_the_string_property_from_xml = () =>
        {
            result.StringProperty.ShouldEqual("john");
        };

        private It should_deserialise_the_int_property_from_xml = () =>
        {
            result.IntProperty.ShouldEqual(1);
        };

        private It should_deserialise_the_null_int_property_from_xml = () =>
        {
            result.NullInt.ShouldEqual(1);
        };

        private It should_deserialise_the_decimal_property_from_xml = () =>
        {
            result.DecimalProperty.ShouldEqual(2000);
        };

        private It should_deserialise_the_date_property_from_xml = () =>
        {
            result.Date.ShouldEqual(eventDate);
        };
    }
}
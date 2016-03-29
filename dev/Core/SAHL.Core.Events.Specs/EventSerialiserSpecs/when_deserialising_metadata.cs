using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Core.Events.Specs.EventSerialiserSpecs
{
    public class when_deserialising_metadata : WithFakes
    {
        private static IEventSerialiser eventSerialiser;
        private static string serializedMetadata;
        private static IServiceRequestMetadata metadataToSerialise;
        private static int numberOfProperties;

        private Establish context = () =>
        {
            eventSerialiser = new EventSerialiser();
            numberOfProperties = typeof(IServiceRequestMetadata).GetProperties().Count();
            metadataToSerialise = new ServiceRequestMetadata(new Dictionary<string, string>() { { "user", "John Doe" }, { "ip", "192.168.0.0" } });
            serializedMetadata = eventSerialiser.SerialiseEventMetadata(metadataToSerialise);
            metadataToSerialise = null;
        };

        private Because of = () =>
        {
            metadataToSerialise = eventSerialiser.DeserialiseEventMetadata(serializedMetadata);
        };

        private It should_deserialise_xml_with_to_an_event = () =>
        {
            metadataToSerialise.ShouldNotBeNull();
        };

        private It should_deserialise_the_xml_into_an_event_with_properties_set_to_xml_node_values = () =>
        {
            XDocument xDoc = XDocument.Parse(serializedMetadata);
            var nodes = xDoc.Root.Nodes().ToArray();
            for (int i = 0; i < nodes.Length; i++)
            {
                string key = (nodes[i] as XElement).Descendants("Key").SingleOrDefault().Value;
                string value = (nodes[i] as XElement).Descendants("Value").SingleOrDefault().Value;

                key.ShouldNotBeNull();
                value.ShouldNotBeNull();
                key.ShouldEqual(metadataToSerialise.ElementAtOrDefault(i).Key);
                value.ShouldEqual(metadataToSerialise.ElementAtOrDefault(i).Value);
            }
        };
    }
}
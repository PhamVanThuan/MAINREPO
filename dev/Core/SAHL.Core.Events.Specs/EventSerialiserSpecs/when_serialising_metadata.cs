using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SAHL.Core.Events.Specs.EventSerialiserSpecs
{
    public class when_serialising_metadata : WithFakes
    {
        private static IEventSerialiser eventSerialiser;
        private static string result;
        private static ServiceRequestMetadata metadata;
        private static Dictionary<string, string> metadataValues;
        private static int numberOfProperties;

        private Establish context = () =>
        {
            eventSerialiser = new EventSerialiser();
            metadataValues = new Dictionary<string, string>() { { "user", "John Doe" }, { "ip", "192.168.0.0" } };
            numberOfProperties = metadataValues.Keys.Count;
            metadata = new ServiceRequestMetadata(metadataValues);
        };

        private Because of = () =>
        {
            result = eventSerialiser.SerialiseEventMetadata(metadata);
        };

        private It should_serialise_the_metadata_to_xml_with_a_ServiceRequestMetadata_root_element = () =>
        {
            XDocument xDoc = XDocument.Parse(result);
            xDoc.Root.Name.ShouldEqual("ServiceRequestMetadata");
        };

        private It should_serialise_the_metadata_to_xml_with_nodes_corresponding_to_public_properties = () =>
        {
            XDocument xDoc = XDocument.Parse(result);
            xDoc.Root.Nodes().Count().ShouldEqual(numberOfProperties);
        };
    }
}
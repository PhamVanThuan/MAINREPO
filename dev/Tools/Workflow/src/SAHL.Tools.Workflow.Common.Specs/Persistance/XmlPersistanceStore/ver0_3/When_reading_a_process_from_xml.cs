using System.Linq;
using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore.ver0_3;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Specs.Persistance.XmlPersistanceStore.ver0_3
{
    public class When_reading_a_process_from_xml : WithFakes
    {
        static ver0_3_XmlPersistanceStore persistanceStore;
        static ver0_3_XmlNodeNameManager nameManager;
        static XDocument xmlDocument;
        static XElement xmlElement;
        static string processName;
        static string productVersion;
        static string mapVersion;
        static string retrieved;
        static string legacy;
        static string viewableOnUserInterfaceVersion;
        static Process result;

        Establish context = () =>
        {
            nameManager = new ver0_3_XmlNodeNameManager();
            processName = "Origination";
            productVersion = "0.3";
            mapVersion = "2.36.0.31";
            retrieved = "false";
            legacy = "false";
            viewableOnUserInterfaceVersion = "3";

            xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(nameManager.ProcessElementName,
                    new XAttribute(nameManager.NameAttribute, processName),
                    new XAttribute(nameManager.ProductVersionAttributeName, productVersion),
                    new XAttribute(nameManager.MapVersionAttributeName, mapVersion),
                    new XAttribute(nameManager.RetrievedAttributeName, retrieved),
                    new XAttribute(nameManager.Legacy, legacy),
                    new XAttribute(nameManager.ViewableOnUserInterfaceVersion, viewableOnUserInterfaceVersion)
                    ));
            xmlElement = xmlDocument.Elements(nameManager.ProcessElementName).Single();

            persistanceStore = new ver0_3_XmlPersistanceStore();
        };

        Because of = () =>
        {
            result = persistanceStore.ReadProcessFromXml(xmlElement);
        };

        It should_return_a_non_null_process = () =>
        {
            result.ShouldNotBeNull();
        };

        It should_have_set_the_process_name_property = () =>
        {
            result.Name.ShouldEqual<string>(processName);
        };

        It should_have_set_the_process_productversion_property = () =>
        {
            result.ProductVersion.ShouldEqual<string>(productVersion);
        };

        It should_have_set_the_process_mapversion_property = () =>
        {
            result.MapVersion.ShouldEqual<string>(mapVersion);
        };

        It should_have_set_the_process_retrieved_property = () =>
        {
            result.Retrieved.ShouldEqual<string>(retrieved);
        };

        It should_have_set_the_process_legacy_property = () =>
        {
            result.Legacy.ShouldEqual<string>(legacy);
        };

        It should_have_set_the_process_viewable_on_user_interface_version_property = () =>
        {
            result.ViewableOnUserInterfaceVersion.ShouldEqual<string>(viewableOnUserInterfaceVersion);
        };
    }
}

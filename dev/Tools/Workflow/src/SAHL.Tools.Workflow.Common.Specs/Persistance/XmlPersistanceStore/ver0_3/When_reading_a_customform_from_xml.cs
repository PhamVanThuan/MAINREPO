using System.Linq;
using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore.ver0_3;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Specs.Persistance.XmlPersistanceStore.ver0_3
{
    internal class When_reading_a_customform_from_xml : WithFakes
    {
        static ver0_3_XmlPersistanceStore persistanceStore;
        static ver0_3_XmlNodeNameManager nameManager;
        static XDocument xmlDocument;
        static XElement xmlElement;
        static string name;
        static string description;

        static CustomForm result;

        Establish context = () =>
        {
            nameManager = new ver0_3_XmlNodeNameManager();
            name = "App_FurtherLendingCalculator";
            description = "Rework Application";

            xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(nameManager.CustomFormElementName,
                    new XAttribute(nameManager.NameAttribute, name),
                    new XAttribute(nameManager.DescriptionAttribute, description)
                    ));
            xmlElement = xmlDocument.Elements(nameManager.CustomFormElementName).Single();

            persistanceStore = new ver0_3_XmlPersistanceStore();
        };

        Because of = () =>
        {
            result = persistanceStore.ReadCustomFormFromXml(xmlElement);
        };

        It should_return_a_non_null_customform = () =>
        {
            result.ShouldNotBeNull();
        };

        It should_have_set_the_customform_name_property = () =>
        {
            result.Name.ShouldEqual<string>(name);
        };

        It should_have_set_the_customform_description_property = () =>
        {
            result.Description.ShouldEqual<string>(description);
        };
    }
}
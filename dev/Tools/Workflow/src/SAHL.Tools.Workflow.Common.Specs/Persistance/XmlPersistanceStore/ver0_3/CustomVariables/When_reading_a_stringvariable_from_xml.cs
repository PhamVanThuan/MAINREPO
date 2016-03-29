using System.Linq;
using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore.ver0_3;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Specs.Persistance.XmlPersistanceStore.ver0_3.CustomVariables
{
    internal class When_reading_a_stringvariable_from_xml : WithFakes
    {
        static ver0_3_XmlPersistanceStore persistanceStore;
        static ver0_3_XmlNodeNameManager nameManager;
        static XDocument xmlDocument;
        static XElement xmlElement;
        static string variableName;
        static string variableType;
        static CustomVariableTypeEnum variableTypeEnum;
        static int length;

        static StringVariable result;

        Establish context = () =>
        {
            nameManager = new ver0_3_XmlNodeNameManager();
            variableName = "ApplicationKey";
            variableType = nameManager.String;
            variableTypeEnum = CustomVariableTypeEnum.String;
            length = 10;

            xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(nameManager.CustomVariableElementName,
                    new XAttribute(nameManager.NameAttribute, variableName),
                    new XAttribute(nameManager.TypeAttributeName, variableType),
                    new XAttribute(nameManager.LengthAttributeName, length)
                    ));
            xmlElement = xmlDocument.Elements(nameManager.CustomVariableElementName).Single();

            persistanceStore = new ver0_3_XmlPersistanceStore();
        };

        Because of = () =>
        {
            result = persistanceStore.ReadCustomVariableFromXml(xmlElement) as StringVariable;
        };

        It should_return_a_non_null_string_variable = () =>
        {
            result.ShouldNotBeNull();
        };

        It should_have_set_the_customvariable_name_property = () =>
        {
            result.Name.ShouldEqual<string>(variableName);
        };

        It should_have_set_the_customvariable_length_property = () =>
        {
            result.Length.ShouldEqual<int>(length);
        };
    }
}
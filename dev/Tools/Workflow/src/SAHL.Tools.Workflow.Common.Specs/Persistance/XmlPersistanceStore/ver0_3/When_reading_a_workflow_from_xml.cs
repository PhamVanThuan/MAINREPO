using System;
using System.Linq;
using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore.ver0_3;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Specs.Persistance.XmlPersistanceStore.ver0_3
{
    public class When_reading_a_workflow_from_xml : WithFakes
    {
        static ver0_3_XmlPersistanceStore persistanceStore;
        static ver0_3_XmlNodeNameManager nameManager;
        static XDocument xmlDocument;
        static XElement xmlElement;
        static string workflowName;
        static Single locationX;
        static Single locationY;
        static int genericKeyType;
        static SAHL.Tools.Workflow.Common.WorkflowElements.Workflow result;

        Establish context = () =>
        {
            nameManager = new ver0_3_XmlNodeNameManager();
            workflowName = "Readvance Payments";
            locationX = 20;
            locationY = 70;
            genericKeyType = 6;

            xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(nameManager.WorkflowElementName,
                    new XAttribute(nameManager.WorkflowNameAttribute, workflowName),
                    new XAttribute(nameManager.LocationXAttribute, locationX),
                    new XAttribute(nameManager.LocationYAttribute, locationY),
                    new XAttribute(nameManager.GenericKeyTypeKey, genericKeyType)
                    ));
            xmlElement = xmlDocument.Elements(nameManager.WorkflowElementName).Single();

            persistanceStore = new ver0_3_XmlPersistanceStore();
        };

        Because of = () =>
        {
            result = persistanceStore.ReadWorkflowFromXml(xmlElement);
        };

        It should_return_a_non_null_workflow = () =>
        {
            result.ShouldNotBeNull();
        };

        It should_have_set_the_workflow_name_property = () =>
        {
            result.Name.ShouldEqual<string>(workflowName);
        };

        It should_have_set_the_workflow_locationX_property = () =>
        {
            result.LocationX.ShouldEqual<Single>(locationX);
        };

        It should_have_set_the_workflow_locationY_property = () =>
        {
            result.LocationY.ShouldEqual<Single>(locationY);
        };

        It should_have_set_the_workflow_generickeytypekey_property = () =>
        {
            result.GenericKeyType.ShouldEqual<int>(genericKeyType);
        };
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore.ver0_3;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Specs.Persistance.XmlPersistanceStore.ver0_3
{
    internal class When_reading_a_clapperboard_from_xml : WithFakes
    {
        static ver0_3_XmlPersistanceStore persistanceStore;
        static ver0_3_XmlNodeNameManager nameManager;
        static XDocument xmlDocument;
        static XElement xmlElement;
        static string keyVariable;
        static string subject;
        static GlobalRole role;
        static GlobalRole[] availableRoles;
        static string roleName;
        static Single locationX;
        static Single locationY;

        static ClapperBoard result;
        static IList<PostLoadRequirement> postLoadRequirements;

        Establish context = () =>
        {
            nameManager = new ver0_3_XmlNodeNameManager();
            keyVariable = "ApplicationKey";
            subject = "a subject";
            locationX = 20;
            locationY = 70;
            roleName = "TestGlobalRole";
            postLoadRequirements = new List<PostLoadRequirement>();

            xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(nameManager.ClapperBoardElementName,
                    new XAttribute(nameManager.LocationXAttribute, locationX),
                    new XAttribute(nameManager.LocationYAttribute, locationY),
                    new XAttribute(nameManager.LimitAccessToAttribute, roleName),
                    new XAttribute(nameManager.SubjectAttribute, subject),
                    new XAttribute(nameManager.KeyVariableAttribute, keyVariable)
                    ));
            xmlElement = xmlDocument.Elements(nameManager.ClapperBoardElementName).Single();

            persistanceStore = new ver0_3_XmlPersistanceStore();
        };

        Because of = () =>
        {
            result = persistanceStore.ReadClapperBoardFromXml(xmlElement, postLoadRequirements);
        };

        It should_return_a_non_null_clapperboard = () =>
        {
            result.ShouldNotBeNull();
        };

        It should_have_set_the_clapperboard_locationX_property = () =>
        {
            result.LocationX.ShouldEqual<Single>(locationX);
        };

        It should_have_set_the_clapperboard_locationY_property = () =>
        {
            result.LocationY.ShouldEqual<Single>(locationY);
        };

        It should_have_set_the_clapperboard_subject_property = () =>
        {
            result.Subject.ShouldEqual<string>(subject);
        };

        It should_have_set_the_clapperboard_keyvariable_property = () =>
        {
            result.KeyVariable.ShouldEqual<string>(keyVariable);
        };
    }
}
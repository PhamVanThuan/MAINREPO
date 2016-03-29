using System.Collections.Generic;
using System.Xml.Linq;
using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore
{
    public interface IVersionedXmlPersistanceStore
    {
        XmlPersistanceVersionEnum VersionSupported { get; }

        IVersionedNodeNameManager NodeNameManager { get; }

        bool IsXMLDocumentSupported(XDocument xmlDocument);

        Process ReadProcessFromXml(XElement xmlElement);

        XElement WriteProcessToXml(Process element);

        SAHL.Tools.Workflow.Common.WorkflowElements.Workflow ReadWorkflowFromXml(XElement xmlElement);

        XElement WriteWorkflowToXml(SAHL.Tools.Workflow.Common.WorkflowElements.Workflow element);

        AbstractCustomVariable ReadCustomVariableFromXml(XElement xmlElement);

        XElement WriteCustomVariableToXml(AbstractCustomVariable element);

        ExternalActivityDefinition ReadExternalActivityDefinitionFromXml(XElement xmlElement);

        XElement WriteExternalActivityDefinitionToXml(ExternalActivityDefinition element);

        CustomForm ReadCustomFormFromXml(XElement xmlElement);

        XElement WriteCustomFormToXml(CustomForm element);

        ClapperBoard ReadClapperBoardFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements);

        XElement WriteClapperBoardToXml(ClapperBoard element);

        AbstractNamedState ReadStateFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements);

        XElement WriteStateToXml(CustomForm element);

        CodeSection ReadCodeSectionFromXml(XElement xmlElement);

        XElement WriteCodeSectionToXml(CodeSection element);

        AbstractActivity ReadAbstractActivityFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements);

        XElement WriteAbstractActivityToXml(AbstractActivity element);

        AbstractRole ReadAbstractRoleFromXml(XElement xmlElement, IList<PostLoadRequirement> postLoadRequirements);

        XElement WriteAbstractRoleToXml(AbstractRole element);

        UsingStatement ReadUsingStatementFromXml(XElement xmlElement);

        XElement WriteUsingStatementToXml(UsingStatement element);

        AssemblyReference ReadAssemblyReferenceFromXml(XElement xmlElement);

        XElement WriteAssemblyReferenceToXml(AssemblyReference element);

        BusinessStageTransition ReadBusinessStageTransitionFromXml(XElement xmlElement);

        XElement WriteBusinessStageTransitionToXml(BusinessStageTransition element);
    }
}
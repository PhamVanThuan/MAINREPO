using SAHL.Tools.Workflow.Common.WorkflowElements;

namespace SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore
{
    public class PostLoadRequirement
    {
        public PostLoadRequirement(AbstractElement element, string requirementName, string requirementValue)
        {
            this.Element = element;
            this.RequirementName = requirementName;
            this.RequirementValue = requirementValue;
        }

        public AbstractElement Element { get; protected set; }

        public string RequirementName { get; protected set; }

        public string RequirementValue { get; protected set; }
    }
}
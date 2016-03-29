using ObjectMaps.FloBO;

namespace BuildingBlocks.Navigation.FLOBO.AffordabilityAssessments
{
    public class AffordabilityAssessmentNode : AffordabilityAssessmentNodeControls
    {
        public void ClickAffordabilityAssessments()
        {
            base.AffordabilityAssessments.Click();
        }

        public void ClickAffordabilityAssessment(string LegalEntityName)
        {
            base.AffordabilityAssessment(LegalEntityName).Click();
        }

        public void ClickUpdateAffordabilityAssessment()
        {
            base.UpdateAffordabilityAssessment.Click();
        }

        public void ClickAddAffordabilityAssessment()
        {
            base.AddAffordabilityAssessment.Click();
        }

        public void ClickDeleteAffordabilityAssessment()
        {
            base.DeleteAffordabilityAssessment.Click();
        }
    }
}
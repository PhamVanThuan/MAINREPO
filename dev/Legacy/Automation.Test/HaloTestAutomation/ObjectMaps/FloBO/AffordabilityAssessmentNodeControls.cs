using ObjectMaps.NavigationControls;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.FloBO
{
    public class AffordabilityAssessmentNodeControls : BaseNavigation
    {
        [FindBy(Title = "Affordability Assessments")]
        protected Link AffordabilityAssessments { get; set; }

        [FindBy(Title = "Add Affordability Assessment")]
        protected Link AddAffordabilityAssessment { get; set; }

        [FindBy(Title = "Delete Affordability Assessment")]
        protected Link DeleteAffordabilityAssessment { get; set; }

        [FindBy(Title = "Update Affordability Assessment")]
        protected Link UpdateAffordabilityAssessment { get; set; }

        protected Link AffordabilityAssessment(string legalEntityName)
        {
            return base.Document.Link(Find.ByText(new Regex(@"^[\x20-\x7E]*" + legalEntityName + "$")));
        }
    }
}
using ObjectMaps.Pages;
using ObjectMaps.Presenters.Origination;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks.Presenters.Origination
{
    public class LegalEntityRelationshipsAffordabilityAssessment : LegalEntityRelationshipsAffordabilityAssessmentControls
    {
        public void SelectRelationshipType(string relationshipType)
        {
            base.ddlRelationshipType.Select(relationshipType);
        }

        public void ClickAddButton()
        {
            base.btnAdd.Click();
        }
    }
}

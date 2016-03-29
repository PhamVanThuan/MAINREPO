using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LegalEntity
{
    public class LegalEntityRelationshipsRelate : LegalEntityRelationshipsRelateControls
    {
        private readonly IWatiNService watinService;

        public LegalEntityRelationshipsRelate()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }

        /// <summary>
        /// Adds a relationship to the selected legal entity
        /// </summary>
        /// <param name="relationshipType">Relationship Type to Add</param>
        public void AddRelationship(string relationshipType)
        {
            base.ddlRelationshipType.Option(relationshipType).Select();
            base.btnAdd.Click();
        }

        /// <summary>
        /// Updates an existing Legal Entity Relationship
        /// </summary>
        /// <param name="currentRelationshipType">Existing Legal Entity Relationship</param>
        /// <param name="newRelationshipType">New Relationship Type</param>
        public void UpdateRelationship(string currentRelationshipType, string newRelationshipType)
        {
            base.gridSelectRelationship(currentRelationshipType).Click();
            base.ddlRelationshipType.Option(newRelationshipType).Select();
            base.btnAdd.Click();
        }

        /// <summary>
        /// Deletes an existing Legal Entity Relationship
        /// </summary>
        /// <param name="relationshipType">Relationship to Delete</param>
        public void DeleteRelationship(string relationshipType)
        {
            base.gridSelectRelationship(relationshipType).Click();
            watinService.HandleConfirmationPopup(base.btnAdd);
        }
    }
}
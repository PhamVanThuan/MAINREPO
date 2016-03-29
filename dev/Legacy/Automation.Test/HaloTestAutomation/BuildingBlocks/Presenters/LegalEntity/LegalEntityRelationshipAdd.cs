using BuildingBlocks.Presenters.CommonPresenters;
using Common.Enums;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.LegalEntity
{
    /// <summary>
    /// Building blocks for the Legal Entity Relationships Add screen
    /// </summary>
    public class LegalEntityRelationshipAdd : ClientSuperSearchControls
    {
        /// <summary>
        /// Clicks on the [New Legal Entity] button
        /// </summary>
        public void AddNewLegalEntity()
        {
            base.NewLegalEntity.Click();
        }

        /// <summary>
        /// This method will be used to add a legal entity i
        /// </summary>
        /// <param name="idNumber">ID Number of person to add</param>
        /// <param name="relationshipType">Relationship to add between the LE and applicant</param>
        public void AddExistingLegalEntity(string idNumber, string relationshipType)
        {
            base.Document.Page<ClientSuperSearch>().PopulateSearch(null, null, idNumber, null, null);
            base.Document.Page<ClientSuperSearch>().PerformSearch();
            base.Document.Page<ClientSuperSearch>().SelectByIDNumber(idNumber);
            //we will now be on the legal entity update screen
            base.Document.Page<LegalEntityDetails>().Update(MaritalStatusEnum.Single, LanguageEnum.English);
            base.Document.Page<LegalEntityDetails>().ClickSubmitButton();
            base.Document.Page<LegalEntityRelationshipsRelate>().AddRelationship(relationshipType);
        }
    }
}
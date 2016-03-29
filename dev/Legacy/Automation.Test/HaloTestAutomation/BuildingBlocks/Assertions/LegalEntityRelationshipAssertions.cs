using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using System.Linq;

namespace BuildingBlocks.Assertions
{
    /// <summary>
    /// Assertions for legal entity relationships
    /// </summary>
    public static class LegalEntityRelationshipAssertions
    {
        private static readonly ILegalEntityService legalEntityService;

        static LegalEntityRelationshipAssertions()
        {
            legalEntityService = ServiceLocator.Instance.GetService<ILegalEntityService>();
        }

        /// <summary>
        /// Asserts that a given relationship exists between two legal entities.
        /// </summary>
        /// <param name="idNumber">ID Number of the related legalentity</param>
        /// <param name="relationshipType">Legal Entity Relationship Type</param>
        /// <param name="legalentitykey">LegalEntityKey of applicant to whom the related legalentity is being added</param>
        public static void AssertRelationshipExists(string idNumber, string relationshipType, int legalentitykey)
        {
            var r = legalEntityService.GetLegalEntityRelationship(legalentitykey);
            bool b = r.RowList.Any(item => item.Column(0).Value == idNumber && item.Column(1).Value == relationshipType);
            Assert.True(b, string.Format("Legal Entity Relationship of type {0} for LE with ID Number {1} not found", relationshipType, idNumber));
        }

        /// <summary>
        /// This assertion ensures that a duplicate legal entity relationship record has not been added to the database for a legal entity
        /// and a given relationship type
        /// </summary>
        /// <param name="idNumber">IDNumber of person who is being setup as a related LE</param>
        /// <param name="relationshipType">Relationship Type</param>
        /// <param name="legalentitykey">Legal Entity</param>
        public static void AssertDuplicateRelationshipDoesNotExist(string idNumber, string relationshipType, int legalentitykey)
        {
            var r = legalEntityService.GetLegalEntityRelationship(legalentitykey);
            int i = 0;
            if (r.RowList.Any(item => item.Column(0).Value == idNumber && item.Column(1).Value == relationshipType))
            {
                i++;
            }
            Assert.AreEqual(i, 1, string.Format("Duplicate Legal Entity Relationship of type {0} for LE with ID Number {1} exists.", relationshipType, idNumber));
        }

        /// <summary>
        /// This assertion will make sure that a relationship does not exist against a legal entite when provided with the relationship
        /// type and the legal entity's ID number
        /// </summary>
        /// <param name="existingRelationship">Relationship that shouldnt exist</param>
        /// <param name="idNumber"> </param>
        /// <param name="legalentitykey">Legal Entity to whom the person was previously related.</param>
        public static void AssertRelationshipDoesNotExist(string existingRelationship, string idNumber, int legalentitykey)
        {
            var r = legalEntityService.GetLegalEntityRelationship(legalentitykey);
            bool b = false;
            if (r.HasResults)
            {
                if (r.RowList.Any(item => item.Column(0).Value == idNumber && item.Column(1).Value == existingRelationship))
                {
                    b = true;
                }
            }
            Assert.False(b, string.Format("Legal Entity Relationship of type {0} for LE with ID Number {1} should not exist",
                existingRelationship, idNumber));
        }
    }
}
using BuildingBlocks;
using BuildingBlocks.Assertions;
using BuildingBlocks.Navigation;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Reflection;

namespace ApplicationCaptureTests.Views.LegalEntityFloBo
{
    /// <summary>
    /// Contains the tests for Legal Entity Relationships
    /// </summary>
    [RequiresSTA]
    public class LegalEntityRelationshipsTest : TestBase<LegalEntityRelationshipsRelate>
    {
        #region globalVariables

        /// <summary>
        /// Application Number
        /// </summary>
        private int _offerKey;

        private int _legalEntityKey;

        /// <summary>
        /// used in test 002 and 003 as the id number of a Legal Entity to create a relationship for
        /// </summary>
        private string _legalEntityIDNumber;

        #endregion globalVariables

        #region TestSetUpTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            //create an application
            base.Browser = Helper.CreateApplicationWithBrowser(TestUsers.BranchConsultant10, out _offerKey);
            _legalEntityIDNumber = Service<ILegalEntityService>().GetIDNumberForRelationship();
            _legalEntityKey = base.Service<IApplicationService>().GetFirstApplicantLegalEntityKeyOnOffer(_offerKey);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().CloseLoanNodesFLOBO(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Task();
            base.Browser.Page<X2Worklist>().SelectCaseFromWorklist(base.Browser, WorkflowStates.ApplicationCaptureWF.ApplicationCapture, _offerKey);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<LegalEntityNode>().LegalEntity(_offerKey);
        }

        #endregion TestSetUpTearDown

        #region Tests

        /// <summary>
        /// This test will add a new legal entity in a legal entity relationship to an applicant on our application.
        /// </summary>
        [Test, Description("This test will add a new legal entity in a legal entity relationship to an applicant on our application.")]
        public void _001_AddNewLegalEntityAsLERelationship()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityRelationships(NodeTypeEnum.Add);
            base.Browser.Page<LegalEntityRelationshipAdd>().AddNewLegalEntity();

            //we are now on the add legal entity screen
            string id = IDNumbers.GetNextIDNumber();
            base.Browser.Page<LegalEntityDetails>().AddLegalEntity(false, id, null, false, SalutationType.Mr,
                "T", "Test", "Surname", "Test", Gender.Male, MaritalStatus.Single, PopulationGroup.Coloured, Education.Diploma,
                CitizenType.SACitizen, "", "", Language.English, Language.English, "Alive", "031", "5605000", "", "", "", "", "", "", true,
                true, true, true, true, "", Service<ICommonService>().GetDateOfBirthFromIDNumber(id));
            //we should now be on the legal entity relationships screen
            base.View.AddRelationship(RelationshipType.Trustee);
            LegalEntityRelationshipAssertions.AssertRelationshipExists(id, RelationshipType.Trustee, _legalEntityKey);
        }

        /// <summary>
        /// This test will add an existing legal entity in a legal entity relationship to an applicant on our application.
        /// </summary>
        [Test, Description("This test will add an existing legal entity in a legal entity relationship to an applicant on our application.")]
        public void _002_AddExistingLegalEntityAsLERelationship()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityRelationships(NodeTypeEnum.Add);
            base.Browser.Page<LegalEntityRelationshipAdd>().AddExistingLegalEntity(_legalEntityIDNumber, RelationshipType.PowerofAttorney);
            //Assert the relationship exists
            LegalEntityRelationshipAssertions.AssertRelationshipExists(_legalEntityIDNumber, RelationshipType.PowerofAttorney, _legalEntityKey);
        }

        /// <summary>
        /// This test ensures the a duplicate legal entity relationship cannot be added. A user should be prevented from adding the same legal entity
        /// in the same type of relationship to an applicant.
        /// </summary>
        [Test, Description(@"This test ensures the a duplicate legal entity relationship cannot be added. A user should be prevented from adding the same legal entity
        in the same type of relationship to an applicant.")]
        public void _003_AddExistingRelationshipForLegalEntity()
        {
            Console.WriteLine(String.Format(@"--********{0}********", MethodBase.GetCurrentMethod()));
            base.Browser.Navigate<LegalEntityNode>().LegalEntityRelationships(NodeTypeEnum.Add);
            base.Browser.Page<LegalEntityRelationshipAdd>().AddExistingLegalEntity(_legalEntityIDNumber, RelationshipType.PowerofAttorney);
            //a warning message should be received
            base.Browser.Page<BasePageAssertions>().AssertValidationMessageExists(
                string.Format("Legal Entity already exists with a relationship of '{0}'.", RelationshipType.PowerofAttorney));
            //assert that the data has not been added
            LegalEntityRelationshipAssertions.AssertDuplicateRelationshipDoesNotExist(_legalEntityIDNumber, RelationshipType.PowerofAttorney, _legalEntityKey);
        }

        /// <summary>
        /// This test will take an existing legal entity relationship and update the relationship type.
        /// </summary>
        [Test, Description("This test will take an existing legal entity relationship and update the relationship type")]
        public void _004_UpdateLegalEntityRelationship()
        {
            base.Browser.Navigate<LegalEntityNode>().LegalEntityRelationships(NodeTypeEnum.Update);
            string existingRelationship;
            string idnumber;
            Service<ILegalEntityService>().GetExistingRelationship(_legalEntityKey, _legalEntityIDNumber, out existingRelationship, out idnumber);
            base.View.UpdateRelationship(existingRelationship,
                RelationshipType.ImmediateFamilyMember);
            //assert the relationship has been updated
            LegalEntityRelationshipAssertions.AssertRelationshipExists(idnumber, RelationshipType.ImmediateFamilyMember,
                _legalEntityKey);
        }

        /// <summary>
        /// This test will remove a legal entity relationship
        /// </summary>
        [Test, Description("This test will remove a legal entity relationship")]
        public void _005_DeleteLegalEntityRelationship()
        {
            Console.WriteLine(String.Format(@"--********{0}********", MethodBase.GetCurrentMethod()));
            base.Browser.Navigate<LegalEntityNode>().LegalEntityRelationships(NodeTypeEnum.Delete);
            string existingRelationship;
            string idnumber;
            Service<ILegalEntityService>().GetExistingRelationship(_legalEntityKey, _legalEntityIDNumber, out existingRelationship, out idnumber);
            base.View.DeleteRelationship(existingRelationship);
            //check the relationship doesnt exist
            LegalEntityRelationshipAssertions.AssertRelationshipDoesNotExist(existingRelationship, idnumber, _legalEntityKey);
        }

        #endregion Tests

        #region HelperMethods

        #endregion HelperMethods
    }
}
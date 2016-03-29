using BuildingBlocks;
using BuildingBlocks.Navigation.CBO.LoanServicing;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LoanServicingTests.Views.LoanAdjustments
{
    [RequiresSTA]
    public class AddRemoveSuretorTests : TestBase<RelatedLegalEntitySuretorRemove>
    {
        #region PrivateVariables

        private Automation.DataModels.Account Account;

        private int _legalEntityKey;

        #endregion PrivateVariables

        #region SetupTearDown

        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            base.OnTestStart();
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().Menu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.NavigationHelper>().LegalEntityMenu(base.Browser);
            base.Browser.Navigate<BuildingBlocks.Navigation.MenuNode>().CloseLoanNodesCBO();
            Account = base.Service<IAccountService>().GetVariableLoanAccountByMainApplicantCount(1, 1, AccountStatusEnum.Open);
            _legalEntityKey = base.Browser.Page<ClientSuperSearch>().SearchByAccountKeyForFirstMainApplicant(Account.AccountKey);
            base.Browser.Navigate<LoanServicingCBO>().LoanAccountNode(Account.AccountKey);
        }

        #endregion SetupTearDown

        /// <summary>
        /// This test will add an existing legal entity as a surety to an account. It searches for the legal entity using the advanced search functionality
        /// and then uses that legal entity to add to the account.
        /// </summary>
        [Test, Description(@"This test will add an existing legal entity as a surety to an account. It searches for the legal entity using the advanced search
            functionality and then uses that legal entity to add to the account.")]
        public void AddExistingLegalEntityAsSurety()
        {
            string idNumber = IDNumbers.GetNextIDNumber();
            var random = new Random();
            string emailAddress = string.Format(@"suretyTest-{0}@test.co.za", random.Next(0, 25000).ToString());
            base.Browser.Navigate<LoanServicingCBO>().AddSuretor();
            int suretyLegalEntityKey = Service<ILegalEntityService>().CreateNewLegalEntity(emailAddress, idNumber);
            //for some reason we need to wait, otherwise the client super search does not return our new legal entity.
            base.Browser.Page<ClientSuperSearch>().PerformAdvancedSearch(emailAddress, "Person", "None", idNumber);
            base.Browser.Page<LegalEntityDetailsSuretorAddExisting>().AddSuretor();
            var suretyList = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(Account.AccountKey, RoleTypeEnum.Suretor, GeneralStatusEnum.Active);
            Assert.That(suretyList.Contains(idNumber), string.Format(@"Legal Entity {0} was not added as a surety to account {1}",
                suretyLegalEntityKey, Account));
        }

        /// <summary>
        /// This test ensures that the user can select a surety to be removed from the account using the Remove Suretor functionality.
        /// </summary>
        [Test, Description(@"This test ensures that the user can select a surety to be removed from the account using the Remove Suretor functionality.")]
        public void RemoveSuretyFromAccount()
        {
            //insert role
            int legalEntityKey = base.Service<IAccountService>().AddRoleToAccount(Account.AccountKey, RoleTypeEnum.Suretor, GeneralStatusEnum.Active);
            var legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().RemoveSuretor();
            base.View.SelectLegalEntityByIDNumber(legalEntity.IdNumber);
            base.View.ClickRemove();
            var inactiveSureties = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(Account.AccountKey, RoleTypeEnum.Suretor, GeneralStatusEnum.Inactive);
            Assert.That(inactiveSureties.Contains(legalEntity.IdNumber),
                    string.Format(@"Legal Entity {0} was not made inactive as a surety on Account {1}", legalEntityKey, Account));
        }

        /// <summary>
        /// You should not be allowed to select a Main Applicant from the grid that is used to select which surety needs to be removed from the account.
        /// </summary>
        [Test, Description(@"You should not be allowed to select a Main Applicant from the grid that is used to select which surety needs to be removed
            from the account.")]
        public void CannotRemoveMainApplicantFromAccount()
        {
            base.Browser.Navigate<LoanServicingCBO>().RemoveSuretor();
            var legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: _legalEntityKey);
            base.View.SelectLegalEntityByIDNumber(legalEntity.IdNumber);
            //check the status of the button
            base.View.AssertRemoveButtonEnabled(false);
        }

        /// <summary>
        /// You cannot try and remove a surety that is already marked as inactive.
        /// </summary>
        [Test, Description("You cannot try and remove a surety that is already marked as inactive.")]
        public void CannotRemoveInactiveSurety()
        {
            //insert an INACTIVE suretor role
            int legalEntityKey = base.Service<IAccountService>().AddRoleToAccount(Account.AccountKey, RoleTypeEnum.Suretor, GeneralStatusEnum.Inactive);
            var legalEntity = Service<ILegalEntityService>().GetLegalEntity(legalentitykey: legalEntityKey);
            base.Browser.Navigate<LoanServicingCBO>().RemoveSuretor();
            base.View.SelectLegalEntityByIDNumber(legalEntity.IdNumber);
            //check the status of the button
            base.View.AssertRemoveButtonEnabled(false);
        }

        /// <summary>
        /// Adds a new legal entity as a surety to an account.
        /// </summary>
        [Test, Description("Adds a new legal entity as a surety to an account.")]
        public void AddNewLegalEntityAsSurety()
        {
            base.Browser.Navigate<LoanServicingCBO>().AddSuretor();
            base.Browser.Page<ClientSuperSearch>().AddNewLegalEntity();
            string idNumber = IDNumbers.GetNextIDNumber();
            //we need to add legal entity details
            base.Browser.Page<LegalEntityDetails>().AddLegalEntity(RoleType.Suretor, false, idNumber, "TestNewSurety", "CanBeAdded", "011", "1234567");
            var suretyList = base.Service<IAccountService>().GetIDNumbersForRoleOnAccount(Account.AccountKey, RoleTypeEnum.Suretor, GeneralStatusEnum.Active);
            Assert.That(suretyList.Contains(idNumber), string.Format(@"Legal Entity with ID Number: {0} was not added as a surety to account {1}",
                idNumber, Account));
        }

        /// <summary>
        /// When adding a new legal entity as a surety you should only be able to select a Natural Person and the Role Type should only contain Suretor
        /// </summary>
        [Test, Description(@"When adding a new legal entity as a surety you should only be able to select a Natural Person and the Role Type should
        only contain Suretor")]
        public void CanOnlyAddNaturalPersonSurety()
        {
            base.Browser.Navigate<LoanServicingCBO>().AddSuretor();
            base.Browser.Page<ClientSuperSearch>().AddNewLegalEntity();
            base.Browser.Page<LegalEntityDetails>().AssertRoleDropdownContents(new List<string> { RoleType.Suretor }, true);
            base.Browser.Page<LegalEntityDetails>().AssertLegalEntityTypeDropdownContents(new List<string> { LegalEntityType.NaturalPerson });
        }
    }
}
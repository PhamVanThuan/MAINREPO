using BuildingBlocks;
using BuildingBlocks.Presenters.CommonPresenters;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using WatiN.Core;
using Description = NUnit.Framework.DescriptionAttribute;

namespace LoanServicingTests.Views.ClientSearch
{
    [RequiresSTA]
    public sealed class AdvancedSearchTests : BuildingBlocks.TestBase<ClientSuperSearch>
    {
        protected override void OnTestFixtureSetup()
        {
            base.OnTestFixtureSetup();
            base.Browser = new TestBrowser(TestUsers.HaloUser);
        }

        protected override void OnTestStart()
        {
            Helper.NavigateToClientSuperSearchAdvanced(base.Browser);
            base.OnTestStart();
        }

        /// <summary>
        /// Tests that you can search for a natural person legal entity who does not play a role in any accounts or offers by using their ID Number as
        /// search criteria
        /// </summary>
        [Test, Description(@"Tests that you can search for a natural person legal entity who does not play a role in any accounts or offers by using their
        ID Number as  search criteria")]
        public void SearchForNaturalPersonUnrelatedToAccountByIdNumber()
        {
            var legalEntity = Service<ILegalEntityService>().GetUnrelatedLegalEntity(LegalEntityTypeEnum.NaturalPerson);
            string idNumber = legalEntity.Column("idnumber").GetValueAs<string>();
            base.View.PerformAdvancedSearch(idNumber, "Person", "None");
            Helper.AssertSearchResults(base.Browser, string.Empty, string.Empty, idNumber, string.Empty, string.Empty);
        }

        /// <summary>
        /// Tests that you can search for a natural person legal entity who does not play a role in any accounts or offers by using their first name and surname as
        /// search criteria
        /// </summary>
        [Test, Description(@"Tests that you can search for a natural person legal entity who does not play a role in any accounts or offers by using their
        ID Number as  search criteria")]
        public void SearchForNaturalPersonUnrelatedToAccountByFirstNameAndSurname()
        {
            var legalEntity = Service<ILegalEntityService>().GetUnrelatedLegalEntity(LegalEntityTypeEnum.NaturalPerson);
            string searchCriteria = string.Format("{0} {1}", legalEntity.Column("firstnames").GetValueAs<string>(),
                                        legalEntity.Column("surname").GetValueAs<string>());
            base.View.PerformAdvancedSearch(searchCriteria, "Person", "None");
            Helper.AssertSearchResults(base.Browser, string.Empty, string.Empty, searchCriteria, string.Empty, string.Empty);
        }

        /// <summary>
        /// Tests that you can search for a company that does not play a role in an account or an offer by using their registration number as search criteria
        /// </summary>
        [Test, Description(@"Tests that you can search for a company that does not play a role in an account or an offer by using their registration number as
            search criteria")]
        public void SearchForNonNaturalPersonUnrelatedToAccountByRegNumber(
            [Values(LegalEntityTypeEnum.Trust, LegalEntityTypeEnum.Company, LegalEntityTypeEnum.CloseCorporation)] LegalEntityTypeEnum legalEntityType)
        {
            var legalEntity = Service<ILegalEntityService>().GetUnrelatedLegalEntity(legalEntityType);
            string regNumber = legalEntity.Column("registrationNumber").GetValueAs<string>();
            base.View.PerformAdvancedSearch(regNumber, "Company", "None");
            Helper.AssertSearchResults(base.Browser, string.Empty, string.Empty, regNumber, string.Empty, string.Empty);
        }

        /// <summary>
        /// Tests that you can search for a natural person or a company legal entity who does not play a role in any accounts or offers by using their
        /// email address as search criteria
        /// </summary>
        [Test, Sequential, Description(@"Tests that you can search for a natural person or a company legal entity who does not play a role in any accounts or
        offers by using their email address as search criteria")]
        public void SearchForLegalEntityUnrelatedToAccountByEmailAddress(
            [Values(LegalEntityTypeEnum.NaturalPerson, LegalEntityTypeEnum.Company)] LegalEntityTypeEnum legalEntityType)
        {
            var legalEntity = Service<ILegalEntityService>().GetUnrelatedLegalEntity(legalEntityType);
            string criteria = legalEntity.Column("emailaddress").GetValueAs<string>();
            string type = legalEntityType == LegalEntityTypeEnum.NaturalPerson ? "Person" : "Company";
            base.View.PerformAdvancedSearch(criteria, type, "None");
            Helper.AssertSearchResults(base.Browser, string.Empty, string.Empty, criteria, string.Empty, string.Empty);
        }
    }
}
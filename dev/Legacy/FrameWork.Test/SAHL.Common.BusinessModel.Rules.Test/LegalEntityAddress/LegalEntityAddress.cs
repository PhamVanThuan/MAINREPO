using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.LegalEntityAddress;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;

namespace SAHL.Common.BusinessModel.Rules.Test.LegalEntityAddress
{
    [TestFixture]
    public class LegalEntityAddress : RuleBase
    {

        protected ILegalEntityAddress _legalEntityAddress = null;
        protected IAddressType addressType = null;
        protected IAddressFormat addressFormat = null;


        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _legalEntityAddress = _mockery.StrictMock<ILegalEntityAddress>();
        }

        [TearDown]
        public new void TearDown()
        {
            base.TearDown();
        }
        #region LegalEntityAddressResidentialConditionalAddressFormatsTest
        [NUnit.Framework.Test]
        public void LegalEntityAddressResidentialConditionalAddressFormatsTest()
        {
            LegalEntityAddressResidentialConditionalAddressFormats rule = new LegalEntityAddressResidentialConditionalAddressFormats();

            IAddressBox addressBox = _mockery.StrictMock<IAddressBox>();
            IAddressClusterBox addressCluster = _mockery.StrictMock<IAddressClusterBox>();
            IAddressFreeText addressFree = _mockery.StrictMock<IAddressFreeText>();
            IAddressPostnetSuite addressPostnet = _mockery.StrictMock<IAddressPostnetSuite>();
            IAddressPrivateBag addressPrivateBag = _mockery.StrictMock<IAddressPrivateBag>();
            IAddressStreet addressStreet = _mockery.StrictMock<IAddressStreet>();

            SetupLegalEntityAddress(AddressTypes.Residential);
            SetupResult.For(_legalEntityAddress.Address).Return(addressBox);
            ExecuteRule(rule, 1, _legalEntityAddress);

            SetupLegalEntityAddress(AddressTypes.Residential);
            SetupResult.For(_legalEntityAddress.Address).Return(addressCluster);
            ExecuteRule(rule, 1, _legalEntityAddress);

            SetupLegalEntityAddress(AddressTypes.Residential);
            SetupResult.For(_legalEntityAddress.Address).Return(addressFree);
            ExecuteRule(rule, 0, _legalEntityAddress);

            SetupLegalEntityAddress(AddressTypes.Residential);
            SetupResult.For(_legalEntityAddress.Address).Return(addressPostnet);
            ExecuteRule(rule, 1, _legalEntityAddress);

            SetupLegalEntityAddress(AddressTypes.Residential);
            SetupResult.For(_legalEntityAddress.Address).Return(addressPrivateBag);
            ExecuteRule(rule, 1, _legalEntityAddress);

            SetupLegalEntityAddress(AddressTypes.Residential);
            SetupResult.For(_legalEntityAddress.Address).Return(addressStreet);
            ExecuteRule(rule, 0, _legalEntityAddress);


        }
        #endregion LegalEntityAddressResidentialConditionalAddressFormatsTest

        #region LegalEntityAddressEffectiveDateMinimumTest
        [NUnit.Framework.Test]
        public void LegalEntityAddressEffectiveDateMinimumTest()
        {
            LegalEntityAddressEffectiveDateMinimum rule = new LegalEntityAddressEffectiveDateMinimum();

            // a minute before today for an old LE Address - should pass
            SetupResult.For(_legalEntityAddress.Key).Return(1);
            SetupResult.For(_legalEntityAddress.EffectiveDate).Return(DateTime.Today.AddMinutes(-1));
            ExecuteRule(rule, 0, _legalEntityAddress);

            // a minute before today for a new LE Address - should fail
            SetupResult.For(_legalEntityAddress.Key).Return(0);
            SetupResult.For(_legalEntityAddress.EffectiveDate).Return(DateTime.Today.AddMinutes(-1));
            ExecuteRule(rule, 1, _legalEntityAddress);

            // today for an old LE Address - should pass
            SetupResult.For(_legalEntityAddress.Key).Return(1);
            SetupResult.For(_legalEntityAddress.EffectiveDate).Return(DateTime.Today);
            ExecuteRule(rule, 0, _legalEntityAddress);

            // today for a new LE Address - should pass
            SetupResult.For(_legalEntityAddress.Key).Return(0);
            SetupResult.For(_legalEntityAddress.EffectiveDate).Return(DateTime.Today);
            ExecuteRule(rule, 0, _legalEntityAddress);

        }
        #endregion LegalEntityAddressEffectiveDateMinimumTest

        #region LegalEntityAddressEffectiveDateMaximumTest
        [NUnit.Framework.Test]
        public void LegalEntityAddressEffectiveDateMaximumTest()
        {
            LegalEntityAddressEffectiveDateMaximum rule = new LegalEntityAddressEffectiveDateMaximum();

            // today - should pass
            SetupResult.For(_legalEntityAddress.EffectiveDate).Return(DateTime.Today);
            ExecuteRule(rule, 0, _legalEntityAddress);

            // 6 months - should pass
            SetupResult.For(_legalEntityAddress.EffectiveDate).Return(DateTime.Today.AddMonths(6));
            ExecuteRule(rule, 0, _legalEntityAddress);

            // 6 months + 1 day - should fail
            SetupResult.For(_legalEntityAddress.EffectiveDate).Return(DateTime.Today.AddMonths(6).AddDays(1));
            ExecuteRule(rule, 1, _legalEntityAddress);

        }
        #endregion LegalEntityAddressEffectiveDateMaximumTest


        #region LegalEntityAddressPostalConditionalAddressFormatsTest
        [NUnit.Framework.Test]
        public void LegalEntityAddressPostalConditionalAddressFormatsTest()
        {
            LegalEntityAddressPostalConditionalAddressFormats rule = new LegalEntityAddressPostalConditionalAddressFormats();

            IAddressType addressType = _mockery.StrictMock<IAddressType>();
            IAddressBox addressBox = _mockery.StrictMock<IAddressBox>();
            IAddressClusterBox addressCluster = _mockery.StrictMock<IAddressClusterBox>();
            IAddressFreeText addressFree = _mockery.StrictMock<IAddressFreeText>();
            IAddressPostnetSuite addressPostnet = _mockery.StrictMock<IAddressPostnetSuite>();
            IAddressPrivateBag addressPrivateBag = _mockery.StrictMock<IAddressPrivateBag>();
            IAddressStreet addressStreet = _mockery.StrictMock<IAddressStreet>();


            // check residential address with free text - should pass as residential addresses are ignored
            SetupLegalEntityAddress(AddressTypes.Residential);
            SetupResult.For(_legalEntityAddress.Address).Return(addressFree);
            ExecuteRule(rule, 0, _legalEntityAddress);

            // check with free text that is postal - should fail
            SetupLegalEntityAddress(AddressTypes.Postal);
            SetupResult.For(_legalEntityAddress.Address).Return(addressFree);
            ExecuteRule(rule, 1, _legalEntityAddress);

            // address box - should pass
            SetupLegalEntityAddress(AddressTypes.Postal);
            SetupResult.For(_legalEntityAddress.Address).Return(addressBox);
            ExecuteRule(rule, 0, _legalEntityAddress);

            // address box - should pass
            SetupLegalEntityAddress(AddressTypes.Postal);
            SetupResult.For(_legalEntityAddress.Address).Return(addressCluster);
            ExecuteRule(rule, 0, _legalEntityAddress);

            // address box - should pass
            SetupResult.For(_legalEntityAddress.AddressType).Return(addressType);
            SetupResult.For(addressType.Key).Return((int)AddressTypes.Postal);
            SetupResult.For(_legalEntityAddress.Address).Return(addressPostnet);
            ExecuteRule(rule, 0, _legalEntityAddress);

            // address box - should pass
            SetupLegalEntityAddress(AddressTypes.Postal);
            SetupResult.For(_legalEntityAddress.Address).Return(addressPrivateBag);
            ExecuteRule(rule, 0, _legalEntityAddress);

            // address box - should pass
            SetupLegalEntityAddress(AddressTypes.Postal);
            SetupResult.For(_legalEntityAddress.Address).Return(addressStreet);
            ExecuteRule(rule, 0, _legalEntityAddress);

        }
        #endregion LegalEntityAddressPostalConditionalAddressFormatsTest

        #region LegalEntityAddressAccountMailingAddressCheckTest
        /// <summary>
        /// Ensures that a legal entity address cannot be deactivated if there 
        /// are any open accounts attached.
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityAddressAccountMailingAddressCheckTest()
        {
            // active general status will result in a pass always
            LegalEntityAddressAccountMailingAddressCheckHelper(GeneralStatuses.Active, AccountStatuses.Open, true, 0);
            LegalEntityAddressAccountMailingAddressCheckHelper(GeneralStatuses.Active, AccountStatuses.Open, false, 0);

            // account status not open will result in pass always
            foreach (AccountStatuses accs in Enum.GetValues(typeof(AccountStatuses)))
            {
                if (accs != AccountStatuses.Open)
                {
                    LegalEntityAddressAccountMailingAddressCheckHelper(GeneralStatuses.Inactive, accs, true, 0);
                    LegalEntityAddressAccountMailingAddressCheckHelper(GeneralStatuses.Inactive, accs, false, 0);
                }
            }

            // should pass if the address is the mailing address on one of the LE's accounts, otherwise it will fail
            LegalEntityAddressAccountMailingAddressCheckHelper(GeneralStatuses.Inactive, AccountStatuses.Open, false, 0);
            LegalEntityAddressAccountMailingAddressCheckHelper(GeneralStatuses.Inactive, AccountStatuses.Open, true, 1);
        }


        /// <summary>
        /// Helper method to set up the expectations for the LegalEntityAddressAccountMailingAddressCheck test.
        /// </summary>
        /// <param name="gs"></param>
        private void LegalEntityAddressAccountMailingAddressCheckHelper(GeneralStatuses gs, AccountStatuses accStatus, bool isMailingAddress, int expectedMessageCount)
        {
            LegalEntityAddressAccountMailingAddressCheck rule = new LegalEntityAddressAccountMailingAddressCheck();

            IAddressStreet address = _mockery.StrictMock<IAddressStreet>();
            IAddressStreet address2 = _mockery.StrictMock<IAddressStreet>();
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            IEventList<IMailingAddress> mailingAddresses = new EventList<IMailingAddress>();
            IMailingAddress mailingAddress = _mockery.StrictMock<IMailingAddress>();


            SetupResult.For(_legalEntityAddress.Address).Return(address);
            SetupResult.For(address.Key).Return(1);
            SetupResult.For(address2.Key).Return(2);
            SetupResult.For(_legalEntityAddress.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)gs);
            SetupResult.For(_legalEntityAddress.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.Roles).Return(roles);
            roles.Add(new DomainMessageCollection(), role);
            SetupResult.For(role.Account).Return(account);
            SetupResult.For(account.Key).Return(1);
            SetupResult.For(account.AccountStatus).Return(accountStatus);
            SetupResult.For(accountStatus.Key).Return((int)accStatus);
            SetupResult.For(account.MailingAddresses).Return(mailingAddresses);
            mailingAddresses.Add(new DomainMessageCollection(), mailingAddress);
            SetupResult.For(mailingAddress.Address).Return(isMailingAddress ? address : address2);

            ExecuteRule(rule, expectedMessageCount, _legalEntityAddress);

        }
        #endregion LegalEntityAddressAccountMailingAddressCheckTest

        #region LegalEntityAddressApplicationMailingAddressCheckTest
        /// <summary>
        /// Ensures that a legal entity address cannot be deactivated if there 
        /// are any open applications attached.
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityAddressApplicationMailingAddressCheckTest()
        {
            // active general status will result in a pass always
            LegalEntityAddressApplicationMailingAddressCheckHelper(GeneralStatuses.Active, OfferStatuses.Open, true, 0);
            LegalEntityAddressApplicationMailingAddressCheckHelper(GeneralStatuses.Active, OfferStatuses.Open, false, 0);

            // account status not open will result in pass always
            foreach (OfferStatuses offs in Enum.GetValues(typeof(OfferStatuses)))
            {
                if (offs != OfferStatuses.Open)
                {
                    LegalEntityAddressApplicationMailingAddressCheckHelper(GeneralStatuses.Inactive, offs, true, 0);
                    LegalEntityAddressApplicationMailingAddressCheckHelper(GeneralStatuses.Inactive, offs, false, 0);
                }
            }

            // should pass if the address is the mailing address on one of the LE's accounts, otherwise it will fail
            LegalEntityAddressApplicationMailingAddressCheckHelper(GeneralStatuses.Inactive, OfferStatuses.Open, false, 0);
            LegalEntityAddressApplicationMailingAddressCheckHelper(GeneralStatuses.Inactive, OfferStatuses.Open, true, 1);
        }

        /// <summary>
        /// Helper method to set up the expectations for the LegalEntityAddressApplicationMailingAddressCheckHelper test.
        /// </summary>
        /// <param name="gs"></param>
        private void LegalEntityAddressApplicationMailingAddressCheckHelper(GeneralStatuses gs, OfferStatuses offStatus, bool isMailingAddress, int expectedMessageCount)
        {
            LegalEntityAddressApplicationMailingAddressCheck rule = new LegalEntityAddressApplicationMailingAddressCheck();

            IAddressStreet address = _mockery.StrictMock<IAddressStreet>();
            IAddressStreet address2 = _mockery.StrictMock<IAddressStreet>();
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            IEventList<IApplicationRole> roles = new EventList<IApplicationRole>();
            IApplicationRole role = _mockery.StrictMock<IApplicationRole>();
            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationStatus appStatus = _mockery.StrictMock<IApplicationStatus>();
            IEventList<IApplicationMailingAddress> mailingAddresses = new EventList<IApplicationMailingAddress>();
            IApplicationMailingAddress mailingAddress = _mockery.StrictMock<IApplicationMailingAddress>();

            roles.Add(new DomainMessageCollection(), role);

            SetupResult.For(_legalEntityAddress.Address).Return(address);
            SetupResult.For(address.Key).Return(1);
            SetupResult.For(address2.Key).Return(2);
            SetupResult.For(_legalEntityAddress.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)gs);
            SetupResult.For(_legalEntityAddress.LegalEntity).Return(legalEntity);
            SetupResult.For(legalEntity.GetApplicationRolesByRoleTypeGroups(OfferRoleTypeGroups.Client)).Return(new ReadOnlyEventList<IApplicationRole>(roles));
            SetupResult.For(role.Application).Return(app);
            SetupResult.For(app.Key).Return(1);
            SetupResult.For(app.ApplicationStatus).Return(appStatus);
            SetupResult.For(appStatus.Key).Return((int)offStatus);
            SetupResult.For(app.ApplicationMailingAddresses).Return(mailingAddresses);
            mailingAddresses.Add(new DomainMessageCollection(), mailingAddress);
            SetupResult.For(mailingAddress.Address).Return(isMailingAddress ? address : address2);

            ExecuteRule(rule, expectedMessageCount, _legalEntityAddress);

        }
        #endregion LegalEntityAddressApplicationMailingAddressCheckTest

        #region LegalEntityAddressDoNotDeleteOnRoleTest
        /// <summary>
        /// Rule that checks to ensure the LegalEntityAddress is not deleted when the Role is Suretor or Main Applicant
        /// and there is only one active address
        /// </summary>
        [NUnit.Framework.Test]
        public void LegalEntityAddressDoNotDeleteOnRoleTest()
        {
            // active general status will result in a pass always
            LegalEntityAddressDoNotDeleteOnRoleHelper(GeneralStatuses.Active, AccountStatuses.Open, OfferStatuses.Open, 0, (int)OfferRoleTypes.MainApplicant, (int)RoleTypes.MainApplicant, 2, (int)GeneralStatuses.Active, 1, 1);

            // general status inactive, noAddress > 1 so can quit and address can be delete - fail
            LegalEntityAddressDoNotDeleteOnRoleHelper(GeneralStatuses.Inactive, AccountStatuses.Open, OfferStatuses.Open, 1, (int)OfferRoleTypes.MainApplicant, (int)RoleTypes.MainApplicant, 2, (int)GeneralStatuses.Active, 1, 1);

            // if account status is closed and offer status is closed then fail - shouldn't matter if status is closed/declined etc.
            LegalEntityAddressDoNotDeleteOnRoleHelper(GeneralStatuses.Inactive, AccountStatuses.Closed, OfferStatuses.Closed, 1, (int)OfferRoleTypes.MainApplicant, (int)RoleTypes.MainApplicant, 1, (int)GeneralStatuses.Active, 1, 1);

            // role type is suretor or mainapplicant it will fail
            LegalEntityAddressDoNotDeleteOnRoleHelper(GeneralStatuses.Inactive, AccountStatuses.Open, OfferStatuses.Open, 1, (int)OfferRoleTypes.MainApplicant, (int)RoleTypes.Suretor, 1, (int)GeneralStatuses.Active, 1, 1);
            LegalEntityAddressDoNotDeleteOnRoleHelper(GeneralStatuses.Inactive, AccountStatuses.Open, OfferStatuses.Open, 1, (int)OfferRoleTypes.Suretor, (int)RoleTypes.MainApplicant, 1, (int)GeneralStatuses.Active, 1, 1);

        }

        /// <summary>
        /// Helper method to set up the expectations for the LegalEntityAddressDoNotDeleteOnRoleHelper test.
        /// </summary>
        /// <param name="gs"></param>
        private void LegalEntityAddressDoNotDeleteOnRoleHelper(GeneralStatuses gs, AccountStatuses accStatus, OfferStatuses offStatus, int expectedMessageCount, int appRoleTypes, int accRoleTypes, int noAddresses, int genStatKey, int legEntKey, int leaKey)
        {
            LegalEntityAddressDoNotDeleteOnRole rule = new LegalEntityAddressDoNotDeleteOnRole();
                
            IAddressStreet address1 = _mockery.StrictMock<IAddressStreet>();
            IAddressStreet address2 = _mockery.StrictMock<IAddressStreet>();
            
            IGeneralStatus generalStatus = _mockery.StrictMock<IGeneralStatus>();
            ILegalEntity legalEntity = _mockery.StrictMock<ILegalEntity>();
            
            IEventList<IApplicationRole> appRoles = new EventList<IApplicationRole>();
            IApplicationRole appRole = _mockery.StrictMock<IApplicationRole>();
            IApplicationRoleType appRoleType = _mockery.StrictMock<IApplicationRoleType>();
            IApplication app = _mockery.StrictMock<IApplication>();
            IApplicationStatus appStatus = _mockery.StrictMock<IApplicationStatus>();

            IEventList<IRole> roles = new EventList<IRole>();
            IRole role = _mockery.StrictMock<IRole>();
            IRoleType roleType = _mockery.StrictMock<IRoleType>();
            IAccount account = _mockery.StrictMock<IAccount>();
            IAccountStatus accountStatus = _mockery.StrictMock<IAccountStatus>();
            
            IEventList<ILegalEntityAddress> leas = new EventList<ILegalEntityAddress>();
            ILegalEntityAddress lea = _mockery.StrictMock<ILegalEntityAddress>();
            IGeneralStatus gen = _mockery.StrictMock<IGeneralStatus>();

            // need to setup to take in a param for no of address and spit out the right number.

            SetupResult.For(_legalEntityAddress.Address).Return(address1);
            SetupResult.For(_legalEntityAddress.Key).Return(legEntKey);
            SetupResult.For(lea.Key).Return(leaKey);
            SetupResult.For(address1.Key).Return(1);
            SetupResult.For(address2.Key).Return(2);
            SetupResult.For(gen.Key).Return(genStatKey);
            SetupResult.For(lea.GeneralStatus).Return(gen);
            SetupResult.For(lea.Address).Return(address1);
            leas.Add(new DomainMessageCollection(), lea);
            if (noAddresses > 1)
            {
                leas.Add(new DomainMessageCollection(), _legalEntityAddress);
            }
            SetupResult.For(legalEntity.LegalEntityAddresses).Return(leas);

            SetupResult.For(_legalEntityAddress.GeneralStatus).Return(generalStatus);
            SetupResult.For(generalStatus.Key).Return((int)gs);
            SetupResult.For(_legalEntityAddress.LegalEntity).Return(legalEntity);

            SetupResult.For(legalEntity.ApplicationRoles).Return(appRoles);
            SetupResult.For(appRoleType.Key).Return(appRoleTypes);
            SetupResult.For(appRole.ApplicationRoleType).Return(appRoleType);
            appRoles.Add(new DomainMessageCollection(), appRole);
            SetupResult.For(appRole.Application).Return(app);
            SetupResult.For(appStatus.Key).Return((int)offStatus);
            SetupResult.For(app.ApplicationStatus).Return(appStatus);

            SetupResult.For(legalEntity.Roles).Return(roles);
            SetupResult.For(roleType.Key).Return(accRoleTypes);
            SetupResult.For(role.RoleType).Return(roleType);
            roles.Add(new DomainMessageCollection(), role);
            SetupResult.For(role.Account).Return(account);
            SetupResult.For(accountStatus.Key).Return((int)accStatus);
            SetupResult.For(account.AccountStatus).Return(accountStatus);

            SetupResult.For(app.Key).Return(1);

            ExecuteRule(rule, expectedMessageCount, _legalEntityAddress);
            
        }



        #endregion


        #region Helpers
        /// <summary>
        /// Helper method to set up the expectations for a residential address
        /// </summary>
        private void SetupLegalEntityAddress(AddressTypes addressType)
        {
            IAddressType addrType = _mockery.StrictMock<IAddressType>();
            SetupResult.For(_legalEntityAddress.AddressType).Return(addrType);
            SetupResult.For(addrType.Key).Return((int)addressType);
        }
        #endregion

    }
}

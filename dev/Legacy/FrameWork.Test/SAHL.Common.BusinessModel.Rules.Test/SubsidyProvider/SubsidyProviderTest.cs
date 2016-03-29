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
using SAHL.Common.BusinessModel;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules.SubsidyProvider;

namespace SAHL.Common.BusinessModel.Rules.Test.SubsidyProvider
{
    [TestFixture]
    public class SubsidyProviderTest : RuleBase
    {
        private ISubsidyProvider _subsidyProvider = null;
        private ILegalEntity _legalEntity = null;
        private ILegalEntityType _legalEntityType = null;

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            _subsidyProvider = _mockery.StrictMock<ISubsidyProvider>();
            _legalEntity = _mockery.StrictMock<ILegalEntity>();
            SetupResult.For(_subsidyProvider.LegalEntity).IgnoreArguments().Return(_legalEntity);
            _legalEntityType = _mockery.StrictMock<ILegalEntityType>();
            SetupResult.For(_legalEntity.LegalEntityType).Return(_legalEntityType);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderAddUniqueTestFail()
        {
            SubsidyProviderAddUnique rule = new SubsidyProviderAddUnique();

            SetupResult.For(_legalEntity.DisplayName).IgnoreArguments().Return("Department of State Expenditure");
            SetupResult.For(_subsidyProvider.Key).Return(0);
            ExecuteRule(rule, 1, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderAddUniqueTestPass()
        {
            SubsidyProviderAddUnique rule = new SubsidyProviderAddUnique();

            SetupResult.For(_legalEntity.DisplayName).IgnoreArguments().Return("Non Existant Provider");
            SetupResult.For(_subsidyProvider.Key).Return(0);
            ExecuteRule(rule, 0, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityTypeCompanyTestPass()
        {
            SubsidyProviderLegalEntityType rule = new SubsidyProviderLegalEntityType();

            SetupResult.For(_legalEntityType.Key).Return((int)LegalEntityTypes.Company);

            ExecuteRule(rule, 0, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityTypeCCTestPass()
        {
            SubsidyProviderLegalEntityType rule = new SubsidyProviderLegalEntityType();

            SetupResult.For(_legalEntityType.Key).Return((int)LegalEntityTypes.CloseCorporation);

            ExecuteRule(rule, 0, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityTypeTrusTestPass()
        {
            SubsidyProviderLegalEntityType rule = new SubsidyProviderLegalEntityType();

            SetupResult.For(_legalEntityType.Key).Return((int)LegalEntityTypes.Trust);

            ExecuteRule(rule, 0, _subsidyProvider);
        }


        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityTypeNaturalPersonTestFail()
        {
            SubsidyProviderLegalEntityType rule = new SubsidyProviderLegalEntityType();

            SetupResult.For(_legalEntityType.Key).Return((int)LegalEntityTypes.NaturalPerson);

            ExecuteRule(rule, 1, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityTypeUnknownTestFail()
        {
            SubsidyProviderLegalEntityType rule = new SubsidyProviderLegalEntityType();

            SetupResult.For(_legalEntityType.Key).Return((int)LegalEntityTypes.Unknown);

            ExecuteRule(rule, 1, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityEmailAddressTestFail()
        {
            SubsidyProviderLegalEntityEmailAddress rule = new SubsidyProviderLegalEntityEmailAddress();

            SetupResult.For(_legalEntity.EmailAddress).Return("Incorrect Address");

            ExecuteRule(rule, 1, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityEmailAddressTestPass()
        {
            SubsidyProviderLegalEntityEmailAddress rule = new SubsidyProviderLegalEntityEmailAddress();

            SetupResult.For(_legalEntity.EmailAddress).Return("shomas@sahomeloans.com");

            ExecuteRule(rule, 0, _subsidyProvider);
        }

        [NUnit.Framework.Test]
        public void SubsidyProviderLegalEntityBlankEmailAddressTestPass()
        {
            SubsidyProviderLegalEntityEmailAddress rule = new SubsidyProviderLegalEntityEmailAddress();

            SetupResult.For(_legalEntity.EmailAddress).Return("");

            ExecuteRule(rule, 0, _subsidyProvider);
        }

        /// <summary>
        /// Tests the SubsidyProviderAddressMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void SubsidyProviderAddressMandatoryTest()
        {
            SubsidyProviderAddressMandatory rule = new SubsidyProviderAddressMandatory();

            IEventList<ILegalEntityAddress> leAddresses = new EventList<ILegalEntityAddress>();
            ILegalEntityAddress leAddress = _mockery.StrictMock<ILegalEntityAddress>();

            // submit with no address - should fail
            SetupResult.For(_legalEntity.LegalEntityAddresses).Return(leAddresses);
            ExecuteRule(rule, 1, _subsidyProvider);

            // submit with 1 address - should fail
            leAddresses.Add(null, leAddress);
            SetupResult.For(_subsidyProvider.LegalEntity).Return(_legalEntity);
            SetupResult.For(_legalEntity.LegalEntityAddresses).Return(leAddresses);
            ExecuteRule(rule, 0, _subsidyProvider);

            // submit with no legal entity - should pass
            SetupResult.For(_subsidyProvider.LegalEntity).Return(null);
            ExecuteRule(rule, 0, _subsidyProvider);

        }

    }
}

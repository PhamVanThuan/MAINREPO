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
using SAHL.Common.BusinessModel.Rules.Address;

namespace SAHL.Common.BusinessModel.Rules.Test.Address
{
    [TestFixture]
    public class Freetext : AddressBase
    {
        private IAddressFreeText _address;

        [NUnit.Framework.SetUp]
        public new void Setup()
        {
            base.Setup();
            _address = _mockery.StrictMock<IAddressFreeText>();
        }

        [TearDown]
        public override void TearDown()
        {
        }

        [NUnit.Framework.Test]
        public void AddressFreetextConditionalInternationalAddressTest()
        {
            AddressFreetextConditionalInternationalAddress rule = new AddressFreetextConditionalInternationalAddress();

            SetupResult.For(_address.RRR_CountryDescription).Return("South Africa");
            ExecuteRule(rule, 1, _address);

            SetupResult.For(_address.RRR_CountryDescription).Return("NOT South Africa");
            ExecuteRule(rule, 0, _address);

        }

        [NUnit.Framework.Test]
        public void AddressFreetextOneLineMandatoryTest()
        {
            AddressFreetextOneLineMandatory rule = new AddressFreetextOneLineMandatory();

            // this should fail, as none of the lines are set
            SetupResult.For(_address.FreeText1).Return(null);
            SetupResult.For(_address.FreeText2).Return(null);
            SetupResult.For(_address.FreeText3).Return(null);
            SetupResult.For(_address.FreeText4).Return(null);
            SetupResult.For(_address.FreeText5).Return(null);
            ExecuteRule(rule, 1, _address);

            // the following should pass
            SetupResult.For(_address.FreeText1).Return("Test");
            SetupResult.For(_address.FreeText2).Return(null);
            SetupResult.For(_address.FreeText3).Return(null);
            SetupResult.For(_address.FreeText4).Return(null);
            SetupResult.For(_address.FreeText5).Return(null);
            ExecuteRule(rule, 0, _address);

        }

        [NUnit.Framework.Test]
        public void AddressFreeTextCountryMandatoryTest()
        {
            AddressFreeTextCountryMandatory rule = new AddressFreeTextCountryMandatory();

            SetupResult.For(_address.PostOffice).Return(null);
            ExecuteRule(rule, 1, _address);

            IPostOffice postOffice = _mockery.StrictMock<IPostOffice>();
            SetupResult.For(_address.PostOffice).Return(postOffice);
            ExecuteRule(rule, 0, _address);

        }

    }
}

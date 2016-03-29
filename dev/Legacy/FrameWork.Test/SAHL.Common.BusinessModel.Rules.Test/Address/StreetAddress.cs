using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.Address;
using Rhino.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace SAHL.Common.BusinessModel.Rules.Test.Address
{
    [TestFixture]
    public class StreetAddress : AddressBase
    {
        protected IAddressStreet _address = null;

        [NUnit.Framework.SetUp]
        public override void Setup()
        {
            base.Setup();
            _address = _mockery.StrictMock<IAddressStreet>();
        }

        [TearDown]
        public override void TearDown()
        {
        }


        [NUnit.Framework.Test]
        public void AddressStreetConditionalUnitBuildingNoOrNameRequiresStreetTest()
        {
            AddressStreetConditionalUnitBuildingNoOrNameRequiresStreet rule = new AddressStreetConditionalUnitBuildingNoOrNameRequiresStreet();

            SetupResult.For(_address.StreetName).Return("StreetName");
            SetupResult.For(_address.StreetNumber).Return("10100011");
            SetupResult.For(_address.UnitNumber).Return("");
            SetupResult.For(_address.BuildingNumber).Return("1");
            SetupResult.For(_address.BuildingName).Return("");
            ExecuteRule(rule, 0, _address);

            SetupResult.For(_address.StreetName).Return("");
            SetupResult.For(_address.StreetNumber).Return("");
            SetupResult.For(_address.UnitNumber).Return("");
            SetupResult.For(_address.BuildingNumber).Return("");
            SetupResult.For(_address.BuildingName).Return("");
            ExecuteRule(rule, 1, _address);

        }


        [NUnit.Framework.Test]
        public void AddressStreetMandatoryStreetOrBuildingOrUnitTest()
        {
            AddressStreetMandatoryStreetOrBuildingOrUnit rule = new AddressStreetMandatoryStreetOrBuildingOrUnit();

            SetupResult.For(_address.StreetName).Return("");
            SetupResult.For(_address.StreetNumber).Return("");
            SetupResult.For(_address.UnitNumber).Return("");
            SetupResult.For(_address.BuildingNumber).Return("");
            SetupResult.For(_address.BuildingName).Return("");
            ExecuteRule(rule, 1, _address);

        }

        [NUnit.Framework.Test]
        public void AddressStreetMandatoryStreetNameTest()
        {
            AddressStreetMandatoryStreetName rule = new AddressStreetMandatoryStreetName();

            // Pass as there is some text
            SetupResult.For(_address.StreetName).Return(" space then some stuff then more spaces   ");
            ExecuteRule(rule, 0, _address);

            // FAIL as streetname is empty
            SetupResult.For(_address.StreetName).Return("");
            ExecuteRule(rule, 1, _address);

            // FAIL as streetname is only spaces
            SetupResult.For(_address.StreetName).Return("      ");
            ExecuteRule(rule, 1, _address);


        }
    }
}

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

namespace SAHL.Common.BusinessModel.Rules.Test.Address
{
    public class AddressBase : RuleBase
    {
        protected IAddressRepository addyRepo = null;

        public override void Setup()
        {
            base.Setup();
            addyRepo = _mockery.StrictMock<IAddressRepository>();
            MockCache.Add(typeof(IAddressRepository).ToString(), addyRepo);
        }

        public override void TearDown()
        {
            base.TearDown();
        }
    }
}

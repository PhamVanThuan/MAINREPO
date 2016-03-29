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
using SAHL.BusinessModel.Interfaces.Repositories;

namespace SAHL.Rules.Test.Address
{
    //[TestFixture]
    //public class ClusterBoxAddress : AddressBase
    //{
    //    protected ISuburb suburb = null;
    //    protected IAddressClusterBox addy = null;
    //    protected IPostOffice postoffice = null;

    //    [NUnit.Framework.SetUp]
    //    public void Setup()
    //    {
    //        base.Setup();
    //        addy = _mockery.CreateMock<IAddressClusterBox>();
    //    }

    //    [TearDown]
    //    public void Teardown()
    //    {
    //    }

    //    protected void SetupPostOffice()
    //    {
    //        postoffice = _mockery.CreateMock<IPostOffice>();
    //        SetupResult.For(addy.PostOffice).Return(postoffice);
    //    }

    //}
}

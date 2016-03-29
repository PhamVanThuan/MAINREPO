using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Test;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using System.Configuration;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class SubsidyProviderTest : TestBase
    {
        [Test]
        public void GetSubsidyProviderAndParentByKey()
        {
            using (new SessionScope())
            {
                SubsidyProvider_DAO sp = SubsidyProvider_DAO.FindFirst();
                DomainMessageCollection messages = new DomainMessageCollection();
                IReadOnlyEventList<ISubsidyProvider> list = SubsidyProvider.GetSubsidyProviderAndParentByKey(messages, sp.Key);
                Assert.IsTrue(list.Count > 0);
            }
        }
    }
}

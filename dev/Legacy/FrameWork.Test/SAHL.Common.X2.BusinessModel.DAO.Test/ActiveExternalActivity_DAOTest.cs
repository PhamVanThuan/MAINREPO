using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.Security;

using Castle.ActiveRecord;
using NUnit.Framework;
using System.Security.Principal;
using SAHL.Common.X2.BusinessModel.DAO;

namespace SAHL.Common.X2.BusinessModel.Test
{
    [TestFixture]
    public class ActiveExternalActivity_DAOTest : TestBase
    {
        [Test]
        public void Find()
        {
            base.TestFind<ActiveExternalActivity_DAO>("X2.X2.ActiveExternalActivity", "ID");
        }
    }
}

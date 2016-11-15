using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Castle.ActiveRecord;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    [TestFixture]
    public class Operator_DAOTest : TestBase
    {
        [Test]
        public void Find()
        {
            using (new SessionScope())
            {
                SAHL.Common.BusinessModel.DAO.Operator_DAO Operator = SAHL.Common.BusinessModel.DAO.Operator_DAO.FindFirst();
            }
        }
    }
}
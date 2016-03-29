using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;


namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="AuditBond_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class AuditBond_DAOTest : TestBase
    {

        /// <summary>
        /// Tests the retrieval of a <see cref="AuditBond_DAO"/> object.
        /// </summary>
        [Test]
        public void Find()
        {

            AuditBond_DAO D = base.TestFind<AuditBond_DAO>("AuditBond", "AuditNumber");

        }


    }
}
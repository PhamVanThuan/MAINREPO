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
    /// Class for testing the <see cref="AuditBondMortgageLoan_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class AuditBondMortgageLoan_DAOTest : TestBase
    {

        /// <summary>
        /// Tests the retrieval of a <see cref="AuditBondMortgageLoan_DAO"/> object.
        /// </summary>
        [Test]
        public void Find()
        {

            AuditBondMortgageLoan_DAO D = base.TestFind<AuditBondMortgageLoan_DAO>("AuditBondMortgageLoan", "AuditNumber");

        }


    }
}
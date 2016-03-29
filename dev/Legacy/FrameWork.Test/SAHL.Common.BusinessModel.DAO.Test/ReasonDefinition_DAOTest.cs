using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="ReasonDefinition_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class ReasonDefinition_DAOTest : TestBase
    {
        /// <summary>
        /// Verifies that the OSP list is populated and usable.
        /// </summary>
        [Test]
        public void CheckOSP()
        {
            using (new SessionScope())
            {
                ReasonDefinition_DAO D = base.TestFind<ReasonDefinition_DAO>("ReasonDefinition", "ReasonDefinitionKey");
                int cnt = D.OriginationSourceProducts.Count;

                if (cnt > 0)
                {
                    OriginationSourceProduct_DAO OSP = D.OriginationSourceProducts[0];
                    Assert.IsNotNull(OSP);
                }
            }
        }
    }
}

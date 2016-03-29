using System;
using System.Collections.Generic;
using System.Data;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="Property_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class Property_DAOTest : TestBase
    {
        /// <summary>
        /// Tests the retrieval of valuations off a property.
        /// </summary>
        [Test]
        public void Valuations()
        {
            using (new SessionScope())
            {
                string query = "select top 1 PropertyKey from [2AM].[dbo].[Valuation] (nolock)";
                DataTable DT = base.GetQueryResults(query);
                Assert.That(DT.Rows.Count == 1);

                int PropertyKey = Convert.ToInt32(DT.Rows[0][0]);

                Property_DAO prop = Property_DAO.Find(PropertyKey);
                IList<Valuation_DAO> list = prop.Valuations;

                Assert.That(list.Count > 0);
            }
        }

        /// <summary>
        /// Tests the retrieval of valuations off a property.
        /// </summary>
        [Test]
        public void Valuations2()
        {
            using (new SessionScope())
            {
                string query1 = "select top 1 PropertyKey from [2AM].[dbo].[Valuation] (nolock)";
                DataTable DT1 = base.GetQueryResults(query1);

                int PropertyKey = Convert.ToInt32(DT1.Rows[0][0]);


                string query = "select * from [2AM].[dbo].[Valuation] (nolock) where PropertyKey = " + PropertyKey;
                DataTable DT = base.GetQueryResults(query);

                Property_DAO prop = Property_DAO.Find(PropertyKey);
                IList<Valuation_DAO> list = prop.Valuations;

                Assert.That(list.Count == DT.Rows.Count);
            }
        }
    }
}

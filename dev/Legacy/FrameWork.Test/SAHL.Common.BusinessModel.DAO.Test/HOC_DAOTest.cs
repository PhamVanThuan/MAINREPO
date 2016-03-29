using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.BusinessModel.DAO;

using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Test.DAOHelpers;

namespace SAHL.Common.BusinessModel.DAO.Test
{
    /// <summary>
    /// Class for testing the <see cref="HOC_DAO"/> entity.
    /// </summary>
    [TestFixture]
    public class HOC_DAOTest : TestBase
    {
        [Ignore("You should not be accessing properties this way.")]
        [Test]
        public void HOCProperties()
        {
            //using (new SessionScope())
            //{
            //    HOC_DAO HD = base.TestFind<HOC_DAO>("HOC", "FinancialServiceKey");

            //    int cnt = HD.Properties.Count;
            //    foreach (Property_DAO Prop in HD.Properties)
            //    {
            //        string desc = Prop.PropertyDescription1;
            //    }
            //}
        }
	}
}


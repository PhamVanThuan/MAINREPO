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
using Castle.ActiveRecord.Queries;
using SAHL.Common.X2.BusinessModel.DAO;

namespace SAHL.Common.X2.BusinessModel.Test
{
    [TestFixture]
    public class WorkList_DAOTest : TestBase
    {
        [Test]
        public void Find()
        {
            base.TestFind<WorkList_DAO>("X2.X2.WorkList", "ID");
        }

        [Test]
        public void FindByADUserName()
        {
            
        }

        [Test]
        public void Query()
        {
            string query = "SELECT DISTINCT wl FROM SAHL.Common.X2.BusinessModel.DAO.WorkList_DAO wl WHERE wl.Instance.State.ID = 45183 AND wl.ADUserName IN ('MADUMBI\\andrewr','Everyone','Madumbi\\andrewr')";

            SimpleQuery q = new SimpleQuery(typeof(WorkList_DAO), query);
            WorkList_DAO[] result = WorkList_DAO.ExecuteQuery(q) as WorkList_DAO[];

            if (result == null)
                result = new WorkList_DAO[0];

       }
    }
}

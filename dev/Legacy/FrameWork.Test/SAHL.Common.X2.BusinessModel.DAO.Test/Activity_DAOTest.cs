using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.Security;

using Castle.ActiveRecord;
using NUnit.Framework;
using System.Security.Principal;
using Castle.ActiveRecord.Queries;


namespace SAHL.Common.X2.BusinessModel.Test
{
    [TestFixture]
    public class Activity_DAOTest : TestBase
    {
        [Test]
        public void Find()
        {
            base.TestFind<Activity_DAO>("X2.X2.Activity", "ID");
        }

        [Test]
        public void Query()
        {
            //string query = "SELECT DISTINCT a FROM Activity_DAO a join Instance_DAO i on i.StateID = a.StateID AND a.Type.Name = 'User'";  
            string query = "select IA.Activity from Instance_DAO I left join I.InstanceActivitySecurities as IA where IA.ADUserName = 'madumbi\\andrewr' and I.ID = 137894 and IA.Activity.ActivityType.Name = 'user'";

            SimpleQuery q = new SimpleQuery(typeof(Activity_DAO), query);
            Activity_DAO[] result = Activity_DAO.ExecuteQuery(q) as Activity_DAO[];

            if (result == null)
                result = new Activity_DAO[0];
        }
    }
}

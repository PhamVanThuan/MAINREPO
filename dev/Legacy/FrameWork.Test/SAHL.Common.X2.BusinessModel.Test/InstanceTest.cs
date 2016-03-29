using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Test;
using System.Data;

namespace SAHL.Common.X2.BusinessModel.Test
{
    [TestFixture]
    public class InstanceTest : TestBase
    {
        [Test]
        public void FindByPrincipal()
        {
            using (new SessionScope())
            {
                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string groups = CurrentPrincipalCache.GetCachedRolesAsStringForQuery(true, true);
                string query = string.Format("Select distinct i.* from [X2].[X2].[WorkList] wl (nolock) "
                    + "join [X2].[X2].[Instance] i (nolock) on i.ID = wl.InstanceID "
                    + "Where wl.ADUserName in ({0})", groups);

                Helper.FillFromQuery(DT, query, con, parameters);

                IEventList<IInstance> list = Instance.FindByPrincipal(this.TestPrincipal);

                Assert.That(list.Count == DT.Rows.Count);

                for (int i = 0; i < list.Count; i++)
                {
                    long ID = (long)DT.Rows[i][0];
                    long listID = list[i].ID;
                    Assert.That(listID == ID);
                }
            }
        }

        [Test]
        public void FindByPrincipalWithNullPrincipal()
        {
            using (new SessionScope())
            {
                IEventList<IInstance> list = Instance.FindByPrincipal(null);
                Assert.That(list == null);
            }
        }
    }
}
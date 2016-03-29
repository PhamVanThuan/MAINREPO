using SAHL.Core.Data;
using SAHL.DomainServiceChecks.Managers.X2InstanceDataManager.Statement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.DomainServiceChecks.Managers.X2InstanceDataManager
{
    public class X2DataManager : IX2DataManager
    {
        private IDbFactory dbFactory;

        public X2DataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
        public bool DoesInstanceIdExist(int instanceId)
        {
            DoesInstanceExistStatement statement = new DoesInstanceExistStatement(instanceId);
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                var results = db.SelectOne(statement);
                return (results > 0);
            }
        }
    }
}

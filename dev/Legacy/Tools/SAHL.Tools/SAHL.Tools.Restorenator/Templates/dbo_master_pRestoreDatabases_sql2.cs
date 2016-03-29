using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Restorenator.Templates
{
    public partial class dbo_master_pRestoreDatabases_sql
    {
        public dbo_master_pRestoreDatabases_sql(List<Restore> dbRestores,string userID)
        {
            this.Restores = dbRestores;
            this.UserID = userID;
        }

        public List<Restore> Restores { get; set; }

        public string UserID { get; set; }
    }

    public class Restore
    {
        public string DatabaseName { get; set; }
        public string RestoreFromPath { get; set; }
        public string RestoreToPath { get; set; }
    }
}

using SAHL.Tools.Restorenator.Templates;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Restorenator
{
    public class RestoreDatabase
    {
        public static void GenerateRestoreScript(string username, string databases, string restoreFromPath, string filePath)
        {
            string[] restores = databases.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<Restore> dbRestores = new List<Restore>();
            foreach (var restore in restores)
            {
                string[] restoreParameters = restore.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                Restore dbRestore = new Restore
                {
                    DatabaseName = restoreParameters[0],
                    RestoreFromPath = restoreFromPath,
                    RestoreToPath = restoreParameters[1]

                };
                dbRestores.Add(dbRestore);
            }

            dbo_master_pRestoreDatabases_sql pRestoreDatabasesProc = new dbo_master_pRestoreDatabases_sql(dbRestores, username);
            string pRestoreDatabasesProcGenerated = pRestoreDatabasesProc.TransformText();

            string fileName = filePath + @"\" + Properties.Resources.dynamicRestoreScript;

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(pRestoreDatabasesProcGenerated);
            }


        }
    }
}

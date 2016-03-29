using System.Collections.Generic;
using System.Data;

namespace SAHL.X2Designer.Connectivity
{
    public class Servers
    {
        public static List<string> GetServerNames()
        {
            List<string> lstServers = new List<string>();
            if (MainForm.App.Servers != null)
            {
                // sort the servernames
                DataRow[] drServers = MainForm.App.Servers.Select("", "ServerName ASC");

                foreach (DataRow row in drServers)
                {
                    string ServerName = "";
                    string InstanceName = "";
                    if (row[0] != null)
                        ServerName = row[0].ToString();
                    if (row[1] != null)
                        InstanceName = row[1].ToString();

                    if (InstanceName != "")
                        lstServers.Add(ServerName + "\\" + InstanceName);
                    else
                        lstServers.Add(ServerName);
                }
            }
            return lstServers;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace SAHL.Tools.Scriptenator.lib
{
    public class Script
    {
        private string serverName { get; set; }

        private string userName { get; set; }

        private string password { get; set; }

        public Script()
        {
        }

        public Script(string serverName, string userName, string password)
        {
            this.serverName = serverName;
            this.userName = userName;
            this.password = password;
        }

        public void ExecuteScriptenatorFile(string connectionString, string scriptenatorPath, string scriptenatorfile, bool dbRestore)
        {
            var compiledConnectionString = new SqlConnectionStringBuilder(connectionString);

            var sqlScripts = GetFilesFromScripenatorFile(scriptenatorPath, scriptenatorfile);

            using (var sqlcmd = new SqlExecutor(scriptenatorPath))
            {
                int scriptNum = 0;
                foreach (var script in sqlScripts)
                {
                    scriptNum++;
                    string filename = Path.Combine(scriptenatorPath, script);
                    try
                    {
                        if (dbRestore)
                        {
                            using (Timer timer = new Timer(GetRestoreStatus, compiledConnectionString, 30 * 1000, 120 * 1000))
                            {
                                Console.WriteLine(string.Format("{0} of {1}...{2}", scriptNum, sqlScripts.Count, filename));
                                sqlcmd.RunSqlFile(filename, compiledConnectionString.DataSource, compiledConnectionString.UserID, compiledConnectionString.Password);
                                Console.WriteLine("...SUCCESS");
                            }
                        }
                        else
                        {
                            Console.WriteLine(string.Format("{0} of {1}...{2}", scriptNum, sqlScripts.Count, filename));
                            sqlcmd.RunSqlFile(filename, compiledConnectionString.DataSource, compiledConnectionString.UserID, compiledConnectionString.Password);
                            Console.WriteLine("...SUCCESS");
                        }
                    }
                    catch (Exception scriptException)
                    {
                        Console.WriteLine(string.Format("...FAILURE: Error is: {0}", scriptException.Message));
                        throw;
                    }
                }
            }
        }

        private string[] restoreStatusEntry = new string[] { };

        private void GetRestoreStatus(object state)
        {
            SqlConnectionStringBuilder connection = (SqlConnectionStringBuilder)state;
            string output;
            using (var sqlcmd = new SqlExecutor(""))
            {
                output = sqlcmd.RunSqlCommand("Select * From [master].dbo.RestoreStatus", connection.DataSource, connection.UserID, connection.Password);
            }
            string[] newEntries = output.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (newEntries.Length > restoreStatusEntry.Length)
            {
                restoreStatusEntry = newEntries;
                foreach (string item in restoreStatusEntry.Skip(restoreStatusEntry.Length - 10))
                {
                    Console.WriteLine(item);
                }
            }
        }

        public void ExecuteScriptenatorFile(string scriptenatorPath, string scriptenatorfile)
        {
            var sqlScripts = GetFilesFromScripenatorFile(scriptenatorPath, scriptenatorfile);
            try
            {
                using (var sqlcmd = new SqlExecutor(scriptenatorPath))
                {
                    foreach (var script in sqlScripts)
                    {
                        string filename = Path.Combine(scriptenatorPath, script);
                        sqlcmd.RunSqlFile(filename, serverName, userName, password);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public List<string> GetFilesFromScripenatorFile(string scriptenatorPath, string scriptenatorfile)
        {
            XDocument xdoc = XDocument.Load(Path.Combine(scriptenatorPath, scriptenatorfile));
            List<string> sqlScripts = xdoc.Descendants("BatchParameters").Descendants("File").Select(x => x.Attribute("FileName").Value).ToList<string>();
            return sqlScripts;
        }

        public List<ScriptenatorFile> GetScriptenatorFilesInDirectory(string directory)
        {
            List<ScriptenatorFile> scriptenatorFiles = new List<ScriptenatorFile>();
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);
            FileInfo[] files = directoryInfo.GetFiles("*.xml");
            foreach (var file in files)
            {
                scriptenatorFiles.Add(new ScriptenatorFile(file.DirectoryName, file.Name));
            }
            return scriptenatorFiles;
        }

        public List<string> CheckFilesThatDontExistForEachScriptenatorFileInDirectory(string directory)
        {
            List<ScriptenatorFile> scriptenatorFiles = GetScriptenatorFilesInDirectory(directory);
            List<string> filesDontExist = new List<string>();
            foreach (var scriptenatorFile in scriptenatorFiles)
            {
                List<string> files = CheckFilesThatDontExistForEachScriptenatorFile(scriptenatorFile.Directory, scriptenatorFile.FileName);
                filesDontExist.AddRange(files);
            }
            return filesDontExist;
        }

        public List<string> CheckFilesThatDontExistForEachScriptenatorFile(string scriptenatorPath, string scriptenatorfile)
        {
            List<string> files = GetFilesFromScripenatorFile(scriptenatorPath, scriptenatorfile);
            List<string> filesDontExist = new List<string>();
            foreach (var file in files)
            {
                string fileToCheck = Path.Combine(scriptenatorPath, file);
                if (!File.Exists(fileToCheck))
                {
                    filesDontExist.Add(string.Format("File does not exist {0}", fileToCheck));
                }
            }
            return filesDontExist;
        }

        public void ProcessScriptenatorFileInDirectory(string directory)
        {
            List<ScriptenatorFile> scriptenatorFiles = GetScriptenatorFilesInDirectory(directory);
            foreach (var scriptenatorFile in scriptenatorFiles)
            {
                ExecuteScriptenatorFile(scriptenatorFile.Directory, scriptenatorFile.FileName);
            }
        }
    }
}
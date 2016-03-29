using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SAHL.Tools.Scriptenator.lib
{
    public class SqlExecutor : IDisposable
    {
        private Process process;
        private ProcessStartInfo processStartInformation;

        public SqlExecutor(string rootfolder)
        {
            processStartInformation = new ProcessStartInfo("sqlcmd.exe");
            processStartInformation.CreateNoWindow = true;
            processStartInformation.UseShellExecute = false;
            processStartInformation.WorkingDirectory = rootfolder;
            processStartInformation.ErrorDialog = true;
            processStartInformation.RedirectStandardError = true;
            processStartInformation.RedirectStandardOutput = true;
        }

        public void RunSqlFile(string filename, string servername)
        {
            processStartInformation.Arguments = string.Format("-S \"{0}\" -E -i \"{1}\" -b", servername, filename);
            RunSqlFile();
        }

        public void RunSqlFile(string filename, string servername, string username, string password)
        {
            object[] args = new object[4];
            args[0] = servername;
            args[1] = username;
            args[2] = password;
            args[3] = filename;

            processStartInformation.Arguments = string.Format("-S \"{0}\" -U \"{1}\" -P \"{2}\" -i \"{3}\" -b", args);

            RunSqlFile();
        }

        public string RunSqlCommand(string sqlcommand, string servername, string username, string password)
        {
            object[] args = new object[4];
            args[0] = servername;
            args[1] = username;
            args[2] = password;
            args[3] = sqlcommand;

            processStartInformation.Arguments = string.Format("-S \"{0}\" -U \"{1}\" -P \"{2}\" -Q \"{3}\" -b", args);

            string output = RunSqlFile();
            return output;
        }

        private string RunSqlFile()
        {
            process = Process.Start(processStartInformation);
            string stdout = process.StandardOutput.ReadToEnd();
            string stderror = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0 || !string.IsNullOrEmpty(stderror))
            {
                throw new Exception(string.Concat(stdout, stderror));
            }
            return stdout;
        }

        public void Kill()
        {
            try
            {
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }
            }
            finally
            {
                if (process != null)
                {
                    process.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Kill();
        }
    }
}
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Diagnostics;

namespace SAHL.VS.Build.Web
{
    public class WebJSLint : Task
    {
        private bool result = true;
        private string prevMessage;
        private string _projectDirPath;
        private string _repoRootPath;

        [Required]
        public string ProjectDir
        {
            get { return this._projectDirPath; }
            set { this._projectDirPath = value; }
        }

        [Required]
        public string RepoRootPath
        {
            get { return this._repoRootPath; }
            set { this._repoRootPath = value; }
        }

        public override bool Execute()
        {
            string arguments = string.Format(@"/C grunt --gruntfile={0}Build\NodeJS\jshint.js --basePath={1}", this._repoRootPath, this._projectDirPath);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo(@"cmd.exe", arguments)
                {
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.OutputDataReceived += OutputDataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return result;
        }

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                LintTokenizer token = new LintTokenizer(e.Data);
                if (token.IsErrorMessage)
                {
                    Log.LogError("jshint", "-1", "jshint", prevMessage, token.Line, token.Column, 0, 0, token.Message);
                    this.result = false;
                }
                else
                {
                    prevMessage = token.Value;
                }
                Log.LogMessage(MessageImportance.High, token.Value);
            }
        }
    }
}
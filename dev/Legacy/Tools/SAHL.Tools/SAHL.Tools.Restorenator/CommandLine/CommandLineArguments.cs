using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.Restorenator
{
    public class RestorenatorCommandLineArguments
    {
		[Option('c', "connectionString", Required = true, HelpText = "ConnectionString i.e. (Data Source=localhost; Initial Catalog=2am; User Id=sa; Password=sa;.")]
		public string ConnectionString { get; set; }

        [Option('d', "scriptDirectory", Required = true, HelpText = "Scriptenator Directory e.g. (c:/Scripts/).")]
		public string ScriptDirectory { get; set; }

        [Option('r', "restores", Required = true, HelpText = "Database(s) to be restored. Combination of the database name and filepath.")]
		public string Databases { get; set; }

        [Option('p', "restoreFromPath", Required = true, HelpText = @"The location of the backups e.g. \\sahlm02\Backups\Daily")]
		public string RestoreFromPath { get; set; }
}
}

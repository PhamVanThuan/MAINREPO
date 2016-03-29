using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace SAHL.Tools.Scriptenator
{
    public class CommandLineArguments
    {
        [Option('c', "connectionString", Required = true, HelpText = "ConnectionString i.e. (Data Source=localhost; Initial Catalog=2am; User Id=sa; Password=sa;.")]
		public string ConnectionString { get; set; }

        [Option('d', "scriptDirectory", Required = true, HelpText = "Scriptenator Directory e.g. (c:/Scripts/).")]
		public string ScriptDirectory { get; set; }

        [Option('f', "fileName", Required = true, HelpText = "Scriptenator File to Execute e.g. (2.36.0.1.a.xml).")]
		public string ScriptFileName { get; set; }

        [Option('r', "databaseRestore", Required = false, HelpText = "Used to determine if the process being executed is for a database restore e.g. true or false")]
		public bool DatabaseRestore { get; set; }
    }
}
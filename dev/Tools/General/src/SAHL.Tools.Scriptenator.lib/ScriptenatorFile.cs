using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Tools.Scriptenator.lib
{
	public class ScriptenatorFile
	{
		public ScriptenatorFile(string directory, string fileName)
		{
			this.Directory = directory;
			this.FileName = fileName;
		}

		public string Directory { get; set; }

		public string FileName { get; set; }
	}
}

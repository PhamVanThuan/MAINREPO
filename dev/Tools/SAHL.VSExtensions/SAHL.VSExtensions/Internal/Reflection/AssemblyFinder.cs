using SAHL.VSExtensions.Interfaces;
using SAHL.VSExtensions.Interfaces.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SAHomeloans.SAHL_VSExtensions.Internal.Reflection
{
    public class AssemblyFinder : IAssemblyFinder
    {
        private ISAHLProject project;

        public AssemblyFinder(ISAHLProject project)
        {
            this.project = project;
        }

        public IEnumerable<IScannableAssembly> Where(Func<string, bool> function)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(project.GetLatestBuildLocation());
            return dirInfo.GetFiles("*.dll", SearchOption.TopDirectoryOnly).Where(x => function(x.Name)).Select(x => new ScannableAssembly(x.FullName));
        }
    }
}
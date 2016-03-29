using SAHL.Core.Services;
using System;
using System.IO;
using System.Reflection;

namespace SAHL.Core.Testing.FileConventions
{
    public class HaloUIAssemblyConvention : SAHLFileConvention, IFileConvention
    {
        public bool Process(FileInfo file)
        {
            return base.Run(file, () =>
            {
                return file.Name.Contains("SAHL.UI.Halo.");
            });
        }
    }
}

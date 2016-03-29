using SAHL.Core.IoC;
using SAHL.Core.Testing.FileConventions;
using System;
using System.IO;
using System.Reflection;

namespace SAHL.Core.Testing.FileConventions
{
    public class SAHLTestsAssemblyConvention : SAHLFileConvention, IFileConvention
    {
        public bool Process(FileInfo file)
        {
            return Run(file, () =>
            {
                return file.Name.Contains("SAHL.Core.Tests.dll");
            });
        }
    }
}
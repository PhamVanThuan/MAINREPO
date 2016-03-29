using SAHL.Core.IoC;
using SAHL.Core.Testing.FileConventions;
using System;
using System.IO;
using System.Reflection;

namespace SAHL.Core.Testing.FileConventions
{
    public class ComcorpAssemblyConvention : SAHLFileConvention, IFileConvention
    {
        public bool Process(FileInfo file)
        {
            return base.Run(file, () =>
            {
                return file.Name.Contains("SAHL.WCFServices.ComcorpConnector");
            });
        }
    }
}
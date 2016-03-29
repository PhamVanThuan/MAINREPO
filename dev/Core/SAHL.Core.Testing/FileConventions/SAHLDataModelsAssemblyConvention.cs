using System.Linq;
using System.Collections.Generic;
using System.IO;
using SAHL.Core.Testing.FileConventions;
using System.Reflection;
namespace SAHL.Core.Testing.FileConventions
{
    public class SAHLDataModelsAssemblyConvention : SAHLFileConvention, IFileConvention
    {
        public bool Process(FileInfo file)
        {
            return base.Run(file, () =>
            {
                if (file.Name.Contains("Test") || file.Name.Contains("Client") || file.Name.Contains("Specs"))
                {
                    return false;
                }
                return file.Name.Contains("SAHL.Core.Data.Models") || file.Name.Contains("DomainServiceCheck");
            });
        }
    }
}

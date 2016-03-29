using StructureMap.Configuration.DSL;
using System.IO.Abstractions;

namespace SAHL.Config.Services.DocumentManager.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<System.IO.Abstractions.IFileSystem>().Use<System.IO.Abstractions.FileSystem>();
        }
    }
}
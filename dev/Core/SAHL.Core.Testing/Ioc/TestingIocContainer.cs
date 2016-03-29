using SAHL.Core.Data;
using SAHL.Core.Testing.FileConventions;
using SAHL.Core.Testing.Providers;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace SAHL.Core.Testing.Ioc
{
    public sealed class TestingIocContainer<T, T2> : Container, ITestingIocContainer<T, T2>
        where T : IFileConvention
        where T2 : IRegistrationConvention, new()
    {
        private T fileConvention;
        private TestingIoc testingIoc;
        private Type[] registeredTypes;
        public TestingIocContainer(TestingIoc testingIoc)
        {
            this.testingIoc = testingIoc;
            this.registeredTypes = new Type[0];
        }
        public void Initialise()
        {
            this.fileConvention = Activator.CreateInstance<T>();

            this.Configure(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssembliesFromApplicationBaseDirectory(assembly => this.fileConvention.Process(new FileInfo(assembly.Location)));
                    scan.Convention<T2>();
                });
            });

            this.registeredTypes = 
                    this.Model.PluginTypes
                    .Where(x => x.PluginType.Namespace.Contains("SAHL"))
                    .Select((x => x.PluginType)).ToArray();
        }

        public IEnumerable<Type> GetRegisteredTypes()
        {
            return this.registeredTypes;
        }
    }
}

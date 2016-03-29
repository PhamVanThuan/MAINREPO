using SAHL.Core.Testing.FileConventions;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Linq;
using System.Collections.Generic;
using SAHL.Core.Testing.Providers;
using SAHL.Core.Data;
using SAHL.Core.Logging;
using SAHL.Core.Data.Context.Configuration;
using SAHL.Core.Data.Dapper;
using SAHL.Core.Data.Configuration;
using StructureMap.Pipeline;
namespace SAHL.Core.Testing.Ioc
{
    public sealed class TestingIoc : Container, ITestingIoc
    {
        public void Configure<T, T2>()
            where T : IFileConvention
            where T2 : IRegistrationConvention, new()
        {
            this.Configure(x =>
            {
                x.For<ITestingIocContainer<T, T2>>()
                    .Use<TestingIocContainer<T, T2>>().OnCreation((container) =>
                    {
                        container.Initialise();
                    });

                x.For<ITestParamsProvider<T, T2>>()
                  .Use<TestParamsProvider<T, T2>>();
            });
        }

        public static ITestingIoc Initialise()
        {
            var testingIoc = new TestingIoc();
            var config = new Action<ConfigurationExpression>((x) =>
            {
                x.Scan(scan =>
                  {
                      scan.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                      scan.TheCallingAssembly();
                      scan.LookForRegistries();
                      scan.WithDefaultConventions();
                      scan.AddAllTypesOf<IUIStatementsProvider>();
                  });
                x.For<ITestingIoc>().Singleton().Use(testingIoc);
            });

            testingIoc.Configure(config);
            //Have to configure Main container as well because For<IIocContainer>().Use<StructureMapIocContainer>() still use the container on ObjectFactory.
            ObjectFactory.Configure(config);
            DefaultExplicitArguments.Container = testingIoc;
            return testingIoc;
        }

    }
}
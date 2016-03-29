using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.Core;
using StructureMap;
using SAHL.Config.Core;

namespace SAHL.Config.Services.Tests.Conventions
{
    [TestFixture]
    public class TestQueryHandlerDecoratorConvention
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(y => y.FullName.StartsWith("SAHL"));
                            scanner.TheCallingAssembly();
                            scanner.WithDefaultConventions();
                        });

                    expression.For<IIocContainer>().Use<StructureMapIocContainer>();
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(iocContainer);
        }

    }
}

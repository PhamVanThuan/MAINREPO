using System;

using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.IoC;
using SAHL.Core.Data;
using SAHL.Core.Testing.Fakes;

namespace SAHL.Services.Halo.Server.Tests
{
    public abstract class TestQueryHandlerBase
    {
        protected IIocContainer iocContainer;
        protected IDbFactory dbFactory;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                            scanner.LookForRegistries();
                        });

                    expression.For<IDbFactory>().Use<FakeDbFactory>();
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);

            this.dbFactory = ObjectFactory.GetInstance<IDbFactory>();
            Assert.IsNotNull(this.dbFactory);

            var allStartables = this.iocContainer.GetAllInstances<IStartable>();
            foreach (var startable in allStartables)
            {
                Console.WriteLine("Starting {0}", startable.GetType().Name);
                Assert.DoesNotThrow(startable.Start);
            }
        }
    }
}

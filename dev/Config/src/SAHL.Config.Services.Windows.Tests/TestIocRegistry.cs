using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Logging;
using SAHL.Config.Services.Core;

namespace SAHL.Config.Services.Windows.Tests
{
    [TestFixture]
    public class TestIocRegistry
    {
        private IIocContainer iocContainer;

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
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void Constructor_ShouldRegisterILoggerSource()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var loggerSource = iocContainer.GetInstance<ILoggerSource>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(loggerSource);
        }

        [Test]
        public void Constructor_ShouldRegisterNamedInstanceOfILoggerSource()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var loggerSource = iocContainer.GetInstance<ILoggerSource>("AppStartUpLogSource");
            //---------------Test Result -----------------------
            Assert.IsNotNull(loggerSource);
        }

        [Test]
        public void Constructor_ShouldRegisterIWindowsServiceSettings()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var loggerSource = iocContainer.GetInstance<IWindowsServiceSettings>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(loggerSource);
        }

        [Test]
        public void Constructor_ShouldRegisterIServiceSettings()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var loggerSource = iocContainer.GetInstance<IServiceSettings>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(loggerSource);
            Assert.IsInstanceOf<WindowsServiceSettings>(loggerSource);
        }
    }
}

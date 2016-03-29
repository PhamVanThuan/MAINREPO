using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloWizardBaseTileConfiguration
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dynamicAction = new TestWizardAction();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dynamicAction);
        }

        [Test]
        public void Properties_ShouldSetRelevantProperties()
        {
            //---------------Set up test pack-------------------
            var contextData = "testContexData";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dynamicAction = new TestWizardAction(contextData);
            //---------------Test Result -----------------------
            Assert.AreEqual("Test", dynamicAction.Group);
            Assert.AreEqual("Test Action", dynamicAction.Name);
            Assert.AreEqual("icon", dynamicAction.IconName);
            Assert.AreEqual(1234, dynamicAction.Sequence);
            Assert.IsNotNull(dynamicAction.TileConfiguration);
            Assert.IsInstanceOf<TestRootTileConfiguration>(dynamicAction.TileConfiguration);
            Assert.IsNotNull(dynamicAction.WizardTileConfiguration);
            Assert.IsInstanceOf<TestWizardConfiguration>(dynamicAction.WizardTileConfiguration);
            Assert.AreEqual(contextData, dynamicAction.ContextData);
        }

        private class TestWizardAction : HaloTileWizardActionBase<TestRootTileConfiguration, TestWizardConfiguration>
        {
            public TestWizardAction(string contextData = null)
                : base("Test Action", "icon", "Test", 1234, contextData)
            {
            }
        }

        private class TestRootTileConfiguration : HaloSubTileConfiguration,
                                                  IHaloRootTileConfiguration
        {
            public TestRootTileConfiguration()
                : base("Mortgage Loan", "MortgageLoan", 2)
            {
            }
        }

        private class TestWizardConfiguration : HaloWizardBaseTileConfiguration
        {
            public TestWizardConfiguration() 
                : base("Test Wizard", WizardType.Sequential)
            {
            }
        }
    }
}

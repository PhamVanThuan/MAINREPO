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
    public class TestHaloWizardBaseWorkflowConfiguration
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configuration = new TestWizardWorkflowConfiguration();
            //---------------Test Result -----------------------
            Assert.IsNotNull(configuration);
        }

        [Test]
        public void Properties_ShouldSetRelevantProperties()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configuration = new TestWizardWorkflowConfiguration();
            //---------------Test Result -----------------------
            Assert.AreEqual("Test Workflow Wizard", configuration.Name);
            Assert.AreEqual(WizardType.Sequential, configuration.WizardType);
            Assert.AreEqual("Process", configuration.ProcessName);
            Assert.AreEqual("Workflow", configuration.WorkflowName);
            Assert.AreEqual("Activity", configuration.ActivityName);
        }

        private class TestWizardWorkflowConfiguration : HaloWizardBaseWorkflowConfiguration
        {
            public TestWizardWorkflowConfiguration() 
                : base("Test Workflow Wizard", WizardType.Sequential, "Process", "Workflow", "Activity")
            {
            }
        }
    }
}

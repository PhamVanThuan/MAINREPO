using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Core.Testing;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.Halo.Queries;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestGetWizardconfigurationQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var businessContext = new BusinessContext("", GenericKeyType.Account, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetWizardConfigurationQuery("wizardName", "process", "workflow", "activity", businessContext);
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [TestCase("businessContext")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<GetWizardConfigurationQuery>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            const string wizardName   = "wizard";
            const string processName  = "process";
            const string workflowName = "workflow";
            const string activityName = "activity";
            var businessContext       = new BusinessContext("", GenericKeyType.Account, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetWizardConfigurationQuery(wizardName, processName, workflowName, activityName, businessContext);
            //---------------Test Result -----------------------
            Assert.AreEqual(wizardName, query.WizardName);
            Assert.AreEqual(processName, query.ProcessName);
            Assert.AreEqual(workflowName, query.WorkflowName);
            Assert.AreEqual(activityName, query.ActivityName);
            Assert.AreEqual(businessContext, query.BusinessContext);
        }
    }
}

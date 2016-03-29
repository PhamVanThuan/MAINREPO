using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public partial class HostCommandsConvention
    {
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostMethodNames")]
        public void CheckForDuplicateCommands(string hostMethodName)
        {
            string commandName = hostMethodName + "Command";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            // check if a command exists by convention
            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandName).ToList();
            Assert.That(types.Count <= 1);
        }
    }
}
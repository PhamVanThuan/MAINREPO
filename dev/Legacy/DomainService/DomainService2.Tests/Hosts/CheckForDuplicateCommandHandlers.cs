using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public partial class HostCommandHandlerConventions
    {
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostMethodNames")]
        public void CheckForDuplicateCommandHandlers(string hostMethodName)
        {
            string commandName = hostMethodName + "Command";
            string commandHandlerName = commandName + "Handler";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            // check if a command exists by convention
            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandHandlerName).ToList();
            Assert.That(types.Count == 1);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public partial class HostCommandHandlersConvention
    {
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostMethodNames")]
        public void CheckForMissingCommandHandlers(string hostMethodName)
        {
            string commandName = hostMethodName + "Command";
            string commandHandlerName = commandName + "Handler";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandHandlerName).ToList();
            Assert.That(types.Count >= 1);
        }
    }
}
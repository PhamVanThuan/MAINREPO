using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public partial class HostCommandConventions
    {
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostMethodNamesThatReturnResults")]
        public void CheckCommandForReturnResultNaming(string hostMethodName)
        {
            // each host method name passed in is a method that returns a value

            // get the command name based on the host methodname
            string commandName = hostMethodName + "Command";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            // get the command type and assert that it has a result property
            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandName).ToList();
            if (types.Count == 1)
            {
                Type commandType = types[0];
                PropertyInfo[] properties = commandType.GetProperties().Where(x => x.Name.EndsWith("Result")).ToArray();
                Assert.That(properties != null);
                Assert.That(properties.Length == 1);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public partial class HostRuleCommandsConvention
    {
        //need to come back and look at this and improve heuristics.
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostMethodNames")]
        public void CheckForMissingRuleCommands(string hostMethodName)
        {
            string commandName = hostMethodName + "Command";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandName).ToList();
            Assert.That(types.Count >= 1);
            foreach (Type t in types)
            {
                if (t.BaseType.Name == "RuleDomainServiceCommand" || t.BaseType.Name == "RuleSetDomainServiceCommand")
                {
                    Assert.That(t.Name.StartsWith("Check"));
                    Assert.That(hostMethodName.EndsWith("Rules") || hostMethodName.EndsWith("Rule"));
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public partial class HostRuleCommandHandlersConvention
    {
        //need to come back and look at this and improve heuristics.
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostMethodNames")]
        public void CheckForMissingRuleCommandHandlers(string hostMethodName)
        {
            string commandName = hostMethodName + "Command";
            string commandHandlerName = commandName + "Handler";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandHandlerName).ToList();
            Assert.That(types.Count >= 1);

            foreach (Type t in types)
            {
                bool isIHandlesDomainServiceRuleCommandHandler = t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandlesDomainServiceCommand<>));
                if (isIHandlesDomainServiceRuleCommandHandler)
                {
                    if (t.GetInterfaces()[0].GetGenericArguments()[0].BaseType.Name == "RuleDomainServiceCommand" || t.GetInterfaces()[0].GetGenericArguments()[0].BaseType.Name == "RuleSetDomainServiceCommand")
                    {
                        Assert.That(t.Name.StartsWith("Check"));
                        Assert.That(hostMethodName.EndsWith("Rules") || hostMethodName.EndsWith("Rule"));
                    }
                }
            }
        }
    }
}
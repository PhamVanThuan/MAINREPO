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
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostAndMethodNamesThatHaveOutParams")]
        public void CheckCommandForHostParamNaming(string hostAndMethod)
        {
            string[] splits = hostAndMethod.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            string hostFullName = splits[0];

            string hostMethodName = splits[1];

            string commandName = hostMethodName + "Command";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            // get the command type and assert that it has a result property
            List<Type> types = assembly.GetExportedTypes().Where(x => x.Name == commandName).ToList();
            if (types.Count == 1)
            {
                Type commandType = types[0];
                Type hostType = assembly.GetType(hostFullName);
                MethodInfo mi = hostType.GetMethod(hostMethodName);
                ParameterInfo[] parameters = mi.GetParameters().Where(x => x.IsOut).ToArray();
                PropertyInfo[] properties = commandType.GetProperties();

                foreach (ParameterInfo pi in parameters)
                {
                    string paramName = pi.Name.ToLower();
                    PropertyInfo prop = properties.Where(x => x.Name.ToLower() == paramName).Single();
                    Assert.That(prop != null);
                }
            }
        }
    }
}
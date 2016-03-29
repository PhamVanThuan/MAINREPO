using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;

namespace DomainService2.Tests.Commands
{
    [TestFixture]
    public partial class CommandConvetions
    {
        [Test, TestCaseSource(typeof(CommandProvider), "GetCommandTypes")]
        public void CheckCommandConstructorParametersAreSetOnProperties(Type commandType)
        {
            TypeValueProvider typeValueProvider = new TypeValueProvider();

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            // get the constructor
            ConstructorInfo mi = commandType.GetConstructors().Single();

            // get the constructor args
            ParameterInfo[] pis = mi.GetParameters();

            // get the command properties
            PropertyInfo[] props = commandType.GetProperties();

            List<object> constructorArgs = new List<object>();
            foreach (ParameterInfo pi in pis)
            {
                constructorArgs.Add(typeValueProvider.GetValueForType(pi.ParameterType));
            }

            IDomainServiceCommand command = (IDomainServiceCommand)assembly.CreateInstance(commandType.FullName, false, BindingFlags.CreateInstance, null, constructorArgs.ToArray(), null, new object[] { });

            // now check that each constructor argument has been set on a command property
            foreach (ParameterInfo pi in pis)
            {
                string paramName = pi.Name.ToLower();
                PropertyInfo prop = props.Where(x => x.Name.ToLower() == paramName).Single();
                object result = prop.GetValue(command, null);

                typeValueProvider.AssertTypeValueIsCorrect(result);
            }
        }
    }
}
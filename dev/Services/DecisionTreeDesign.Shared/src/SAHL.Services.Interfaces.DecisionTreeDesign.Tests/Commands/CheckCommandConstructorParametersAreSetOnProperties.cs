using NUnit.Framework;
using SAHL.Core;
using SAHL.Core.Attributes;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Tests.Commands
{
    [TestFixture]
    public partial class CommandConventions
    {
        [Test, TestCaseSource(typeof(CommandProvider), "GetServiceCommandTypes")]
        public void CheckCommandConstructorParametersAreSetOnProperties(Type commandType)
        {
            TypeValueProvider typeValueProvider = new TypeValueProvider();

            var assembly = Assembly.GetAssembly(typeof(SaveDecisionTreeAsCommand));

            // get the constructor
            ConstructorInfo mi = commandType.GetConstructors().Single();

            // get the constructor args
            ParameterInfo[] pis = mi.GetParameters();

            // get the command properties
            PropertyInfo[] props = commandType.GetProperties();

            var constructorKeyVals = new Dictionary<string, object>();
            var testParams = commandType.GetCustomAttribute<ConstructorTestParams>();
            if (testParams != null && testParams.ConstructorArgs.Count() > 1)
            {
                var parameters = testParams.ConstructorArgs.Split(';');

                foreach (var param in parameters)
                {
                    var keyVal = param.Split('=');
                    var key = keyVal[0].Replace(" ", String.Empty);
                    var value = keyVal[1] == "null" ? null : keyVal[1];
                    constructorKeyVals.Add(key, value);
                }
            }

            List<object> constructorArgs = new List<object>();
            foreach (ParameterInfo pi in pis)
            {
                if (constructorKeyVals.ContainsKey(pi.Name))
                {
                    constructorArgs.Add(constructorKeyVals[pi.Name]);
                }
                else
                {
                    constructorArgs.Add(typeValueProvider.GetValueForType(pi.ParameterType));
                }
            }

            ServiceCommand command = (ServiceCommand)assembly.CreateInstance(commandType.FullName, false, BindingFlags.CreateInstance, null, constructorArgs.ToArray(), null, new object[] { });

            // now check that each constructor argument has been set on a command property
            foreach (ParameterInfo pi in pis)
            {
                string paramName = pi.Name.ToLower();

                if (!constructorKeyVals.ContainsKey(pi.Name))
                {
                    PropertyInfo prop = props.Where(x => x.Name.ToLower() == paramName).Single();
                    object result = prop.GetValue(command, null);

                    typeValueProvider.AssertTypeValueIsCorrect(result);
                }
            }
        }
    }
}
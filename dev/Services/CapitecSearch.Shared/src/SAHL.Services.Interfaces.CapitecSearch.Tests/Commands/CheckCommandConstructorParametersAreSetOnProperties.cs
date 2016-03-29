using NUnit.Framework;
using SAHL.Core;
using SAHL.Core.Attributes;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.CapitecSearch.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.CapitecSearch.Tests.Commands
{
    [TestFixture]
    public partial class CommandConventions
    {
        [Test, TestCaseSource(typeof(CommandProvider), "GetCapitecSearchServiceCommandTypes")]
        public void CheckCommandConstructorParametersAreSetOnProperties(Type commandType)
        {
            ICapitecSearchServiceCommand command = null;

            TypeValueProvider typeValueProvider = new TypeValueProvider();

            var assembly = Assembly.GetAssembly(typeof(RefreshLuceneIndexCommand));

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
                    if (pi.ParameterType.IsValueType)
                    {
                        constructorArgs.Add(typeValueProvider.GetValueForType(pi.ParameterType));
                    }
                    else if (pi.ParameterType == typeof(SAHL.Core.SystemMessages.ISystemMessageCollection))
                    {
                        constructorArgs.Add(SystemMessageCollection.Empty());
                    }
                    else if (pi.ParameterType == typeof(string))
                    {
                        constructorArgs.Add(typeValueProvider.GetValueForType(pi.ParameterType));
                    }
                    else if (pi.ParameterType.Name.StartsWith("IEnumerable"))
                    {
                        Type enumerableType = pi.ParameterType.GetGenericArguments()[0];
                        Type openGenericListType = typeof(List<>);
                        openGenericListType = openGenericListType.MakeGenericType(new[] { enumerableType });

                        dynamic emptyList = Activator.CreateInstance(openGenericListType);

                        constructorArgs.Add(emptyList);
                    }
                    else if (pi.ParameterType.Name.EndsWith("[]"))
                    {
                        var instance = GetInstance(pi.ParameterType.GetElementType());
                        var list = new object[] { instance };
                        constructorArgs.Add(list);
                    }
                    else
                    {
                        var instance = GetInstance(pi.ParameterType);
                        constructorArgs.Add(instance);
                    }
                }
            }

            if (command == null)
            {
                command = (ICapitecSearchServiceCommand)assembly.CreateInstance(commandType.FullName, false, BindingFlags.CreateInstance, null, constructorArgs.ToArray(), null, new object[] { });
            }

            // now check that each constructor argument has been set on a command property
            foreach (ParameterInfo pi in pis)
            {
                string paramName = pi.Name.ToLower();

                if (!constructorKeyVals.ContainsKey(pi.Name))
                {
                    PropertyInfo prop = props.Where(x => x.Name.ToLower() == paramName).Single();
                    object result = prop.GetValue(command, null);

                    if (pi.GetType().IsValueType)
                    {
                        typeValueProvider.AssertTypeValueIsCorrect(result);
                    }
                }
            }
        }

        public object GetInstance(Type commandType)
        {
            TypeValueProvider typeValueProvider = new TypeValueProvider();

            var assembly = Assembly.GetAssembly(commandType);

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

            var instance = assembly.CreateInstance(commandType.FullName, false, BindingFlags.CreateInstance, null, constructorArgs.ToArray(), null, new object[] { });
            return instance;
        }

        [Test]
        public void Custom()
        {
            CheckCommandConstructorParametersAreSetOnProperties(typeof(RefreshLuceneIndexCommand));
        }
    }
}
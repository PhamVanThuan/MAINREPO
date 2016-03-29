using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;
using NUnit.Framework;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.Tests.Hosts
{
    [TestFixture]
    public class HostConventions
    {
        [Test, TestCaseSource(typeof(HostsProvider), "GetHostAndMethodNamesThatReturnResults")]
        public void CheckHostsReturnCommandResultAndSetOutParameters(string hostAndMethod)
        {
            TypeValueProvider typeValueProvider = new TypeValueProvider();

            string[] splits = hostAndMethod.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            string hostFullName = splits[0];

            string hostMethodName = splits[1];

            string commandName = hostMethodName + "Command";

            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));

            // make a fake commandHandler
            ICommandHandler handler = new FakeCommandHandler();
            IDomainMessageCollection messages = new DomainMessageCollection();

            // create the host
            IDomainHost host = (IDomainHost)assembly.CreateInstance(hostFullName, false, BindingFlags.CreateInstance, null, new object[] { handler }, null, null);
            Type hostType = assembly.GetType(hostFullName);

            // check if the hostmethod returns the property value
            MethodInfo mi = hostType.GetMethods().Where(x => x.Name == hostMethodName).Single();

            // setup some parameters for the method arguments
            List<int> outParameters = new List<int>();

            List<object> allParameters = new List<object>();

            ParameterInfo[] parameters = mi.GetParameters();
            bool hasReturnValue = false;
            ParameterInfo returnParameterInfo;

            if (mi.ReturnType != typeof(void))
            {
                hasReturnValue = true;
                returnParameterInfo = mi.ReturnParameter;
            }

            List<ParameterInfo> outs = new List<ParameterInfo>();
            int loopCount = 0;
            foreach (ParameterInfo pi in parameters)
            {
                if (pi.ParameterType == typeof(IDomainMessageCollection))
                {
                    allParameters.Add(messages);
                }
                else
                {
                    if (pi.IsOut)
                    {
                        // set the default value for
                        object value = GetDefault(pi.ParameterType);
                        allParameters.Add(value);
                        outParameters.Add(loopCount);
                    }
                    else
                    {
                        object value = typeValueProvider.GetValueForType(pi.ParameterType);
                        allParameters.Add(value);
                    }
                }

                if (pi.IsOut)
                {
                    outs.Add(pi);
                }
                loopCount++;
            }

            try
            {
                // invoke the handler
                object[] allParamsArray = allParameters.ToArray();
                object result = mi.Invoke(host, allParamsArray);

                // check that the parameters set on the command are propogated through to the host

                // first check for return values
                if (hasReturnValue)
                {
                    try
                    {
                        typeValueProvider.AssertTypeValueIsCorrect(result);
                    }
                    catch
                    {
                        throw;
                    }
                }

                // check for out parameters
                if (outs.Count > 0)
                {
                    foreach (int outValIndex in outParameters)
                    {
                        typeValueProvider.AssertTypeValueIsCorrect(allParamsArray[outValIndex]);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public object GetDefault(Type t)
        {
            var realType = t.GetElementType();
            if (realType != null)
            {
                t = realType;
            }

            if (t == typeof(string))
            {
                return string.Empty;
            }
            else
            {
                return this.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
            }
        }

        public T GetDefaultGeneric<T>()
        {
            return default(T);
        }
    }
}
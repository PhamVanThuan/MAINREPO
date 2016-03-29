using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;

namespace DomainService2.Tests.Hosts
{
    public class HostsProvider
    {
        internal static List<string> exclusions = new List<string>() { "DoesNotMeetCreditSignatureRequirements" };

        public static IEnumerable GetHostMethodNames()
        {
            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(IDomainHost).Name).Count() > 0
                          ).ToList();

            // Loop through the public methods and return the names
            List<string> methodNames = new List<string>();
            foreach (Type hostType in types)
            {
                methodNames.AddRange(hostType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public).Select(x => x.Name));
                foreach (string toRemove in HostsProvider.exclusions)
                {
                    methodNames.Remove(toRemove);
                }
            }

            return methodNames;
        }

        public static IEnumerable GetHostMethodNamesThatReturnResults()
        {
            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(IDomainHost).Name).Count() > 0
                          ).ToList();

            // Loop through the public methods and return the names
            List<string> methodNames = new List<string>();
            foreach (Type hostType in types)
            {
                methodNames.AddRange(hostType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public).Where(x => x.ReturnType != typeof(void)).Select(x => x.Name));
                foreach (string toRemove in HostsProvider.exclusions)
                {
                    methodNames.Remove(toRemove);
                }
            }

            return methodNames;
        }

        public static IEnumerable GetHostAndMethodNamesThatReturnResults()
        {
            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(IDomainHost).Name).Count() > 0
                          ).ToList();

            // Loop through the public methods and return the names
            List<string> methodNames = new List<string>();
            foreach (Type hostType in types)
            {
                string[] methodNameStrs = hostType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public).Where(x => x.ReturnType != typeof(void)).Select(x => x.Name).ToArray();
                foreach (string methodNameStr in methodNameStrs)
                {
                    methodNames.Add(string.Format("{0}||{1}", hostType.FullName, methodNameStr));
                }
            }

            return methodNames;
        }

        public static IEnumerable GetHostAndMethodNamesThatHaveOutParams()
        {
            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(IDomainHost).Name).Count() > 0
                          ).ToList();

            // Loop through the public methods and return the names
            List<string> methodNames = new List<string>();
            foreach (Type hostType in types)
            {
                MethodInfo[] methods = hostType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                foreach (MethodInfo mi in methods)
                {
                    ParameterInfo[] parameters = mi.GetParameters().Where(x => x.IsOut).ToArray();
                    if (parameters.Length > 0)
                    {
                        methodNames.Add(string.Format("{0}||{1}", hostType.FullName, mi.Name));
                    }
                }
            }

            return methodNames;
        }
    }
}
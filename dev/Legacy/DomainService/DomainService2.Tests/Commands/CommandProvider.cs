using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DomainService2.Workflow.Cap2;

namespace DomainService2.Tests.Commands
{
    public class CommandProvider
    {
        public static IEnumerable GetCommandTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(CheckReadvanceDoneRulesCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(IDomainServiceCommand).Name).Count() > 0
                          ).ToList();

            return types;
        }
    }
}
using SAHL.Services.Interfaces.DecisionTreeDesign.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Tests.Commands
{
    public class CommandProvider
    {
        public static IEnumerable GetServiceCommandTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(SaveDecisionTreeAsCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(IDecisionTreeServiceCommand).Name).Count() > 0
                          ).ToList();

            return types;
        }

        public static IEnumerable GetServiceCommandNames()
        {
            var decisionTreeServiceCommandTypes = GetServiceCommandTypes();

            var commandNames = new List<string>();
            foreach (Type commandType in decisionTreeServiceCommandTypes)
            {
                commandNames.Add(commandType.Name);
            }

            return commandNames;
        }
    }
}
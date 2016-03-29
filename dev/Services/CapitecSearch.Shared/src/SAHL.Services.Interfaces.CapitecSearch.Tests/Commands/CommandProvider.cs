using SAHL.Services.Interfaces.CapitecSearch.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.CapitecSearch.Tests.Commands
{
    public class CommandProvider
    {
        public static IEnumerable GetCapitecSearchServiceCommandTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(RefreshLuceneIndexCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(ICapitecSearchServiceCommand).Name).Count() > 0
                          ).ToList();

            return types;
        }

        public static IEnumerable GetCapitecSearchServiceCommandNames()
        {
            var capitecServiceCommandTypes = GetCapitecSearchServiceCommandTypes();

            var commandNames = new List<string>();
            foreach (Type commandType in capitecServiceCommandTypes)
            {
                commandNames.Add(commandType.Name);
            }

            return commandNames;
        }
    }
}
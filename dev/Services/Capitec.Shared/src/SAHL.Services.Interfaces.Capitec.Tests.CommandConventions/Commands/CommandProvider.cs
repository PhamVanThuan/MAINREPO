using SAHL.Services.Interfaces.Capitec.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.Capitec.Tests.Commands
{
    public class CommandProvider
    {
        public static IEnumerable GetCapitecServiceCommandTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(LoginCommand));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(ICapitecServiceCommand).Name).Count() > 0
                          ).ToList();

            return types;
        }

        public static IEnumerable GetCapitecServiceCommandNames()
        {
            var capitecServiceCommandTypes = GetCapitecServiceCommandTypes();

            var commandNames = new List<string>();
            foreach (Type commandType in capitecServiceCommandTypes)
            {
                commandNames.Add(commandType.Name);
            }

            return commandNames;
        }
    }
}

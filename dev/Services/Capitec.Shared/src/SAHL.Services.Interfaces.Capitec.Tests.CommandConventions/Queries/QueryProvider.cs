using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SAHL.Services.Interfaces.Capitec.Tests.Queries
{
    public class QueryProvider
    {
        public static IEnumerable GetCapitecServiceQueryTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(GetUsersQuery));
            List<Type> types = assembly.GetTypes().Where(x =>
                !x.IsInterface &&
                !x.IsGenericType &&
                !x.IsAbstract &&
                x.GetInterfaces().Where(i => i.Name == typeof(IServiceQuery).Name).Count() > 0
                ).ToList();
            return types;
        }

        public static IEnumerable GetCapitecServiceQueryNames()
        {
            var capitecServiceQueryTypes = GetCapitecServiceQueryTypes();
            var queryNames = new List<string>();
            foreach (Type queryType in capitecServiceQueryTypes)
            {
                queryNames.Add(queryType.Name);
            }
            return queryNames;
        }
    }
}

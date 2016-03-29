using SAHL.Core.Services;
using SAHL.Services.Interfaces.CapitecSearch.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.CapitecSearch.Tests.Queries
{
    public class QueryProvider
    {
        public static IEnumerable GetCapitecSearchServiceQueryTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(ApplicationStatusQuery));
            List<Type> types = assembly.GetTypes().Where(x =>
                !x.IsInterface &&
                !x.IsGenericType &&
                !x.IsAbstract &&
                x.GetInterfaces().Where(i => i.Name == typeof(IServiceQuery).Name).Count() > 0
                ).ToList();
            return types;
        }

        public static IEnumerable GetCapitecSearchServiceQueryNames()
        {
            var capitecServiceQueryTypes = GetCapitecSearchServiceQueryTypes();
            var queryNames = new List<string>();
            foreach (Type queryType in capitecServiceQueryTypes)
            {
                queryNames.Add(queryType.Name);
            }
            return queryNames;
        }
    }
}
using SAHL.Core.Services;
using SAHL.Services.Interfaces.DecisionTreeDesign.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Tests.Queries
{
    public class QueryProvider
    {
        public static IEnumerable GetServiceQueryTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(GetDecisionTreeQuery));
            List<Type> types = assembly.GetTypes().Where(x =>
                !x.IsInterface &&
                !x.IsGenericType &&
                !x.IsAbstract &&
                x.GetInterfaces().Where(i => i.Name == typeof(IServiceQuery).Name).Count() > 0
                ).ToList();
            return types;
        }

        public static IEnumerable GetServiceQueryNames()
        {
            var capitecServiceQueryTypes = GetServiceQueryTypes();
            var queryNames = new List<string>();
            foreach (Type queryType in capitecServiceQueryTypes)
            {
                queryNames.Add(queryType.Name);
            }
            return queryNames;
        }
    }
}
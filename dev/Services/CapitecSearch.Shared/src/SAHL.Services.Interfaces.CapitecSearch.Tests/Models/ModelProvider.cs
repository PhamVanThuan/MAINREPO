using NUnit.Framework;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.CapitecSearch.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.CapitecSearch.Tests.Models
{
    public class ModelProvider
    {
        public static IEnumerable GetCapitecSearchServiceModelTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(ApplicationStatusQueryResult));
            List<Type> types = assembly.GetTypes().Where(x =>
                            !x.IsInterface &&
                            !x.IsGenericType &&
                            !x.IsAbstract &&
                            x.GetInterfaces().Where(i => i.Name == typeof(IServiceQueryResult).Name).Count() > 0
                          ).ToList();

            if (types.Count == 0)
            {
                types.Add(null);
            }

            return types;
        }

        public static IEnumerable GetCapitecSearchServiceModelNames()
        {
            var capitecServiceModelTypes = GetCapitecSearchServiceModelTypes();

            var modelTypes = new List<Type>();
            foreach (Type modelType in capitecServiceModelTypes)
            {
                modelTypes.Add(modelType);
            }

            return modelTypes;
        }
    }
}
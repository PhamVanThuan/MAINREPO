using NUnit.Framework;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.Capitec.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Services.Interfaces.Capitec.Tests.Models
{
    public class ModelProvider
    {
        public static IEnumerable GetCapitecServiceModelTypes()
        {
            var assembly = Assembly.GetAssembly(typeof(GetUserQueryResult));
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

        public static IEnumerable GetCapitecServiceModelNames()
        {
            var capitecServiceModelTypes = GetCapitecServiceModelTypes();

            var modelTypes = new List<Type>();
            foreach (Type modelType in capitecServiceModelTypes)
            {
                modelTypes.Add(modelType);
            }

            return modelTypes;
        }
    }
}
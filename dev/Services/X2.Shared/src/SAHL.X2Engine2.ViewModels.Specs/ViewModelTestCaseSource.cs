using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SAHL.Core.Data;
using SAHL.X2Engine2.ViewModels.SqlStatement;

namespace SAHL.X2Engine2.ViewModels.Specs
{
    public class ViewModelTestCaseSource
    {
        public static IEnumerable<Type> GetSqlStatements()
        {
            var types = GetTypes(Assembly.GetAssembly(typeof(ActivityByActivityNameAndInstanceIdSqlStatement)), typeof(ISqlStatement<>));
            return types;
        }

        private static IList<Type> GetTypes(Assembly assembly, Type interfaceType)
        {
            return assembly.GetTypes().Where(x =>
                                !x.IsInterface &&
                                !x.IsGenericType &&
                                !x.IsAbstract &&
                                x.GetInterfaces().Where(i => i.Name == interfaceType.Name).Count() > 0
                              ).ToList();
        }
    }
}
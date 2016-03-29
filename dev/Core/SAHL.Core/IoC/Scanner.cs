using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Core.IoC
{
    public class Scanner
    {
        private static Scanner instance;
        private static readonly object lockObject = new object();

        public static Scanner Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Scanner();
                    }
                    return instance;
                }
            }
            set
            {
                instance = value;
            }
        }

        public IEnumerable<Type> Scan(IEnumerable<Assembly> assemblies, IEnumerable<ITypeScannerConvention> conventions)
        {
            List<Type> scanResults = new List<Type>();

            //leave this as own variable, incase debugging visually to find assemblies
            foreach (Assembly assembly in assemblies)
            {
                //leave this as variable, in the case of wanting to debug, and check all types expected are there for given assembly
                Type[] types = assembly.GetTypes();

                scanResults.AddRange(types.Where(type => conventions.Any(convention => convention.Process(type))));
            }

            return scanResults;
        }
    }
}
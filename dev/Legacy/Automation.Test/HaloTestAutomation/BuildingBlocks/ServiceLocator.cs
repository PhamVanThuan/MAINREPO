using Automation.DataAccess;
using Automation.DataAccess.DataHelper;
using Automation.DataAccess.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingBlocks
{
    public class ServiceLocator
    {
        private static ServiceLocator instance;
        private static IEnumerable<Type> assemblyTypes;

        public static ServiceLocator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceLocator();
                }
                return instance;
            }
        }

        private IKernel kernel;

        private ServiceLocator()
        {
            kernel = new StandardKernel();
            BindClassesAssignableFrom("BuildingBlocks.Services.Contracts", typeof(ServiceLocator));
            BindClassesAssignableFrom("Automation.DataAccess.DataHelper._2AM.Contracts", typeof(IDataContext));
            BindClassesAssignableFrom("Automation.DataAccess.DataHelper.Capitec", typeof(IDataHelper));
            kernel.Bind<IDataContext>().To<DataContext>().InSingletonScope();

        }
       
        private void BindClassesAssignableFrom(string interfaceNamespace, Type assembly)
        {
            var assemblyTypes = assembly.Assembly.GetTypes();
            IEnumerable<Type> serviceInterfaces = assemblyTypes
                .Where(x => x.Namespace == interfaceNamespace).ToList();
            foreach (var i in serviceInterfaces)
            {
                var type = assemblyTypes.Where(x => x.IsPublic && !x.IsInterface && i.IsAssignableFrom(x)).First();
                kernel.Bind(i).To(type).InSingletonScope();
            }
        }

        public T GetService<T>()
        {
            return kernel.Get<T>();
        }
    }
}
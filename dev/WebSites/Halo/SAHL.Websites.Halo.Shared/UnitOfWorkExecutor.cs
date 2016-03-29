using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;

namespace SAHL.Websites.Halo.Shared
{
    public class UnitOfWorkExecutor : IUnitOfWorkExecutor
    {
        private readonly IPrincipal _currentUser;

        public UnitOfWorkExecutor(IPrincipal currentUser)
        {
            if (currentUser == null) { throw new ArgumentNullException("currentUser"); }
            _currentUser = currentUser;
        }

        public bool Execute<T>() where T : IUnitOfWorkAction
        {
            var actions = this.LoadActions<T>();
            return actions != null && actions.All(currentAction => currentAction.Execute());
        }

        private IEnumerable<T> LoadActions<T>() where T : IUnitOfWorkAction
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            if (assembly == null) { return null; }

            var types = assembly.GetTypes()
                                .Where(type => type.GetInterfaces().Contains(typeof (T)) && !type.IsAbstract)
                                .ToList();
            if (!types.Any()) { return null; }

            var instances = types.Select(currentType =>
                                                {
                                                    var instance         = (T) Activator.CreateInstance(currentType);
                                                    instance.CurrentUser = _currentUser;

                                                    return instance;
                                                })
                                 .OrderBy(InstanceCreationEditor => InstanceCreationEditor.Sequence);
            return instances;
        }
    }
}

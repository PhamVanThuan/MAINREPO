using System;
using System.Linq;
using System.Collections.Generic;

using StructureMap;
using StructureMap.Pipeline;

using SAHL.Core;

namespace SAHL.Config.Core
{
    public class StructureMapIocContainer : IIocContainer
    {
        private bool disposing;

        private IContainer Container { get { return ObjectFactory.Container; } }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return this.Container.GetAllInstances(type).Cast<object>();
        }

        public void Inject<T>(T objectToInject)
        {
            this.Container.Inject(typeof(T), objectToInject);
        }

        public IEnumerable<T> GetAllInstances<T>()
        {
            return this.Container.GetAllInstances<T>();
        }

        public object GetInstance(Type type)
        {
            return this.Container.TryGetInstance(type);
        }

        public object GetInstance(Type type, string namedInstance)
        {
            return this.Container.TryGetInstance(type, namedInstance);
        }

        public T GetInstance<T>()
        {
            return this.Container.TryGetInstance<T>();
        }

        public T GetInstance<T, T1>(T1 objectToStartWith)
        {
            var args = new ExplicitArguments();
            args.Set(objectToStartWith);
            return this.Container.GetInstance<T>(args);
        }

        public T GetInstance<T>(string namedInstance)
        {
            return this.Container.TryGetInstance<T>(namedInstance);
        }

        public object GetConcreteInstance(Type concreteType)
        {
            return this.Container.GetInstance(concreteType);
        }
    }
}
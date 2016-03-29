using System;
using System.Collections.Generic;

namespace SAHL.Core
{
    public interface IIocContainer
    {
        T GetInstance<T>();

        T GetInstance<T>(string namedInstance);

        object GetInstance(Type type);

        object GetConcreteInstance(Type concreteType);

        object GetInstance(Type type, string namedInstance);

        IEnumerable<T> GetAllInstances<T>();

        IEnumerable<object> GetAllInstances(Type type);

        void Inject<T>(T objectToInject);

        T GetInstance<T, T1>(T1 objectToStartWith);
    }
}
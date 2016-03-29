using System;
using SAHL.Common.Attributes;

namespace SAHL.Common.Factories
{
    public class FactoryCacheItem
    {
        string _assemblyFullName;
        Type _factoryItemType;
        Type _factoryItemInstanceType;
        FactoryTypeLifeTime _lifeTime;
        object _singletonInstance;

        public FactoryCacheItem(string AssemblyName, Type FactoryType, Type FactoryInstanceType, FactoryTypeLifeTime LifeTime)
        {
            _assemblyFullName = AssemblyName;
            _factoryItemType = FactoryType;
            _lifeTime = LifeTime;
            _factoryItemInstanceType = FactoryInstanceType;
        }

        public string AssemblyFullName
        {
            get { return _assemblyFullName; }
        }

        public Type FactoryItemType
        {
            get { return _factoryItemType; }
        }

        public Type FactoryItemInstanceType
        {
            get { return _factoryItemInstanceType; }
        }

        public FactoryTypeLifeTime LifeTime
        {
            get { return _lifeTime; }
        }

        public object SingletonInstance
        {
            get { return _singletonInstance; }
            set { _singletonInstance = value; }
        }
    }
}
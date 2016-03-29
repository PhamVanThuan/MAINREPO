using System;

namespace SAHL.Common.Attributes
{
    public enum FactoryTypeLifeTime
    {
        Transient,
        Singleton
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class FactoryTypeAttribute : Attribute
    {
        protected Type _FactoryItemType;
        protected FactoryTypeLifeTime _LifeTime;

        public FactoryTypeAttribute(Type FactoryType)
        {
            _FactoryItemType = FactoryType;
            _LifeTime = LifeTime;
        }

        public Type FactoryType
        {
            get { return _FactoryItemType; }
        }

        public FactoryTypeLifeTime LifeTime
        {
            get { return _LifeTime; }
            set { _LifeTime = value; }
        }
    }
}
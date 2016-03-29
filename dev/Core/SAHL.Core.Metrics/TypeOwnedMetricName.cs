using System;

namespace SAHL.Core.Metrics
{
    public class TypeOwnedMetricName<T> : IMetricName, ITypedMetricName
    {
        public TypeOwnedMetricName(string name)
        {
            this.Name = name;
        }

        public Type Owner { get { return typeof(T); } }

        public string Name { get; protected set; }

        public string Value
        {
            get { return string.Format("{0}_{1}", Owner, Name); }
        }

        public override bool Equals(object obj)
        {
            return !object.ReferenceEquals(null, obj) && obj is TypeOwnedMetricName<T> && object.Equals(((TypeOwnedMetricName<T>)obj).Value, this.Value);
        }

        public override int GetHashCode()
        {
            return (this.Value.GetHashCode());
        }
    }
}
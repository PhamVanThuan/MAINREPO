using Newtonsoft.Json;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.BusinessModel
{
    public class BusinessKey
    {
        public BusinessKey()
        {
        }

        [JsonConstructor]
        public BusinessKey(long key, GenericKeyType keyType)
        {
            this.Key     = key;
            this.KeyType = keyType;
        }

        public GenericKeyType KeyType { get; set; }

        public long Key { get; set; }

        public bool Equals(BusinessKey other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }
            return this.Key == other.Key
                && this.KeyType == other.KeyType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((BusinessKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Key.GetHashCode();
                hashCode = (hashCode * 397) ^ KeyType.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(BusinessKey left, BusinessKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BusinessKey left, BusinessKey right)
        {
            return !Equals(left, right);
        }
    }
}

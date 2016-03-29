using Newtonsoft.Json;
using System.Diagnostics;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.BusinessModel
{
    public class BusinessContext
    {
        [JsonConstructor]
        public BusinessContext(string context, GenericKeyType keyType, long businesKeyValue)
        {
            this.Context     = context;
            this.BusinessKey = new BusinessKey(businesKeyValue, keyType);
        }

        public BusinessContext(string context, BusinessKey businessKey)
            : this(context, businessKey.KeyType, businessKey.Key)
        {
        }

        public string Context { get; protected set; }

        public BusinessKey BusinessKey { get; protected set; }

        public bool Equals(BusinessContext other)
        {
            if (ReferenceEquals(null, other)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }
            return this.BusinessKey.Equals(other.BusinessKey)
                && this.Context.Equals(other.Context,System.StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals((BusinessContext)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.BusinessKey.GetHashCode();
                hashCode = (hashCode * 397) ^ Context.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(BusinessContext left, BusinessContext right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BusinessContext left, BusinessContext right)
        {
            return !Equals(left, right);
        }
    }
}

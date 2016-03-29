using System;
using Newtonsoft.Json;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.UI.Context
{
    public class TileBusinessContext : BusinessContext
    {
        public TileBusinessContext(BusinessContext businessContext, Type tileModelType, Type tileConfigurationType)
            : this(businessContext.Context, businessContext.BusinessKey.KeyType, businessContext.BusinessKey.Key, tileModelType, tileConfigurationType)
        {
        }

        [JsonConstructor]
        public TileBusinessContext(string context, BusinessKey businessKey, Type tileModelType, Type tileConfigurationType)
            : this(context, businessKey.KeyType, businessKey.Key, tileModelType, tileConfigurationType)
        {
        }

        public TileBusinessContext(string context, GenericKeyType keyType, long businesKeyValue, Type tileModelType, Type tileConfigurationType)
            : base(context, keyType, businesKeyValue)
        {
            this.TileModelType = tileModelType;
            this.TileConfigurationType = tileConfigurationType;
        }

        public Type TileModelType { get; protected set; }

        public Type TileConfigurationType { get; protected set; }

        public bool Equals(TileBusinessContext other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return base.Equals(other)
                   && this.TileModelType == other.TileModelType
                   && this.TileConfigurationType == other.TileConfigurationType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((TileBusinessContext) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                if (this.TileModelType != null)
                {
                    hashCode = (hashCode * 397) ^ this.TileModelType.GetHashCode();
                }
                if (this.TileConfigurationType != null)
                {
                    hashCode = (hashCode * 397) ^ this.TileConfigurationType.GetHashCode();
                }
                return hashCode;
            }
        }

        public static bool operator ==(TileBusinessContext left, TileBusinessContext right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TileBusinessContext left, TileBusinessContext right)
        {
            return !Equals(left, right);
        }
    }
}
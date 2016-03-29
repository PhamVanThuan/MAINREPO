using System;
using Newtonsoft.Json;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;

namespace SAHL.Core.UI.Context
{
    public class EditorBusinessContext : BusinessContext
    {
        public EditorBusinessContext(BusinessContext businessContext, Type editorType, Type editorModelConfigurationType)
            : this(businessContext.Context, businessContext.BusinessKey.KeyType, businessContext.BusinessKey.Key, editorType, editorModelConfigurationType)
        {
        }

        [JsonConstructor]
        public EditorBusinessContext(string context, BusinessKey businessKey, Type editorType, Type editorModelConfigurationType)
            : this(context, businessKey.KeyType, businessKey.Key, editorType, editorModelConfigurationType)
        {
        }

        public EditorBusinessContext(string context, GenericKeyType keyType, long businesKeyValue, Type editorType, Type editorModelConfigurationType)
            : base(context, keyType, businesKeyValue)
        {
            this.EditorType = editorType;
            this.EditorModelConfigurationType = editorModelConfigurationType;
        }

        public Type EditorType { get; protected set; }

        public Type EditorModelConfigurationType { get; protected set; }

        public bool Equals(EditorBusinessContext other)
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
                   && this.EditorType == other.EditorType
                   && this.EditorModelConfigurationType == other.EditorModelConfigurationType;
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
            return Equals((EditorBusinessContext) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                if (this.EditorType != null)
                {
                    hashCode = (hashCode * 397) ^ this.EditorType.GetHashCode();
                    hashCode = (hashCode * 397) ^ this.EditorModelConfigurationType.GetHashCode();
                }
                return hashCode;
            }
        }

        public static bool operator ==(EditorBusinessContext left, EditorBusinessContext right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EditorBusinessContext left, EditorBusinessContext right)
        {
            return !Equals(left, right);
        }
    }
}
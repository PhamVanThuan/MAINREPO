using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Shared.DataAccess
{
	[Serializable]
	public abstract class ModelBase : MarshalByRefObject, IEquatable<ModelBase>
	{
		protected ModelBase()
		{
		}

		public virtual int Id { get; protected set; }

		public virtual bool Equals(ModelBase obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			if (ReferenceEquals(this, obj))
			{
				return true;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}
			return obj.Id == Id;
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
			if (GetType() != obj.GetType())
			{
				return false;
			}
			return Equals((ModelBase)obj);
		}

		public override int GetHashCode()
		{
			return (Id.GetHashCode() * 397) ^ GetType().GetHashCode();
		}

		public static bool operator ==(ModelBase left, ModelBase right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ModelBase left, ModelBase right)
		{
			return !Equals(left, right);
		}
	}
}

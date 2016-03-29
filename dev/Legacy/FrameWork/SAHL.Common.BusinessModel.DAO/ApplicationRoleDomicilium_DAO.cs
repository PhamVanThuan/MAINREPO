using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
	[GenericTest]
	[ActiveRecord("OfferRoleDomicilium", Schema = "dbo", Lazy = true)]
	public class ApplicationRoleDomicilium_DAO : DB_2AM<ApplicationRoleDomicilium_DAO>
	{
		private int _key;
		private LegalEntityDomicilium_DAO _legalEntityDomicilium;
		private ApplicationRole_DAO _applicationRole;
		private DateTime? _changeDate;
		private ADUser_DAO _adUserDAO;

		[PrimaryKey(PrimaryKeyType.Native, "OfferRoleDomiciliumKey", ColumnType = "Int32")]
		public virtual int Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		[BelongsTo("LegalEntityDomiciliumKey", NotNull = true)]
		public virtual LegalEntityDomicilium_DAO LegalEntityDomicilium
		{
			get
			{
				return this._legalEntityDomicilium;
			}
			set
			{
				this._legalEntityDomicilium = value;
			}
		}

		[BelongsTo("OfferRoleKey", NotNull = true)]
		public virtual ApplicationRole_DAO ApplicationRole
		{
			get
			{
				return this._applicationRole;
			}
			set
			{
				this._applicationRole = value;
			}
		}

		[BelongsTo("ADUserKey", NotNull = true)]
		public virtual ADUser_DAO ADUser
		{
			get
			{
				return this._adUserDAO;
			}
			set
			{
				this._adUserDAO = value;
			}
		}

		[Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
		public virtual System.DateTime? ChangeDate
		{
			get
			{
				return this._changeDate;
			}
			set
			{
				this._changeDate = value;
			}
		}
	}
}

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
	[ActiveRecord("LegalEntityDomicilium", Schema = "dbo", Lazy = true)]
	public class LegalEntityDomicilium_DAO : DB_2AM<LegalEntityDomicilium_DAO>
	{
		private int _key;
		private LegalEntityAddress_DAO _legalEntityAddressDAO;
		private GeneralStatus_DAO _generalStatusDAO;
		private DateTime? _changeDate;
		private ADUser_DAO _adUserDAO;

		[PrimaryKey(PrimaryKeyType.Native, "LegalEntityDomiciliumKey", ColumnType = "Int32")]
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

		[BelongsTo("LegalEntityAddressKey", NotNull = true)]
		public virtual LegalEntityAddress_DAO LegalEntityAddress
		{
			get
			{
				return this._legalEntityAddressDAO;
			}
			set
			{
				this._legalEntityAddressDAO = value;
			}
		}

		[BelongsTo("GeneralStatusKey", NotNull = true)]
		public virtual GeneralStatus_DAO GeneralStatus
		{
			get
			{
				return this._generalStatusDAO;
			}
			set
			{
				this._generalStatusDAO = value;
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
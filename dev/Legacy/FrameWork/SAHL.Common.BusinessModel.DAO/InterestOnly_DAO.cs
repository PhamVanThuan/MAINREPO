using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
	[ActiveRecord("InterestOnly", Schema = "product", Lazy = true)]
	public partial class InterestOnly_DAO : DB_2AM<InterestOnly_DAO>
	{
		private int _key;

		private FinancialServiceAttribute_DAO _financialServiceAttribute;

		private System.DateTime _entryDate;

		private System.DateTime _maturityDate;

		[Property("EntryDate", ColumnType = "Timestamp", NotNull = true)]
		public virtual System.DateTime EntryDate
		{
			get
			{
				return this._entryDate;
			}
			set
			{
				this._entryDate = value;
			}
		}

		[Property("MaturityDate", ColumnType = "Timestamp", NotNull = true)]
		public virtual System.DateTime MaturityDate
		{
			get
			{
				return this._maturityDate;
			}
			set
			{
				this._maturityDate = value;
			}
		}

		[PrimaryKey(PrimaryKeyType.Foreign, Column = "FinancialServiceAttributeKey")]
		public virtual int Key
		{
			get { return this._key; }
			set { this._key = value; }
		}

		[OneToOne]
		public virtual FinancialServiceAttribute_DAO FinancialServiceAttribute
		{
			get
			{
				return this._financialServiceAttribute;
			}
			set
			{
				this._financialServiceAttribute = value;
			}
		}
	}
}
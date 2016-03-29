using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("HelpDeskQuery", Schema = "dbo")]
    public partial class HelpDeskQuery_DAO : DB_2AM<HelpDeskQuery_DAO>
    {
        private string _description;

        private System.DateTime _insertDate;

        private Memo_DAO _memo;

        private System.DateTime? _resolvedDate;

        private int _key;

        private HelpDeskCategory_DAO _helpDeskCategory;

        [Property("Description", ColumnType = "String", NotNull = true, Length = 80)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [Property("InsertDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Insert Date is a mandatory field")]
        public virtual System.DateTime InsertDate
        {
            get
            {
                return this._insertDate;
            }
            set
            {
                this._insertDate = value;
            }
        }

        [BelongsTo("MemoKey")]
        public virtual Memo_DAO Memo
        {
            get
            {
                return this._memo;
            }
            set
            {
                this._memo = value;
            }
        }

        [Property("ResolvedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ResolvedDate
        {
            get
            {
                return this._resolvedDate;
            }
            set
            {
                this._resolvedDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "HelpDeskQueryKey", ColumnType = "Int32")]
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

        [BelongsTo("HelpDeskCategoryKey", NotNull = true)]
        [ValidateNonEmpty("Help Desk Category is a mandatory field")]
        public virtual HelpDeskCategory_DAO HelpDeskCategory
        {
            get
            {
                return this._helpDeskCategory;
            }
            set
            {
                this._helpDeskCategory = value;
            }
        }
    }
}
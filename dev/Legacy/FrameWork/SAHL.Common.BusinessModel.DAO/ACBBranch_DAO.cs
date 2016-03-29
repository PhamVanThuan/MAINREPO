using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ACBBranch", Schema = "dbo")]
    public partial class ACBBranch_DAO : DB_2AM<ACBBranch_DAO>
    {
        private ACBBank_DAO _aCBBank;

        private string _aCBBranchDescription;

        private char _activeIndicator;

        private string _key;

        /// <summary>
        /// The primary key from the ACBBank table to which the branch belongs.
        /// </summary>
        [BelongsTo(Column = "ACBBankCode", NotNull = true)]
        [ValidateNonEmpty("ACB Bank is a mandatory field")]
        public virtual ACBBank_DAO ACBBank
        {
            get
            {
                return this._aCBBank;
            }
            set
            {
                this._aCBBank = value;
            }
        }

        /// <summary>
        /// The description of the branch. e.g. Durban North
        /// </summary>
        [Property("ACBBranchDescription", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("ACB Branch Description is a mandatory field")]
        public virtual string ACBBranchDescription
        {
            get
            {
                return this._aCBBranchDescription;
            }
            set
            {
                this._aCBBranchDescription = value;
            }
        }

        /// <summary>
        /// Indicates whether this branch record is active or not.
        /// </summary>
        [Property("ActiveIndicator", ColumnType = "Char", NotNull = true)]
        [ValidateNonEmpty("Active Indicator is a mandatory field")]
        public virtual char ActiveIndicator
        {
            get
            {
                return this._activeIndicator;
            }
            set
            {
                this._activeIndicator = value;
            }
        }

        /// <summary>
        /// The distinct branch code which is allocated to each branch of a bank.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "ACBBranchCode")]//, ColumnType = "String")]
        public virtual string Key
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
    }
}
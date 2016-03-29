using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ACBType", Schema = "dbo")]
    public partial class ACBType_DAO : DB_2AM<ACBType_DAO>
    {
        private string _aCBTypeDescription;

        private int _key;

        /// <summary>
        /// The type of bank account. i.e. A Cheque or Savings account
        /// </summary>
        [Property("ACBTypeDescription", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("ACB Type Description is a mandatory field")]
        public virtual string ACBTypeDescription
        {
            get
            {
                return this._aCBTypeDescription;
            }
            set
            {
                this._aCBTypeDescription = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "ACBTypeNumber", ColumnType = "Int32", UnsavedValue = "-1")]
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
    }
}
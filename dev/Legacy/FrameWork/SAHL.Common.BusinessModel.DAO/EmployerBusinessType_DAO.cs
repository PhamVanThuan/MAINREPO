using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// EmployerBusinessType_DAO is used to hold the different business types which can be applied to an Employer.
    /// </summary>
    [ActiveRecord("EmployerBusinessType", Schema = "dbo")]
    public partial class EmployerBusinessType_DAO : DB_2AM<EmployerBusinessType_DAO>
    {
        private string _description;

        private int _Key;

        /// <summary>
        /// The description of the Employer Business type. e.g. Company/Sole Proprietor
        /// </summary>
        [Property("Description", ColumnType = "String", NotNull = true, Length = 25)]
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "EmployerBusinessTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }
    }
}
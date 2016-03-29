using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("EmploymentVerificationProcess", Schema = "dbo")]
    public partial class EmploymentVerificationProcess_DAO : DB_2AM<EmploymentVerificationProcess_DAO>
    {
        private int _key;
        private Employment_DAO _employment;
        private EmploymentVerificationProcessType_DAO _employmentVerificationProcessType;
        private string _userID;
        private System.DateTime? _changeDate;

        [PrimaryKey(PrimaryKeyType.Native, "EmploymentVerificationProcessKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        [BelongsTo("EmploymentKey", NotNull = true)]
        public virtual Employment_DAO Employment
        {
            get { return this._employment; }
            set { this._employment = value; }
        }

        [BelongsTo("EmploymentVerificationProcessTypeKey", NotNull = true)]
        public virtual EmploymentVerificationProcessType_DAO EmploymentVerificationProcessType
        {
            get { return this._employmentVerificationProcessType; }
            set { this._employmentVerificationProcessType = value; }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        public virtual string UserID
        {
            get { return this._userID; }
            set { this._userID = value; }
        }

        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ChangeDate
        {
            get { return this._changeDate; }
            set { this._changeDate = value; }
        }
    }
}
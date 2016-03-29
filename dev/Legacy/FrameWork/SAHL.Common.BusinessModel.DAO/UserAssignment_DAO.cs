using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserAssignment", Schema = "dbo")]
    public partial class UserAssignment_DAO : DB_2AM<UserAssignment_DAO>
    {
        private int _financialServiceKey;

        private int _originationSourceProductKey;

        private System.DateTime _assignmentDate;

        private string _assigningUser;

        private string _assignedUser;

        private int _Key;

        [Property("FinancialServiceKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Financial Service Key is a mandatory field")]
        public virtual int FinancialServiceKey
        {
            get
            {
                return this._financialServiceKey;
            }
            set
            {
                this._financialServiceKey = value;
            }
        }

        [Property("OriginationSourceProductKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Origination Source Product Key is a mandatory field")]
        public virtual int OriginationSourceProductKey
        {
            get
            {
                return this._originationSourceProductKey;
            }
            set
            {
                this._originationSourceProductKey = value;
            }
        }

        [Property("AssignmentDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Assignment Date is a mandatory field")]
        public virtual System.DateTime AssignmentDate
        {
            get
            {
                return this._assignmentDate;
            }
            set
            {
                this._assignmentDate = value;
            }
        }

        [Property("AssigningUser", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Assigning User is a mandatory field")]
        public virtual string AssigningUser
        {
            get
            {
                return this._assigningUser;
            }
            set
            {
                this._assigningUser = value;
            }
        }

        [Property("AssignedUser", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Assigned User is a mandatory field")]
        public virtual string AssignedUser
        {
            get
            {
                return this._assignedUser;
            }
            set
            {
                this._assignedUser = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "UserAssignmentKey", ColumnType = "Int32")]
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
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserGroupAssignment", Schema = "dbo")]
    public partial class UserGroupAssignment_DAO : DB_2AM<UserGroupAssignment_DAO>
    {

        private int _genericKey;

        private System.DateTime _changeDate;

        private System.DateTime _insertedDate;

        private int _Key;

        private ADUser_DAO _aDUser;
        
        private UserGroupMapping_DAO _userGroupMapping;

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ChangeDate
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

        [Property("InsertedDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime InsertedDate
        {
            get
            {
                return this._insertedDate;
            }
            set
            {
                this._insertedDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "UserGroupAssignmentKey", ColumnType = "Int32")]
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

        [BelongsTo("ADUserKey", NotNull = true)]
        public virtual ADUser_DAO ADUser
        {
            get
            {
                return this._aDUser;
            }
            set
            {
                this._aDUser = value;
            }
        }

      [BelongsTo("UserGroupMappingKey", NotNull = true)]
      public virtual UserGroupMapping_DAO UserGroupMapping
      {
        get
        {
          return this._userGroupMapping;
        }
        set
        {
          this._userGroupMapping = value;
        }
      }
    }
}

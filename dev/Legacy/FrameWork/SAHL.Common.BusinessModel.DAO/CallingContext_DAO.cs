using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("CallingContext", Schema = "dbo")]
    public partial class CallingContext_DAO : DB_2AM<CallingContext_DAO>
    {
        private int _key;
        private CallingContextType_DAO _callingContextType;
        private string _callingProcess;
        private string _callingMethod;
        private string _callingState;

        [PrimaryKey(PrimaryKeyType.Assigned, "CallingContextKey", ColumnType = "Int32")]
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

        [BelongsTo("CallingContextTypeKey", NotNull = true)]
        public virtual CallingContextType_DAO CallingContextType
        {
            get
            {
                return this._callingContextType;
            }
            set
            {
                this._callingContextType = value;
            }
        }

        [Property("CallingProcess", ColumnType = "String")]
        public virtual string CallingProcess
        {
            get
            {
                return this._callingProcess;
            }
            set
            {
                this._callingProcess = value;
            }
        }

        [Property("CallingMethod", ColumnType = "String")]
        public virtual string CallingMethod
        {
            get
            {
                return this._callingMethod;
            }
            set
            {
                this._callingMethod = value;
            }
        }

        [Property("CallingState", ColumnType = "String")]
        public virtual string CallingState
        {
            get
            {
                return this._callingState;
            }
            set
            {
                this._callingState = value;
            }
        }
    }
}
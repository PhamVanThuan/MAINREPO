using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ClientAnswerValue", Schema = "Survey", Lazy = true)]
    public partial class ClientAnswerValue_DAO : DB_2AM<ClientAnswerValue_DAO>
    {
        private int _key;

        private string _value;

        private ClientAnswer_DAO _clientAnswer;

        [PrimaryKey(PrimaryKeyType.Foreign, "ClientAnswerKey")]
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

        [Property("Value", ColumnType = "String", NotNull = true)]
        public virtual string Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        [OneToOne]
        public virtual ClientAnswer_DAO ClientAnswer
        {
            get
            {
                return this._clientAnswer;
            }
            set
            {
                this._clientAnswer = value;
            }
        }
    }
}
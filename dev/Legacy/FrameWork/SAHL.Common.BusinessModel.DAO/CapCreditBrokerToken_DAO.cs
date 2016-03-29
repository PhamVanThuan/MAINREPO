using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CapCreditBrokerToken", Schema = "dbo")]
    public partial class CapCreditBrokerToken_DAO : DB_2AM<CapCreditBrokerToken_DAO>
    {
        private bool _lastAssigned;

        private int _key;

        private Broker_DAO _broker;

        [Property("LastAssigned", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Last Assigned is a mandatory field")]
        public virtual bool LastAssigned
        {
            get
            {
                return this._lastAssigned;
            }
            set
            {
                this._lastAssigned = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CapCreditBrokerKey", ColumnType = "Int32")]
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

        [BelongsTo("BrokerKey", NotNull = true)]
        [ValidateNonEmpty("Broker is a mandatory field")]
        public virtual Broker_DAO Broker
        {
            get
            {
                return this._broker;
            }
            set
            {
                this._broker = value;
            }
        }
    }
}
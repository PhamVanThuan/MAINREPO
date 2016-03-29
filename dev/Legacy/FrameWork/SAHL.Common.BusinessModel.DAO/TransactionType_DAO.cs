using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("TransactionType", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public class TransactionType_DAO : DB_2AM<TransactionType_DAO>
    {
        private int _key;

        private string _description;

        private string _sPVDescription;

        private string _gLAccount;

        private string _gLAccountContra;

        private GeneralStatus_DAO _generalStatus;

        private TransactionType_DAO _reversalTransactionType;

        private IList<TransactionGroup_DAO> _transactionGroups;

        [PrimaryKey(PrimaryKeyType.Assigned, "TransactionTypeKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [Property("SPVDescription", ColumnType = "String")]
        public virtual string SPVDescription
        {
            get
            {
                return this._sPVDescription;
            }
            set
            {
                this._sPVDescription = value;
            }
        }

        [Property("GLAccount", ColumnType = "String")]
        public virtual string GLAccount
        {
            get
            {
                return this._gLAccount;
            }
            set
            {
                this._gLAccount = value;
            }
        }

        [Property("GLAccountContra", ColumnType = "String")]
        public virtual string GLAccountContra
        {
            get
            {
                return this._gLAccountContra;
            }
            set
            {
                this._gLAccountContra = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("ReversalTransactionTypeKey")]
        public virtual TransactionType_DAO ReversalTransactionType
        {
            get
            {
                return this._reversalTransactionType;
            }
            set
            {
                this._reversalTransactionType = value;
            }
        }

        [HasAndBelongsToMany(typeof(TransactionGroup_DAO), Table = "TransactionTypeGroup", Schema = "fin", ColumnKey = "TransactionTypeKey", ColumnRef = "TransactionGroupKey", Lazy = true)]
        public virtual IList<TransactionGroup_DAO> TransactionGroups
        {
            get
            {
                return this._transactionGroups;
            }
            set
            {
                this._transactionGroups = value;
            }
        }
    }
}
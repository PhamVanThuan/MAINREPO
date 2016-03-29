using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("AssetTransfer", Schema = "dbo", Lazy = true)]
    public partial class AssetTransfer_DAO : DB_2AM<AssetTransfer_DAO>
    {
        private string _clientSurname;

        private int _sPVKey;

        private double _loanTotalBondAmount;

        private double _loanCurrentBalance;

        private string _userName;

        private char _transferedYN;

        private int _Key;

        [Property("ClientSurname", ColumnType = "String")]
        public virtual string ClientSurname
        {
            get
            {
                return this._clientSurname;
            }
            set
            {
                this._clientSurname = value;
            }
        }

        [Property("SPVKey", ColumnType = "Int32")]
        public virtual int SPVKey
        {
            get
            {
                return this._sPVKey;
            }
            set
            {
                this._sPVKey = value;
            }
        }

        [Property("LoanTotalBondAmount", ColumnType = "Double")]
        public virtual double LoanTotalBondAmount
        {
            get
            {
                return this._loanTotalBondAmount;
            }
            set
            {
                this._loanTotalBondAmount = value;
            }
        }

        [Property("LoanCurrentBalance", ColumnType = "Double")]
        public virtual double LoanCurrentBalance
        {
            get
            {
                return this._loanCurrentBalance;
            }
            set
            {
                this._loanCurrentBalance = value;
            }
        }

        [Property("UserName", ColumnType = "String")]
        public virtual string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }

        [Property("TransferedYN", ColumnType = "AnsiChar")]
        public virtual char TransferedYN
        {
            get
            {
                return this._transferedYN;
            }
            set
            {
                this._transferedYN = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "AccountKey", ColumnType = "Int32")]
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
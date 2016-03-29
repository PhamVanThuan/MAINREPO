using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Fees", Schema = "dbo")]
    public partial class Fees_DAO : DB_2AM<Fees_DAO>
    {
        private double? _feeNaturalTransferDuty;

        private double? _feeNaturalConveyancing;

        private double? _feeNaturalVAT;

        private double? _feeLegalTransferDuty;

        private double? _feeLegalConveyancing;

        private double? _feeLegalVAT;

        private double? _feeBondStamps;

        private double? _feeBondConveyancing;

        private double? _feeBondVAT;

        private double? _feeAdmin;

        private double? _feeValuation;

        private double? _feeCancelDuty;

        private double? _feeCancelConveyancing;

        private double? _feeCancelVAT;

        private double? _feeFlexiSwitch;

        private double? _feeRCSBondConveyancing;

        private double? _feeRCSBondVAT;

        private double? _feeDeedsOffice;

        private double? _feeRCSBondPreparation;

        private double? _feeBondConveyancing80Pct;

        private double? _feeBondVAT80Pct;

        private double? _feeBondConveyancingNoFICA;

        private double? _feeBondNoFICAVAT;

        private int _key;

        [Property("FeeNaturalTransferDuty", ColumnType = "Double")]
        public virtual double? FeeNaturalTransferDuty
        {
            get
            {
                return this._feeNaturalTransferDuty;
            }
            set
            {
                this._feeNaturalTransferDuty = value;
            }
        }

        [Property("FeeNaturalConveyancing", ColumnType = "Double")]
        public virtual double? FeeNaturalConveyancing
        {
            get
            {
                return this._feeNaturalConveyancing;
            }
            set
            {
                this._feeNaturalConveyancing = value;
            }
        }

        [Property("FeeNaturalVAT", ColumnType = "Double")]
        public virtual double? FeeNaturalVAT
        {
            get
            {
                return this._feeNaturalVAT;
            }
            set
            {
                this._feeNaturalVAT = value;
            }
        }

        [Property("FeeLegalTransferDuty", ColumnType = "Double")]
        public virtual double? FeeLegalTransferDuty
        {
            get
            {
                return this._feeLegalTransferDuty;
            }
            set
            {
                this._feeLegalTransferDuty = value;
            }
        }

        [Property("FeeLegalConveyancing", ColumnType = "Double")]
        public virtual double? FeeLegalConveyancing
        {
            get
            {
                return this._feeLegalConveyancing;
            }
            set
            {
                this._feeLegalConveyancing = value;
            }
        }

        [Property("FeeLegalVAT", ColumnType = "Double")]
        public virtual double? FeeLegalVAT
        {
            get
            {
                return this._feeLegalVAT;
            }
            set
            {
                this._feeLegalVAT = value;
            }
        }

        [Property("FeeBondStamps", ColumnType = "Double")]
        public virtual double? FeeBondStamps
        {
            get
            {
                return this._feeBondStamps;
            }
            set
            {
                this._feeBondStamps = value;
            }
        }

        [Property("FeeBondConveyancing", ColumnType = "Double")]
        public virtual double? FeeBondConveyancing
        {
            get
            {
                return this._feeBondConveyancing;
            }
            set
            {
                this._feeBondConveyancing = value;
            }
        }

        [Property("FeeBondVAT", ColumnType = "Double")]
        public virtual double? FeeBondVAT
        {
            get
            {
                return this._feeBondVAT;
            }
            set
            {
                this._feeBondVAT = value;
            }
        }

        [Property("FeeAdmin", ColumnType = "Double")]
        public virtual double? FeeAdmin
        {
            get
            {
                return this._feeAdmin;
            }
            set
            {
                this._feeAdmin = value;
            }
        }

        [Property("FeeValuation", ColumnType = "Double")]
        public virtual double? FeeValuation
        {
            get
            {
                return this._feeValuation;
            }
            set
            {
                this._feeValuation = value;
            }
        }

        [Property("FeeCancelDuty", ColumnType = "Double")]
        public virtual double? FeeCancelDuty
        {
            get
            {
                return this._feeCancelDuty;
            }
            set
            {
                this._feeCancelDuty = value;
            }
        }

        [Property("FeeCancelConveyancing", ColumnType = "Double")]
        public virtual double? FeeCancelConveyancing
        {
            get
            {
                return this._feeCancelConveyancing;
            }
            set
            {
                this._feeCancelConveyancing = value;
            }
        }

        [Property("FeeCancelVAT", ColumnType = "Double")]
        public virtual double? FeeCancelVAT
        {
            get
            {
                return this._feeCancelVAT;
            }
            set
            {
                this._feeCancelVAT = value;
            }
        }

        [Property("FeeFlexiSwitch", ColumnType = "Double")]
        public virtual double? FeeFlexiSwitch
        {
            get
            {
                return this._feeFlexiSwitch;
            }
            set
            {
                this._feeFlexiSwitch = value;
            }
        }

        [Property("FeeRCSBondConveyancing", ColumnType = "Double")]
        public virtual double? FeeRCSBondConveyancing
        {
            get
            {
                return this._feeRCSBondConveyancing;
            }
            set
            {
                this._feeRCSBondConveyancing = value;
            }
        }

        [Property("FeeRCSBondVAT", ColumnType = "Double")]
        public virtual double? FeeRCSBondVAT
        {
            get
            {
                return this._feeRCSBondVAT;
            }
            set
            {
                this._feeRCSBondVAT = value;
            }
        }

        [Property("FeeDeedsOffice")]
        public virtual double? FeeDeedsOffice
        {
            get
            {
                return this._feeDeedsOffice;
            }
            set
            {
                this._feeDeedsOffice = value;
            }
        }

        [Property("FeeRCSBondPreparation")]
        public virtual double? FeeRCSBondPreparation
        {
            get
            {
                return this._feeRCSBondPreparation;
            }
            set
            {
                this._feeRCSBondPreparation = value;
            }
        }

        [Property("FeeBondConveyancing80Pct", ColumnType = "Double")]
        public virtual double? FeeBondConveyancing80Pct
        {
            get
            {
                return this._feeBondConveyancing80Pct;
            }
            set
            {
                this._feeBondConveyancing80Pct = value;
            }
        }

        [Property("FeeBondVAT80Pct", ColumnType = "Double")]
        public virtual double? FeeBondVAT80Pct
        {
            get
            {
                return this._feeBondVAT80Pct;
            }
            set
            {
                this._feeBondVAT80Pct = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "FeeRange", ColumnType = "Int32")]
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

        [Property("FeeBondConveyancingNoFICA", ColumnType = "Double")]
        public virtual double? FeeBondConveyancingNoFICA
        {
            get
            {
                return this._feeBondConveyancingNoFICA;
            }
            set
            {
                this._feeBondConveyancingNoFICA = value;
            }
        }

        [Property("FeeBondNoFICAVAT", ColumnType = "Double")]
        public virtual double? FeeBondNoFICAVAT
        {
            get
            {
                return this._feeBondNoFICAVAT;
            }
            set
            {
                this._feeBondNoFICAVAT = value;
            }
        }
    
    }
}
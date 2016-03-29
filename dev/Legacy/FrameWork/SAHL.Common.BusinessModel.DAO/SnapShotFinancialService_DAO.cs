using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("SnapShotFinancialService", Schema = "DebtCounselling")]
    public class SnapShotFinancialService_DAO : DB_2AM<SnapShotFinancialService_DAO>
    {
        private int _key;

        private Account_DAO _account;

        private FinancialService_DAO _financialService;

        private FinancialServiceType_DAO _financialServiceType;

        private ResetConfiguration_DAO _resetConfiguration;

        private double _activeMarketRate;

        private Margin_DAO _margin;

        private double _installment;

        private SnapShotAccount_DAO _snapShotAccount;

        private IList<SnapShotFinancialAdjustment_DAO> _snapShotFinancialAdjustments;

        [PrimaryKey(PrimaryKeyType.Native, "SnapShotFinancialServiceKey", ColumnType = "Int32")]
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

        [BelongsTo("AccountKey", NotNull = true)]
        public virtual Account_DAO Account
        {
            get
            {
                return this._account;
            }
            set
            {
                this._account = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = true)]
        public virtual FinancialService_DAO FinancialService
        {
            get
            {
                return this._financialService;
            }
            set
            {
                this._financialService = value;
            }
        }

        [BelongsTo("FinancialServiceTypeKey", NotNull = true)]
        public virtual FinancialServiceType_DAO FinancialServiceType
        {
            get
            {
                return this._financialServiceType;
            }
            set
            {
                this._financialServiceType = value;
            }
        }

        [BelongsTo("ResetConfigurationKey", NotNull = true)]
        public virtual ResetConfiguration_DAO ResetConfiguration
        {
            get
            {
                return this._resetConfiguration;
            }
            set
            {
                this._resetConfiguration = value;
            }
        }

        [Property("ActiveMarketRate", ColumnType = "Double")]
        public virtual double ActiveMarketRate
        {
            get
            {
                return this._activeMarketRate;
            }
            set
            {
                this._activeMarketRate = value;
            }
        }

        [BelongsTo("MarginKey")]
        public virtual Margin_DAO Margin
        {
            get
            {
                return this._margin;
            }
            set
            {
                this._margin = value;
            }
        }

        [Property("Installment", ColumnType = "Double")]
        public virtual double Installment
        {
            get
            {
                return this._installment;
            }
            set
            {
                this._installment = value;
            }
        }

        [HasMany(typeof(SnapShotFinancialAdjustment_DAO), ColumnKey = "SnapShotFinancialServiceKey", Table = "SnapShotFinancialAdjustment")]
        public virtual IList<SnapShotFinancialAdjustment_DAO> SnapShotFinancialAdjustments
        {
            get
            {
                return this._snapShotFinancialAdjustments;
            }
            set
            {
                this._snapShotFinancialAdjustments = value;
            }
        }

        [BelongsTo("SnapShotAccountKey", NotNull = true)]
        public virtual SnapShotAccount_DAO SnapShotAccount
        {
            get
            {
                return this._snapShotAccount;
            }
            set
            {
                this._snapShotAccount = value;
            }
        }
    }
}
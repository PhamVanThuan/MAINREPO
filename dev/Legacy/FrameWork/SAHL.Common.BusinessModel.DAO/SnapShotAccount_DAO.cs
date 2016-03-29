using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("SnapShotAccount", Schema = "DebtCounselling")]
    public class SnapShotAccount_DAO : DB_2AM<SnapShotAccount_DAO>
    {
        private int _key;

        private Account_DAO _account;

        private int _remainingInstallments;

        private Product_DAO _product;

        private Valuation_DAO _valuation;

        private System.DateTime _insertDate;

        private DebtCounselling_DAO _debtCounselling;

        private double _hOCPremium;

        private double _lifePremium;

        private double _monthlyServiceFee;

        private IList<SnapShotFinancialService_DAO> _snapShotFinancialServices;

        [PrimaryKey(PrimaryKeyType.Native, "SnapShotAccountKey", ColumnType = "Int32")]
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

        [Property("RemainingInstallments", ColumnType = "Int32", NotNull = true)]
        public virtual int RemainingInstallments
        {
            get
            {
                return this._remainingInstallments;
            }
            set
            {
                this._remainingInstallments = value;
            }
        }

        [BelongsTo("ProductKey", Insert = false, Update = false)]
        public virtual Product_DAO Product
        {
            get
            {
                return this._product;
            }
            set
            {
                this._product = value;
            }
        }

        [BelongsTo("ValuationKey")]
        public virtual Valuation_DAO Valuation
        {
            get
            {
                return this._valuation;
            }
            set
            {
                this._valuation = value;
            }
        }

        [Property("InsertDate", ColumnType = "Timestamp")]
        public virtual System.DateTime InsertDate
        {
            get
            {
                return this._insertDate;
            }
            set
            {
                this._insertDate = value;
            }
        }

        [BelongsTo("DebtCounsellingKey", NotNull = true)]
        public virtual DebtCounselling_DAO DebtCounselling
        {
            get
            {
                return this._debtCounselling;
            }
            set
            {
                this._debtCounselling = value;
            }
        }

        [Property("HOCPremium", ColumnType = "Double")]
        public virtual double HOCPremium
        {
            get
            {
                return this._hOCPremium;
            }
            set
            {
                this._hOCPremium = value;
            }
        }

        [Property("LifePremium", ColumnType = "Double")]
        public virtual double LifePremium
        {
            get
            {
                return this._lifePremium;
            }
            set
            {
                this._lifePremium = value;
            }
        }

        [Property("MonthlyServiceFee", ColumnType = "Double")]
        public virtual double MonthlyServiceFee
        {
            get
            {
                return this._monthlyServiceFee;
            }
            set
            {
                this._monthlyServiceFee = value;
            }
        }

        [HasMany(typeof(SnapShotFinancialService_DAO), ColumnKey = "SnapShotAccountKey", Schema = "DebtCounselling", Table = "SnapShotFinancialService")]
        public virtual IList<SnapShotFinancialService_DAO> SnapShotFinancialServices
        {
            get
            {
                return this._snapShotFinancialServices;
            }
            set
            {
                this._snapShotFinancialServices = value;
            }
        }
    }
}
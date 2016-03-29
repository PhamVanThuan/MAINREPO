using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Fixed property assets.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "6", Lazy = true)]
    public class AssetLiabilityLiabilityLoan_DAO : AssetLiability_DAO
    {
        private string _financialInstitution;

        private DateTime? _dateRepayable;

        private double _instalmentValue;

        private double _liabilityValue;

        private AssetLiabilitySubType_DAO _loanType;

        [Property("Date", ColumnType = "Timestamp", NotNull = false)]
        public virtual System.DateTime? DateRepayable
        {
            get
            {
                return this._dateRepayable;
            }
            set
            {
                this._dateRepayable = value;
            }
        }

        [Property("CompanyName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Financial Institution is a mandatory field")]
        public virtual string FinancialInstitution
        {
            get
            {
                return this._financialInstitution;
            }
            set
            {
                this._financialInstitution = value;
            }
        }

        [Property("Cost", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Instalment Value is a mandatory field")]
        public virtual double InstalmentValue
        {
            get
            {
                return this._instalmentValue;
            }
            set
            {
                this._instalmentValue = value;
            }
        }

        [Property("LiabilityValue", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Liability Value is a mandatory field")]
        public virtual double LiabilityValue
        {
            get
            {
                return this._liabilityValue;
            }
            set
            {
                this._liabilityValue = value;
            }
        }

        [BelongsTo("AssetLiabilitySubTypeKey", NotNull = true)]
        [ValidateNonEmpty("Loan Type is a mandatory field")]
        public virtual AssetLiabilitySubType_DAO LoanType
        {
            get
            {
                return this._loanType;
            }
            set
            {
                this._loanType = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLiabilityLoan_DAO Find(int id)
        {
            return ActiveRecordBase<AssetLiabilityLiabilityLoan_DAO>.Find(id).As<AssetLiabilityLiabilityLoan_DAO>();
        }

        public new static AssetLiabilityLiabilityLoan_DAO Find(object id)
        {
            return ActiveRecordBase<AssetLiabilityLiabilityLoan_DAO>.Find(id).As<AssetLiabilityLiabilityLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLiabilityLoan_DAO FindFirst()
        {
            return ActiveRecordBase<AssetLiabilityLiabilityLoan_DAO>.FindFirst().As<AssetLiabilityLiabilityLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AssetLiabilityLiabilityLoan_DAO FindOne()
        {
            return ActiveRecordBase<AssetLiabilityLiabilityLoan_DAO>.FindOne().As<AssetLiabilityLiabilityLoan_DAO>();
        }

        #endregion Static Overrides
    }
}
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ApplicationInformationVariFixLoan_DAO is instantiated in order to retrieve those details specific to a VariFix Loan
    /// Application.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferInformationVarifixLoan", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInformationVarifixLoan_DAO : DB_2AM<ApplicationInformationVarifixLoan_DAO>
    {
        private double _fixedPercent;

        private double _fixedInstallment;

        private System.DateTime? _electionDate;

        private int _applicationInformationKey;

        private int _conversionStatus;

        private MarketRate_DAO _marketRate;

        private ApplicationInformation_DAO _applicationInformation;

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "OfferInformationKey")]
        public virtual int Key
        {
            get { return _applicationInformationKey; }
            set { _applicationInformationKey = value; }
        }

        /// <summary>
        /// The Percentage of the Loan that the client wishes to fix.
        /// </summary>
        [Property("FixedPercent", ColumnType = "Double")]
        public virtual double FixedPercent
        {
            get
            {
                return this._fixedPercent;
            }
            set
            {
                this._fixedPercent = value;
            }
        }

        /// <summary>
        /// The Instalment due on the Fixed Portion of the Loan.
        /// </summary>
        [Property("FixedInstallment", ColumnType = "Double")]
        public virtual double FixedInstallment
        {
            get
            {
                return this._fixedInstallment;
            }
            set
            {
                this._fixedInstallment = value;
            }
        }

        /// <summary>
        /// The date which the client elected to take the VariFix product.
        /// </summary>
        [Property("ElectionDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ElectionDate
        {
            get
            {
                return this._electionDate;
            }
            set
            {
                this._electionDate = value;
            }
        }

        /// <summary>
        /// The market rate key.
        /// </summary>
        [BelongsTo("MarketRateKey")]
        public virtual MarketRate_DAO MarketRate
        {
            get
            {
                return this._marketRate;
            }
            set
            {
                this._marketRate = value;
            }
        }

        /// <summary>
        /// The Conversion Status.
        /// </summary>
        [Property("ConversionStatus", ColumnType = "Int32")]
        public virtual int ConversionStatus
        {
            get
            {
                return this._conversionStatus;
            }
            set
            {
                this._conversionStatus = value;
            }
        }

        [OneToOne]
        public virtual ApplicationInformation_DAO ApplicationInformation
        {
            get
            {
                return this._applicationInformation;
            }
            set
            {
                this._applicationInformation = value;
            }
        }

        public virtual void Clone(ApplicationInformationVarifixLoan_DAO VF)
        {
            VF.ElectionDate = this.ElectionDate;
            VF.FixedInstallment = this.FixedInstallment;
            VF.FixedPercent = this.FixedPercent;
            VF.MarketRate = this.MarketRate;
        }
    }
}
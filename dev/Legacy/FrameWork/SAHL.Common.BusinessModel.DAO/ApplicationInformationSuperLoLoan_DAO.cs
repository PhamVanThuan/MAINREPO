using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ApplicationInformationSuperLoLoan_DAO is instantiated in order to retrieve those details specific to a Super Lo
    /// Application.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferInformationSuperLoLoan", Schema = "dbo", Lazy = true)]
    public partial class ApplicationInformationSuperLoLoan_DAO : DB_2AM<ApplicationInformationSuperLoLoan_DAO>
    {
        private System.DateTime? _electionDate;

        private double _pPThresholdYr1;

        private double _pPThresholdYr2;

        private double _pPThresholdYr3;

        private double _pPThresholdYr4;

        private double _pPThresholdYr5;

        private int _status;

        private int _key;

        private ApplicationInformation_DAO _applicationInformation;

        /// <summary>
        /// Primary Key. This is also a foreign key reference to the OfferInformation table.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Foreign, Column = "OfferInformationKey")]
        public virtual int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// The date on which the client elected to take the Super Lo Product option.
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
        /// The Prepayment Threshold for the 1st year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        [Property("PPThresholdYr1")]
        public virtual double PPThresholdYr1
        {
            get
            {
                return this._pPThresholdYr1;
            }
            set
            {
                this._pPThresholdYr1 = value;
            }
        }

        /// <summary>
        /// The Prepayment Threshold for the 2nd year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        [Property("PPThresholdYr2")]
        public virtual double PPThresholdYr2
        {
            get
            {
                return this._pPThresholdYr2;
            }
            set
            {
                this._pPThresholdYr2 = value;
            }
        }

        /// <summary>
        /// The Prepayment Threshold for the 3rd year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        [Property("PPThresholdYr3")]
        public virtual double PPThresholdYr3
        {
            get
            {
                return this._pPThresholdYr3;
            }
            set
            {
                this._pPThresholdYr3 = value;
            }
        }

        /// <summary>
        /// The Prepayment Threshold for the 4th year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        [Property("PPThresholdYr4")]
        public virtual double PPThresholdYr4
        {
            get
            {
                return this._pPThresholdYr4;
            }
            set
            {
                this._pPThresholdYr4 = value;
            }
        }

        /// <summary>
        /// The Prepayment Threshold for the 5th year of the loyalty period. A client is opted out of Super Lo should they
        /// breach this threshold.
        /// </summary>
        [Property("PPThresholdYr5")]
        public virtual double PPThresholdYr5
        {
            get
            {
                return this._pPThresholdYr5;
            }
            set
            {
                this._pPThresholdYr5 = value;
            }
        }

        /// <summary>
        /// The Status.
        /// </summary>
        [Property("Status", ColumnType = "Int32")]
        public virtual int Status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
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

        public virtual void Clone(ApplicationInformationSuperLoLoan_DAO SL)
        {
            SL.ElectionDate = this.ElectionDate;
            SL.PPThresholdYr1 = this.PPThresholdYr1;
            SL.PPThresholdYr2 = this.PPThresholdYr2;
            SL.PPThresholdYr3 = this.PPThresholdYr3;
            SL.PPThresholdYr4 = this.PPThresholdYr4;
            SL.PPThresholdYr5 = this.PPThresholdYr5;
        }
    }
}
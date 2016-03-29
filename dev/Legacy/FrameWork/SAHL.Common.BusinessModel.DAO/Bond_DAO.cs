using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Bond", Schema = "dbo", Lazy = true)]
    public partial class Bond_DAO : DB_2AM<Bond_DAO>
    {
        private string _bondRegistrationNumber;

        private System.DateTime _bondRegistrationDate;

        private double _bondRegistrationAmount;

        private double _bondLoanAgreementAmount;

        private string _userID;

        private System.DateTime _changeDate;

        private int _key;

        // this is mapped as a many to many relationship to MortgageLoan.
        //private IList<BondMortgageLoan> _bondMortgageLoans;

        private IList<MortgageLoan_DAO> _mortgageLoans;

        private IList<LoanAgreement_DAO> _loanAgreements;

        private Attorney_DAO _attorney;

        private DeedsOffice_DAO _deedsOffice;

        private Application_DAO _application;

        [Property("BondRegistrationNumber", ColumnType = "String", Length = 15)]
        public virtual string BondRegistrationNumber
        {
            get
            {
                return this._bondRegistrationNumber;
            }
            set
            {
                this._bondRegistrationNumber = value;
            }
        }

        [Lurker]
        [Property("BondRegistrationDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Bond Registration Date is a mandatory field")]
        public virtual System.DateTime BondRegistrationDate
        {
            get
            {
                return this._bondRegistrationDate;
            }
            set
            {
                this._bondRegistrationDate = value;
            }
        }

        [Property("BondRegistrationAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Bond Registration Amount is a mandatory field")]
        public virtual double BondRegistrationAmount
        {
            get
            {
                return this._bondRegistrationAmount;
            }
            set
            {
                this._bondRegistrationAmount = value;
            }
        }

        [Property("BondLoanAgreementAmount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Bond Loan Agreement Amount is a mandatory field")]
        public virtual double BondLoanAgreementAmount
        {
            get
            {
                return this._bondLoanAgreementAmount;
            }
            set
            {
                this._bondLoanAgreementAmount = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "BondKey", ColumnType = "Int32")]
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

        // this is mapped as a many to many relationship to MortgageLoan.
        //[HasMany(typeof(BondMortgageLoan), ColumnKey = "BondKey", Table = "BondMortgageLoan")]
        //public virtual IList<BondMortgageLoan> BondMortgageLoans
        //{
        //    get
        //    {
        //        return this._bondMortgageLoans;
        //    }
        //    set
        //    {
        //        this._bondMortgageLoans = value;
        //    }
        //}

        [HasMany(typeof(LoanAgreement_DAO), ColumnKey = "BondKey", Table = "LoanAgreement", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<LoanAgreement_DAO> LoanAgreements
        {
            get
            {
                return this._loanAgreements;
            }
            set
            {
                this._loanAgreements = value;
            }
        }

        [BelongsTo("AttorneyKey", NotNull = true)]
        [ValidateNonEmpty("Attorney is a mandatory field")]
        public virtual Attorney_DAO Attorney
        {
            get
            {
                return this._attorney;
            }
            set
            {
                this._attorney = value;
            }
        }

        [BelongsTo("DeedsOfficeKey", NotNull = true)]
        [ValidateNonEmpty("Deeds Office is a mandatory field")]
        public virtual DeedsOffice_DAO DeedsOffice
        {
            get
            {
                return this._deedsOffice;
            }
            set
            {
                this._deedsOffice = value;
            }
        }

        [BelongsTo("OfferKey", NotNull = false)]
        public virtual Application_DAO Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
            }
        }

        [HasAndBelongsToMany(typeof(MortgageLoan_DAO), Table = "BondMortgageLoan", ColumnKey = "BondKey", ColumnRef = "FinancialServiceKey", Lazy = true)]
        public virtual IList<MortgageLoan_DAO> MortgageLoans
        {
            get
            {
                return _mortgageLoans;
            }
            set
            {
                _mortgageLoans = value;
            }
        }
    }
}
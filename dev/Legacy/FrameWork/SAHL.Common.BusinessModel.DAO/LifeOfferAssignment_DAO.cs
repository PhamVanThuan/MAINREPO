using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LifeOfferAssignment", Schema = "dbo", Lazy = true)]
    public partial class LifeOfferAssignment_DAO : DB_2AM<LifeOfferAssignment_DAO>
    {
        private int _key;
        private int _offerKey;
        private int _loanAccountKey;
        private int _loanOfferKey;
        private int _loanOfferTypeKey;
        private DateTime _dateAssigned;
        private string _adUserName;

        [PrimaryKey(PrimaryKeyType.Native, "LifeOfferAssignmentKey", ColumnType = "Int32")]
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

        [Property("OfferKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("OfferKey is a mandatory field")]
        public virtual int OfferKey
        {
            get
            {
                return this._offerKey;
            }
            set
            {
                this._offerKey = value;
            }
        }

        [Property("LoanAccountKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("LoanAccountKey is a mandatory field")]
        public virtual int LoanAccountKey
        {
            get
            {
                return this._loanAccountKey;
            }
            set
            {
                this._loanAccountKey = value;
            }
        }

        [Property("LoanOfferKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("LoanOfferKey is a mandatory field")]
        public virtual int LoanOfferKey
        {
            get
            {
                return this._loanOfferKey;
            }
            set
            {
                this._loanOfferKey = value;
            }
        }

        [Property("LoanOfferTypeKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("LoanOfferTypeKey is a mandatory field")]
        public virtual int LoanOfferTypeKey
        {
            get
            {
                return this._loanOfferTypeKey;
            }
            set
            {
                this._loanOfferTypeKey = value;
            }
        }

        /// <summary>
        /// The date when the life offer was assigned
        /// </summary>
        [Property("DateAssigned", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("DateAssigned is a mandatory field")]
        public virtual System.DateTime DateAssigned
        {
            get
            {
                return this._dateAssigned;
            }
            set
            {
                this._dateAssigned = value;
            }
        }

        [Property("ADUserName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("ADUserName is a mandatory field")]
        public virtual string ADUserName
        {
            get
            {
                return this._adUserName;
            }
            set
            {
                this._adUserName = value;
            }
        }
    }
}
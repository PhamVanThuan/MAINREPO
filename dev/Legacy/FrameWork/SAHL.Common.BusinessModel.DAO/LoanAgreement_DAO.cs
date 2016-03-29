using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LoanAgreement", Schema = "dbo")]
    public partial class LoanAgreement_DAO : DB_2AM<LoanAgreement_DAO>
    {
        private System.DateTime _agreementDate;

        private double _amount;

        private string _userName;

        private System.DateTime _changeDate;

        private int _Key;

        private Bond_DAO _bond;

        [Lurker]
        [Property("AgreementDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Agreement Date is a mandatory field")]
        public virtual System.DateTime AgreementDate
        {
            get
            {
                return this._agreementDate;
            }
            set
            {
                this._agreementDate = value;
            }
        }

        [Lurker]
        [Property("Amount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Amount is a mandatory field")]
        public virtual double Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        [Lurker]
        [Property("UserName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User Name is a mandatory field")]
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

        [Lurker]
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

        [Lurker]
        [PrimaryKey(PrimaryKeyType.Native, "LoanAgreementKey", ColumnType = "Int32")]
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

        [Lurker]
        [BelongsTo("BondKey", NotNull = true)]
        [ValidateNonEmpty("Bond is a mandatory field")]
        public virtual Bond_DAO Bond
        {
            get { return this._bond; }
            set { this._bond = value; }
        }

        //[Lurker]
        //[BelongsTo("FinancialServiceKey", NotNull = true)]
        //public virtual MortgageLoan_DAO MortgageLoan
        //{
        //    get
        //    {
        //        return this._mortgageLoan;
        //    }
        //    set
        //    {
        //        this._mortgageLoan = value;
        //    }
        //}

        public override void SaveAndFlush()
        {
            if (this.Key != 0)
                throw new Exception("The Loan Agreement may not be updated.");

            base.SaveAndFlush();
        }

        //public override void Delete()
        //{
        //    throw new Exception("The Loan Agreement may not be deleted.");
        //}
    }
}
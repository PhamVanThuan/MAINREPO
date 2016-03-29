using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ResetConfiguration", Schema = "dbo", Lazy = true)]
    public partial class ResetConfiguration_DAO : DB_2AM<ResetConfiguration_DAO>
    {
        private string _intervalType;

        private int _intervalDuration;

        private System.DateTime _resetDate;

        private System.DateTime _actionDate;

        private char _businessDayIndicator;

        private string _description;

        private int _key;

        private IList<FinancialServiceType_DAO> _financialServiceTypes;

        //private IList<ApplicationMortgageLoan_DAO> _applicationMortgageLoans;

        private IList<OriginationSourceProductConfiguration_DAO> _originationSourceProductConfigurations;

        // private IList<Trade_DAO> _trades;

        [Property("IntervalType", NotNull = true, Length = 2)]
        [ValidateNonEmpty("Interval Type is a mandatory field")]
        public virtual string IntervalType
        {
            get
            {
                return this._intervalType;
            }
            set
            {
                this._intervalType = value;
            }
        }

        [Property("IntervalDuration", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Interval Duration is a mandatory field")]
        public virtual int IntervalDuration
        {
            get
            {
                return this._intervalDuration;
            }
            set
            {
                this._intervalDuration = value;
            }
        }

        [Property("ResetDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Reset Date is a mandatory field")]
        public virtual System.DateTime ResetDate
        {
            get
            {
                return this._resetDate;
            }
            set
            {
                this._resetDate = value;
            }
        }

        [Property("ActionDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Action Date is a mandatory field")]
        public virtual System.DateTime ActionDate
        {
            get
            {
                return this._actionDate;
            }
            set
            {
                this._actionDate = value;
            }
        }

        [Property("BusinessDayIndicator", NotNull = true)]
        [ValidateNonEmpty("Business Day Indication is a mandatory field")]
        public virtual char BusinessDayIndicator
        {
            get
            {
                return this._businessDayIndicator;
            }
            set
            {
                this._businessDayIndicator = value;
            }
        }

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ResetConfigurationKey", ColumnType = "Int32")]
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

        [HasMany(typeof(FinancialServiceType_DAO), ColumnKey = "ResetConfigurationKey", Table = "FinancialServiceType", Lazy = true)]
        public virtual IList<FinancialServiceType_DAO> FinancialServiceTypes
        {
            get
            {
                return this._financialServiceTypes;
            }
            set
            {
                this._financialServiceTypes = value;
            }
        }

        // this is a lookup, can't expose these here.
        //[HasMany(typeof(ApplicationMortgageLoan_DAO), ColumnKey = "ResetConfigurationKey", Table = "OfferMortgageLoan", Lazy = true)]
        //public virtual IList<ApplicationMortgageLoan_DAO> ApplicationMortgageLoans
        //{
        //    get
        //    {
        //        return this._applicationMortgageLoans;
        //    }
        //    set
        //    {
        //        this._applicationMortgageLoans = value;
        //    }
        //}

        // TODO: OriginationSourceProductConfigurations property
        [HasMany(typeof(OriginationSourceProductConfiguration_DAO), ColumnKey = "ResetConfigurationKey", Table = "OriginationSourceProductConfiguration", Lazy = true)]
        public virtual IList<OriginationSourceProductConfiguration_DAO> OriginationSourceProductConfigurations
        {
            get
            {
                return this._originationSourceProductConfigurations;
            }
            set
            {
                this._originationSourceProductConfigurations = value;
            }
        }

        // Trade is not being mapped
        //[HasMany(typeof(Trade_DAO), ColumnKey = "ResetConfigurationKey", Table = "Trade", Lazy = true)]
        //public virtual IList<Trade_DAO> Trades
        //{
        //    get
        //    {
        //        return this._trades;
        //    }
        //    set
        //    {
        //        this._trades = value;
        //    }
        //}
    }
}
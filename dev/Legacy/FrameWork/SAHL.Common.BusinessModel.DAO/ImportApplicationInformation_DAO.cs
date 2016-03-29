using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportOfferInformation", Schema = "dbo")]
    public partial class ImportApplicationInformation_DAO : DB_2AM<ImportApplicationInformation_DAO>
    {

        private int _applicationTerm;

        private double _cashDeposit;

        private double _propertyValuation;

        private double _feesTotal;

        private double _interimInterest;

        private double _monthlyInstalment;

        private double _hOCPremium;

        private double _lifePremium;

        private double _preApprovedAmount;

        private double _maxCashAllowed;

        private double _maxQuickCashAllowed;

        private double _requestedQuickCashAmount;

        private double _bondToRegister;

        private double _lTV;

        private double _pTI;

        private int _key;

        private ImportApplication_DAO _importApplication;

        [Property("OfferTerm", ColumnType = "Int32")]
        public virtual int ApplicationTerm
        {
            get
            {
                return this._applicationTerm;
            }
            set
            {
                this._applicationTerm = value;
            }
        }

        [Property("CashDeposit", ColumnType = "Double")]
        public virtual double CashDeposit
        {
            get
            {
                return this._cashDeposit;
            }
            set
            {
                this._cashDeposit = value;
            }
        }

        [Property("PropertyValuation", ColumnType = "Double")]
        public virtual double PropertyValuation
        {
            get
            {
                return this._propertyValuation;
            }
            set
            {
                this._propertyValuation = value;
            }
        }

        [Property("FeesTotal", ColumnType = "Double")]
        public virtual double FeesTotal
        {
            get
            {
                return this._feesTotal;
            }
            set
            {
                this._feesTotal = value;
            }
        }

        [Property("InterimInterest", ColumnType = "Double")]
        public virtual double InterimInterest
        {
            get
            {
                return this._interimInterest;
            }
            set
            {
                this._interimInterest = value;
            }
        }

        [Property("MonthlyInstalment", ColumnType = "Double")]
        public virtual double MonthlyInstalment
        {
            get
            {
                return this._monthlyInstalment;
            }
            set
            {
                this._monthlyInstalment = value;
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

        [Property("PreApprovedAmount", ColumnType = "Double")]
        public virtual double PreApprovedAmount
        {
            get
            {
                return this._preApprovedAmount;
            }
            set
            {
                this._preApprovedAmount = value;
            }
        }

        [Property("MaxCashAllowed", ColumnType = "Double")]
        public virtual double MaxCashAllowed
        {
            get
            {
                return this._maxCashAllowed;
            }
            set
            {
                this._maxCashAllowed = value;
            }
        }

        [Property("MaxQuickCashAllowed", ColumnType = "Double")]
        public virtual double MaxQuickCashAllowed
        {
            get
            {
                return this._maxQuickCashAllowed;
            }
            set
            {
                this._maxQuickCashAllowed = value;
            }
        }

        [Property("RequestedQuickCashAmount", ColumnType = "Double")]
        public virtual double RequestedQuickCashAmount
        {
            get
            {
                return this._requestedQuickCashAmount;
            }
            set
            {
                this._requestedQuickCashAmount = value;
            }
        }

        [Property("BondToRegister", ColumnType = "Double")]
        public virtual double BondToRegister
        {
            get
            {
                return this._bondToRegister;
            }
            set
            {
                this._bondToRegister = value;
            }
        }

        [Property("LTV", ColumnType = "Double")]
        public virtual double LTV
        {
            get
            {
                return this._lTV;
            }
            set
            {
                this._lTV = value;
            }
        }

        [Property("PTI", ColumnType = "Double")]
        public virtual double PTI
        {
            get
            {
                return this._pTI;
            }
            set
            {
                this._pTI = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferInformationKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferKey", NotNull =  true)]
        public virtual ImportApplication_DAO ImportApplication
        {
            get
            {
                return this._importApplication;
            }
            set
            {
                this._importApplication = value;
            }
        }
    }
}

using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("SANTAMPolicyTracking", Schema = "dbo")]
    public partial class SANTAMPolicyTracking_DAO : DB_2AM<SANTAMPolicyTracking_DAO>
    {
        private int _sANTAMPolicyTrackingKey;

        private string _policyNumber;

        private string _quoteNumber;

        private int _campaignTargetContactKey;

        private int _legalEntityKey;

        private int _accountKey;

        private System.DateTime _activeDate;

        private System.DateTime _canceldate;

        private double _monthlyPremium;

        private int _collectionDay;

        private int _key;

        private SANTAMPolicyStatus_DAO _sANTAMPolicyStatus;

        [Property("SANTAMPolicyTrackingKey", ColumnType = "Int32", NotNull = true)]
        public virtual int SANTAMPolicyTrackingKey
        {
            get
            {
                return this._sANTAMPolicyTrackingKey;
            }
            set
            {
                this._sANTAMPolicyTrackingKey = value;
            }
        }

        [Property("PolicyNumber", ColumnType = "String", NotNull = true)]
        public virtual string PolicyNumber
        {
            get
            {
                return this._policyNumber;
            }
            set
            {
                this._policyNumber = value;
            }
        }

        [Property("QuoteNumber", ColumnType = "String", NotNull = true)]
        public virtual string QuoteNumber
        {
            get
            {
                return this._quoteNumber;
            }
            set
            {
                this._quoteNumber = value;
            }
        }

        [Property("CampaignTargetContactKey", ColumnType = "Int32", NotNull = true)]
        public virtual int CampaignTargetContactKey
        {
            get
            {
                return this._campaignTargetContactKey;
            }
            set
            {
                this._campaignTargetContactKey = value;
            }
        }

        [Property("LegalEntityKey", ColumnType = "Int32", NotNull = true)]
        public virtual int LegalEntityKey
        {
            get
            {
                return this._legalEntityKey;
            }
            set
            {
                this._legalEntityKey = value;
            }
        }

        [Property("AccountKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountKey
        {
            get
            {
                return this._accountKey;
            }
            set
            {
                this._accountKey = value;
            }
        }

        [Property("ActiveDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ActiveDate
        {
            get
            {
                return this._activeDate;
            }
            set
            {
                this._activeDate = value;
            }
        }

        [Property("Canceldate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime Canceldate
        {
            get
            {
                return this._canceldate;
            }
            set
            {
                this._canceldate = value;
            }
        }

        [Property("MonthlyPremium", ColumnType = "Double", NotNull = true)]
        public virtual double MonthlyPremium
        {
            get
            {
                return this._monthlyPremium;
            }
            set
            {
                this._monthlyPremium = value;
            }
        }

        [Property("CollectionDay", ColumnType = "Int32", NotNull = true, Length = 2)]
        public virtual int CollectionDay
        {
            get
            {
                return this._collectionDay;
            }
            set
            {
                this._collectionDay = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "SANTAMPolicyTrackingPrimaryKey", ColumnType = "Int32")]
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

        [BelongsTo("SANTAMPolicyStatusKey", NotNull = true)]
        public virtual SANTAMPolicyStatus_DAO SANTAMPolicyStatus
        {
            get
            {
                return this._sANTAMPolicyStatus;
            }
            set
            {
                this._sANTAMPolicyStatus = value;
            }
        }
    }
}
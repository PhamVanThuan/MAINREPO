
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Offer", Schema = "dbo")]
    public partial class Application_WTF_DAO : DB_Test_WTF<Application_WTF_DAO>
    {

        private int _applicationTypeKey;

        private int _applicationStatusKey;

        private System.DateTime? _applicationStartDate;

        private System.DateTime? _applicationEndDate;

        private int _accountKey;

        private string _reference;

        private int _applicationCampaignKey;

        private int _applicationSourceKey;

        private int _reservedAccountKey;

        private int _originationSourceKey;

        private int _estimateNumberApplicants;

        private int _key;

        private IList<ApplicationRole_WTF_DAO> _applicationRoles;

        [Property("OfferTypeKey", ColumnType = "Int32")]
        public virtual int ApplicationTypeKey
        {
            get
            {
                return this._applicationTypeKey;
            }
            set
            {
                this._applicationTypeKey = value;
            }
        }

        [Property("OfferStatusKey", ColumnType = "Int32")]
        public virtual int ApplicationStatusKey
        {
            get
            {
                return this._applicationStatusKey;
            }
            set
            {
                this._applicationStatusKey = value;
            }
        }

        [Property("OfferStartDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ApplicationStartDate
        {
            get
            {
                return this._applicationStartDate;
            }
            set
            {
                this._applicationStartDate = value;
            }
        }

        [Property("OfferEndDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ApplicationEndDate
        {
            get
            {
                return this._applicationEndDate;
            }
            set
            {
                this._applicationEndDate = value;
            }
        }

        [Property("AccountKey", ColumnType = "Int32")]
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

        [Property("Reference", ColumnType = "String")]
        public virtual string Reference
        {
            get
            {
                return this._reference;
            }
            set
            {
                this._reference = value;
            }
        }

        [Property("OfferCampaignKey", ColumnType = "Int32")]
        public virtual int ApplicationCampaignKey
        {
            get
            {
                return this._applicationCampaignKey;
            }
            set
            {
                this._applicationCampaignKey = value;
            }
        }

        [Property("OfferSourceKey", ColumnType = "Int32")]
        public virtual int ApplicationSourceKey
        {
            get
            {
                return this._applicationSourceKey;
            }
            set
            {
                this._applicationSourceKey = value;
            }
        }

        [Property("ReservedAccountKey", ColumnType = "Int32")]
        public virtual int ReservedAccountKey
        {
            get
            {
                return this._reservedAccountKey;
            }
            set
            {
                this._reservedAccountKey = value;
            }
        }

        [Property("OriginationSourceKey", ColumnType = "Int32")]
        public virtual int OriginationSourceKey
        {
            get
            {
                return this._originationSourceKey;
            }
            set
            {
                this._originationSourceKey = value;
            }
        }

        [Property("EstimateNumberApplicants", ColumnType = "Int32")]
        public virtual int EstimateNumberApplicants
        {
            get
            {
                return this._estimateNumberApplicants;
            }
            set
            {
                this._estimateNumberApplicants = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationRole_WTF_DAO), ColumnKey = "OfferKey", Table = "OfferRole")]
        public virtual IList<ApplicationRole_WTF_DAO> ApplicationRoles
        {
            get
            {
                return this._applicationRoles;
            }
            set
            {
                this._applicationRoles = value;
            }
        }
    }
}


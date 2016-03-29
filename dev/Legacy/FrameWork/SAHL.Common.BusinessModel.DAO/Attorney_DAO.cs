using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Attorney", Schema = "dbo", Lazy = true)]
    public partial class Attorney_DAO : DB_2AM<Attorney_DAO>
    {
        private string _attorneyContact;

        private GeneralStatus_DAO _generalStatus;

        private double? _attorneyMandate;

        private int? _attorneyWorkFlowEnabled;

        private double? _attorneyLoanTarget;

        private double? _attorneyFurtherLoanTarget;

        private bool? _attorneyLitigationInd;

        private bool? _attorneyRegistrationInd;

        private int _Key;

        private LegalEntity_DAO _legalEntity;

        //private IList<Bond_DAO> _bonds;

        //private IList<ApplicationMortgageLoan_DAO> _applicationMortgageLoans;

        //private Address_DAO _address;

        private DeedsOffice_DAO _deedsOffice;

        private IList<OriginationSource_DAO> _originationSources;

        [Property("AttorneyContact", ColumnType = "String")]
        public virtual string AttorneyContact
        {
            get
            {
                return this._attorneyContact;
            }
            set
            {
                this._attorneyContact = value;
            }
        }

        [BelongsTo(Column = "GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [Property("AttorneyMandate", ColumnType = "Double")]
        public virtual double? AttorneyMandate
        {
            get
            {
                return this._attorneyMandate;
            }
            set
            {
                this._attorneyMandate = value;
            }
        }

        [Property("AttorneyWorkFlowEnabled", ColumnType = "Int32")]
        public virtual int? AttorneyWorkFlowEnabled
        {
            get
            {
                return this._attorneyWorkFlowEnabled;
            }
            set
            {
                this._attorneyWorkFlowEnabled = value;
            }
        }

        [Property("AttorneyLoanTarget", ColumnType = "Double")]
        public virtual double? AttorneyLoanTarget
        {
            get
            {
                return this._attorneyLoanTarget;
            }
            set
            {
                this._attorneyLoanTarget = value;
            }
        }

        [Property("AttorneyFurtherLoanTarget", ColumnType = "Double")]
        public virtual double? AttorneyFurtherLoanTarget
        {
            get
            {
                return this._attorneyFurtherLoanTarget;
            }
            set
            {
                this._attorneyFurtherLoanTarget = value;
            }
        }

        [Property("AttorneyLitigationInd", ColumnType = "Boolean")]
        public virtual bool? AttorneyLitigationInd
        {
            get
            {
                return this._attorneyLitigationInd;
            }
            set
            {
                this._attorneyLitigationInd = value;
            }
        }

        [Property("AttorneyRegistrationInd", ColumnType = "Boolean")]
        public virtual bool? AttorneyRegistrationInd
        {
            get
            {
                return this._attorneyRegistrationInd;
            }
            set
            {
                this._attorneyRegistrationInd = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AttorneyKey", ColumnType = "Int32")]
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

        // commented, this is a lookup
        //[HasMany(typeof(Bond_DAO), ColumnKey = "AttorneyKey", Table = "Bond")]
        //public virtual IList<Bond_DAO> Bonds
        //{
        //    get
        //    {
        //        return this._bonds;
        //    }
        //    set
        //    {
        //        this._bonds = value;
        //    }
        //}

        // commented, this is a lookup
        //[HasMany(typeof(ApplicationMortgageLoan_DAO), ColumnKey = "AttorneyKey", Table = "OfferMortgageLoan")]
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

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        //[BelongsTo("AddressKey", NotNull = false)]
        //public virtual Address_DAO Address
        //{
        //    get
        //    {
        //        return this._address;
        //    }
        //    set
        //    {
        //        this._address = value;
        //    }
        //}

        // todo: check if DeedsOffice FK should be nullable
        [BelongsTo("DeedsOfficeKey", NotNull = false)]
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

        [HasAndBelongsToMany(typeof(OriginationSource_DAO), ColumnRef = "OriginationSourceKey", ColumnKey = "AttorneyKey", Lazy = true, Schema = "dbo", Table = "OriginationSourceAttorney")]
        public virtual IList<OriginationSource_DAO> OriginationSources
        {
            get
            {
                return this._originationSources;
            }
            set
            {
                this._originationSources = value;
            }
        }
    }
}
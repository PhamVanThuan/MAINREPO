using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportOffer", Schema = "dbo")]
    public partial class ImportApplication_DAO : DB_2AM<ImportApplication_DAO>
    {
        private double _applicationAmount;

        private DateTime? _applicationStartDate;

        private DateTime? _applicationEndDate;

        private string _mortgageLoanPurposeKey;

        private string _applicantTypeKey;

        private int _numberApplicants;

        private DateTime? _homePurchaseDate;

        private DateTime? _bondRegistrationDate;

        private double _currentBondValue;

        private DateTime? _deedsOfficeDate;

        private string _bondFinancialInstitution;

        private double _existingLoan;

        private double _purchasePrice;

        private string _reference;

        private string _errorMsg;

        private int _importID;

        private int _key;

        private IList<ImportLegalEntity_DAO> _importLegalEntities;

        private ImportStatus_DAO _importStatus;

        private ImportFile_DAO _importFile;

        [Property("OfferAmount", ColumnType = "Double")]
        public virtual double ApplicationAmount
        {
            get
            {
                return this._applicationAmount;
            }
            set
            {
                this._applicationAmount = value;
            }
        }

        [Property("OfferStartDate")]
        public virtual DateTime? ApplicationStartDate
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

        [Property("OfferEndDate")]
        public virtual DateTime? ApplicationEndDate
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

        [Property("MortgageLoanPurposeKey", ColumnType = "String")]
        public virtual string MortgageLoanPurposeKey
        {
            get
            {
                return this._mortgageLoanPurposeKey;
            }
            set
            {
                this._mortgageLoanPurposeKey = value;
            }
        }

        [Property("ApplicantTypeKey", ColumnType = "String")]
        public virtual string ApplicantTypeKey
        {
            get
            {
                return this._applicantTypeKey;
            }
            set
            {
                this._applicantTypeKey = value;
            }
        }

        [Property("NumberApplicants", ColumnType = "Int32")]
        public virtual int NumberApplicants
        {
            get
            {
                return this._numberApplicants;
            }
            set
            {
                this._numberApplicants = value;
            }
        }

        [Property("HomePurchaseDate")]
        public virtual DateTime? HomePurchaseDate
        {
            get
            {
                return this._homePurchaseDate;
            }
            set
            {
                this._homePurchaseDate = value;
            }
        }

        [Property("BondRegistrationDate")]
        public virtual DateTime? BondRegistrationDate
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

        [Property("CurrentBondValue", ColumnType = "Double")]
        public virtual double CurrentBondValue
        {
            get
            {
                return this._currentBondValue;
            }
            set
            {
                this._currentBondValue = value;
            }
        }

        [Property("DeedsOfficeDate")]
        public virtual DateTime? DeedsOfficeDate
        {
            get
            {
                return this._deedsOfficeDate;
            }
            set
            {
                this._deedsOfficeDate = value;
            }
        }

        [Property("BondFinancialInstitution", ColumnType = "String")]
        public virtual string BondFinancialInstitution
        {
            get
            {
                return this._bondFinancialInstitution;
            }
            set
            {
                this._bondFinancialInstitution = value;
            }
        }

        [Property("ExistingLoan", ColumnType = "Double")]
        public virtual double ExistingLoan
        {
            get
            {
                return this._existingLoan;
            }
            set
            {
                this._existingLoan = value;
            }
        }

        [Property("PurchasePrice", ColumnType = "Double")]
        public virtual double PurchasePrice
        {
            get
            {
                return this._purchasePrice;
            }
            set
            {
                this._purchasePrice = value;
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

        [Property("ErrorMsg", ColumnType = "String")]
        public virtual string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
            set
            {
                this._errorMsg = value;
            }
        }

        [Property("ImportID", ColumnType = "Int32")]
        public virtual int ImportID
        {
            get
            {
                return this._importID;
            }
            set
            {
                this._importID = value;
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

        [HasMany(typeof(ImportLegalEntity_DAO), ColumnKey = "OfferKey", Table = "ImportLegalEntity", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public virtual IList<ImportLegalEntity_DAO> ImportLegalEntities
        {
            get
            {
                return this._importLegalEntities;
            }
            set
            {
                this._importLegalEntities = value;
            }
        }

        [BelongsTo("ImportStatusKey", NotNull = true)]
        [ValidateNonEmpty("Import Status is a mandatory field")]
        public virtual ImportStatus_DAO ImportStatus
        {
            get
            {
                return this._importStatus;
            }
            set
            {
                this._importStatus = value;
            }
        }

        [BelongsTo("FileKey", NotNull = true)]
        [ValidateNonEmpty("Import File is a mandatory field")]
        public virtual ImportFile_DAO ImportFile
        {
            get
            {
                return this._importFile;
            }
            set
            {
                this._importFile = value;
            }
        }
    }
}
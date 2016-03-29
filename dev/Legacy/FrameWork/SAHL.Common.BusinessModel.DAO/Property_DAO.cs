using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)] //we can not delete these
    [ActiveRecord("Property", Schema = "dbo", Lazy = true)]
    public partial class Property_DAO : DB_2AM<Property_DAO>
    {
        private int _key;
        private PropertyType_DAO _propertyType;
        private TitleType_DAO _titleType;
        private AreaClassification_DAO _areaClassification;
        private OccupancyType_DAO _occupancyType;
        private Address_DAO _address;
        private string _propertyDescription1;
        private string _propertyDescription2;
        private string _propertyDescription3;
        private double? _deedsOfficeValue;
        private System.DateTime? _currentBondDate;
        private string _erfNumber;
        private string _erfPortionNumber;
        private string _sectionalSchemeName;
        private string _sectionalUnitNumber;
        private DeedsPropertyType_DAO _deedsPropertyType;
        private string _erfSuburbDescription;
        private string _erfMetroDescription;
        private DataProvider_DAO _dataProvider;
        private IList<PropertyData_DAO> _propertyDatas;
        //private IList<Account_DAO> _accountProperties;
        private IList<FinancialService_DAO> _mortgageLoanProperties;
        private IList<PropertyTitleDeed_DAO> _propertyTitleDeeds;
        private IList<Valuation_DAO> _valuations;
        private IList<PropertyAccessDetails_DAO> _propertyAccessDetails;

        [Property("PropertyDescription1", ColumnType = "String")]
        public virtual string PropertyDescription1
        {
            get
            {
                return this._propertyDescription1;
            }
            set
            {
                this._propertyDescription1 = value;
            }
        }

        [Property("PropertyDescription2", ColumnType = "String")]
        public virtual string PropertyDescription2
        {
            get
            {
                return this._propertyDescription2;
            }
            set
            {
                this._propertyDescription2 = value;
            }
        }

        [Property("PropertyDescription3", ColumnType = "String")]
        public virtual string PropertyDescription3
        {
            get
            {
                return this._propertyDescription3;
            }
            set
            {
                this._propertyDescription3 = value;
            }
        }

        [Property("DeedsOfficeValue", ColumnType = "Double")]
        public virtual double? DeedsOfficeValue
        {
            get
            {
                return this._deedsOfficeValue;
            }
            set
            {
                this._deedsOfficeValue = value;
            }
        }

        [Property("CurrentBondDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CurrentBondDate
        {
            get
            {
                return this._currentBondDate;
            }
            set
            {
                this._currentBondDate = value;
            }
        }

        [Property("ErfNumber", ColumnType = "String")]
        public virtual string ErfNumber
        {
            get
            {
                return this._erfNumber;
            }
            set
            {
                this._erfNumber = value;
            }
        }

        [Property("ErfPortionNumber", ColumnType = "String")]
        public virtual string ErfPortionNumber
        {
            get
            {
                return this._erfPortionNumber;
            }
            set
            {
                this._erfPortionNumber = value;
            }
        }

        [Property("SectionalSchemeName", ColumnType = "String")]
        public virtual string SectionalSchemeName
        {
            get
            {
                return this._sectionalSchemeName;
            }
            set
            {
                this._sectionalSchemeName = value;
            }
        }

        [Property("SectionalUnitNumber", ColumnType = "String")]
        public virtual string SectionalUnitNumber
        {
            get
            {
                return this._sectionalUnitNumber;
            }
            set
            {
                this._sectionalUnitNumber = value;
            }
        }

        [Property("ErfSuburbDescription", ColumnType = "String")]
        public virtual string ErfSuburbDescription
        {
            get
            {
                return this._erfSuburbDescription;
            }
            set
            {
                this._erfSuburbDescription = value;
            }
        }

        [Property("ErfMetroDescription", ColumnType = "String")]
        public virtual string ErfMetroDescription
        {
            get
            {
                return this._erfMetroDescription;
            }
            set
            {
                this._erfMetroDescription = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "PropertyKey", ColumnType = "Int32")]
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

        [HasAndBelongsToMany(typeof(FinancialService_DAO), Schema = "fin", Table = "MortgageLoan", ColumnKey = "PropertyKey", ColumnRef = "FinancialServiceKey", Lazy = true)]
        public virtual IList<FinancialService_DAO> MortgageLoanProperties
        {
            get
            {
                return _mortgageLoanProperties;
            }
            set
            {
                _mortgageLoanProperties = value;
            }
        }

        [HasMany(typeof(PropertyTitleDeed_DAO), ColumnKey = "PropertyKey", Table = "PropertyTitleDeed", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<PropertyTitleDeed_DAO> PropertyTitleDeeds
        {
            get
            {
                if (_propertyTitleDeeds == null)
                    _propertyTitleDeeds = new List<PropertyTitleDeed_DAO>();

                return this._propertyTitleDeeds;
            }
            set
            {
                this._propertyTitleDeeds = value;
            }
        }

        [HasMany(typeof(Valuation_DAO), ColumnKey = "PropertyKey", Table = "Valuation", OrderBy = "IsActive", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<Valuation_DAO> Valuations
        {
            get
            {
                return this._valuations;
            }
            set
            {
                this._valuations = value;
            }
        }

        [Lurker]
        [HasMany(typeof(PropertyAccessDetails_DAO), ColumnKey = "PropertyKey", Table = "PropertyAccessDetails", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<PropertyAccessDetails_DAO> PropertyAccessDetails
        {
            get
            {
                if (_propertyAccessDetails == null)
                    _propertyAccessDetails = new List<PropertyAccessDetails_DAO>();

                return this._propertyAccessDetails;
            }
            set
            {
                this._propertyAccessDetails = value;
            }
        }

        /// <summary>
        /// The address of the Property
        /// </summary>
        [BelongsTo("AddressKey", NotNull = false)]
        public virtual Address_DAO Address
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }

        /// <summary>
        /// The Area Classification of property
        /// </summary>
        [BelongsTo("AreaClassificationKey", NotNull = false)]
        public virtual AreaClassification_DAO AreaClassification
        {
            get
            {
                return this._areaClassification;
            }
            set
            {
                this._areaClassification = value;
            }
        }

        [BelongsTo("DeedsPropertyTypeKey", NotNull = false)]
        public virtual DeedsPropertyType_DAO DeedsPropertyType
        {
            get
            {
                return this._deedsPropertyType;
            }
            set
            {
                this._deedsPropertyType = value;
            }
        }

        [BelongsTo("OccupancyTypeKey", NotNull = false)]
        public virtual OccupancyType_DAO OccupancyType
        {
            get
            {
                return this._occupancyType;
            }
            set
            {
                this._occupancyType = value;
            }
        }

        [BelongsTo("PropertyTypeKey", NotNull = false)]
        public virtual PropertyType_DAO PropertyType
        {
            get
            {
                return this._propertyType;
            }
            set
            {
                this._propertyType = value;
            }
        }

        [BelongsTo("TitleTypeKey", NotNull = false)]
        public virtual TitleType_DAO TitleType
        {
            get
            {
                return this._titleType;
            }
            set
            {
                this._titleType = value;
            }
        }

        //[HasAndBelongsToMany(typeof(Account_DAO), Table = "AccountProperty", ColumnKey = "PropertyKey", ColumnRef = "AccountKey", Lazy = true)]
        //public virtual IList<Account_DAO> AccountProperties
        //{
        //    get
        //    {
        //        return _accountProperties;
        //    }
        //    set
        //    {
        //        _accountProperties = value;
        //    }
        //}

        [BelongsTo("DataProviderKey", NotNull = false)]
        public virtual DataProvider_DAO DataProvider
        {
            get
            {
                return this._dataProvider;
            }
            set
            {
                this._dataProvider = value;
            }
        }

        [HasMany(typeof(PropertyData_DAO), ColumnKey = "PropertyKey", Table = "PropertyData", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<PropertyData_DAO> PropertyDatas
        {
            get
            {
                if (_propertyDatas == null)
                    _propertyDatas = new List<PropertyData_DAO>();

                return this._propertyDatas;
            }
            set
            {
                this._propertyDatas = value;
            }
        }
    }
}
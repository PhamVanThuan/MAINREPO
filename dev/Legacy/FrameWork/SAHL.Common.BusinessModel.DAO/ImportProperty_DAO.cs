using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportProperty", Schema = "dbo")]
    public partial class ImportProperty_DAO : DB_2AM<ImportProperty_DAO>
    {

        private string _propertyTypeKey;

        private string _titleTypeKey;

        private string _areaClassificationKey;

        private string _occupancyTypeKey;

        private string _propertyDescription1;

        private string _propertyDescription2;

        private string _propertyDescription3;

        private double _deedsOfficeValue;

        private DateTime? _currentBondDate;

        private string _erfNumber;

        private string _erfPortionNumber;

        private string _sectionalSchemeName;

        private string _sectionalUnitNumber;

        private string _deedsPropertyTypeKey;

        private string _erfSuburbDescription;

        private string _erfMetroDescription;

        private string _titleDeedNumber;

        private int _key;

        private ImportApplication_DAO _importApplication;

        [Property("PropertyTypeKey", ColumnType = "String")]
        public virtual string PropertyTypeKey
        {
            get
            {
                return this._propertyTypeKey;
            }
            set
            {
                this._propertyTypeKey = value;
            }
        }

        [Property("TitleTypeKey", ColumnType = "String")]
        public virtual string TitleTypeKey
        {
            get
            {
                return this._titleTypeKey;
            }
            set
            {
                this._titleTypeKey = value;
            }
        }

        [Property("AreaClassificationKey", ColumnType = "String")]
        public virtual string AreaClassificationKey
        {
            get
            {
                return this._areaClassificationKey;
            }
            set
            {
                this._areaClassificationKey = value;
            }
        }

        [Property("OccupancyTypeKey", ColumnType = "String")]
        public virtual string OccupancyTypeKey
        {
            get
            {
                return this._occupancyTypeKey;
            }
            set
            {
                this._occupancyTypeKey = value;
            }
        }

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
        public virtual double DeedsOfficeValue
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

        [Property("CurrentBondDate")]
        public virtual DateTime? CurrentBondDate
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

        [Property("DeedsPropertyTypeKey", ColumnType = "String")]
        public virtual string DeedsPropertyTypeKey
        {
            get
            {
                return this._deedsPropertyTypeKey;
            }
            set
            {
                this._deedsPropertyTypeKey = value;
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

        [Property("TitleDeedNumber", ColumnType = "String")]
        public virtual string TitleDeedNumber
        {
            get
            {
                return this._titleDeedNumber;
            }
            set
            {
                this._titleDeedNumber = value;
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

        [BelongsTo("OfferKey", NotNull = true)]
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

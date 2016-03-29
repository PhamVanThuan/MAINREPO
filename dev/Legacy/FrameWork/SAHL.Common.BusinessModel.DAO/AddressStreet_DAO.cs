using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Street format.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "1", Lazy = true)]
    public class AddressStreet_DAO : Address_DAO
    {
        private string _buildingNumber;

        private string _buildingName;

        private string _streetNumber;

        private string _streetName;

        private string _unitNumber;

        private Suburb_DAO _suburb;

        /// <summary>
        /// The Building Number of the Address.
        /// </summary>
        [Property("BuildingNumber", ColumnType = "String", Length = 10)]
        public virtual string BuildingNumber
        {
            get
            {
                return this._buildingNumber;
            }
            set
            {
                this._buildingNumber = value;
            }
        }

        /// <summary>
        /// The Building Name of the Address.
        /// </summary>
        [Property("BuildingName", ColumnType = "String")]
        public virtual string BuildingName
        {
            get
            {
                return this._buildingName;
            }
            set
            {
                this._buildingName = value;
            }
        }

        /// <summary>
        /// The Street Number of the Address.
        /// </summary>
        [Property("StreetNumber", ColumnType = "String", Length = 10)]
        public virtual string StreetNumber
        {
            get
            {
                return this._streetNumber;
            }
            set
            {
                this._streetNumber = value;
            }
        }

        /// <summary>
        /// The Street Name of the Address.
        /// </summary>
        [Property("StreetName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Street Name is a mandatory field")]
        public virtual string StreetName
        {
            get
            {
                return this._streetName;
            }
            set
            {
                this._streetName = value;
            }
        }

        /// <summary>
        /// The Unit Number of the Address.
        /// </summary>
        [Property("UnitNumber", ColumnType = "String", Length = 15)]
        public virtual string UnitNumber
        {
            get
            {
                return this._unitNumber;
            }
            set
            {
                this._unitNumber = value;
            }
        }

        /// <summary>
        /// The Suburb which the Address belongs to.
        /// </summary>
        [BelongsTo("SuburbKey")]
        [ValidateNonEmpty("Suburb is a mandatory field")]
        public virtual Suburb_DAO Suburb
        {
            get
            {
                return this._suburb;
            }
            set
            {
                this._suburb = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressStreet_DAO Find(int id)
        {
            return ActiveRecordBase<AddressStreet_DAO>.Find(id).As<AddressStreet_DAO>();
        }

        public new static AddressStreet_DAO Find(object id)
        {
            return ActiveRecordBase<AddressStreet_DAO>.Find(id).As<AddressStreet_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressStreet_DAO FindFirst()
        {
            return ActiveRecordBase<AddressStreet_DAO>.FindFirst().As<AddressStreet_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressStreet_DAO FindOne()
        {
            return ActiveRecordBase<AddressStreet_DAO>.FindOne().As<AddressStreet_DAO>();
        }

        #endregion Static Overrides
    }
}
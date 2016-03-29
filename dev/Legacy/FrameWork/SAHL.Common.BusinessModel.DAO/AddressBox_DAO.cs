using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Box format.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "2", Lazy = true)]
    public class AddressBox_DAO : Address_DAO
    {
        private string _boxNumber;

        private PostOffice_DAO _postOffice;

        /// <summary>
        /// The Post Office Box Number of the Address
        /// </summary>
        [Property("BoxNumber", ColumnType = "String", Length = 15)]
        [ValidateNonEmpty("Box Number is a mandatory field")]
        public virtual string BoxNumber
        {
            get
            {
                return this._boxNumber;
            }
            set
            {
                this._boxNumber = value;
            }
        }

        /// <summary>
        /// The Post Office which the Address belongs to.
        /// </summary>
        [BelongsTo("PostOfficeKey")]
        [ValidateNonEmpty("Post Office is a mandatory field")]
        public virtual PostOffice_DAO PostOffice
        {
            get
            {
                return this._postOffice;
            }
            set
            {
                this._postOffice = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressBox_DAO Find(int id)
        {
            return ActiveRecordBase<AddressBox_DAO>.Find(id).As<AddressBox_DAO>();
        }

        public new static AddressBox_DAO Find(object id)
        {
            return ActiveRecordBase<AddressBox_DAO>.Find(id).As<AddressBox_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressBox_DAO FindFirst()
        {
            return ActiveRecordBase<AddressBox_DAO>.FindFirst().As<AddressBox_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressBox_DAO FindOne()
        {
            return ActiveRecordBase<AddressBox_DAO>.FindOne().As<AddressBox_DAO>();
        }

        #endregion Static Overrides
    }
}
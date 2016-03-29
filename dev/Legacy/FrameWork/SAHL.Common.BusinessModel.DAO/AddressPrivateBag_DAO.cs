using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Private Bag format.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "4", Lazy = true)]
    public class AddressPrivateBag_DAO : Address_DAO
    {
        private string _privateBagNumber;

        private PostOffice_DAO _postOffice;

        /// <summary>
        /// The Private Bag Number of the Address
        /// </summary>
        [Property("BoxNumber", ColumnType = "String", Length = 15)]
        [ValidateNonEmpty("Private Bag Number is a mandatory field")]
        public virtual string PrivateBagNumber
        {
            get
            {
                return this._privateBagNumber;
            }
            set
            {
                this._privateBagNumber = value;
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
        public static AddressPrivateBag_DAO Find(int id)
        {
            return ActiveRecordBase<AddressPrivateBag_DAO>.Find(id).As<AddressPrivateBag_DAO>();
        }

        public new static AddressPrivateBag_DAO Find(object id)
        {
            return ActiveRecordBase<AddressPrivateBag_DAO>.Find(id).As<AddressPrivateBag_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressPrivateBag_DAO FindFirst()
        {
            return ActiveRecordBase<AddressPrivateBag_DAO>.FindFirst().As<AddressPrivateBag_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressPrivateBag_DAO FindOne()
        {
            return ActiveRecordBase<AddressPrivateBag_DAO>.FindOne().As<AddressPrivateBag_DAO>();
        }

        #endregion Static Overrides
    }
}
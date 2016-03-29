using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Postnet Suite format.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "3", Lazy = true)]
    public class AddressPostnetSuite_DAO : Address_DAO
    {
        private string _suiteNumber;

        private string _privateBagNumber;

        private PostOffice_DAO _postOffice;

        /// <summary>
        /// The Postnet Box Number of the Address.
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
        /// The Postnet Suite Number of the Address.
        /// </summary>
        [Property("SuiteNumber", ColumnType = "String", Length = 15)]
        [ValidateNonEmpty("Suite Number is a mandatory field")]
        public virtual string SuiteNumber
        {
            get
            {
                return this._suiteNumber;
            }
            set
            {
                this._suiteNumber = value;
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
        public static AddressPostnetSuite_DAO Find(int id)
        {
            return ActiveRecordBase<AddressPostnetSuite_DAO>.Find(id).As<AddressPostnetSuite_DAO>();
        }

        public new static AddressPostnetSuite_DAO Find(object id)
        {
            return ActiveRecordBase<AddressPostnetSuite_DAO>.Find(id).As<AddressPostnetSuite_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressPostnetSuite_DAO FindFirst()
        {
            return ActiveRecordBase<AddressPostnetSuite_DAO>.FindFirst().As<AddressPostnetSuite_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressPostnetSuite_DAO FindOne()
        {
            return ActiveRecordBase<AddressPostnetSuite_DAO>.FindOne().As<AddressPostnetSuite_DAO>();
        }

        #endregion Static Overrides
    }
}
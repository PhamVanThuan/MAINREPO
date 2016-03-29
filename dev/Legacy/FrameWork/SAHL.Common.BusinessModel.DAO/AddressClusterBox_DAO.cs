using Castle.ActiveRecord;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Address_DAO base class and is used to instantiate an Address in Cluster Box format.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "6", Lazy = true)]
    public class AddressClusterBox_DAO : Address_DAO
    {
        private string _clusterBoxNumber;

        private PostOffice_DAO _postOffice;

        /// <summary>
        /// The Cluster Box number of the Address
        /// </summary>
        [Property("BoxNumber", ColumnType = "String", Length = 15)]
        [ValidateNonEmpty("Cluster Box Number is a mandatory field")]
        public virtual string ClusterBoxNumber
        {
            get
            {
                return this._clusterBoxNumber;
            }
            set
            {
                this._clusterBoxNumber = value;
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
        public static AddressClusterBox_DAO Find(int id)
        {
            return ActiveRecordBase<AddressClusterBox_DAO>.Find(id).As<AddressClusterBox_DAO>();
        }

        public new static AddressClusterBox_DAO Find(object id)
        {
            return ActiveRecordBase<AddressClusterBox_DAO>.Find(id).As<AddressClusterBox_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressClusterBox_DAO FindFirst()
        {
            return ActiveRecordBase<AddressClusterBox_DAO>.FindFirst().As<AddressClusterBox_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static AddressClusterBox_DAO FindOne()
        {
            return ActiveRecordBase<AddressClusterBox_DAO>.FindOne().As<AddressClusterBox_DAO>();
        }

        #endregion Static Overrides
    }
}
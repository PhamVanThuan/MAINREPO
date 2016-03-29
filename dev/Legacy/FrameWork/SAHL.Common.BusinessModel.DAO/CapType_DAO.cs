using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// CapType_DAO is used to hold the values of the different CAP types which can be offered to a client. The client
    /// can either be offered 1%,2% or 3% above their current rate.
    /// </summary>
    [ActiveRecord("CapType", Schema = "dbo")]
    public partial class CapType_DAO : DB_2AM<CapType_DAO>
    {
        private string _description;

        private double _value;

        private int _Key;

        private IList<CapTypeConfigurationDetail_DAO> _capTypeConfigurationDetails;

        // Not Mapped
        //private IList<Trade> _trades;
        /// <summary>
        /// The Description of the CAP Type. e.g. 2% Above Current Rate
        /// </summary>
        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// The percentage value above the clients rate which applies to the CAP Type. The example above would have a value of 0.02
        /// </summary>
        [Property("Value", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Value is a mandatory field")]
        public virtual double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "CapTypeKey", ColumnType = "Int32")]
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

        /// <summary>
        /// A CAP Type can have many configuration details set up against it in the CapTypeConfigurationDetail table.
        /// </summary>
        [HasMany(typeof(CapTypeConfigurationDetail_DAO), ColumnKey = "CapTypeKey", Table = "CapTypeConfigurationDetail", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<CapTypeConfigurationDetail_DAO> CapTypeConfigurationDetails
        {
            get
            {
                return this._capTypeConfigurationDetails;
            }
            set
            {
                this._capTypeConfigurationDetails = value;
            }
        }

        //[HasMany(typeof(Trade), ColumnKey = "CapTypeKey", Table = "Trade")]
        //public virtual IList<Trade> Trades
        //{
        //    get
        //    {
        //        return this._trades;
        //    }
        //    set
        //    {
        //        this._trades = value;
        //    }
        //}
    }
}
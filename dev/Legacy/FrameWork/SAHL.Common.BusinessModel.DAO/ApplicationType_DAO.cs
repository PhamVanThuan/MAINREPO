using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The ApplicationType_DAO class specifies the different types of Applications.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferType", Schema = "dbo", Lazy = true)]
    public partial class ApplicationType_DAO : DB_2AM<ApplicationType_DAO>
    {
        private string _description;

        private int _key;

        //commented as this is a lookup.
        //private IList<Offer_DAO> _applications;

        //private OfferSourceConfiguration_DAO _applicationSourceConfiguration;
        /// <summary>
        /// The Description of the Application Type e.g. Readvance/Further Loan
        /// </summary>
        [Property("Description", ColumnType = "String")]
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
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "OfferTypeKey", ColumnType = "Int32")]
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

        //commented as this is a lookup.
        //[HasMany(typeof(Offer_DAO), ColumnKey = "OfferTypeKey", Table = "Offer")]
        //public virtual IList<Offer_DAO> Applications
        //{
        //    get
        //    {
        //        return this._applications;
        //    }
        //    set
        //    {
        //        this._applications = value;
        //    }
        //}

        //[BelongsTo("OfferSourceConfigurationKey", NotNull = false)]
        //public virtual OfferSourceConfiguration_DAO ApplicationSourceConfiguration
        //{
        //    get
        //    {
        //        return this._applicationSourceConfiguration;
        //    }
        //    set
        //    {
        //        this._applicationSourceConfiguration = value;
        //    }
        //}
    }
}
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The ApplicationStatus_DAO class specifies the different statuses that an Application can have.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferStatus", Schema = "dbo", Lazy = true)]
    public partial class ApplicationStatus_DAO : DB_2AM<ApplicationStatus_DAO>
    {
        private string _description;

        private int _key;

        //commented as this is a lookup.
        //private IList<Offer_DAO> _applications;
        /// <summary>
        /// The description of the Application Status
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
        [PrimaryKey(PrimaryKeyType.Assigned, "OfferStatusKey", ColumnType = "Int32")]
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
        //[HasMany(typeof(Offer_DAO), ColumnKey = "OfferStatusKey", Table = "Offer", Lazy = true)]
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
    }
}
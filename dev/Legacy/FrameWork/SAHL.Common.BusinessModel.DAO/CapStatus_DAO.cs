using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// CapStatus_DAO is used to hold the different statuses that a CapOffer is assigned.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("CapStatus", Schema = "dbo")]
    public partial class CapStatus_DAO : DB_2AM<CapStatus_DAO>
    {
        private string _description;

        private int _key;

        //private IList<CapApplication_DAO> _capApplications;

        //private IList<CapApplicationDetail_DAO> _capApplicationDetails;
        /// <summary>
        /// The CapStatus Description e.g. Open CAP 2 Offer, Forms Sent
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
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "CapStatusKey", ColumnType = "Int32")]
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

        #region commented, this is a lookup.

        //[HasMany(typeof(CapApplication_DAO), ColumnKey = "CapStatusKey", Table = "CapOffer", Lazy = true)]
        //public virtual IList<CapApplication_DAO> CapApplications
        //{
        //    get
        //    {
        //        return this._capApplications;
        //    }
        //    set
        //    {
        //        this._capApplications = value;
        //    }
        //}

        //[HasMany(typeof(CapApplicationDetail_DAO), ColumnKey = "CapStatusKey", Table = "CapOfferDetail", Lazy = true)]
        //public virtual IList<CapApplicationDetail_DAO> CapApplicationDetails
        //{
        //    get
        //    {
        //        return this._capApplicationDetails;
        //    }
        //    set
        //    {
        //        this._capApplicationDetails = value;
        //    }
        //}

        #endregion commented, this is a lookup.
    }
}
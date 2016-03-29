using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This class specifies the source of an application. ie. The Internet or Campaign etc.
    /// </summary>
    [ActiveRecord("OfferSource", Schema = "dbo", Lazy = true)]
    public partial class ApplicationSource_DAO : DB_2AM<ApplicationSource_DAO>
    {
        private string _description;

        private GeneralStatus_DAO _generalStatus;

        private int _key;

        /// <summary>
        /// A description of the application source.
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
        /// Indicates whether the source is currently used.
        /// </summary>
        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        /// <summary>
        /// The value used to identify a applicaition source.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "OfferSourceKey", ColumnType = "Int32")]
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
    }
}
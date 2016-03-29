using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CapNTUReason", Schema = "dbo")]
    public partial class CapNTUReason_DAO : DB_2AM<CapNTUReason_DAO>
    {
        private string _description;

        private int _key;

        private IList<CapApplicationDetail_DAO> _capApplicationDetails;

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

        [PrimaryKey(PrimaryKeyType.Native, "CapNTUReasonKey", ColumnType = "Int32")]
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

        [HasMany(typeof(CapApplicationDetail_DAO), ColumnKey = "CapNTUReasonKey", Table = "CapOfferDetail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<CapApplicationDetail_DAO> CapApplicationDetails
        {
            get
            {
                return this._capApplicationDetails;
            }
            set
            {
                this._capApplicationDetails = value;
            }
        }
    }
}
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DetailType", Schema = "dbo", Lazy = true)]
    public partial class DetailType_DAO : DB_2AM<DetailType_DAO>
    {
        private string _description;

        private bool _allowUpdateDelete;

        private bool _allowUpdate;

        private bool _allowScreen;

        private int _Key;

        private DetailClass_DAO _detailClass;

        private GeneralStatus_DAO _generalStatus;

        [Property("Description", ColumnType = "String", NotNull = true, Length = 50)]
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

        [Property("AllowUpdateDelete", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Allow Update/Delete is a mandatory field")]
        public virtual bool AllowUpdateDelete
        {
            get
            {
                return this._allowUpdateDelete;
            }
            set
            {
                this._allowUpdateDelete = value;
            }
        }

        [Property("AllowUpdate", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Allow Update is a mandatory field")]
        public virtual bool AllowUpdate
        {
            get
            {
                return this._allowUpdate;
            }
            set
            {
                this._allowUpdate = value;
            }
        }

        [Property("AllowScreen", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Allow Screen is a mandatory field")]
        public virtual bool AllowScreen
        {
            get
            {
                return this._allowScreen;
            }
            set
            {
                this._allowScreen = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "DetailTypeKey", ColumnType = "Int32")]
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

        [Lurker]
        [BelongsTo("DetailClassKey", NotNull = true)]
        [ValidateNonEmpty("Detail Class is a mandatory field")]
        public virtual DetailClass_DAO DetailClass
        {
            get
            {
                return this._detailClass;
            }
            set
            {
                this._detailClass = value;
            }
        }

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
    }
}
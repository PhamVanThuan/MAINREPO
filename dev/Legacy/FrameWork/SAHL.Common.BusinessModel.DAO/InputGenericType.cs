using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("InputGenericType", Schema = "dbo", Lazy = false)]
    public partial class InputGenericType_DAO : DB_2AM<InputGenericType_DAO>
    {
        private int _key;

        private CBOMenu_DAO _coreBusinessObjectMenu;

        private GenericKeyTypeParameter_DAO _genericKeyTypeParameter;

        [PrimaryKey(PrimaryKeyType.Native, "InputGenericTypeKey", ColumnType = "Int32")]
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

        [BelongsTo("CoreBusinessObjectKey", NotNull = true)]
        public virtual CBOMenu_DAO CoreBusinessObjectMenu
        {
            get
            {
                return this._coreBusinessObjectMenu;
            }
            set
            {
                this._coreBusinessObjectMenu = value;
            }
        }

        [BelongsTo("GenericKeyTypeParameterKey", NotNull = true)]
        public virtual GenericKeyTypeParameter_DAO GenericKeyTypeParameter
        {
            get
            {
                return this._genericKeyTypeParameter;
            }
            set
            {
                this._genericKeyTypeParameter = value;
            }
        }
    }
}
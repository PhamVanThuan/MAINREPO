using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("GenericKeyTypeParameter", Schema = "dbo", Lazy = false)]
    public partial class GenericKeyTypeParameter_DAO : DB_2AM<GenericKeyTypeParameter_DAO>
    {
        private string _parameterName;

        private ParameterType_DAO _parameterTypeKey;

        private int _key;

        //private IList<CBOMenu_DAO> _coreBusinessObjectMenus;

        private GenericKeyType_DAO _genericKeyType;

        [Property("ParameterName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Parameter Name is a mandatory field")]
        public virtual string ParameterName
        {
            get
            {
                return this._parameterName;
            }
            set
            {
                this._parameterName = value;
            }
        }

        [BelongsTo("ParameterTypeKey", NotNull = true)]
        [ValidateNonEmpty("Parameter Type Key is a mandatory field")]
        public virtual ParameterType_DAO ParameterTypeKey
        {
            get
            {
                return this._parameterTypeKey;
            }
            set
            {
                this._parameterTypeKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "GenericKeyTypeParameterKey", ColumnType = "Int32")]
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

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }
    }
}
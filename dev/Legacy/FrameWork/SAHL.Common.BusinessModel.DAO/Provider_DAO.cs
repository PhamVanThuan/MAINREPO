using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTestAttribute]
    [ActiveRecord("Provider", Schema = "deb")]
    public partial class Provider_DAO : DB_2AM<Provider_DAO>
    {
        private int _key;

        private string _description;

        private string _url;

        private GeneralStatus_DAO _generalStatus;

        private bool _isDefault;

        private decimal _maxCollectionAmount;

        [PrimaryKey(PrimaryKeyType.Assigned, "ProviderKey", ColumnType = "Int32")]
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

        [Property("URL", ColumnType = "String")]
        public virtual string URL
        {
            get
            {
                return this._url;
            }
            set
            {
                this._url = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
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

        [Property("IsDefault", ColumnType = "Boolean")]
        public virtual bool IsDefault
        {
            get
            {
                return this._isDefault;
            }
            set
            {
                this._isDefault = value;
            }
        }

        [Property("MaxCollectionAmount", ColumnType = "Decimal")]
        public virtual decimal MaxCollectionAmount
        {
            get
            {
                return this._maxCollectionAmount;
            }
            set
            {
                this._maxCollectionAmount = value;
            }
        }
    }
}
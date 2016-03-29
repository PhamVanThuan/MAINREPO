using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Court", Schema = "debtcounselling")]
    public partial class Court_DAO : DB_2AM<Court_DAO>
    {
        private int _key;

        private CourtType_DAO _courtType;

        private Province_DAO _province;

        private string _name;

        private GeneralStatus_DAO _generalStatus;

        [PrimaryKey(PrimaryKeyType.Assigned, "CourtKey", ColumnType = "Int32")]
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

        [BelongsTo("CourtTypeKey", NotNull = true)]
        public virtual CourtType_DAO CourtType
        {
            get
            {
                return this._courtType;
            }
            set
            {
                this._courtType = value;
            }
        }

        [BelongsTo("ProvinceKey", NotNull = true)]
        public virtual Province_DAO Province
        {
            get
            {
                return this._province;
            }
            set
            {
                this._province = value;
            }
        }

        [Property("Name", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Court Name is a mandatory field")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
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
    }
}
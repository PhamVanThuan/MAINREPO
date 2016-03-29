using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("HearingType", Schema = "debtcounselling")]
    public partial class HearingType_DAO : DB_2AM<HearingType_DAO>
    {
        private int _key;

        private string _description;

        [PrimaryKey(PrimaryKeyType.Assigned, "HearingTypeKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Hearing Type Description is a mandatory field")]
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
    }
}
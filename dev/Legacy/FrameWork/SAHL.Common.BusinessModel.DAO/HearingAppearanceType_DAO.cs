using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("HearingAppearanceType", Schema = "debtcounselling")]
    public partial class HearingAppearanceType_DAO : DB_2AM<HearingAppearanceType_DAO>
    {
        private int _key;

        private HearingType_DAO _hearingType;

        private string _description;

        [PrimaryKey(PrimaryKeyType.Assigned, "HearingAppearanceTypeKey", ColumnType = "Int32")]
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

        [BelongsTo("HearingTypeKey", NotNull = true)]
        public virtual HearingType_DAO HearingType
        {
            get
            {
                return this._hearingType;
            }
            set
            {
                this._hearingType = value;
            }
        }

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Hearing Appearance Type Description is a mandatory field")]
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
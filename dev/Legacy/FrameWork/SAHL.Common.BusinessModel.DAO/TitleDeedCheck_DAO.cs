using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[DoNotTestWithGenericTest]
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("vw_TitleDeedCheck", Schema = "dbo")]
    public partial class TitleDeedCheck_DAO : DB_SAHL<TitleDeedCheck_DAO>
    {
        private string _key;

        private string _titleDeedIndicator;

        [PrimaryKey(PrimaryKeyType.Assigned, "Loannumber")]
        public virtual string Key
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

        [Property("TitleDeedIndicator", NotNull = true, Length = 7)]
        [ValidateNonEmpty("Title Deed Indicator is a mandatory field")]
        public virtual string TitleDeedIndicator
        {
            get
            {
                return this._titleDeedIndicator;
            }
            set
            {
                this._titleDeedIndicator = value;
            }
        }
    }
}
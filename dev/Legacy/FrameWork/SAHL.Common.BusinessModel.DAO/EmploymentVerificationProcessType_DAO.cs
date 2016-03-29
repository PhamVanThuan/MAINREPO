using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("EmploymentVerificationProcessType", Schema = "dbo")]
    public partial class EmploymentVerificationProcessType_DAO : DB_2AM<EmploymentVerificationProcessType_DAO>
    {
        private string _description;
        private int _key;
        private GeneralStatus_DAO _generalStatus;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "EmploymentVerificationProcessTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status Key is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get { return this._generalStatus; }
            set { this._generalStatus = value; }
        }
    }
}
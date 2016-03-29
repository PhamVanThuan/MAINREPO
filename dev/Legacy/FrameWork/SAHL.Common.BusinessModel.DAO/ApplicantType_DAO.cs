using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ApplicantType", Schema = "dbo", Lazy = true)]
    public partial class ApplicantType_DAO : DB_2AM<ApplicantType_DAO>
    {
        private string _description;

        private int _applicantTypeKey;

        [Property("Description", ColumnType = "String", Length = 50)]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "ApplicantTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._applicantTypeKey;
            }
            set
            {
                this._applicantTypeKey = value;
            }
        }
    }
}
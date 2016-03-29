using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("SPVCompany", Schema = "spv", Lazy = true)]
    public partial class SPVCompany_DAO : DB_2AM<SPVCompany_DAO>
    {
        private int _key;
        private string _Description;

        [PrimaryKey(PrimaryKeyType.Assigned, "SPVCompanyKey", ColumnType = "Int32")]
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

        [Property("Description")]
        public virtual string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                this._Description = value;
            }
        }
    }
}
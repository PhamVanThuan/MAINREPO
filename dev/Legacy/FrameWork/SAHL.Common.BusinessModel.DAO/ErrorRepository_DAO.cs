using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ErrorRepository", Schema = "dbo")]
    public class ErrorRepository_DAO : DB_2AM<ErrorRepository_DAO>
    {
        private int _key;
        private string _Description;
        private bool _Active;

        [PrimaryKey(PrimaryKeyType.Assigned, "ErrorRepositoryKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return _key;
            }
            set { _key = value; }
        }

        [Property("Description", ColumnType = "String", NotNull = true, Length = 255)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        [Property("Active", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Active is a mandatory field")]
        public virtual bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
            }
        }
    }
}
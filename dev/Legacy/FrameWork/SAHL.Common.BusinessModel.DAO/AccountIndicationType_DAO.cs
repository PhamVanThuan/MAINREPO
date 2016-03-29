using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("AccountIndicationType", Schema = "dbo", Lazy = true)]
    public partial class AccountIndicationType_DAO : DB_2AM<AccountIndicationType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<AccountIndication_DAO> _accountIndications;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "AccountIndicationTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        //[HasMany(typeof(AccountIndication_DAO), ColumnKey = "AccountIndicationTypeKey", Table = "AccountIndication")]
        //public virtual IList<AccountIndication_DAO> AccountIndications
        //{
        //    get
        //    {
        //        return this._accountIndications;
        //    }
        //    set
        //    {
        //        this._accountIndications = value;
        //    }
        //}
    }
}
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferCapitecDetail", Schema = "dbo", Lazy = true)]
    public partial class ApplicationCapitecDetail_DAO : DB_2AM<ApplicationCapitecDetail_DAO>
    {
        private int _key;

        private int _applicationKey;

        private string _branch;

        private string _consultant;

        [PrimaryKey(PrimaryKeyType.Native, "OfferCapitecDetailKey", ColumnType = "Int32")]
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

        [Property("OfferKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ApplicationKey
        {
            get
            {
                return this._applicationKey;
            }
            set
            {
                this._applicationKey = value;
            }
        }

        [Property("Branch", ColumnType = "String")]
        public virtual string Branch
        {
            get
            {
                return this._branch;
            }
            set
            {
                this._branch = value;
            }
        }

        [Property("Consultant", ColumnType = "String")]
        public virtual string Consultant
        {
            get
            {
                return this._consultant;
            }
            set
            {
                this._consultant = value;
            }
        }
    }
}
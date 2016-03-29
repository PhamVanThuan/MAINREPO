using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("CancellationType", Schema = "dbo")]
    public partial class CancellationType_DAO : DB_2AM<CancellationType_DAO>
    {
        private string _description;

        private string _cancellationWebCode;

        private int _Key;

        [Property("Description", ColumnType = "String", NotNull = true, Length = 50)]
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

        [Property("CancellationWebCode", ColumnType = "String", Length = 100)]
        public virtual string CancellationWebCode
        {
            get
            {
                return this._cancellationWebCode;
            }
            set
            {
                this._cancellationWebCode = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "CancellationTypeKey", ColumnType = "Int32")]
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
    }
}
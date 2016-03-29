using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Lookup", Schema = "dbo", CustomAccess = Constants.ReadonlyAccess)]
    public partial class Lookup_DAO : DB_ImageIndex<Lookup_DAO>
    {
        private decimal _sTORid;

        private string _field;

        private string _text;

        private decimal _key;

        [Property("STORid", ColumnType = "Decimal")]
        public virtual decimal STORid
        {
            get
            {
                return this._sTORid;
            }
            set
            {
                this._sTORid = value;
            }
        }

        [Property("Field", ColumnType = "String")]
        public virtual string Field
        {
            get
            {
                return this._field;
            }
            set
            {
                this._field = value;
            }
        }

        [Property("Text", ColumnType = "String")]
        public virtual string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ID", ColumnType = "Decimal")]
        public virtual decimal Key
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
    }
}
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("PreContextFilter", Schema = "search")]
    public partial class PreContextFilter_DAO : DB_2AM<PreContextFilter_DAO>
    {
        private int _key;

        private int _contextKey;

        private int _offerTypeKey;

        [PrimaryKey(PrimaryKeyType.Native, "PreContextFilterKey", ColumnType = "Int32")]
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

        [Property("ContextKey", ColumnType = "Int32")]
        public virtual int ContextKey
        {
            get
            {
                return this._contextKey;
            }
            set
            {
                this._contextKey = value;
            }
        }

        [Property("OfferTypeKey", ColumnType = "Int32")]
        public virtual int OfferTypeKey
        {
            get
            {
                return this._offerTypeKey;
            }
            set
            {
                this._offerTypeKey = value;
            }
        }
    }
}
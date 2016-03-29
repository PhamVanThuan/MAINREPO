using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("CorrespondenceDetail", Schema = "dbo", Lazy = true)]
    public partial class CorrespondenceDetail_DAO : DB_2AM<CorrespondenceDetail_DAO>
    {
        private int _key;

        private Correspondence_DAO _correspondence;

        private string _correspondenceText;

        [PrimaryKey(PrimaryKeyType.Foreign, Column = "CorrespondenceKey", ColumnType = "Int32")]
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

        [OneToOne()]
        public virtual Correspondence_DAO Correspondence
        {
            get
            {
                return this._correspondence;
            }
            set
            {
                this._correspondence = value;
            }
        }

        [Property("CorrespondenceText", ColumnType = "String", NotNull = true)]
        public virtual string CorrespondenceText
        {
            get
            {
                return this._correspondenceText;
            }
            set
            {
                this._correspondenceText = value;
            }
        }
    }
}
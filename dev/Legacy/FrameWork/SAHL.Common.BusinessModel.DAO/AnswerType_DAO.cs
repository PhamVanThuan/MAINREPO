using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("AnswerType", Schema = "Survey", Lazy = true)]
    public partial class AnswerType_DAO : DB_2AM<AnswerType_DAO>
    {
        private int _key;

        private string _description;

        //private IList<Answer_DAO> _answers;

        [PrimaryKey(PrimaryKeyType.Assigned, "AnswerTypeKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        //[HasMany(typeof(Answer_DAO), ColumnKey = "AnswerTypeKey", Table = "Answer")]
        //public virtual IList<Answer_DAO> Answers
        //{
        //    get
        //    {
        //        return this._answers;
        //    }
        //    set
        //    {
        //        this._answers = value;
        //    }
        //}
    }
}
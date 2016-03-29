using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AnswerImage", Schema = "Survey", Lazy = true)]
    public partial class AnswerImage_DAO : DB_2AM<AnswerImage_DAO>
    {
        private int _key;

        private Answer_DAO _answer;

        private Image_DAO _image;

        [PrimaryKey(PrimaryKeyType.Native, "AnswerImageKey", ColumnType = "Int32")]
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

        [BelongsTo("AnswerKey", NotNull = true)]
        public virtual Answer_DAO Answer
        {
            get
            {
                return this._answer;
            }
            set
            {
                this._answer = value;
            }
        }

        [BelongsTo("ImageKey", NotNull = true)]
        public virtual Image_DAO Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
            }
        }
    }
}
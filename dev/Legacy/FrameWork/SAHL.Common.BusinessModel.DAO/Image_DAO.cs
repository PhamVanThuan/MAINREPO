using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Image", Schema = "Survey")]
    public partial class Image_DAO : DB_2AM<Image_DAO>
    {
        private int _key;

        private string _uRL;

        private IList<AnswerImage_DAO> _answerImages;

        [PrimaryKey(PrimaryKeyType.Native, "ImageKey", ColumnType = "Int32")]
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

        [Property("URL", ColumnType = "String", NotNull = true)]
        public virtual string URL
        {
            get
            {
                return this._uRL;
            }
            set
            {
                this._uRL = value;
            }
        }

        [HasMany(typeof(AnswerImage_DAO), ColumnKey = "ImageKey", Table = "AnswerImage")]
        public virtual IList<AnswerImage_DAO> AnswerImages
        {
            get
            {
                return this._answerImages;
            }
            set
            {
                this._answerImages = value;
            }
        }
    }
}
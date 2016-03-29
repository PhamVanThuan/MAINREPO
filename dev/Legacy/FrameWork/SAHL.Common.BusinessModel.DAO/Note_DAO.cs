using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Note", Schema = "dbo", Lazy = true)]
    public partial class Note_DAO : DB_2AM<Note_DAO>
    {
        private int _key;

        private GenericKeyType_DAO _genericKeyType;

        private int _genericKey;

        private IList<NoteDetail_DAO> _noteDetails;

        private DateTime? _diaryDate;

        [PrimaryKey(PrimaryKeyType.Native, "NoteKey", ColumnType = "Int32")]
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

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [HasMany(typeof(NoteDetail_DAO), ColumnKey = "NoteKey", Table = "NoteDetail", Inverse = true)]
        public virtual IList<NoteDetail_DAO> NoteDetails
        {
            get
            {
                return this._noteDetails;
            }
            set
            {
                this._noteDetails = value;
            }
        }

        [Property("DiaryDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual DateTime? DiaryDate
        {
            get
            {
                return this._diaryDate;
            }
            set
            {
                this._diaryDate = value;
            }
        }
    }
}
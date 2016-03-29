using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("NoteDetail", Schema = "dbo", Lazy = true)]
    public partial class NoteDetail_DAO : DB_2AM<NoteDetail_DAO>
    {
        private int _key;

        private string _tag;

        private string _workflowState;

        private System.DateTime _insertedDate;

        private string _noteText;

        private LegalEntity_DAO _legalEntity;

        private Note_DAO _note;

        [PrimaryKey(PrimaryKeyType.Native, "NoteDetailKey", ColumnType = "Int32")]
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

        [Property("Tag", ColumnType = "String")]
        public virtual string Tag
        {
            get
            {
                return this._tag;
            }
            set
            {
                this._tag = value;
            }
        }

        [Property("WorkflowState", ColumnType = "String")]
        public virtual string WorkflowState
        {
            get
            {
                return this._workflowState;
            }
            set
            {
                this._workflowState = value;
            }
        }

        [Property("InsertedDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime InsertedDate
        {
            get
            {
                return this._insertedDate;
            }
            set
            {
                this._insertedDate = value;
            }
        }

        /// <summary>
        /// #22300 - DC Notes issue
        /// The ColumnType of this property was changed from 'String' to 'StringClob' because NHibernate was truncating the text when inserting
        /// into the destination column in the database (which is nvarchar(max)). See the following links for more info on this fix.
        /// http://www.tritac.com/bp-21-fluent-nhibernate-nvarcharmax-fields-truncated-to-4000-characters
        /// http://issues.castleproject.org/issue/AR-262
        /// </summary>
        [Property("NoteText", ColumnType = "StringClob", NotNull = true)]
        public virtual string NoteText
        {
            get
            {
                return this._noteText;
            }
            set
            {
                this._noteText = value;
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [BelongsTo("NoteKey", NotNull = true)]
        public virtual Note_DAO Note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
            }
        }
    }
}
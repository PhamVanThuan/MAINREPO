using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DocumentReferenceObject",  Schema = "dbo")]
    public partial class DocumentReferenceObject_DAO : DB_2AM<DocumentReferenceObject_DAO>
    {

        private string _tableName;

        private string _columnName;

        private int _Key;

        // todo: Uncomment when DocumentTypeReferenceObject implemented
        //private IList<DocumentTypeReferenceObject> _documentTypeReferenceObjects;

        [Property("TableName", ColumnType = "String", NotNull = true)]
        public virtual string TableName
        {
            get
            {
                return this._tableName;
            }
            set
            {
                this._tableName = value;
            }
        }

        [Property("ColumnName", ColumnType = "String", NotNull = true)]
        public virtual string ColumnName
        {
            get
            {
                return this._columnName;
            }
            set
            {
                this._columnName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "DocumentReferenceObjectKey", ColumnType = "Int32")]
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

            //[HasMany(typeof(DocumentTypeReferenceObject), ColumnKey = "DocumentReferenceObjectKey", Table = "DocumentTypeReferenceObject")]
            //public virtual IList<DocumentTypeReferenceObject> DocumentTypeReferenceObjects
            //{
            //    get
            //    {
            //        return this._documentTypeReferenceObjects;
            //    }
            //    set
            //    {
            //        this._documentTypeReferenceObjects = value;
            //    }
            //}
        }
    }
}

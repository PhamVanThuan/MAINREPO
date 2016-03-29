using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DocumentGroup", Schema = "dbo", Lazy = true)]
    public partial class DocumentGroup_DAO : DB_2AM<DocumentGroup_DAO>
    {
        private string _description;

        private int _Key;
        // todo: Uncomment when DocumentTypeGroup is implemented
        //private IList<DocumentTypeGroup> _documentTypeGroups;

        [Property("Description", ColumnType = "String")]
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

        [PrimaryKey(PrimaryKeyType.Native, "DocumentGroupKey", ColumnType = "Int32")]
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

        // todo: Uncomment when DocumentTypeGroup implemented
        //[HasMany(typeof(DocumentTypeGroup), ColumnKey = "DocumentGroupKey", Table = "DocumentTypeGroup")]
        //public virtual IList<DocumentTypeGroup> DocumentTypeGroups
        //{
        //    get
        //    {
        //        return this._documentTypeGroups;
        //    }
        //    set
        //    {
        //        this._documentTypeGroups = value;
        //    }
        //}
    }
}
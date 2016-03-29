using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ImportStatus", Schema = "dbo")]
    public partial class ImportStatus_DAO : DB_2AM<ImportStatus_DAO>
    {
        private string _description;

        private int _key;

        private IList<ImportApplication_DAO> _importApplications;

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "ImportStatusKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ImportApplication_DAO), ColumnKey = "ImportStatusKey", Table = "ImportOffer", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public virtual IList<ImportApplication_DAO> ImportApplications
        {
            get
            {
                return this._importApplications;
            }
            set
            {
                this._importApplications = value;
            }
        }
    }
}
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ReportType", Schema = "dbo")]
    public partial class ReportType_DAO : DB_2AM<ReportType_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<ReportStatement> _reportStatements;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "ReportTypeKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(ReportStatement), ColumnKey = "ReportTypeKey", Table = "ReportStatement")]
        //public virtual IList<ReportStatement> ReportStatements
        //{
        //    get
        //    {
        //        return this._reportStatements;
        //    }
        //    set
        //    {
        //        this._reportStatements = value;
        //    }
        //}
    }
}
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ReportParameterType", Schema = "dbo")]
    public partial class ReportParameterType_DAO : DB_2AM<ReportParameterType_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        // private IList<ReportParameter> _reportParameters;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "ReportParameterTypeKey", ColumnType = "Int32")]
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
        //[HasMany(typeof(ReportParameter), ColumnKey = "ParameterTypeKey", Table = "ReportParameter")]
        //public virtual IList<ReportParameter> ReportParameters
        //{
        //    get
        //    {
        //        return this._reportParameters;
        //    }
        //    set
        //    {
        //        this._reportParameters = value;
        //    }
        //}
    }
}
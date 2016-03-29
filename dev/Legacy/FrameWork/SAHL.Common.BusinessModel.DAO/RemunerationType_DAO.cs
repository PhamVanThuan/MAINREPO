using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("RemunerationType", Schema = "dbo")]
    public partial class RemunerationType_DAO : DB_2AM<RemunerationType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<Employment_DAO> _employments;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "RemunerationTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(Employment_DAO), ColumnKey = "RemunerationTypeKey", Table = "Employment", Lazy= true)]
        //public virtual IList<Employment_DAO> Employments
        //{
        //    get
        //    {
        //        return this._employments;
        //    }
        //    set
        //    {
        //        this._employments = value;
        //    }
        //}
    }
}
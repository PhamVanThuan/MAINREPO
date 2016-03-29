using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("EmploymentSector", Schema = "dbo")]
    public partial class EmploymentSector_DAO : DB_2AM<EmploymentSector_DAO>
    {
        private string _description;

        private GeneralStatus_DAO _generalStatus;

        private int _Key;

        //private IList<Employer_DAO> _employers;

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

        [BelongsTo(Column = "GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatusKey
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "EmploymentSectorKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(Employer_DAO), ColumnKey = "EmploymentSectorKey", Table = "Employer",Lazy=true)]
        //public virtual IList<Employer_DAO> Employers
        //{
        //    get
        //    {
        //        return this._employers;
        //    }
        //    set
        //    {
        //        this._employers = value;
        //    }
        //}
    }
}
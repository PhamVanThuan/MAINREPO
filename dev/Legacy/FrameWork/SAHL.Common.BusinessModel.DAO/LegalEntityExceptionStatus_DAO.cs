using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LegalEntityExceptionStatus", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityExceptionStatus_DAO : DB_2AM<LegalEntityExceptionStatus_DAO>
    {
        private string _description;

        private int _key;

        //private IList<LegalEntity_DAO> _legalEntities;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "LegalEntityExceptionStatusKey", ColumnType = "Int32")]
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

        //Commented, this is a lookup.
        //[HasMany(typeof(LegalEntity_DAO), ColumnKey = "LegalEntityExceptionStatusKey", Table = "LegalEntity", Lazy = true)]
        //public virtual IList<LegalEntity_DAO> LegalEntities
        //{
        //    get
        //    {
        //        return this._legalEntities;
        //    }
        //    set
        //    {
        //        this._legalEntities = value;
        //    }
        //}
    }
}
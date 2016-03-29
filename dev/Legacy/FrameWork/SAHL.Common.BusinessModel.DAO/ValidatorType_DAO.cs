using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ValidatorType", Schema = "dbo")]
    public partial class ValidatorType_DAO : DB_2AM<ValidatorType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<Validator_DAO> _validators;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "ValidatorTypeKey", ColumnType = "Int32")]
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
        //[HasMany(typeof(Validator_DAO), ColumnKey = "ValidatorTypeKey", Table = "Validator")]
        //public virtual IList<Validator_DAO> Validators
        //{
        //    get
        //    {
        //        return this._validators;
        //    }
        //    set
        //    {
        //        this._validators = value;
        //    }
        //}
    }
}
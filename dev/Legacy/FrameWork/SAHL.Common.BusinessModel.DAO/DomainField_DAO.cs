using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DomainField", Schema = "dbo")]
    public partial class DomainField_DAO : DB_2AM<DomainField_DAO>
    {
        private string _description;

        private string _displayDescription;

        private string _key;

        // commented, this is a lookup.
        //private IList<ReportParameter_DAO> _reportParameters;

        // commented, this is a lookup.
        //private IList<Validator_DAO> _validators;

        private FormatType_DAO _formatType;

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

        [Property("DisplayDescription", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Display Description is a mandatory field")]
        public virtual string DisplayDescription
        {
            get
            {
                return this._displayDescription;
            }
            set
            {
                this._displayDescription = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "DomainFieldKey", ColumnType = "String")]
        public virtual string Key
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
        //[HasMany(typeof(ReportParameter), ColumnKey = "DomainFieldKey", Table = "ReportParameter")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(Validator), ColumnKey = "DomainFieldKey", Table = "Validator")]
        //public virtual IList<Validator> Validators
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

        [BelongsTo("FormatTypeKey", NotNull = false)]
        public virtual FormatType_DAO FormatType
        {
            get
            {
                return this._formatType;
            }
            set
            {
                this._formatType = value;
            }
        }
    }
}
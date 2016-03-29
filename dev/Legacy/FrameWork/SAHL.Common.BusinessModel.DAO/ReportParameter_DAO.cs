using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [Lurker]
    [ActiveRecord("ReportParameter", Schema = "dbo")]
    public partial class ReportParameter_DAO : DB_2AM<ReportParameter_DAO>
    {
        private string _parameterName;

        //
        //private string _parameterType;

        private int? _parameterLength;

        private string _displayName;

        private bool? _required;

        private string _statementName;

        private int _key;

        // commented, this is a lookup.
        //private IList<CorrespondenceParameters> _correspondenceParameters;

        private DomainField_DAO _domainField;

        private ReportParameterType_DAO _reportParameterType;

        private ReportStatement_DAO _reportStatement;

        [Property("ParameterName", ColumnType = "String")]
        public virtual string ParameterName
        {
            get
            {
                return this._parameterName;
            }
            set
            {
                this._parameterName = value;
            }
        }

        //[Property("ParameterType", ColumnType = "String")]
        //public virtual string ParameterType
        //{
        //    get
        //    {
        //        return this._parameterType;
        //    }
        //    set
        //    {
        //        this._parameterType = value;
        //    }
        //}

        [Property("ParameterLength", ColumnType = "Int32")]
        public virtual int? ParameterLength
        {
            get
            {
                return this._parameterLength;
            }
            set
            {
                this._parameterLength = value;
            }
        }

        [Property("DisplayName", ColumnType = "String")]
        public virtual string DisplayName
        {
            get
            {
                return this._displayName;
            }
            set
            {
                this._displayName = value;
            }
        }

        [Property("Required", ColumnType = "Boolean")]
        public virtual bool? Required
        {
            get
            {
                return this._required;
            }
            set
            {
                this._required = value;
            }
        }

        [Property("StatementName", ColumnType = "String")]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "ReportParameterKey", ColumnType = "Int32")]
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

        // commented this is a lookup.
        //[HasMany(typeof(CorrespondenceParameters), ColumnKey = "ReportParameterKey", Table = "CorrespondenceParameters")]
        //public virtual IList<CorrespondenceParameters> CorrespondenceParameters
        //{
        //    get
        //    {
        //        return this._correspondenceParameters;
        //    }
        //    set
        //    {
        //        this._correspondenceParameters = value;
        //    }
        //}

        [BelongsTo("DomainFieldKey", NotNull = false)]
        public virtual DomainField_DAO DomainField
        {
            get
            {
                return this._domainField;
            }
            set
            {
                this._domainField = value;
            }
        }

        [BelongsTo("ParameterTypeKey", NotNull = false)]
        public virtual ReportParameterType_DAO ReportParameterType
        {
            get
            {
                return this._reportParameterType;
            }
            set
            {
                this._reportParameterType = value;
            }
        }

        [BelongsTo("ReportStatementKey", NotNull = false)]
        public virtual ReportStatement_DAO ReportStatement
        {
            get
            {
                return this._reportStatement;
            }
            set
            {
                this._reportStatement = value;
            }
        }
    }
}
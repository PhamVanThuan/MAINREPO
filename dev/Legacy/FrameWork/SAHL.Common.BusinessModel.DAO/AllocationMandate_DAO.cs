using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AllocationMandate", Schema = "dbo")]
    public class AllocationMandate_DAO : DB_2AM<AllocationMandate_DAO>
    {
        private string _name;

        private string _description;

        private string _typeName;

        private string _parameterValue;

        private int _key;

        //private IList<AllocationMandateOperator_DAO> _allocationMandateOperators;

        private ParameterType_DAO _parameterType;

        [Property("Name", ColumnType = "String", NotNull = true, Length = 50)]
        [ValidateNonEmpty("Name is a mandatory field")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

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

        [Property("TypeName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Type Name is a mandatory field")]
        public virtual string TypeName
        {
            get
            {
                return this._typeName;
            }
            set
            {
                this._typeName = value;
            }
        }

        [Property("ParameterValue", ColumnType = "String")]
        public virtual string ParameterValue
        {
            get
            {
                return this._parameterValue;
            }
            set
            {
                this._parameterValue = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AllocationMandateKey", ColumnType = "Int32")]
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

        [BelongsTo("ParameterTypeKey")]
        public virtual ParameterType_DAO ParameterType
        {
            get
            {
                return this._parameterType;
            }
            set
            {
                this._parameterType = value;
            }
        }
    }
}
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OrganisationStructureAttributeType", Schema = "dbo", Lazy = true)]
    public partial class OrganisationStructureAttributeType_DAO : DB_2AM<OrganisationStructureAttributeType_DAO>
    {
        private string _description;

        private int _length;

        private int _Key;

        private IList<OrganisationStructureAttribute_DAO> _organisationStructureAttributes;

        private DataType_DAO _dataType;

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

        [Property("Length", ColumnType = "Int32")]
        public virtual int Length
        {
            get
            {
                return this._length;
            }
            set
            {
                this._length = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OrganisationStructureAttributeTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(OrganisationStructureAttribute_DAO), ColumnKey = "OrganisationStructureAttributeTypeKey", Table = "OrganisationStructureAttribute", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<OrganisationStructureAttribute_DAO> OrganisationStructureAttributes
        {
            get
            {
                return this._organisationStructureAttributes;
            }
            set
            {
                this._organisationStructureAttributes = value;
            }
        }

        [BelongsTo("DataTypeKey", NotNull = true)]
        [ValidateNonEmpty("Data Type is a mandatory field")]
        public virtual DataType_DAO DataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }
    }
}
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OrganisationStructureAttribute", Schema = "dbo", Lazy = true)]
    public partial class OrganisationStructureAttribute_DAO : DB_2AM<OrganisationStructureAttribute_DAO>
    {
        private string _attributeValue;

        private int _Key;

        private OrganisationStructure_DAO _organisationStructure;
        private OrganisationStructureAttributeType_DAO _organisationStructureAttributeType;

        [Property("AttributeValue", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Attribute Value is a mandatory field")]
        public virtual string AttributeValue
        {
            get
            {
                return this._attributeValue;
            }
            set
            {
                this._attributeValue = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OrganisationStructureAttributeKey", ColumnType = "Int32")]
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

        [BelongsTo("OrganisationStructureKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Structure is a mandatory field")]
        public virtual OrganisationStructure_DAO OrganisationStructure
        {
            get
            {
                return this._organisationStructure;
            }
            set
            {
                this._organisationStructure = value;
            }
        }

        [BelongsTo("OrganisationStructureAttributeTypeKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Structure Attribute Type is a mandatory field")]
        public virtual OrganisationStructureAttributeType_DAO OrganisationStructureAttributeType
        {
            get
            {
                return this._organisationStructureAttributeType;
            }
            set
            {
                this._organisationStructureAttributeType = value;
            }
        }
    }
}
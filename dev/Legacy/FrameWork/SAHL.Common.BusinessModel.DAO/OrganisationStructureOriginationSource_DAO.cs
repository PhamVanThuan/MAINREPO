using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OrganisationStructureOriginationSource", Schema = "dbo", Lazy = true)]
    public partial class OrganisationStructureOriginationSource_DAO : DB_2AM<OrganisationStructureOriginationSource_DAO>
    {
        private int _key;

        private OrganisationStructure_DAO _organisationStructure;

        private OriginationSource_DAO _originationSource;

        [PrimaryKey(PrimaryKeyType.Native, "OrganisationStructureOriginationSourceKey", ColumnType = "Int32")]
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

        [BelongsTo(Column = "OrganisationStructureKey", NotNull = true)]
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

        [BelongsTo(Column = "OriginationSourceKey", NotNull = true)]
        [ValidateNonEmpty("Origination Source is a mandatory field")]
        public virtual OriginationSource_DAO OriginationSource
        {
            get
            {
                return this._originationSource;
            }
            set
            {
                this._originationSource = value;
            }
        }
    }
}
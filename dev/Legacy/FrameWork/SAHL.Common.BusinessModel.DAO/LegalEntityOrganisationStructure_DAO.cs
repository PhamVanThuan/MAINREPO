using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LegalEntityOrganisationStructure", Schema = "dbo")]
    public partial class LegalEntityOrganisationStructure_DAO : DB_2AM<LegalEntityOrganisationStructure_DAO>
    {
        private int _key;

        private LegalEntity_DAO _legalEntity;

        private OrganisationStructure_DAO _organisationStructure;

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityOrganisationStructureKey", ColumnType = "Int32")]
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

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity Key is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }

        [BelongsTo("OrganisationStructureKey", NotNull = true)]
        [ValidateNonEmpty("Organisation Structure Key is a mandatory field")]
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
    }
}
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("LegalEntityOrganisationStructureHistory", Schema = "dbo")]
    public partial class LegalEntityOrganisationStructureHistory_DAO : DB_2AM<LegalEntityOrganisationStructureHistory_DAO>
    {
        private LegalEntityOrganisationStructure_DAO _legalEntityOrganisationStructure;

        private LegalEntity_DAO _legalEntity;

        private OrganisationStructure_DAO _organisationStructure;

        private System.DateTime _changeDate;

        private string _action;

        private int _key;

        [BelongsTo("LegalEntityOrganisationStructureKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity Organisation Structure is a mandatory field")]
        public virtual LegalEntityOrganisationStructure_DAO LegalEntityOrganisationStructure
        {
            get
            {
                return this._legalEntityOrganisationStructure;
            }
            set
            {
                this._legalEntityOrganisationStructure = value;
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
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
        [ValidateNonEmpty("Organisation Structure is a mandatory field")]
        public virtual OrganisationStructure_DAO OrganisationStructureKey
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

        [Property("ChangeDate", ColumnType = "Timestamp")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [Property("Action", ColumnType = "String", NotNull = true, Length = 1)]
        public virtual string Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityOrganisationStructureHistoryKey", ColumnType = "Int32")]
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
    }
}
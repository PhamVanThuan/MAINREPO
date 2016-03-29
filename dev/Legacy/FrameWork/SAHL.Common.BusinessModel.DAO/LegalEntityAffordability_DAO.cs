using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The LegalEntityAffordability_DAO class contains the information regarding the Legal Entity's Affordability Assessment.
    /// </summary>
    [ActiveRecord("LegalEntityAffordability", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityAffordability_DAO : DB_2AM<LegalEntityAffordability_DAO>
    {
        private double _amount;

        private string _description;

        private int _key;

        private AffordabilityType_DAO _affordabilityType;

        private LegalEntity_DAO _legalEntity;

        private Application_DAO _application;

        /// <summary>
        /// The Rand Value of the Affordability Assessment Entry.
        /// </summary>
        [Property("Amount", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Amount is a mandatory field")]
        public virtual double Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        /// <summary>
        /// Description field.
        /// </summary>
        [Property("Description", ColumnType = "String")]
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "LegalEntityAffordabilityKey", ColumnType = "Int32")]
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

        /// <summary>
        /// The specific Affordability Assessment category that we are capturing information for. e.g. Basic Salary, Rental, Commission etc.
        /// </summary>
        [BelongsTo("AffordabilityTypeKey", NotNull = true)]
        [ValidateNonEmpty("Affordability Type is a mandatory field")]
        public virtual AffordabilityType_DAO AffordabilityType
        {
            get
            {
                return this._affordabilityType;
            }
            set
            {
                this._affordabilityType = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the Legal Entity table. Each Affordability Assessment entry belongs to a single Legal Entity.
        /// </summary>
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

        /// <summary>
        /// The foreign key reference to the Offer table. Each Affordability Assessment entry belongs to a single Application.
        /// </summary>
        [BelongsTo("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Application is a mandatory field")]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }
    }
}
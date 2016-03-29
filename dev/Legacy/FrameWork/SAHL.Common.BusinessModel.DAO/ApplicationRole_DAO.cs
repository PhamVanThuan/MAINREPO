using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// OfferRole_DAO is instantiated to represent the different Roles that Legal Entities are playing on the Application.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("OfferRole", Schema = "dbo", Lazy = true)]
    public partial class ApplicationRole_DAO : DB_2AM<ApplicationRole_DAO>
    {
        private System.DateTime _statusChangeDate;

        private int _Key;

        private ApplicationRoleType_DAO _applicationRoleType;

        private GeneralStatus_DAO _generalStatus;

        private int _legalEntityKey;

        private int _applicationKey;

        private IList<ApplicationRoleAttribute_DAO> _applicationRoleAttributes;

        private IList<ApplicationDeclaration_DAO> _applicationDeclarations;

		private IList<ApplicationRoleDomicilium_DAO> _applicationRoleDomiciliums;

		private IList<LegalEntityDomicilium_DAO> _legalEntityDomiciliums;

        /// <summary>
        /// The date on which the status of the Role was last changed.
        /// </summary>
        [Property("StatusChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Status Change Date is a mandatory field")]
        public virtual System.DateTime StatusChangeDate
        {
            get
            {
                return this._statusChangeDate;
            }
            set
            {
                this._statusChangeDate = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "OfferRoleKey", ColumnType = "Int32")]
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

        /// <summary>
        /// Each Role belongs to a specific Application Role Type. The Role Types are defined in the OfferRoleType table and include
        /// Insurer, Valuator, Branch Consultant etc.
        /// </summary>
        [BelongsTo("OfferRoleTypeKey", NotNull = true)]
        [ValidateNonEmpty("Application Role Type is a mandatory field")]
        public virtual ApplicationRoleType_DAO ApplicationRoleType
        {
            get
            {
                return this._applicationRoleType;
            }
            set
            {
                this._applicationRoleType = value;
            }
        }

        /// <summary>
        /// The status of the ApplicationRole either Active or Inactive.
        /// </summary>
        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        /// <summary>
        /// The details regarding the Legal Entity playing the Role in the Application is stored in the LegalEntity table. This is
        /// the LegalEntityKey for that Legal Entity.
        /// </summary>
        /// <remarks>Exposed as a key for performance reasons.</remarks>
        [Property("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual int LegalEntityKey
        {
            get
            {
                return this._legalEntityKey;
            }
            set
            {
                this._legalEntityKey = value;
            }
        }

        /// <summary>
        /// The key of the application to which the ApplicationRole belongs.
        /// </summary>
        /// <remarks>Exposed as a key for performance reasons.</remarks>
        [Property("OfferKey", NotNull = true)]
        [ValidateNonEmpty("Application is a mandatory field")]
        public virtual int ApplicationKey
        {
            get
            {
                return this._applicationKey;
            }
            set
            {
                this._applicationKey = value;
            }
        }

        /// <summary>
        /// A collection of role attributes that are defined for this Role.
        /// </summary>
        [HasMany(typeof(ApplicationRoleAttribute_DAO), Table = "OfferRoleAttribute", ColumnKey = "OfferRoleKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationRoleAttribute_DAO> ApplicationRoleAttributes
        {
            get { return _applicationRoleAttributes; }
            set { _applicationRoleAttributes = value; }
        }

        /// <summary>
        /// A collection of application declarations that are defined for this Role.
        /// </summary>
        [HasMany(typeof(ApplicationDeclaration_DAO), Table = "OfferDeclaration", ColumnKey = "OfferRoleKey", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationDeclaration_DAO> ApplicationDeclarations
        {
            get { return _applicationDeclarations; }
            set { _applicationDeclarations = value; }
        }

		[Lurker]
		[HasMany(typeof(ApplicationRoleDomicilium_DAO), ColumnKey = "OfferRoleKey", Table = "OfferRoleDomicilium", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = true)]
		public virtual IList<ApplicationRoleDomicilium_DAO> ApplicationRoleDomiciliums
		{
			get
			{
				return this._applicationRoleDomiciliums;
			}
			set
			{
				this._applicationRoleDomiciliums = value;
			}
		}
    }
}
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("OfferRoleType", Schema = "dbo", Lazy = true)]
    public partial class ApplicationRoleType_DAO : DB_2AM<ApplicationRoleType_DAO>
    {
        private string _description;

        private int _Key;

        private ApplicationRoleTypeGroup_DAO _applicationRoleTypeGroup;

        //private IList<ApplicationRole_DAO> _applicationRoles;

        private IList<OrganisationStructure_DAO> _OfferRoleTypeOrganisationStructures;

        /// <summary>
        /// The description of the Application Role Type
        /// </summary>
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "OfferRoleTypeKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(ApplicationRole_DAO), ColumnKey = "OfferRoleTypeKey", Table = "OfferRole",Lazy = true)]
        //public virtual IList<ApplicationRole_DAO> ApplicationRoles
        //{
        //    get
        //    {
        //        return this._applicationRoles;
        //    }
        //    set
        //    {
        //        this._applicationRoles = value;
        //    }
        //}
        /// <summary>
        /// This is the relationship between the OrganisationStructure and the ApplicationRoleType as defined in the
        /// OfferRoleTypeOrganisationStructureMapping. An ApplicationRoleType can have many OrganisationStructures and thus an
        /// OrganisationStructure can be related to many ApplicationRoleTypes.
        /// </summary>
        [HasAndBelongsToMany(typeof(OrganisationStructure_DAO), Table = "OfferRoleTypeOrganisationStructureMapping", ColumnKey = "OfferRoleTypeKey", ColumnRef = "OrganisationStructureKey", Lazy = true)]
        public virtual IList<OrganisationStructure_DAO> OfferRoleTypeOrganisationStructures
        {
            get
            {
                return _OfferRoleTypeOrganisationStructures;
            }
            set
            {
                _OfferRoleTypeOrganisationStructures = value;
            }
        }

        /// <summary>
        /// Determines the Application Role Type Group to which the Application Role Type belongs.
        /// </summary>
        /// <remarks>Fetch type is join as an OfferRoleType is almost always used in the context of it's group.</remarks>
        [BelongsTo("OfferRoleTypeGroupKey", NotNull = true, Fetch = FetchEnum.Join)]
        [ValidateNonEmpty("Application Role Type Group is a mandatory field")]
        public virtual ApplicationRoleTypeGroup_DAO ApplicationRoleTypeGroup
        {
            get
            {
                return this._applicationRoleTypeGroup;
            }
            set
            {
                this._applicationRoleTypeGroup = value;
            }
        }
    }
}
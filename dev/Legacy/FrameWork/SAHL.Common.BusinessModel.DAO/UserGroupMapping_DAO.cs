using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("UserGroupMapping", Schema = "dbo")]
    public partial class UserGroupMapping_DAO : DB_2AM<UserGroupMapping_DAO>
    {

        private int _Key;

        private IList<UserGroupAssignment_DAO> _userGroupAssignments;

        private FunctionalGroupDefinition_DAO _functionalGroupDefinition;

        private OrganisationStructure_DAO _organisationStructure;

        [PrimaryKey(PrimaryKeyType.Native, "UserGroupMappingKey", ColumnType = "Int32")]
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

        [HasMany(typeof(UserGroupAssignment_DAO), ColumnKey = "UserGroupMappingKey", Table = "UserGroupAssignment", Lazy = true)]
        public virtual IList<UserGroupAssignment_DAO> UserGroupAssignments
        {
            get
            {
                return this._userGroupAssignments;
            }
            set
            {
                this._userGroupAssignments = value;
            }
        }

        [BelongsTo("FunctionalGroupDefinitionKey", NotNull = true)]
        [ValidateNonEmpty("Functional Group Definition is a mandatory field")]
        public virtual FunctionalGroupDefinition_DAO FunctionalGroupDefinition
        {
            get
            {
                return this._functionalGroupDefinition;
            }
            set
            {
                this._functionalGroupDefinition = value;
            }
        }

        [BelongsTo("OrganisationStructureKey", NotNull = true)]
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
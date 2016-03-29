using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AllocationMandateSet", Schema = "dbo")]
    public class AllocationMandateSet_DAO : DB_2AM<AllocationMandateSet_DAO>
    {
        private int _key;

        private IList<AllocationMandateOperator_DAO> _allocationMandateOperators;

        private IList<UserOrganisationStructure_DAO> _userOrganisationStructures;

        private AllocationMandateSetGroup_DAO _allocationMandateSetGroup;

        [PrimaryKey(PrimaryKeyType.Native, "AllocationMandateSetKey", ColumnType = "Int32")]
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

        [HasMany(typeof(AllocationMandateOperator_DAO), ColumnKey = "AllocationMandateSetKey", Table = "AllocationMandateOperator", Inverse = true)]
        public virtual IList<AllocationMandateOperator_DAO> AllocationMandateOperators
        {
            get
            {
                return this._allocationMandateOperators;
            }
            set
            {
                this._allocationMandateOperators = value;
            }
        }

        [HasAndBelongsToMany(typeof(UserOrganisationStructure_DAO), Table = "AllocationMandateSetUserOrganisationStructure", ColumnKey = "AllocationMandateSetKey", ColumnRef = "UserOrganisationStructureKey", Lazy = true)]
        public virtual IList<UserOrganisationStructure_DAO> UserOrganisationStructures
        {
            get
            {
                return this._userOrganisationStructures;
            }
            set
            {
                this._userOrganisationStructures = value;
            }
        }

        [BelongsTo("AllocationMandateSetGroupKey", NotNull = true)]
        [ValidateNonEmpty("Allocation Mandate Set Group is a mandatory field")]
        public virtual AllocationMandateSetGroup_DAO AllocationMandateSetGroup
        {
            get
            {
                return this._allocationMandateSetGroup;
            }
            set
            {
                this._allocationMandateSetGroup = value;
            }
        }
    }
}
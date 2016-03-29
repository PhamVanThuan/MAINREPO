using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AllocationMandateSetGroup", Schema = "dbo")]
    public class AllocationMandateSetGroup_DAO : DB_2AM<AllocationMandateSetGroup_DAO>
    {
        private string _allocationGroupName;

        private int _key;

        private IList<AllocationMandateSet_DAO> _allocationMandateSets;

        private OrganisationStructure_DAO _organisationStructure;

        [Property("AllocationGroupName", ColumnType = "String", NotNull = true, Length = 255)]
        [ValidateNonEmpty("Allocation Group Name is a mandatory field")]
        public virtual string AllocationGroupName
        {
            get
            {
                return this._allocationGroupName;
            }
            set
            {
                this._allocationGroupName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "AllocationMandateSetGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(AllocationMandateSet_DAO), ColumnKey = "AllocationMandateSetGroupKey", Table = "AllocationMandateSet", Inverse = true)]
        public virtual IList<AllocationMandateSet_DAO> AllocationMandateSets
        {
            get
            {
                return this._allocationMandateSets;
            }
            set
            {
                this._allocationMandateSets = value;
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
    }
}
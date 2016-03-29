using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("MortgageLoanPurposeGroup", Schema = "dbo", Lazy = true)]
    public partial class MortgageLoanPurposeGroup_DAO : DB_2AM<MortgageLoanPurposeGroup_DAO>
    {
        private string _description;

        private int _key;

        private IList<MortgageLoanPurpose_DAO> _mortgageLoanPurposes;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "MortgageLoanPurposeGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(MortgageLoanPurpose_DAO), ColumnKey = "MortgageLoanPurposeGroupKey", Table = "MortgageLoanPurpose", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<MortgageLoanPurpose_DAO> MortgageLoanPurposes
        {
            get
            {
                return this._mortgageLoanPurposes;
            }
            set
            {
                this._mortgageLoanPurposes = value;
            }
        }
    }
}
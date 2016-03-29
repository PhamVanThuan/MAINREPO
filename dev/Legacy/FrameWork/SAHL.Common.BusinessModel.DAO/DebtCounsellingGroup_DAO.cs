using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DebtCounsellingGroup", Schema = "DebtCounselling")]
    public class DebtCounsellingGroup_DAO : DB_2AM<DebtCounsellingGroup_DAO>
    {
        private int _key;

        private System.DateTime _createdDate;

        private IList<DebtCounselling_DAO> _debtCounsellingCases;

        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingGroupKey", ColumnType = "Int32")]
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

        [Property("CreatedDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime CreatedDate
        {
            get
            {
                return this._createdDate;
            }
            set
            {
                this._createdDate = value;
            }
        }

        [HasMany(typeof(DebtCounselling_DAO), ColumnKey = "DebtCounsellingGroupKey", Table = "DebtCounselling", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public virtual IList<DebtCounselling_DAO> DebtCounsellingCases
        {
            get
            {
                return this._debtCounsellingCases;
            }
            set
            {
                this._debtCounsellingCases = value;
            }
        }
    }
}
using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FinancialServiceGroup", Schema = "dbo", Lazy = true)]
    public partial class FinancialServiceGroup_DAO : DB_2AM<FinancialServiceGroup_DAO>
    {
        private string _description;

        private int _key;

        private IList<FinancialServiceType_DAO> _financialServiceTypes;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialServiceGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(FinancialServiceType_DAO), ColumnKey = "FinancialServiceGroupKey", Table = "FinancialServiceType", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<FinancialServiceType_DAO> FinancialServiceTypes
        {
            get
            {
                return this._financialServiceTypes;
            }
            set
            {
                this._financialServiceTypes = value;
            }
        }
    }
}
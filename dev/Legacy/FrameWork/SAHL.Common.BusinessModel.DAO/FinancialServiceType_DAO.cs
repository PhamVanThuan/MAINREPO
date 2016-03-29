using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("FinancialServiceType", Schema = "dbo", Lazy = true)]
    public partial class FinancialServiceType_DAO : DB_2AM<FinancialServiceType_DAO>
    {
        private string _description;

        private int _key;

        /* commented, this is a lookup.
        private IList<CommissionTransaction> _commissionTransactions;

        private IList<FinancialService> _financialServices;*/

        private IList<OriginationSourceProductConfiguration_DAO> _originationSourceProductConfigurations;

        private IList<ProductCondition_DAO> _productConditions;

        private FinancialServiceGroup_DAO _financialServiceGroup;

        private ResetConfiguration_DAO _resetConfiguration;

        [Property("Description", ColumnType = "String", NotNull = true, Length = 50)]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "FinancialServiceTypeKey", ColumnType = "Int32")]
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

        [BelongsTo("FinancialServiceGroupKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service Group is a mandatory field")]
        public virtual FinancialServiceGroup_DAO FinancialServiceGroup
        {
            get
            {
                return this._financialServiceGroup;
            }
            set
            {
                this._financialServiceGroup = value;
            }
        }

        /* commented, this is a lookup.
        [HasMany(typeof(CommissionTransaction), ColumnKey = "FinancialServiceTypeKey", Table = "CommissionTransaction")]
        public virtual IList<CommissionTransaction> CommissionTransactions
        {
            get
            {
                return this._commissionTransactions;
            }
            set
            {
                this._commissionTransactions = value;
            }
        }

        [HasMany(typeof(FinancialService), ColumnKey = "FinancialServiceTypeKey", Table = "FinancialService")]
        public virtual IList<FinancialService> FinancialServices
        {
            get
            {
                return this._financialServices;
            }
            set
            {
                this._financialServices = value;
            }
        }
         */

        [HasMany(typeof(OriginationSourceProductConfiguration_DAO), ColumnKey = "FinancialServiceTypeKey", Table = "OriginationSourceProductConfiguration", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<OriginationSourceProductConfiguration_DAO> OriginationSourceProductConfigurations
        {
            get
            {
                return this._originationSourceProductConfigurations;
            }
            set
            {
                this._originationSourceProductConfigurations = value;
            }
        }

        [HasMany(typeof(ProductCondition_DAO), ColumnKey = "FinancialServiceTypeKey", Table = "ProductCondition", Lazy = true)]
        public virtual IList<ProductCondition_DAO> ProductConditions
        {
            get
            {
                return this._productConditions;
            }
            set
            {
                this._productConditions = value;
            }
        }

        [BelongsTo("ResetConfigurationKey", NotNull = false)]
        public virtual ResetConfiguration_DAO ResetConfiguration
        {
            get
            {
                return this._resetConfiguration;
            }
            set
            {
                this._resetConfiguration = value;
            }
        }
    }
}
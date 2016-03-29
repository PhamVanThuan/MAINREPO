using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("RuleItem", Schema = "dbo", Lazy = false)]
    public partial class RuleItem_DAO : DB_2AM<RuleItem_DAO>
    {
        private string _assemblyName;

        private string _typeName;

        private string _name;

        private string _description;

        private string _generalStatusReasonDescription;

        private int _ruleItemKey;

        private IList<RuleParameter_DAO> _ruleParameters;

        private bool _enforceRule;

        private GeneralStatus_DAO _generalStatus;

        [Property("Name", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Name is a mandatory field")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

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

        [Property("GeneralStatusReasonDescription", ColumnType = "String")]
        public virtual string GeneralStatusReasonDescription
        {
            get
            {
                return this._generalStatusReasonDescription;
            }
            set
            {
                this._generalStatusReasonDescription = value;
            }
        }

        [Property("AssemblyName", ColumnType = "String")]
        public virtual string AssemblyName
        {
            get
            {
                return this._assemblyName;
            }
            set
            {
                this._assemblyName = value;
            }
        }

        [Property("TypeName", ColumnType = "String")]
        public virtual string TypeName
        {
            get
            {
                return this._typeName;
            }
            set
            {
                this._typeName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "RuleItemKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._ruleItemKey;
            }
            set
            {
                this._ruleItemKey = value;
            }
        }

        /// <summary>
        /// A list of parameters that apply to the rule.
        /// </summary>
        /// <remarks>This must NOT be lazy loaded, as rules are cached by the rule service and we want these to load up with the object.</remarks>
        [HasMany(typeof(RuleParameter_DAO), ColumnKey = "RuleItemKey", Table = "RuleParameter", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = false)]
        public virtual IList<RuleParameter_DAO> RuleParameters
        {
            get
            {
                return this._ruleParameters;
            }
            set
            {
                this._ruleParameters = value;
            }
        }

        [Property("EnForceRule", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Enforce Rule is a mandatory field")]
        public virtual bool EnforceRule
        {
            get
            {
                return this._enforceRule;
            }
            set
            {
                this._enforceRule = value;
            }
        }

        /// <summary>
        /// Foreign Key reference to the GeneralStatus table. Rules that are marked as Inactive should not be executed by
        /// the domain.
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
    }
}
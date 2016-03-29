using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("AffordabilityType", Schema = "dbo", Lazy = true)]
    public partial class AffordabilityType_DAO : DB_2AM<AffordabilityType_DAO>
    {
        private string _description;

        private bool _descriptionRequired;

        private bool _isExpense;

        private int _Key;

        private IList<LegalEntityAffordability_DAO> _legalEntityAffordabilities;

        private AffordabilityTypeGroup_DAO _affordabilityTypeGroup;

        private int _sequence;

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

        [Property("IsExpense", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Is Expense is a mandatory field")]
        public virtual bool IsExpense
        {
            get
            {
                return this._isExpense;
            }
            set
            {
                this._isExpense = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "AffordabilityTypeKey", ColumnType = "Int32")]
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

        [HasMany(typeof(LegalEntityAffordability_DAO), ColumnKey = "AffordabilityTypeKey", Table = "LegalEntityAffordability", Lazy = true)]
        public virtual IList<LegalEntityAffordability_DAO> LegalEntityAffordabilities
        {
            get
            {
                return this._legalEntityAffordabilities;
            }
            set
            {
                this._legalEntityAffordabilities = value;
            }
        }

        [BelongsTo("AffordabilityTypeGroupKey", NotNull = true)]
        [ValidateNonEmpty("Affordability Type Group is a mandatory field")]
        public virtual AffordabilityTypeGroup_DAO AffordabilityTypeGroup
        {
            get
            {
                return this._affordabilityTypeGroup;
            }
            set
            {
                this._affordabilityTypeGroup = value;
            }
        }

        [Property("DescriptionRequired", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("DescriptionRequired is a mandatory field")]
        public virtual bool DescriptionRequired
        {
            get
            {
                return this._descriptionRequired;
            }
            set
            {
                this._descriptionRequired = value;
            }
        }

        [Property("Sequence", ColumnType = "int", NotNull = true)]
        [ValidateNonEmpty("DescriptionRequired is a mandatory field")]
        public virtual int Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
            }
        }
    }
}
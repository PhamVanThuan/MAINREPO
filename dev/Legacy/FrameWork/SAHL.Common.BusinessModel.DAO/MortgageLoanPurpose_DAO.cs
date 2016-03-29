using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("MortgageLoanPurpose", Schema = "dbo", Lazy = true)]
    public partial class MortgageLoanPurpose_DAO : DB_2AM<MortgageLoanPurpose_DAO>
    {
        private string _description;

        private int _key;

        /*private IList<CreditCriteria_DAO> _creditCriterias;

        private IList<LifeUserGroupPurpose> _lifeUserGroupPurposes;

        private IList<MortgageLoan> _mortgageLoans;

        private IList<OfferMortgageLoan_DAO> _applicationMortgageLoans;
        */

        //TODO: Replace with many to many relationship
        //private IList<OriginationSourceProductPurpose> _originationSourceProductPurposes;

        private MortgageLoanPurposeGroup_DAO _mortgageLoanPurposeGroup;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "MortgageLoanPurposeKey", ColumnType = "Int32")]
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

        /* Commented, this is a lookup.
                [HasMany(typeof(CreditCriteria_DAO), ColumnKey = "MortgageLoanPurposeKey", Table = "CreditCriteria")]
                public virtual IList<CreditCriteria_DAO> CreditCriterias
                {
                    get
                    {
                        return this._creditCriterias;
                    }
                    set
                    {
                        this._creditCriterias = value;
                    }
                }

                [HasMany(typeof(LifeUserGroupPurpose), ColumnKey = "MortgageLoanPurposeKey", Table = "LifeUserGroupPurpose")]
                public virtual IList<LifeUserGroupPurpose> LifeUserGroupPurposes
                {
                    get
                    {
                        return this._lifeUserGroupPurposes;
                    }
                    set
                    {
                        this._lifeUserGroupPurposes = value;
                    }
                }

                [HasMany(typeof(MortgageLoan), ColumnKey = "MortgageLoanPurposeKey", Table = "MortgageLoan")]
                public virtual IList<MortgageLoan> MortgageLoans
                {
                    get
                    {
                        return this._mortgageLoans;
                    }
                    set
                    {
                        this._mortgageLoans = value;
                    }
                }

                [HasMany(typeof(OfferMortgageLoan_DAO), ColumnKey = "MortgageLoanPurposeKey", Table = "OfferMortgageLoan")]
                public virtual IList<OfferMortgageLoan_DAO> ApplicationMortgageLoans
                {
                    get
                    {
                        return this._applicationMortgageLoans;
                    }
                    set
                    {
                        this._applicationMortgageLoans = value;
                    }
                }

         * */

        [BelongsTo("MortgageLoanPurposeGroupKey", NotNull = true)]
        [ValidateNonEmpty("Mortgage Loan Purpose Group is a mandatory field")]
        public virtual MortgageLoanPurposeGroup_DAO MortgageLoanPurposeGroup
        {
            get
            {
                return this._mortgageLoanPurposeGroup;
            }
            set
            {
                this._mortgageLoanPurposeGroup = value;
            }
        }

        //TODO: Replace with many to many relationship
        //[HasMany(typeof(OriginationSourceProductPurpose), ColumnKey = "MortgageLoanPurposeKey", Table = "OriginationSourceProductPurpose")]
        //public virtual IList<OriginationSourceProductPurpose> OriginationSourceProductPurposes
        //{
        //    get
        //    {
        //        return this._originationSourceProductPurposes;
        //    }
        //    set
        //    {
        //        this._originationSourceProductPurposes = value;
        //    }
        //}
    }
}
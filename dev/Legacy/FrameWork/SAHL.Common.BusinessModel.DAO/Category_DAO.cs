using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Category", Schema = "dbo", Lazy = true)]
    public partial class Category_DAO : DB_2AM<Category_DAO>
    {
        private double _value;

        private string _description;

        private int _key;

        /* Commented, this is a lookup
        private IList<CreditCriteria_DAO> _creditCriterias;

        private IList<FinancialService> _financialServices;

        private IList<OfferInformationVariableLoan_DAO> _applicationInformationVariableLoans;

        private IList<ProductCategory> _productCategories;
        */

        [Property("Value", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Value is a mandatory field")]
        public virtual double Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

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

        [PrimaryKey(PrimaryKeyType.Assigned, "CategoryKey", ColumnType = "Int32", UnsavedValue = "-1")]
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

        /* Commented, this is a lookup
        [HasMany(typeof(CreditCriteria_DAO), ColumnKey = "CategoryKey", Table = "CreditCriteria")]
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

        [HasMany(typeof(FinancialService), ColumnKey = "CategoryKey", Table = "FinancialService")]
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

        [HasMany(typeof(OfferInformationVariableLoan_DAO), ColumnKey = "CategoryKey", Table = "OfferInformationVariableLoan")]
        public virtual IList<OfferInformationVariableLoan_DAO> ApplicationInformationVariableLoans
        {
            get
            {
                return this._applicationInformationVariableLoans;
            }
            set
            {
                this._applicationInformationVariableLoans = value;
            }
        }

        [HasMany(typeof(ProductCategory), ColumnKey = "CategoryKey", Table = "ProductCategory")]
        public virtual IList<ProductCategory> ProductCategories
        {
            get
            {
                return this._productCategories;
            }
            set
            {
                this._productCategories = value;
            }
        }
         */
    }
}
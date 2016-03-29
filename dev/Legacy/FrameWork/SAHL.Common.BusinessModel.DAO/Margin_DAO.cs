using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Margin", Schema = "dbo")]
    public partial class Margin_DAO : DB_2AM<Margin_DAO>
    {
        private double _value;

        private string _description;

        private int _key;

        // commented, this is a lookup
        //private IList<CreditCriteria_DAO> _creditCriterias;

        private IList<MarginProduct_DAO> _marginProducts;

        //commented as this is a lookup.
        //private IList<OfferInformationCashUpFront_DAO> _applicationInformationCashUpFronts;

        //private IList<OfferInformationQuickCash_DAO> _applicationInformationQuickCashes;

        //private IList<OfferInformationVariableLoan_DAO> _applicationInformationVariableLoans;

        private IList<ProductCategory_DAO> _productCategories;

        private IList<RateConfiguration_DAO> _rateConfigurations;

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

        [PrimaryKey(PrimaryKeyType.Native, "MarginKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(CreditCriteria_DAO), ColumnKey = "MarginKey", Table = "CreditCriteria")]
        //public virtual IList<CreditCriteria_DAO> CreditCriterias
        //{
        //    get
        //    {
        //        return this._creditCriterias;
        //    }
        //    set
        //    {
        //        this._creditCriterias = value;
        //    }
        //}

        [HasMany(typeof(MarginProduct_DAO), ColumnKey = "MarginKey", Table = "MarginProduct", Lazy = true)]
        public virtual IList<MarginProduct_DAO> MarginProducts
        {
            get
            {
                return this._marginProducts;
            }
            set
            {
                this._marginProducts = value;
            }
        }

        // Commented, this is a lookup.
        //[HasMany(typeof(OfferInformationCashUpFront_DAO), ColumnKey = "MarginKey", Table = "OfferInformationCashUpFront")]
        //public virtual IList<OfferInformationCashUpFront_DAO> ApplicationInformationCashUpFronts
        //{
        //    get
        //    {
        //        return this._applicationInformationCashUpFronts;
        //    }
        //    set
        //    {
        //        this._applicationInformationCashUpFronts = value;
        //    }
        //}

        // Commented, this is a lookup.
        //[HasMany(typeof(OfferInformationQuickCash_DAO), ColumnKey = "MarginKey", Table = "OfferInformationQuickCash")]
        //public virtual IList<OfferInformationQuickCash_DAO> ApplicationInformationQuickCashes
        //{
        //    get
        //    {
        //        return this._applicationInformationQuickCashes;
        //    }
        //    set
        //    {
        //        this._applicationInformationQuickCashes = value;
        //    }
        //}

        // Commented, this is a lookup.
        //[HasMany(typeof(OfferInformationVariableLoan_DAO), ColumnKey = "MarginKey", Table = "OfferInformationVariableLoan")]
        //public virtual IList<OfferInformationVariableLoan_DAO> ApplicationInformationVariableLoans
        //{
        //    get
        //    {
        //        return this._applicationInformationVariableLoans;
        //    }
        //    set
        //    {
        //        this._applicationInformationVariableLoans = value;
        //    }
        //}

        [HasMany(typeof(ProductCategory_DAO), ColumnKey = "MarginKey", Table = "ProductCategory", Lazy = true)]
        public virtual IList<ProductCategory_DAO> ProductCategories
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

        [HasMany(typeof(RateConfiguration_DAO), ColumnKey = "MarginKey", Table = "RateConfiguration", Lazy = true)]
        public virtual IList<RateConfiguration_DAO> RateConfigurations
        {
            get
            {
                return this._rateConfigurations;
            }
            set
            {
                this._rateConfigurations = value;
            }
        }
    }
}
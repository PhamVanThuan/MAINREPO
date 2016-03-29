using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("MarketRate", Schema = "dbo")]
    public partial class MarketRate_DAO : DB_2AM<MarketRate_DAO>
    {
        private double _value;

        private string _description;

        private int _key;

        private IList<MarketRateHistory_DAO> _marketRateHistories;

        /*  Commeted, this is a lookup.
        private IList<OfferInformationCashUpFront> _applicationInformationCashUpFronts;

        private IList<OfferInformationQuickCash> _applicationInformationQuickCashes;

        private IList<OfferInformationVariableLoan> _applicationInformationVariableLoans;*/

        //todo: implement OriginationSourceProductConfiguration_DAO
        // private IList<OriginationSourceProductConfiguration_DAO> _originationSourceProductConfigurations;

        private IList<RateConfiguration_DAO> _rateConfigurations;

        /// <summary>
        /// The current value of this marketrate.  This is always the current marketrate, irrespective of resets.
        /// </summary>
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

        /// <summary>
        /// The description of this marketrate.
        /// </summary>
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

        /// <summary>
        /// This is the primary key, used to identify an instance of Marketrate.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "MarketRateKey", ColumnType = "Int32")]
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

        /// <summary>
        /// A list that contains the history of this marketrate's changes through time.
        /// </summary>
        [HasMany(typeof(MarketRateHistory_DAO), ColumnKey = "MarketRateKey", Table = "MarketRateHistory", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<MarketRateHistory_DAO> MarketRateHistories
        {
            get
            {
                return this._marketRateHistories;
            }
            set
            {
                this._marketRateHistories = value;
            }
        }

        /*
        [HasMany(typeof(OfferInformationCashUpFront), ColumnKey = "MarketRateKey", Table = "OfferInformationCashUpFront")]
        public virtual IList<OfferInformationCashUpFront> ApplicationInformationCashUpFronts
        {
            get
            {
                return this._applicationInformationCashUpFronts;
            }
            set
            {
                this._applicationInformationCashUpFronts = value;
            }
        }

        [HasMany(typeof(OfferInformationQuickCash), ColumnKey = "MarketRateKey", Table = "OfferInformationQuickCash")]
        public virtual IList<OfferInformationQuickCash> ApplicationInformationQuickCashes
        {
            get
            {
                return this._applicationInformationQuickCashes;
            }
            set
            {
                this._applicationInformationQuickCashes = value;
            }
        }

        [HasMany(typeof(OfferInformationVariableLoan), ColumnKey = "MarketRateKey", Table = "OfferInformationVariableLoan")]
        public virtual IList<OfferInformationVariableLoan> ApplicationInformationVariableLoans
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
        */

        // Todo: uncomment once OriginationSourceProductConfiguration_DAO is implemented
        /// <summary>
        /// A collection of all the OriginationSourceProductConfigurtation entries that use this Marketrate.
        /// </summary>
        /// <seealso cref="OriginationSourceProductConfiguration_DAO"/>
        //[HasMany(typeof(OriginationSourceProductConfiguration_DAO), ColumnKey = "MarketRateKey", Table = "OriginationSourceProductConfiguration")]
        //public virtual IList<OriginationSourceProductConfiguration_DAO> OriginationSourceProductConfigurations
        //{
        //    get
        //    {
        //        return this._originationSourceProductConfigurations;
        //    }
        //    set
        //    {
        //        this._originationSourceProductConfigurations = value;
        //    }
        //}

        [HasMany(typeof(RateConfiguration_DAO), ColumnKey = "MarketRateKey", Table = "RateConfiguration", Lazy = true)]
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
using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;

using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{

     [ActiveRecord("OfferInformationRateOverride", Lazy=true, Schema="dbo")]
    public partial class ApplicationInformationRateOverride_DAO : DB_2AM<ApplicationInformationRateOverride_DAO>
    {
        
        private int? _term;

        private Double? _capRate;

        private Double? _capBalance;

        private Double? _floorRate;

        private Double? _fixedRate;

        private Double? _discount;

       
        private int _key;
        
        private ApplicationInformation_DAO _applicationInformation;

       
        [Property("Term")] //, ColumnType="Int32")]
        public virtual int? Term {
            get {
                return this._term;
            }
            set {
                this._term = value;
            }
        }

        [Property("CapRate")] //, ColumnType="Double")]
        public virtual Double? CapRate
        {
            get {
                return this._capRate;
            }
            set {
                this._capRate = value;
            }
        }
         
        [Property("CapBalance")] //, ColumnType="Double")]
        public virtual Double? CapBalance
        {
            get
            {
                return this._capBalance;
            }
            set
            {
                this._capBalance = value;
            }
        }

        [Property("FloorRate")] //, ColumnType="Double")]
        public virtual Double? FloorRate
        {
            get {
                return this._floorRate;
            }
            set {
                this._floorRate = value;
            }
        }

        [Property("FixedRate")] //, ColumnType="Double")]
        public virtual Double? FixedRate
        {
            get {
                return this._fixedRate;
            }
            set {
                this._fixedRate = value;
            }
        }

        [Property("Discount")] //, ColumnType="Double")]
        public virtual Double? Discount
        {
            get {
                return this._discount;
            }
            set {
                this._discount = value;
            }
        }

                
        [PrimaryKey(PrimaryKeyType.Native, "OfferInformationRateOverrideKey", ColumnType="Int32")]
        public virtual int Key {
            get {
                return this._key;
            }
            set {
                this._key = value;
            }
        }
        
        [BelongsTo("OfferInformationKey", NotNull=true)]
        public virtual ApplicationInformation_DAO ApplicationInformation
        {
            get {
                return this._applicationInformation;
            }
            set {
                this._applicationInformation = value;
            }
        }
    }
    
}
